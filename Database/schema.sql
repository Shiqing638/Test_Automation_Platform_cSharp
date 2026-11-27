USE TestFlowDB;
GO

IF OBJECT_ID('dbo.TestRuns', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TestRuns (
        RunId INT IDENTITY(1,1) PRIMARY KEY,
        SuiteName NVARCHAR(200) NOT NULL,
        ExecutedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        TotalTests INT NOT NULL,
        PassedTests INT NOT NULL,
        FailedTests INT NOT NULL,
        ErrorTests INT NOT NULL,
        Status NVARCHAR(20) NOT NULL
    );
END;
GO

IF OBJECT_ID('dbo.TestResults', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TestResults (
        ResultId INT IDENTITY(1,1) PRIMARY KEY,
        RunId INT NOT NULL,
        TestName NVARCHAR(200) NOT NULL,
        Status NVARCHAR(20) NOT NULL,
        ErrorMessage NVARCHAR(MAX) NULL,
        CONSTRAINT FK_TestResults_TestRuns
            FOREIGN KEY (RunId)
            REFERENCES dbo.TestRuns(RunId)
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_TestResults_RunId'
      AND object_id = OBJECT_ID('dbo.TestResults')
)
BEGIN 
    CREATE INDEX IX_TestResults_RunId
    ON dbo.TestResults(RunId);
END;
Go