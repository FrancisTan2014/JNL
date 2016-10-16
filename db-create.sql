USE [master]
GO
/****** Object:  Database [Jnl]    Script Date: 2016/10/16 16:18:20 ******/
CREATE DATABASE [Jnl]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Jnl', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Jnl.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Jnl_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Jnl_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Jnl] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Jnl].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Jnl] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Jnl] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Jnl] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Jnl] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Jnl] SET ARITHABORT OFF 
GO
ALTER DATABASE [Jnl] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Jnl] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Jnl] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Jnl] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Jnl] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Jnl] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Jnl] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Jnl] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Jnl] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Jnl] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Jnl] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Jnl] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Jnl] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Jnl] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Jnl] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Jnl] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Jnl] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Jnl] SET RECOVERY FULL 
GO
ALTER DATABASE [Jnl] SET  MULTI_USER 
GO
ALTER DATABASE [Jnl] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Jnl] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Jnl] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Jnl] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Jnl] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Jnl', N'ON'
GO
ALTER DATABASE [Jnl] SET QUERY_STORE = OFF
GO
USE [Jnl]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Jnl]
GO
/****** Object:  Table [dbo].[Article]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Article](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreateStaffId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_ARTICLE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dictionaries]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dictionaries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_DICTIONARIES] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NOT NULL,
	[ParentName] [nvarchar](50) NOT NULL,
	[GrandParentName] [nvarchar](50) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_DEPARTMENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Staff]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkId] [varchar](20) NOT NULL,
	[SalaryId] [varchar](20) NOT NULL,
	[Identity] [varchar](18) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Gender] [nchar](1) NOT NULL,
	[HireDate] [datetime] NOT NULL,
	[BirthDate] [date] NOT NULL,
	[WorkFlagId] [int] NOT NULL,
	[WorkTypeId] [int] NOT NULL,
	[PoliticalStatusId] [int] NOT NULL,
	[PositionId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Password] [varchar](32) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_STAFF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewStaff]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewStaff]
AS
SELECT
S.Id,S.WorkId,S.SalaryId,S.[Identity],S.Name,S.Gender,S.HireDate,S.BirthDate,
S.WorkFlagId,C.Name AS WorkFlag,
S.WorkTypeId,E.Name AS WorkType,
S.PoliticalStatusId,A.Name AS PoliticalStatus,
S.PositionId,B.Name AS Position,
S.DepartmentId,D.Name AS Department,
S.Password,S.AddTime,S.UpdateTime,S.IsDelete
FROM 
Staff S LEFT JOIN
Department D ON S.DepartmentId=D.Id LEFT JOIN
Dictionaries A ON S.PoliticalStatusId=A.Id LEFT JOIN
Dictionaries B ON S.PositionId=B.Id LEFT JOIN
Dictionaries C ON S.WorkFlagId=C.Id LEFT JOIN
Dictionaries E ON S.WorkTypeId=E.Id
GO
/****** Object:  Table [dbo].[ArticleCategory]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_ARTICLECATEGORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewArticle]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewArticle]
AS
SELECT
A.Id,A.CategoryId,A.Title,A.Content,A.CreateStaffId,A.AddTime,A.IsDelete,
G.Name AS CategoryName,
V.Name AS Creator,V.DepartmentId,V.Department,V.SalaryId,V.WorkId
FROM Article A
LEFT JOIN ArticleCategory G ON A.CategoryId=G.Id
LEFT JOIN ViewStaff V ON A.CreateStaffId=V.Id
GO
/****** Object:  Table [dbo].[WarningImplement]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarningImplement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarningId] [int] NOT NULL,
	[ImplementDepartmentId] [int] NOT NULL,
	[HasImplemented] [bit] NOT NULL,
	[HasResponded] [bit] NOT NULL,
	[ImplementDetail] [nvarchar](max) NOT NULL,
	[RespondTime] [datetime] NOT NULL,
	[ResponseVerifyStatus] [int] NOT NULL,
	[VerifyStaffId] [int] NOT NULL,
	[VerifyTime] [datetime] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_WARNINGIMPLEMENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewWarningImplement]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewWarningImplement]
AS
SELECT
W.Id,W.WarningId,W.ImplementDepartmentId,W.HasImplemented,W.HasResponded,W.ImplementDetail,W.RespondTime,W.ResponseVerifyStatus,W.VerifyStaffId,W.VerifyTime,W.AddTime,W.UpdateTime,W.IsDelete,
D.Name AS ImplementDepart,
V.Name AS VerifyStaff
FROM WarningImplement W
LEFT JOIN Department D ON W.ImplementDepartmentId=D.Id
LEFT JOIN ViewStaff V ON W.VerifyStaffId=V.Id 
GO
/****** Object:  Table [dbo].[Warning]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warning](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WarningTime] [datetime] NOT NULL,
	[TimeLimit] [datetime] NOT NULL,
	[WarningSource] [int] NOT NULL,
	[WarningStaffId] [int] NOT NULL,
	[ImplementDeparts] [nvarchar](255) NOT NULL,
	[WarningTitle] [nvarchar](max) NOT NULL,
	[WarningContent] [nvarchar](max) NOT NULL,
	[ChangeRequirement] [nvarchar](max) NOT NULL,
	[HasBeganImplement] [bit] NOT NULL,
	[HasImplementedAll] [bit] NOT NULL,
	[Visible] [bit] NOT NULL,
	[CreateStaffId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_WARNING] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewWarning]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewWarning]
AS
SELECT
W.Id,W.WarningContent,W.WarningSource,W.WarningStaffId,W.WarningTime,W.TimeLimit,W.WarningTitle,W.ImplementDeparts,W.ChangeRequirement,W.HasBeganImplement,W.HasImplementedAll,W.Visible,W.CreateStaffId,W.AddTime,W.UpdateTime,W.IsDelete,
V.Name AS WarningStaff,V.DepartmentId AS WarningDepartId,V.Department AS WarningDepart,
D.Name WarningSourceName
FROM Warning W
LEFT JOIN ViewStaff V ON W.WarningStaffId=V.Id 
LEFT JOIN Dictionaries D ON W.WarningSource=D.Id
GO
/****** Object:  View [dbo].[ViewLocomotive]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewLocomotive]
AS
SELECT 
D1.Id,D1.Name AS LocoNumber,
D2.Id AS LocoModelId,D2.Name AS LocoModel,
D3.Id AS LocoTypeId,D3.Name AS LocoType 
FROM Dictionaries D1
LEFT JOIN Dictionaries D2 ON D1.ParentId=D2.Id
LEFT JOIN Dictionaries D3 ON D2.ParentId=D3.Id
WHERE D1.Type=14
GO
/****** Object:  Table [dbo].[LocoQuality28]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocoQuality28](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocomotiveId] [int] NOT NULL,
	[ReportTime] [datetime] NOT NULL,
	[RepairTeam] [nvarchar](50) NOT NULL,
	[RepairMethod] [nvarchar](max) NOT NULL,
	[RepairProcessId] [int] NOT NULL,
	[LivingItemId] [int] NOT NULL,
	[LiveItem] [nvarchar](max) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_LOCOQUALITY28] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewLocoQuality28]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewLocoQuality28]
AS
SELECT
L.Id,L.LocomotiveId,L.ReportTime,L.RepairTeam,L.RepairMethod,L.RepairProcessId,L.LivingItemId,L.LiveItem,L.AddTime,L.UpdateTime,L.IsDelete,
M.LocoModel,M.LocoModelId,M.LocoNumber,M.LocoType,M.LocoTypeId,
D.Name AS RepairProcess,
E.Name AS LivingItem
FROM LocoQuality28 L
LEFT JOIN ViewLocomotive M ON L.LocomotiveId=M.Id
LEFT JOIN Dictionaries D ON L.RepairProcessId=D.Id
LEFT JOIN Dictionaries E ON L.LivingItemId=E.Id




GO
/****** Object:  Table [dbo].[LocoQuality6]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocoQuality6](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocomotiveId] [int] NOT NULL,
	[ReportTime] [datetime] NOT NULL,
	[RepairStaffId] [int] NOT NULL,
	[LivingItemId] [int] NOT NULL,
	[BrokenPlace] [nvarchar](max) NOT NULL,
	[RepairMethod] [nvarchar](max) NOT NULL,
	[StartRepair] [datetime] NOT NULL,
	[EndRepair] [datetime] NOT NULL,
	[RepairDesc] [nvarchar](max) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_LOCOQUALITY6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewLocoQuality6]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewLocoQuality6]
AS
SELECT
L.Id,L.LocomotiveId,L.ReportTime,L.RepairStaffId,L.LivingItemId,L.BrokenPlace,L.RepairMethod,L.StartRepair,L.EndRepair,L.RepairDesc,L.AddTime,L.UpdateTime,L.IsDelete,
M.LocoModel,M.LocoModelId,M.LocoNumber,M.LocoType,M.LocoTypeId,
S.Name AS RepairStaff,S.DepartmentId,S.Department,
D.Name AS LivingItem
FROM LocoQuality6 L
LEFT JOIN ViewLocomotive M ON L.LocomotiveId=M.Id
LEFT JOIN ViewStaff S ON L.RepairStaffId=S.Id
LEFT JOIN Dictionaries D ON L.LivingItemId=D.Id



GO
/****** Object:  Table [dbo].[ExamScore]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamScore](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[StaffName] [nvarchar](20) NOT NULL,
	[WorkNo] [varchar](20) NOT NULL,
	[Position] [nvarchar](20) NOT NULL,
	[WorkPlace] [nvarchar](20) NOT NULL,
	[ExamTheme] [nvarchar](50) NOT NULL,
	[ExamSubject] [nvarchar](50) NOT NULL,
	[Score] [decimal](18, 0) NOT NULL,
	[ExamTime] [datetime] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_EXAMSCORE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewExamScore]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewExamScore]
AS
SELECT 
E.Id,E.StaffId,E.StaffName,E.WorkNo,E.Position,E.WorkPlace,E.ExamTheme,E.ExamSubject,E.Score,E.ExamTime,E.AddTime,E.IsDelete,
V.DepartmentId,V.Department,V.PositionId
FROM ExamScore E
LEFT JOIN ViewStaff V ON E.StaffId=V.Id
GO
/****** Object:  Table [dbo].[StaffScore]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffScore](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[MinusScore] [decimal](18, 0) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_STAFFSCORE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewStaffScore]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewStaffScore]
AS
SELECT
S.Id,S.StaffId,S.[Year],S.[Month],S.MinusScore,S.UpdateTime,S.IsDelete,
V.Name AS StaffName,V.DepartmentId,V.Department,V.PositionId,V.Position,V.WorkId,V.SalaryId
FROM StaffScore S
LEFT JOIN ViewStaff V ON S.StaffId=V.Id

GO
/****** Object:  View [dbo].[ViewStaffScoreTotal]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewStaffScoreTotal]
AS
SELECT
S.StaffId,S.[Year],S.MinusScore,
V.Name AS StaffName,V.DepartmentId,V.Department,V.PositionId,V.Position,V.SalaryId,V.WorkId
FROM
(SELECT StaffId,[Year],SUM(MinusScore) AS MinusScore FROM StaffScore GROUP BY StaffId,[Year]) AS S
LEFT JOIN ViewStaff V ON S.StaffId=V.Id
GO
/****** Object:  Table [dbo].[RiskType]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiskType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_RISKTYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Station]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Station](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Spell] [varchar](20) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_STATION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RiskInfo]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiskInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReportStaffId] [int] NOT NULL,
	[OccurrenceLineId] [int] NOT NULL,
	[FirstStationId] [int] NOT NULL,
	[LastStationId] [int] NOT NULL,
	[LocoServiceTypeId] [int] NOT NULL,
	[WeatherId] [int] NOT NULL,
	[OccurrenceTime] [datetime] NOT NULL,
	[RiskSummaryId] [int] NOT NULL,
	[RiskDetails] [nvarchar](max) NOT NULL,
	[RiskReason] [nvarchar](max) NOT NULL,
	[RiskFix] [nvarchar](max) NOT NULL,
	[Visible] [bit] NOT NULL,
	[RiskTypeId] [int] NOT NULL,
	[VerifyStatus] [int] NOT NULL,
	[VerifyStaffId] [int] NOT NULL,
	[VerifyTime] [datetime] NOT NULL,
	[ShowInStressPage] [bit] NOT NULL,
	[DealTimeLimit] [datetime] NOT NULL,
	[HasDealed] [bit] NOT NULL,
	[NeedRoomSign] [bit] NOT NULL,
	[NeedLeaderSign] [bit] NOT NULL,
	[NeedWriteFixDesc] [bit] NOT NULL,
	[NeedStressTrack] [bit] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_RISKINFO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Line]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Line](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[FirstStation] [nvarchar](50) NOT NULL,
	[LastStation] [nvarchar](50) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_LINE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RiskSummary]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiskSummary](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[ParentId] [int] NOT NULL,
	[IsBottom] [bit] NOT NULL,
	[TopestTypeId] [int] NOT NULL,
	[TopestName] [nvarchar](50) NOT NULL,
	[SecondLevelId] [int] NOT NULL,
	[SecondLevelName] [nvarchar](50) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_RISKSUMMARY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewRiskInfo]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewRiskInfo]
AS
SELECT
R.Id,R.ReportStaffId,R.OccurrenceLineId,R.FirstStationId,R.LastStationId,R.LocoServiceTypeId,R.WeatherId,R.OccurrenceTime,R.RiskSummaryId,R.RiskDetails,R.RiskReason,R.RiskFix,R.Visible,R.RiskTypeId,R.VerifyStaffId,R.VerifyStatus,R.VerifyTime,R.ShowInStressPage,R.DealTimeLimit,R.HasDealed,R.NeedRoomSign,R.NeedLeaderSign,R.NeedWriteFixDesc,R.NeedStressTrack,R.AddTime,R.UpdateTime,R.IsDelete,
VS.Name AS ReportStaffName,VS.DepartmentId AS ReportStaffDepartId, VS.Department AS ReportStaffDepart,VS.WorkId,VS.SalaryId,
L.Name AS OccurrenceLineName,
S1.Name AS FirstStationName,
S2.Name AS LastStationName,
DL.Name AS LocoServiceType,
DW.Name AS WeatherLike,
RS.Description AS RiskSummary,RS.TopestTypeId AS RiskTopestLevelId,RS.TopestName AS RiskTopestName,RS.SecondLevelId AS RiskSecondLevelId,RS.SecondLevelName AS RiskSecondLevelName,
RT.Name AS RiskType
FROM RiskInfo R
LEFT JOIN ViewStaff VS ON R.ReportStaffId=VS.Id
LEFT JOIN Line L ON R.OccurrenceLineId=L.Id
LEFT JOIN Station S1 ON R.FirstStationId=S1.Id
LEFT JOIN Station S2 ON R.LastStationId=S2.Id
LEFT JOIN Dictionaries DL ON R.LocoServiceTypeId=DL.Id
LEFT JOIN Dictionaries DW ON R.WeatherId=DW.Id
LEFT JOIN RiskSummary RS ON R.RiskSummaryId=RS.Id
LEFT JOIN RiskType RT ON R.RiskTypeId=RT.Id
GO
/****** Object:  Table [dbo].[RiskResponseStaff]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiskResponseStaff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RiskId] [int] NOT NULL,
	[ResponseStaffId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_RISKRESPONSESTAFF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewRiskResponseStaff]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewRiskResponseStaff]
AS
SELECT
R.Id,R.RiskId,R.ResponseStaffId,R.AddTime,R.IsDelete,
VS.Name,VS.DepartmentId,VS.Department,VS.SalaryId,VS.WorkId
FROM RiskResponseStaff R
LEFT JOIN ViewStaff VS ON R.ResponseStaffId=VS.Id

GO
/****** Object:  View [dbo].[ViewRiskRespondRisk]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewRiskRespondRisk]
AS
SELECT
RS.Id,RS.RiskId,RS.ResponseStaffId,RS.AddTime,RS.IsDelete,RS.Name,RS.DepartmentId,RS.Department,RS.SalaryId,RS.WorkId,
VR.ReportStaffId,VR.OccurrenceLineId,VR.FirstStationId,VR.LastStationId,VR.LocoServiceTypeId,VR.WeatherId,VR.OccurrenceTime,VR.RiskSummaryId,VR.RiskDetails,VR.RiskReason,VR.RiskFix,VR.Visible,VR.RiskTypeId,VR.VerifyStaffId,VR.VerifyStatus,VR.VerifyTime,VR.ShowInStressPage,VR.DealTimeLimit,VR.HasDealed,VR.NeedRoomSign,VR.NeedLeaderSign,VR.NeedWriteFixDesc,VR.NeedStressTrack,VR.ReportStaffName,VR.ReportStaffDepartId,VR.ReportStaffDepart,VR.WorkId AS ReportStaffWorkId,VR.SalaryId AS ReportStaffSalaryId,VR.OccurrenceLineName,VR.FirstStationName,VR.LastStationName,VR.LocoServiceType,VR.WeatherLike,VR.RiskSummary,VR.RiskTopestLevelId,VR.RiskTopestName,VR.RiskSecondLevelId,VR.RiskSecondLevelName,VR.RiskType
FROM ViewRiskResponseStaff RS
LEFT JOIN ViewRiskInfo VR ON RS.RiskId=VR.Id




GO
/****** Object:  Table [dbo].[LineStations]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LineStations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LineId] [int] NOT NULL,
	[StationId] [int] NOT NULL,
	[Sort] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_LINESTATIONS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ViewLine]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewLine]
AS
SELECT
L.Id,L.Name,LS.StationId,S.Name AS StationName,LS.Sort
FROM Line L
LEFT JOIN LineStations LS ON L.Id=LS.LineId
LEFT JOIN Station S ON LS.StationId=S.Id
GO
/****** Object:  View [dbo].[ViewRiskResponse]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewRiskResponse]
AS
SELECT
RS.Id,RS.ResponseStaffId,RS.RiskId,
RI.ReportStaffId,RI.OccurrenceLineId,RI.FirstStationId,RI.LastStationId,RI.LocoServiceTypeId,RI.WeatherId,RI.OccurrenceTime,RI.RiskSummaryId,RI.RiskDetails,RI.RiskReason,RI.RiskFix,RI.Visible,RI.RiskTypeId,RI.VerifyStaffId,RI.VerifyStatus,RI.VerifyTime,RI.ShowInStressPage,RI.DealTimeLimit,RI.HasDealed,RI.NeedRoomSign,RI.NeedLeaderSign,RI.NeedWriteFixDesc,RI.NeedStressTrack,RI.AddTime,RI.UpdateTime,RI.ReportStaffName,RI.ReportStaffDepartId,RI.ReportStaffDepart,RI.WorkId,RI.SalaryId,RI.OccurrenceLineName,RI.FirstStationName,RI.LastStationName,RI.LocoServiceType,RI.WeatherLike,RI.RiskSummary,RI.RiskTopestLevelId,RI.RiskTopestName,RI.RiskSecondLevelId,RI.RiskSecondLevelName,RI.RiskType,
VS.Name AS RiskRespondStaffName, VS.DepartmentId AS RiskRespondDepartId, VS.Department AS RiskRespondDepart,VS.SalaryId AS RiskRespondSalaryId, VS.WorkId AS RiskRespondWorkId
FROM RiskResponseStaff RS
LEFT JOIN ViewRiskInfo RI ON RS.RiskId=RI.Id
LEFT JOIN ViewStaff VS ON RS.ResponseStaffId=VS.Id
GO
/****** Object:  Table [dbo].[Accident]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accident](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OccurrenceTime] [datetime] NOT NULL,
	[Place] [nvarchar](50) NOT NULL,
	[ResponseBureau] [nvarchar](50) NOT NULL,
	[ResponseDepot] [nvarchar](50) NOT NULL,
	[AccidentType] [nvarchar](50) NOT NULL,
	[LocoType] [nvarchar](20) NOT NULL,
	[WeatherLike] [nvarchar](20) NOT NULL,
	[Keywords] [nvarchar](50) NOT NULL,
	[Summary] [nvarchar](max) NOT NULL,
	[Help] [nvarchar](max) NOT NULL,
	[Responsibility] [nvarchar](max) NOT NULL,
	[Lesson] [nvarchar](max) NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_ACCIDENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BasicFile]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BasicFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](50) NOT NULL,
	[FileNumber] [nvarchar](50) NOT NULL,
	[FilePath] [nvarchar](100) NOT NULL,
	[FileType] [int] NOT NULL,
	[PublishLevel] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_BASICFILE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExceptionLog]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[Source] [int] NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[ClassName] [varchar](200) NOT NULL,
	[MethodName] [varchar](200) NOT NULL,
	[Instance] [varchar](200) NOT NULL,
	[StackTrace] [nvarchar](max) NOT NULL,
	[HappenTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_EXCEPTIONLOG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Locomotive]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locomotive](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocoEngineTypeId] [int] NOT NULL,
	[LocoModelId] [int] NOT NULL,
	[LocoNumberId] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_LOCOMOTIVE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NOT NULL,
	[Path] [varchar](100) NOT NULL,
	[Visible] [bit] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_MENU] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TraceInfo]    Script Date: 2016/10/16 16:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TraceInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TraceType] [int] NOT NULL,
	[ResponseDepartmentIds] [varchar](100) NOT NULL,
	[TraceContent] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](50) NOT NULL,
	[FilePath] [nvarchar](200) NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_TRACEINFO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Respon__0E04126B]  DEFAULT ('') FOR [ResponseBureau]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Respon__0EF836A4]  DEFAULT ('') FOR [ResponseDepot]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Accide__0FEC5ADD]  DEFAULT ('') FOR [AccidentType]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__LocoTy__10E07F16]  DEFAULT ('') FOR [LocoType]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Weathe__11D4A34F]  DEFAULT ('') FOR [WeatherLike]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Keywor__12C8C788]  DEFAULT ('') FOR [Keywords]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Summar__13BCEBC1]  DEFAULT ('') FOR [Summary]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Help__14B10FFA]  DEFAULT ('') FOR [Help]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Respon__15A53433]  DEFAULT ('') FOR [Responsibility]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Lesson__1699586C]  DEFAULT ('') FOR [Lesson]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__Reason__178D7CA5]  DEFAULT ('') FOR [Reason]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__AddTim__1881A0DE]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Accident] ADD  CONSTRAINT [DF__Accident__IsDele__1975C517]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF__Article__AddTime__498EEC8D]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF__Article__IsDelet__4A8310C6]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[ArticleCategory] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[ArticleCategory] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[BasicFile] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Department] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Department] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Dictionaries] ADD  CONSTRAINT [DF_Dictionaries_ParentId]  DEFAULT ((0)) FOR [ParentId]
GO
ALTER TABLE [dbo].[Dictionaries] ADD  CONSTRAINT [DF__Dictionar__AddTi__57DD0BE4]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Dictionaries] ADD  CONSTRAINT [DF__Dictionar__IsDel__58D1301D]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[ExamScore] ADD  DEFAULT ((0)) FOR [StaffId]
GO
ALTER TABLE [dbo].[ExamScore] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[ExamScore] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[ExceptionLog] ADD  CONSTRAINT [DF__Exception__Happe__078C1F06]  DEFAULT (getdate()) FOR [HappenTime]
GO
ALTER TABLE [dbo].[ExceptionLog] ADD  CONSTRAINT [DF__Exception__IsDel__0880433F]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Line] ADD  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[Line] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[LineStations] ADD  CONSTRAINT [DF__LineStati__AddTi__6442E2C9]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[LineStations] ADD  CONSTRAINT [DF__LineStati__IsDel__65370702]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Locomotive] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Locomotive] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[LocoQuality28] ADD  CONSTRAINT [DF__LocoQuali__Repai__681373AD]  DEFAULT ('') FOR [RepairMethod]
GO
ALTER TABLE [dbo].[LocoQuality28] ADD  CONSTRAINT [DF_LocoQuality28_LiveItem]  DEFAULT ('') FOR [LiveItem]
GO
ALTER TABLE [dbo].[LocoQuality28] ADD  CONSTRAINT [DF__LocoQuali__AddTi__690797E6]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[LocoQuality28] ADD  CONSTRAINT [DF__LocoQuali__Updat__69FBBC1F]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[LocoQuality28] ADD  CONSTRAINT [DF__LocoQuali__IsDel__6AEFE058]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[LocoQuality6] ADD  CONSTRAINT [DF__LocoQuali__Broke__6DCC4D03]  DEFAULT ('') FOR [BrokenPlace]
GO
ALTER TABLE [dbo].[LocoQuality6] ADD  CONSTRAINT [DF__LocoQuali__Repai__6EC0713C]  DEFAULT ('') FOR [RepairMethod]
GO
ALTER TABLE [dbo].[LocoQuality6] ADD  CONSTRAINT [DF__LocoQuali__AddTi__6FB49575]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[LocoQuality6] ADD  CONSTRAINT [DF__LocoQuali__Updat__70A8B9AE]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[LocoQuality6] ADD  CONSTRAINT [DF__LocoQuali__IsDel__719CDDE7]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__RiskDe__7D0E9093]  DEFAULT ('') FOR [RiskDetails]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__RiskFi__7E02B4CC]  DEFAULT ('') FOR [RiskFix]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__Verify__7EF6D905]  DEFAULT ((1)) FOR [VerifyStatus]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__Verify__7FEAFD3E]  DEFAULT (getdate()) FOR [VerifyTime]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__HasDea__00DF2177]  DEFAULT ((0)) FOR [HasDealed]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__NeedRo__01D345B0]  DEFAULT ((0)) FOR [NeedRoomSign]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__NeedLe__02C769E9]  DEFAULT ((0)) FOR [NeedLeaderSign]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__NeedWr__03BB8E22]  DEFAULT ((0)) FOR [NeedWriteFixDesc]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__NeedSt__04AFB25B]  DEFAULT ((0)) FOR [NeedStressTrack]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__AddTim__05A3D694]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__Update__0697FACD]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[RiskInfo] ADD  CONSTRAINT [DF__RiskInfo__IsDele__078C1F06]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[RiskResponseStaff] ADD  CONSTRAINT [DF__RiskRespo__AddTi__0A688BB1]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RiskResponseStaff] ADD  CONSTRAINT [DF__RiskRespo__IsDel__0B5CAFEA]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[RiskSummary] ADD  CONSTRAINT [DF_RiskSummary_TopestName]  DEFAULT ('') FOR [TopestName]
GO
ALTER TABLE [dbo].[RiskSummary] ADD  CONSTRAINT [DF_RiskSummary_SecondLevelId]  DEFAULT ((0)) FOR [SecondLevelId]
GO
ALTER TABLE [dbo].[RiskSummary] ADD  CONSTRAINT [DF_RiskSummary_SecondLevelName]  DEFAULT ('') FOR [SecondLevelName]
GO
ALTER TABLE [dbo].[RiskSummary] ADD  CONSTRAINT [DF_RiskSummary_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RiskSummary] ADD  CONSTRAINT [DF__RiskSumma__Updat__0E391C95]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[RiskSummary] ADD  CONSTRAINT [DF__RiskSumma__IsDel__0F2D40CE]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[RiskType] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RiskType] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF__Staff__Password__15DA3E5D]  DEFAULT ('') FOR [Password]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF__Staff__UpdateTim__16CE6296]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF__Staff__IsDelete__17C286CF]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[StaffScore] ADD  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[StaffScore] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Station] ADD  CONSTRAINT [DF_Station_Spell]  DEFAULT ('') FOR [Spell]
GO
ALTER TABLE [dbo].[Station] ADD  CONSTRAINT [DF_Station_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Station] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[TraceInfo] ADD  CONSTRAINT [DF_TraceInfo_TraceType]  DEFAULT ((2)) FOR [TraceType]
GO
ALTER TABLE [dbo].[TraceInfo] ADD  CONSTRAINT [DF_TraceInfo_FileName]  DEFAULT ('') FOR [FileName]
GO
ALTER TABLE [dbo].[TraceInfo] ADD  CONSTRAINT [DF__TraceInfo__FileP__1D7B6025]  DEFAULT ('') FOR [FilePath]
GO
ALTER TABLE [dbo].[TraceInfo] ADD  CONSTRAINT [DF__TraceInfo__AddTi__1E6F845E]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[TraceInfo] ADD  CONSTRAINT [DF__TraceInfo__Updat__1F63A897]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[TraceInfo] ADD  CONSTRAINT [DF__TraceInfo__IsDel__2057CCD0]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Warning] ADD  CONSTRAINT [DF_Warning_HasBeganImplement]  DEFAULT ((0)) FOR [HasBeganImplement]
GO
ALTER TABLE [dbo].[Warning] ADD  CONSTRAINT [DF__Warning__HasImpl__1758727B]  DEFAULT ((0)) FOR [HasImplementedAll]
GO
ALTER TABLE [dbo].[Warning] ADD  CONSTRAINT [DF__Warning__Visible__184C96B4]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[Warning] ADD  CONSTRAINT [DF__Warning__AddTime__1940BAED]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[Warning] ADD  CONSTRAINT [DF__Warning__UpdateT__1A34DF26]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[Warning] ADD  CONSTRAINT [DF__Warning__IsDelet__1B29035F]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__HasIm__29E1370A]  DEFAULT ((0)) FOR [HasImplemented]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__HasRe__2AD55B43]  DEFAULT ((0)) FOR [HasResponded]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__Imple__2BC97F7C]  DEFAULT ('') FOR [ImplementDetail]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__Respo__2CBDA3B5]  DEFAULT ((1)) FOR [ResponseVerifyStatus]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__AddTi__2DB1C7EE]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__Updat__2EA5EC27]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[WarningImplement] ADD  CONSTRAINT [DF__WarningIm__IsDel__2F9A1060]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_ARTICLE_REFERENCE_ARTICLEC] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ArticleCategory] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_ARTICLE_REFERENCE_ARTICLEC]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_ARTICLE_REFERENCE_STAFF] FOREIGN KEY([CreateStaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_ARTICLE_REFERENCE_STAFF]
GO
ALTER TABLE [dbo].[LineStations]  WITH NOCHECK ADD  CONSTRAINT [FK_LINESTAT_REFERENCE_LINE] FOREIGN KEY([LineId])
REFERENCES [dbo].[Line] ([Id])
GO
ALTER TABLE [dbo].[LineStations] CHECK CONSTRAINT [FK_LINESTAT_REFERENCE_LINE]
GO
ALTER TABLE [dbo].[LineStations]  WITH NOCHECK ADD  CONSTRAINT [FK_LINESTAT_REFERENCE_STATION] FOREIGN KEY([StationId])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[LineStations] CHECK CONSTRAINT [FK_LINESTAT_REFERENCE_STATION]
GO
ALTER TABLE [dbo].[RiskResponseStaff]  WITH NOCHECK ADD  CONSTRAINT [FK_RISKRESP_REFERENCE_STAFF] FOREIGN KEY([ResponseStaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[RiskResponseStaff] CHECK CONSTRAINT [FK_RISKRESP_REFERENCE_STAFF]
GO
ALTER TABLE [dbo].[Staff]  WITH NOCHECK ADD  CONSTRAINT [FK_STAFF_DEPART_DEPARTME] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_STAFF_DEPART_DEPARTME]
GO
ALTER TABLE [dbo].[Staff]  WITH NOCHECK ADD  CONSTRAINT [FK_STAFF_POLITICAL_DICTIONA] FOREIGN KEY([PoliticalStatusId])
REFERENCES [dbo].[Dictionaries] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_STAFF_POLITICAL_DICTIONA]
GO
ALTER TABLE [dbo].[Staff]  WITH NOCHECK ADD  CONSTRAINT [FK_STAFF_POSITION_DICTIONA] FOREIGN KEY([PositionId])
REFERENCES [dbo].[Dictionaries] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_STAFF_POSITION_DICTIONA]
GO
ALTER TABLE [dbo].[Staff]  WITH NOCHECK ADD  CONSTRAINT [FK_STAFF_WORKFLAG_DICTIONA] FOREIGN KEY([WorkFlagId])
REFERENCES [dbo].[Dictionaries] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_STAFF_WORKFLAG_DICTIONA]
GO
ALTER TABLE [dbo].[Staff]  WITH NOCHECK ADD  CONSTRAINT [FK_STAFF_WORKTYPE_DICTIONA] FOREIGN KEY([WorkTypeId])
REFERENCES [dbo].[Dictionaries] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_STAFF_WORKTYPE_DICTIONA]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事故发生时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'OccurrenceTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事故发生地点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Place'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'责任铁路局' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'ResponseBureau'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'责任机务段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'ResponseDepot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事故类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'AccidentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列车分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'LocoType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天气情况' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'WeatherLike'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关键词' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Keywords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事故概述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Summary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'补救措施' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Help'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'责任追究' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Responsibility'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'吸取到的教训' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Lesson'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事故原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'Reason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'典型事故' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Accident'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章分类Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'CategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'CreateStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleCategory', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleCategory', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleCategory', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleCategory', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleCategory', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件编号（如铁运[2012]111号）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'FileNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件在服务器存储的相对地址，以/开头' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件类型（1 技术规章； 2 企业标准； 3 制度措施）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'FileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件发布等级（1 总公司； 2 铁路局； 3 机务段；）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'PublishLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基础文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BasicFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级单位Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'ParentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级的上级单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'GrandParentName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'铁路部门信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'各种类别字典（如职务、政治面貌等）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dictionaries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人员Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'StaffName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'WorkNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'Position'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'WorkPlace'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'ExamTheme'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试科目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'ExamSubject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成绩' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'Score'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'ExamTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试成绩' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExamScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'Message'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常来源（1 后台；2 安卓app）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常发生所在文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常发生所在类名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'ClassName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常发生所在方法名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'MethodName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引发异常的对象' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'Instance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常堆栈信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'StackTrace'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'HappenTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'程序异常日志表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExceptionLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线路名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起始站名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'FirstStation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'终点站名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'LastStation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线路信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Line'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线路Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations', @level2type=N'COLUMN',@level2name=N'LineId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车站Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations', @level2type=N'COLUMN',@level2name=N'StationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前线路经过此车站的顺序，值越小越先经过' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线路车站关联表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineStations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车类型（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive', @level2type=N'COLUMN',@level2name=N'LocoEngineTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车车型Id（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive', @level2type=N'COLUMN',@level2name=N'LocoModelId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车车号Id（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive', @level2type=N'COLUMN',@level2name=N'LocoNumberId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locomotive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车信息Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'LocomotiveId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预报时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'ReportTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施修班组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'RepairTeam'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修理方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'RepairMethod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修程（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'RepairProcessId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活项（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'LivingItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'LiveItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车质量登记-机统28' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality28'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车信息Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'LocomotiveId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预报时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'ReportTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施修人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'RepairStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活项归属（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'LivingItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'破损处所' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'BrokenPlace'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修理方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'RepairMethod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施修开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'StartRepair'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施修结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'EndRepair'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'施修情况' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'RepairDesc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机车质量登登记-机统6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LocoQuality6'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Visible'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提报人Id（关联人员信息表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'ReportStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件发生线路Id（关联线路表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'OccurrenceLineId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件发生区间起点站Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'FirstStationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件发生区间终点站Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'LastStationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列车类别（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'LocoServiceTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天气情况（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'WeatherId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件发生时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'OccurrenceTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险分类Id（关联风险分类表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'RiskSummaryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险详情' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'RiskDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'问题原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'RiskReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'落实情况' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'RiskFix'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对外是否可见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'Visible'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险分类Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'RiskTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态（1 待本部门审核； 2 待风险办审核；3 风险办否决，待部门重新审核；  4 审核通过；5 已填写整改处置； 6 已填写落实销号）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'VerifyStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'VerifyStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'VerifyTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否进入日分析重点甄选队列，或为真，则此风险信息将在每日重点安全信息甄选统计页面中展示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'ShowInStressPage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处置期限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'DealTimeLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已处置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'HasDealed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要科室签字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'NeedRoomSign'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要领导签字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'NeedLeaderSign'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要填写落实情况' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'NeedWriteFixDesc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否重点追踪' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'NeedStressTrack'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险信息登记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskResponseStaff', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险信息Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskResponseStaff', @level2type=N'COLUMN',@level2name=N'RiskId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'责任人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskResponseStaff', @level2type=N'COLUMN',@level2name=N'ResponseStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskResponseStaff', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskResponseStaff', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险信息责任人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskResponseStaff'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类概述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否最底层，若为真，则表示此项为风险概述内容，否则它表示一个风险概述的分类（如红线、甲、乙等）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'IsBottom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顶级分类Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'TopestTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顶级分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'TopestName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'二级分类Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'SecondLevelId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'二级分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'SecondLevelName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskSummary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskType', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskType', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskType', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskType', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'风险信息分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RiskType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'WorkId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工资号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'SalaryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Identity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Gender'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'参加工作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'HireDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'BirthDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'干部工人标识（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'WorkFlagId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工种（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'WorkTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'政治面貌（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'PoliticalStatusId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职务（关联字典表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'PositionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在单位Id（关联铁路单位表主键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统登录密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人员信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'Year'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'Month'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'MinusScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工扣分表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'StaffScore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车站名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拼音缩写' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'Spell'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车站信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Station'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'追踪类型（1 局追； 2 段追；）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'TraceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'责任部门Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'ResponseDepartmentIds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'追踪内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'TraceContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件文件名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件文件路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信息追踪' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TraceInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'WarningTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'落实时限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'TimeLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'WarningSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警部门Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'WarningStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'落实部门Id（多个部门Id之间用逗号隔开）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'ImplementDeparts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'WarningTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'WarningContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'整改要求' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'ChangeRequirement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已经开始响应' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'HasBeganImplement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否全部响应' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'HasImplementedAll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否对外可见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'Visible'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'CreateStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Warning'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警信息Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'WarningId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'落实部门Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'ImplementDepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已落实' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'HasImplemented'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已响应' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'HasResponded'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警响应' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'ImplementDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'响应时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'RespondTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'响应审核状态（1 未审核； 2 审核通过； 3 响应被否决）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'ResponseVerifyStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'VerifyStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'VerifyTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预警落实信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WarningImplement'
GO
USE [master]
GO
ALTER DATABASE [Jnl] SET  READ_WRITE 
GO
