USE TeamTasksDashboardDb;
GO

/*
    Query: Developer Delay Risk Prediction

    Objetivo:
    Obtener una vista analítica del riesgo de atraso por desarrollador,
    alineada con la lógica implementada en el backend para el dashboard.

    Qué calcula:
    - cantidad de tareas abiertas por desarrollador
    - promedio de días de atraso en tareas abiertas vencidas
    - fecha más próxima de vencimiento entre tareas abiertas
    - fecha más lejana de vencimiento entre tareas abiertas
    - fecha estimada de finalización basada en el atraso promedio
    - bandera de alto riesgo

    Reglas de negocio tomadas del backend:
    - solo se consideran desarrolladores activos
    - una tarea abierta es toda tarea con Status <> 4
    - 4 = Completed
    - AvgDelayDays solo promedia tareas abiertas cuyo DueDate ya venció
    - HighRiskFlag = true cuando:
        * tiene tareas abiertas
        * y además AvgDelayDays >= 3
          o la tarea abierta más próxima ya está vencida
*/

-- Fecha actual en UTC usada como referencia para vencimientos y atraso
DECLARE @Today DATE = CAST(GETUTCDATE() AS DATE);

;WITH DeveloperRiskBase AS
(
    SELECT
        -- Identificador del desarrollador
        d.DeveloperId,

        -- Nombre completo del desarrollador
        DeveloperName = CONCAT(d.FirstName, ' ', d.LastName),

        -- Cantidad de tareas abiertas
        -- Se consideran abiertas todas las tareas con Status distinto de Completed (4)
        OpenTasksCount = COUNT(
            CASE
                WHEN t.Status <> 4 THEN 1
            END
        ),

        -- Promedio de días de atraso
        -- Solo considera tareas abiertas cuyo DueDate ya pasó
        AvgDelayDays = AVG(
            CASE
                WHEN t.Status <> 4
                 AND t.DueDate < @Today
                THEN CAST(DATEDIFF(DAY, t.DueDate, @Today) AS DECIMAL(10, 2))
            END
        ),

        -- Fecha de vencimiento más cercana entre tareas abiertas
        NearestDueDate = MIN(
            CASE
                WHEN t.Status <> 4 THEN t.DueDate
            END
        ),

        -- Fecha de vencimiento más lejana entre tareas abiertas
        LatestDueDate = MAX(
            CASE
                WHEN t.Status <> 4 THEN t.DueDate
            END
        )
    FROM dbo.Developers d
    LEFT JOIN dbo.Tasks t
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
        -- Nombre del desarrollador
        DeveloperName,

        -- Si no hay tareas, se devuelve 0
        OpenTasksCount = ISNULL(OpenTasksCount, 0),

        -- Si no hay atraso, se devuelve 0.00
        -- Se redondea a 2 decimales para mantener consistencia visual
        AvgDelayDays = CAST(ROUND(ISNULL(AvgDelayDays, 0), 2) AS DECIMAL(10, 2)),

        -- Fechas calculadas sobre tareas abiertas
        NearestDueDate,
        LatestDueDate
    FROM DeveloperRiskBase
)
SELECT
    -- Desarrollador
    DeveloperName,

    -- Total de tareas abiertas
    OpenTasksCount,

    -- Promedio de días de atraso
    AvgDelayDays,

    -- Fecha más próxima de vencimiento
    NearestDueDate,

    -- Fecha más lejana de vencimiento
    LatestDueDate,

    -- Fecha estimada de finalización
    -- Se calcula sumando al LatestDueDate el techo del atraso promedio
    PredictedCompletionDate =
        CASE
            WHEN LatestDueDate IS NULL THEN NULL
            ELSE DATEADD(DAY, CEILING(AvgDelayDays), LatestDueDate)
        END,

    -- Bandera de alto riesgo
    -- Se activa si:
    -- 1. tiene tareas abiertas
    -- 2. y además el atraso promedio es >= 3 días
    --    o la fecha más próxima de vencimiento ya pasó
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
    -- Primero los desarrolladores con mayor atraso promedio
    AvgDelayDays DESC,

    -- Luego los que tienen más tareas abiertas
    OpenTasksCount DESC,

    -- Finalmente orden alfabético
    DeveloperName ASC;
GO