USE TeamTasksDashboardDb;
GO

/*
    Stored Procedure: dbo.usp_CreateTask

    Objetivo:
    Crear una nueva tarea validando:
    - existencia del proyecto
    - existencia del desarrollador
    - estado activo del desarrollador
    - valores permitidos de status, priority y estimated complexity

    Notas de enums del backend:
    TaskItemStatus
    1 = ToDo
    2 = InProgress
    3 = Blocked
    4 = Completed

    TaskPriority
    1 = Low
    2 = Medium
    3 = High
*/
CREATE OR ALTER PROCEDURE dbo.usp_CreateTask
    @ProjectId INT,                    -- Id del proyecto al que pertenecerá la tarea
    @Title NVARCHAR(200),             -- Título de la tarea (requerido, máximo 200)
    @Description NVARCHAR(2000) = NULL, -- Descripción opcional de la tarea
    @AssigneeId INT,                  -- Id del desarrollador asignado
    @Status INT,                      -- Estado inicial de la tarea: 1=ToDo, 2=InProgress, 3=Blocked, 4=Completed
    @Priority INT,                    -- Prioridad: 1=Low, 2=Medium, 3=High
    @EstimatedComplexity INT,         -- Complejidad estimada permitida entre 1 y 5
    @DueDate DATETIME2(7)             -- Fecha límite de la tarea
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -----------------------------------------------------------------------
        -- Validaciones básicas de entrada
        -----------------------------------------------------------------------
        IF @ProjectId <= 0
            THROW 50001, 'ProjectId must be greater than zero.', 1;

        IF NULLIF(LTRIM(RTRIM(@Title)), '') IS NULL
            THROW 50002, 'Title is required.', 1;

        IF LEN(LTRIM(RTRIM(@Title))) > 200
            THROW 50003, 'Title must not exceed 200 characters.', 1;

        IF @AssigneeId <= 0
            THROW 50004, 'AssigneeId must be greater than zero.', 1;

        IF @EstimatedComplexity < 1 OR @EstimatedComplexity > 5
            THROW 50005, 'EstimatedComplexity must be between 1 and 5.', 1;

        IF @DueDate IS NULL
            THROW 50006, 'DueDate is required.', 1;

        -----------------------------------------------------------------------
        -- Validación de enums según backend real
        -----------------------------------------------------------------------
        IF @Status NOT IN (1, 2, 3, 4)
            THROW 50007, 'Status is invalid.', 1;

        IF @Priority NOT IN (1, 2, 3)
            THROW 50008, 'Priority is invalid.', 1;

        -----------------------------------------------------------------------
        -- Validación de existencia de entidades relacionadas
        -----------------------------------------------------------------------
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Projects
            WHERE ProjectId = @ProjectId
        )
            THROW 50009, 'Project was not found.', 1;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Developers
            WHERE DeveloperId = @AssigneeId
        )
            THROW 50010, 'Developer was not found.', 1;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Developers
            WHERE DeveloperId = @AssigneeId
              AND IsActive = 1
        )
            THROW 50011, 'Developer is inactive.', 1;

        -----------------------------------------------------------------------
        -- Inserción de la nueva tarea
        -- Si el estado llega como Completed, se registra CompletionDate en UTC
        -----------------------------------------------------------------------
        INSERT INTO dbo.Tasks
        (
            ProjectId,
            Title,
            Description,
            AssigneeId,
            Status,
            Priority,
            EstimatedComplexity,
            DueDate,
            CompletionDate,
            CreatedAt
        )
        VALUES
        (
            @ProjectId,
            LTRIM(RTRIM(@Title)),
            CASE
                WHEN NULLIF(LTRIM(RTRIM(@Description)), '') IS NULL THEN NULL
                ELSE LTRIM(RTRIM(@Description))
            END,
            @AssigneeId,
            @Status,
            @Priority,
            @EstimatedComplexity,
            @DueDate,
            CASE
                WHEN @Status = 4 THEN SYSUTCDATETIME()
                ELSE NULL
            END,
            SYSUTCDATETIME()
        );

        -----------------------------------------------------------------------
        -- Retorno del resultado exitoso
        -----------------------------------------------------------------------
        DECLARE @TaskId INT = SCOPE_IDENTITY();

        SELECT
            Success = CAST(1 AS bit),
            Message = 'Task created successfully.',
            TaskId = @TaskId;
    END TRY
    BEGIN CATCH
        -----------------------------------------------------------------------
        -- Retorno controlado del error
        -----------------------------------------------------------------------
        SELECT
            Success = CAST(0 AS bit),
            Message = ERROR_MESSAGE(),
            TaskId = CAST(NULL AS INT);
    END CATCH
END;
GO

/*
    Ejecución de prueba

    Referencias:
    - @ProjectId = 1  -> proyecto existente
    - @AssigneeId = 1 -> desarrollador existente y activo
    - @Status = 1     -> ToDo
    - @Priority = 2   -> Medium
    - @EstimatedComplexity = 3 -> valor válido entre 1 y 5

    EXEC dbo.usp_CreateTask
        @ProjectId = 1,
        @Title = 'Test task from SP',
        @Description = 'Created manually from SQL Server.',
        @AssigneeId = 1,
        @Status = 1,
        @Priority = 2,
        @EstimatedComplexity = 3,
        @DueDate = '2026-03-25';
    GO
  */
