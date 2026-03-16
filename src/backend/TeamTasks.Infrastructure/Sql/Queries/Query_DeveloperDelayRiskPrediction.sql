DECLARE @Today DATE = CAST(GETUTCDATE() AS DATE);

;WITH DeveloperRiskBase AS
(
    SELECT
        d.DeveloperId,
        DeveloperName = CONCAT(d.FirstName, ' ', d.LastName),
        OpenTasksCount = COUNT(CASE WHEN t.Status <> 4 THEN 1 END),
        AvgDelayDays = AVG(
            CASE
                WHEN t.Status <> 4
                 AND t.DueDate < @Today
                THEN CAST(DATEDIFF(DAY, t.DueDate, @Today) AS DECIMAL(10, 2))
            END
        ),
        NearestDueDate = MIN(
            CASE
                WHEN t.Status <> 4 THEN t.DueDate
            END
        ),
        LatestDueDate = MAX(
            CASE
                WHEN t.Status <> 4 THEN t.DueDate
            END
        )
    FROM Developers d
    LEFT JOIN Tasks t
        ON t.AssigneeId = d.DeveloperId
    WHERE d.IsActive = 1
    GROUP BY
        d.DeveloperId,
        d.FirstName,
        d.LastName
),
DeveloperRiskResult AS
(
    SELECT
        DeveloperName,
        OpenTasksCount = ISNULL(OpenTasksCount, 0),
        AvgDelayDays = CAST(ROUND(ISNULL(AvgDelayDays, 0), 2) AS DECIMAL(10, 2)),
        NearestDueDate,
        LatestDueDate
    FROM DeveloperRiskBase
)
SELECT
    DeveloperName,
    OpenTasksCount,
    AvgDelayDays,
    NearestDueDate,
    LatestDueDate,
    PredictedCompletionDate =
        CASE
            WHEN LatestDueDate IS NULL THEN NULL
            ELSE DATEADD(DAY, CEILING(AvgDelayDays), LatestDueDate)
        END,
    HighRiskFlag = CAST(
        CASE
            WHEN OpenTasksCount > 0
             AND (
                    AvgDelayDays >= 3
                    OR (NearestDueDate IS NOT NULL AND NearestDueDate < @Today)
                 )
            THEN 1
            ELSE 0
        END
        AS bit
    )
FROM DeveloperRiskResult
ORDER BY
    AvgDelayDays DESC,
    OpenTasksCount DESC,
    DeveloperName ASC;