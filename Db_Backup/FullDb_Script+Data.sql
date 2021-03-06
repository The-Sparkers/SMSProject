USE [master]
GO
/****** Object:  Database [SMSDatabase]    Script Date: 3/28/2019 1:40:50 PM ******/
CREATE DATABASE [SMSDatabase]
GO
ALTER DATABASE [SMSDatabase] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SMSDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SMSDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SMSDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SMSDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SMSDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SMSDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [SMSDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SMSDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SMSDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SMSDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SMSDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SMSDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SMSDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SMSDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SMSDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SMSDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SMSDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SMSDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SMSDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SMSDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SMSDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SMSDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SMSDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SMSDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SMSDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [SMSDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SMSDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SMSDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SMSDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SMSDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SMSDatabase] SET QUERY_STORE = OFF
GO
USE [SMSDatabase]
GO
/****** Object:  Table [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE](
	[FundId] [int] NOT NULL,
	[SFeeId] [bigint] NOT NULL,
 CONSTRAINT [PK_ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] PRIMARY KEY CLUSTERED 
(
	[FundId] ASC,
	[SFeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ADDITIONAL_FUNDS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ADDITIONAL_FUNDS](
	[FundId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_ADDITIONAL_FUNDS] PRIMARY KEY CLUSTERED 
(
	[FundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APPLICATIONS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APPLICATIONS](
	[ApplicationId] [bigint] IDENTITY(1,1) NOT NULL,
	[Body] [varchar](max) NOT NULL,
	[Date] [date] NOT NULL,
	[Status] [smallint] NOT NULL,
	[ParentId] [int] NOT NULL,
 CONSTRAINT [PK_APPLICATIONS] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ATTANDANCES]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATTANDANCES](
	[AttandanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[IsAbsent] [bit] NOT NULL,
 CONSTRAINT [PK_ATTANDANCES] PRIMARY KEY CLUSTERED 
(
	[AttandanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLASSES]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLASSES](
	[ClassId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](15) NOT NULL,
	[RollNoIndex] [int] NOT NULL,
	[InchargeId] [int] NOT NULL,
 CONSTRAINT [PK_CLASSES] PRIMARY KEY CLUSTERED 
(
	[ClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DAILY_DAIRIES]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DAILY_DAIRIES](
	[DairyId] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[Content] [varchar](max) NOT NULL,
	[SubjectId] [int] NOT NULL,
 CONSTRAINT [PK_DAILY_DAIRIES] PRIMARY KEY CLUSTERED 
(
	[DairyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DOWNLOADABLES]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOWNLOADABLES](
	[DownloadableId] [bigint] IDENTITY(1,1) NOT NULL,
	[Filename] [varchar](max) NOT NULL,
	[FileExtention] [nchar](10) NOT NULL,
	[DateUpload] [date] NOT NULL,
	[ClassId] [int] NOT NULL,
	[DownloadableName] [varchar](max) NOT NULL,
 CONSTRAINT [PK_DOWNLOADABLES] PRIMARY KEY CLUSTERED 
(
	[DownloadableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EXAMINATIONS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EXAMINATIONS](
	[ExamId] [int] IDENTITY(1,1) NOT NULL,
	[ExamName] [varchar](max) NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[IsPublished] [bit] NOT NULL,
 CONSTRAINT [PK_EXAMINATIONS] PRIMARY KEY CLUSTERED 
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[INVENTORY_CATEGORIES]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[INVENTORY_CATEGORIES](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_INVENTORY_CATEGORIES] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[INVENTORY_ITEMS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[INVENTORY_ITEMS](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[Price] [money] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_INVENTORY_ITEMS] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MONTHLY_SALARIES]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MONTHLY_SALARIES](
	[MSId] [bigint] IDENTITY(1,1) NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[PerAbsentDeduction] [money] NOT NULL,
	[Salary] [money] NOT NULL,
 CONSTRAINT [PK_MONTHLY_SALARIES] PRIMARY KEY CLUSTERED 
(
	[MSId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NON-TEACHING_STAFF]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NON-TEACHING_STAFF](
	[StaffId] [int] NOT NULL,
	[JobType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_NON-TEACHING_STAFF] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NOTIFICATIONS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NOTIFICATIONS](
	[NotificationId] [bigint] IDENTITY(1,1) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Date] [date] NOT NULL,
	[IsSMS] [bit] NOT NULL,
	[IsWeb] [bit] NOT NULL,
	[ParentFlag] [bit] NOT NULL,
	[TeacherFlag] [bit] NOT NULL,
 CONSTRAINT [PK_NOTIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARENT_HAS_ACCOUNT]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARENT_HAS_ACCOUNT](
	[ParentId] [int] NOT NULL,
	[AccountId] [nvarchar](128) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_PARENT_HAS_ACCOUNT] PRIMARY KEY CLUSTERED 
(
	[ParentId] ASC,
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARENT_MONTHLY_FEE]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARENT_MONTHLY_FEE](
	[PFeeId] [bigint] IDENTITY(1,1) NOT NULL,
	[DatePaid] [date] NOT NULL,
	[AmountPaid] [money] NOT NULL,
	[Concession] [money] NOT NULL,
	[ParentId] [int] NOT NULL,
	[TotalAmountDue] [money] NOT NULL,
 CONSTRAINT [PK_PARENT_MONTHLY_FEE] PRIMARY KEY CLUSTERED 
(
	[PFeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARENT_RECEIVES_NOTIFICATIONS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARENT_RECEIVES_NOTIFICATIONS](
	[ParentId] [int] NOT NULL,
	[NotificationId] [bigint] NOT NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_PARENT_RECEIVES_NOTIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[ParentId] ASC,
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARENTS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARENTS](
	[ParentId] [int] IDENTITY(1,1) NOT NULL,
	[FatherName] [varchar](max) NOT NULL,
	[MotherName] [varchar](max) NOT NULL,
	[FCNIC] [nchar](15) NOT NULL,
	[HomeAddress] [varchar](max) NOT NULL,
	[FMCountryCode] [nchar](3) NOT NULL,
	[FMCompanyCode] [nchar](3) NOT NULL,
	[FMNumber] [nchar](7) NOT NULL,
	[MMCountryCode] [nchar](3) NOT NULL,
	[MMCompanyCode] [nchar](3) NOT NULL,
	[MMNumber] [nchar](7) NOT NULL,
	[EmergencyContact] [nchar](15) NOT NULL,
	[EligibiltyThreshold] [int] NOT NULL,
 CONSTRAINT [PK_PARENTS] PRIMARY KEY CLUSTERED 
(
	[ParentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SECTIONS]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SECTIONS](
	[SectionId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](5) NOT NULL,
	[ClassId] [int] NOT NULL,
 CONSTRAINT [PK_SECTIONS] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STAFF]    Script Date: 3/28/2019 1:40:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STAFF](
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[CNIC] [nchar](15) NOT NULL,
	[Address] [varchar](max) NOT NULL,
	[MCountryCode] [nchar](3) NOT NULL,
	[MCompanyCode] [nchar](3) NOT NULL,
	[MNumber] [nchar](7) NOT NULL,
	[Salary] [money] NOT NULL,
	[Gender] [bit] NOT NULL,
	[JoiningDate] [date] NOT NULL,
 CONSTRAINT [PK_STAFF] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STAFF_GIVES_DAILY_ATTANDANCE]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STAFF_GIVES_DAILY_ATTANDANCE](
	[StaffId] [int] NOT NULL,
	[AttandanceId] [bigint] NOT NULL,
 CONSTRAINT [PK_STAFF_GIVES_DAILY_ATTANDANCE] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC,
	[AttandanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STRUCK-OFF_STUDENTS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STRUCK-OFF_STUDENTS](
	[SStudentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[FatherName] [varchar](max) NOT NULL,
	[FatherCellNo] [nchar](15) NOT NULL,
	[BoardExamNo] [varchar](50) NULL,
	[LastClassName] [varchar](50) NOT NULL,
	[DateOfStruck] [date] NOT NULL,
	[BFormNo] [nchar](15) NULL,
	[Gender] [bit] NOT NULL,
 CONSTRAINT [PK_STRUCK-OFF_STUDENTS] PRIMARY KEY CLUSTERED 
(
	[SStudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STUDENT_GET_TEST_RESULTS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENT_GET_TEST_RESULTS](
	[StudentId] [int] NOT NULL,
	[TestId] [bigint] NOT NULL,
	[ObtainedMarks] [decimal](5, 2) NOT NULL,
	[TeacherRemarks] [varchar](max) NOT NULL,
 CONSTRAINT [PK_STUDENT_GET_TEST_RESULTS] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE](
	[StudentId] [int] NOT NULL,
	[AttandanceId] [bigint] NOT NULL,
 CONSTRAINT [PK_STUDENT_GIVES_DAILY_ATTANDANCE] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[AttandanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STUDENT_MONTHLY_FEE]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENT_MONTHLY_FEE](
	[SFeeId] [bigint] IDENTITY(1,1) NOT NULL,
	[Fine] [money] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
 CONSTRAINT [PK_STUDENT_MONTHLY_FEE] PRIMARY KEY CLUSTERED 
(
	[SFeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STUDENT_PURCHASES_ITEMS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENT_PURCHASES_ITEMS](
	[StudentId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[PurchasedPrice] [money] NOT NULL,
 CONSTRAINT [PK_STUDENT_PURCHASES_ITEMS] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[ItemId] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STUDENTS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENTS](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[BFormNumber] [nchar](15) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[AdmissionNo] [varchar](50) NOT NULL,
	[DOB] [date] NOT NULL,
	[DOA] [date] NOT NULL,
	[PrevInstitute] [varchar](max) NULL,
	[MonthlyFee] [money] NOT NULL,
	[ParentId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[RollNo] [int] NOT NULL,
	[Gender] [bit] NOT NULL,
 CONSTRAINT [PK_STUDENTS] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STUDENTS_GET_EXAM_RESULTS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS](
	[StudentId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
	[ExamId] [int] NOT NULL,
	[ObtainedMarks] [decimal](5, 2) NOT NULL,
	[TotalMarks] [decimal](5, 2) NOT NULL,
	[TeacherRemarks] [varchar](max) NOT NULL,
 CONSTRAINT [PK_STUDENTS_GET_EXAM_RESULTS] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[SubjectId] ASC,
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUBJECTS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUBJECTS](
	[SubjectId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ClassId] [int] NOT NULL,
 CONSTRAINT [PK_SUBJECTS] PRIMARY KEY CLUSTERED 
(
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEACHER_HAS_ACCOUNT]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEACHER_HAS_ACCOUNT](
	[StaffId] [int] NOT NULL,
	[AccountId] [nvarchar](128) NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_TEACHER_HAS_ACCOUNT] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC,
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEACHER_RECEIVES_NOTIFICATIONS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEACHER_RECEIVES_NOTIFICATIONS](
	[StaffId] [int] NOT NULL,
	[NotificationId] [bigint] NOT NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_TEACHER_RECEIVES_NOTIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC,
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION](
	[StaffId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
 CONSTRAINT [PK_TEACHER_TEACHES_SUBJECT_OF_A_SECTION] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC,
	[SubjectId] ASC,
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEACHERS_QUALIFICATIONS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEACHERS_QUALIFICATIONS](
	[QId] [int] IDENTITY(1,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[Degree] [varchar](50) NOT NULL,
	[Year] [smallint] NOT NULL,
 CONSTRAINT [PK_TEACHERS_QUALIFICATIONS] PRIMARY KEY CLUSTERED 
(
	[QId] ASC,
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEACHING_STAFF]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEACHING_STAFF](
	[StaffId] [int] NOT NULL,
 CONSTRAINT [PK_TEACHING_STAFF] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TESTS]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TESTS](
	[TestId] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalMarks] [decimal](5, 2) NOT NULL,
	[HeldDate] [date] NOT NULL,
	[SubjectId] [int] NOT NULL,
 CONSTRAINT [PK_TESTS] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] ([FundId], [SFeeId]) VALUES (1, 2)
INSERT [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] ([FundId], [SFeeId]) VALUES (1, 3)
INSERT [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] ([FundId], [SFeeId]) VALUES (2, 3)
SET IDENTITY_INSERT [dbo].[ADDITIONAL_FUNDS] ON 

INSERT [dbo].[ADDITIONAL_FUNDS] ([FundId], [Name], [Amount]) VALUES (1, N'Annual Fund 2018', 1600.0000)
INSERT [dbo].[ADDITIONAL_FUNDS] ([FundId], [Name], [Amount]) VALUES (2, N'Exam Fee', 500.0000)
SET IDENTITY_INSERT [dbo].[ADDITIONAL_FUNDS] OFF
SET IDENTITY_INSERT [dbo].[APPLICATIONS] ON 

INSERT [dbo].[APPLICATIONS] ([ApplicationId], [Body], [Date], [Status], [ParentId]) VALUES (1, N'First Applcation', CAST(N'2018-08-18' AS Date), 1, 6)
INSERT [dbo].[APPLICATIONS] ([ApplicationId], [Body], [Date], [Status], [ParentId]) VALUES (2, N'Second Applcation', CAST(N'2018-08-18' AS Date), 2, 6)
INSERT [dbo].[APPLICATIONS] ([ApplicationId], [Body], [Date], [Status], [ParentId]) VALUES (3, N'Second Applcation', CAST(N'2018-08-18' AS Date), 0, 6)
SET IDENTITY_INSERT [dbo].[APPLICATIONS] OFF
SET IDENTITY_INSERT [dbo].[ATTANDANCES] ON 

INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (1, CAST(N'2018-08-17' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (2, CAST(N'2018-08-17' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (3, CAST(N'2018-08-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (4, CAST(N'2018-08-17' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (5, CAST(N'2018-08-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (6, CAST(N'2018-08-17' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (7, CAST(N'2018-08-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (8, CAST(N'2018-08-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (9, CAST(N'2018-08-14' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10002, CAST(N'2018-08-20' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10003, CAST(N'2018-09-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10004, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10005, CAST(N'2018-09-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10006, CAST(N'2018-09-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10007, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10008, CAST(N'2018-09-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10009, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10010, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10011, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10012, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10013, CAST(N'2018-09-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10014, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10015, CAST(N'2018-09-15' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (10016, CAST(N'2018-09-15' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20003, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20004, CAST(N'2018-09-16' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20005, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20006, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20007, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20008, CAST(N'2018-09-16' AS Date), 1)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20009, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20010, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20011, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20012, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20013, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20014, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20015, CAST(N'2018-09-16' AS Date), 0)
INSERT [dbo].[ATTANDANCES] ([AttandanceId], [Date], [IsAbsent]) VALUES (20016, CAST(N'2018-09-16' AS Date), 0)
SET IDENTITY_INSERT [dbo].[ATTANDANCES] OFF
SET IDENTITY_INSERT [dbo].[CLASSES] ON 

INSERT [dbo].[CLASSES] ([ClassId], [Name], [RollNoIndex], [InchargeId]) VALUES (2, N'One            ', 100, 2013)
INSERT [dbo].[CLASSES] ([ClassId], [Name], [RollNoIndex], [InchargeId]) VALUES (3, N'One            ', 100, 3)
INSERT [dbo].[CLASSES] ([ClassId], [Name], [RollNoIndex], [InchargeId]) VALUES (1002, N'two            ', 500, 2013)
SET IDENTITY_INSERT [dbo].[CLASSES] OFF
SET IDENTITY_INSERT [dbo].[DAILY_DAIRIES] ON 

INSERT [dbo].[DAILY_DAIRIES] ([DairyId], [Date], [Content], [SubjectId]) VALUES (10003, CAST(N'2018-08-20' AS Date), N'New Dairy', 1)
SET IDENTITY_INSERT [dbo].[DAILY_DAIRIES] OFF
SET IDENTITY_INSERT [dbo].[INVENTORY_CATEGORIES] ON 

INSERT [dbo].[INVENTORY_CATEGORIES] ([CategoryId], [Name]) VALUES (1, N'Books')
SET IDENTITY_INSERT [dbo].[INVENTORY_CATEGORIES] OFF
SET IDENTITY_INSERT [dbo].[INVENTORY_ITEMS] ON 

INSERT [dbo].[INVENTORY_ITEMS] ([ItemId], [Quantity], [Name], [Price], [CategoryId]) VALUES (1, 40, N'testItem', 200.0000, 1)
SET IDENTITY_INSERT [dbo].[INVENTORY_ITEMS] OFF
SET IDENTITY_INSERT [dbo].[MONTHLY_SALARIES] ON 

INSERT [dbo].[MONTHLY_SALARIES] ([MSId], [Month], [Year], [StaffId], [PerAbsentDeduction], [Salary]) VALUES (1, 8, 2019, 1004, 10.0000, 8000.0000)
INSERT [dbo].[MONTHLY_SALARIES] ([MSId], [Month], [Year], [StaffId], [PerAbsentDeduction], [Salary]) VALUES (2, 8, 2018, 1004, 10.0000, 8000.0000)
INSERT [dbo].[MONTHLY_SALARIES] ([MSId], [Month], [Year], [StaffId], [PerAbsentDeduction], [Salary]) VALUES (3, 8, 2018, 1002, 200.0000, 15000.0000)
INSERT [dbo].[MONTHLY_SALARIES] ([MSId], [Month], [Year], [StaffId], [PerAbsentDeduction], [Salary]) VALUES (10002, 2, 2017, 2013, 100.0000, 0.0000)
INSERT [dbo].[MONTHLY_SALARIES] ([MSId], [Month], [Year], [StaffId], [PerAbsentDeduction], [Salary]) VALUES (20002, 8, 2018, 2013, 100.0000, 0.0000)
SET IDENTITY_INSERT [dbo].[MONTHLY_SALARIES] OFF
INSERT [dbo].[NON-TEACHING_STAFF] ([StaffId], [JobType]) VALUES (1004, N'Maid')
INSERT [dbo].[NON-TEACHING_STAFF] ([StaffId], [JobType]) VALUES (2012, N'GATEKEEPER')
SET IDENTITY_INSERT [dbo].[NOTIFICATIONS] ON 

INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (2, N'Hello!', CAST(N'2018-08-16' AS Date), 0, 1, 1, 1)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (3, N'Hello!', CAST(N'2018-08-16' AS Date), 0, 1, 1, 1)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (4, N'Hello!', CAST(N'2018-08-16' AS Date), 0, 1, 1, 1)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10003, N'SMS Notification', CAST(N'2018-08-18' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10004, N'SMS Notification', CAST(N'2018-08-18' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10005, N'SMS Notification', CAST(N'2018-08-18' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10006, N'Hello!', CAST(N'2018-08-18' AS Date), 0, 1, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10008, N'Dear Parent, Your have Rs. 2410 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10009, N'Dear Parent, Your have Rs. 2410 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10010, N'Your have Rs. 2410 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10011, N'Your have Rs. 4000 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10012, N'Your have Rs. 0 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10013, N'Your have Rs. 4633 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10014, N'Your have Rs. 2410 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10015, N'Your have Rs. 4633 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (10016, N'Your have Rs. 4000 un-paid at Demo. Please submit all your dues as soon as possible.', CAST(N'2018-09-06' AS Date), 1, 0, 1, 0)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (20008, N'AABCSSaas', CAST(N'2018-09-24' AS Date), 0, 1, 0, 1)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (20009, N'AABCSSaas', CAST(N'2018-09-24' AS Date), 0, 1, 0, 1)
INSERT [dbo].[NOTIFICATIONS] ([NotificationId], [Message], [Date], [IsSMS], [IsWeb], [ParentFlag], [TeacherFlag]) VALUES (20010, N'hello', CAST(N'2018-09-24' AS Date), 0, 1, 1, 1)
SET IDENTITY_INSERT [dbo].[NOTIFICATIONS] OFF
SET IDENTITY_INSERT [dbo].[PARENT_MONTHLY_FEE] ON 

INSERT [dbo].[PARENT_MONTHLY_FEE] ([PFeeId], [DatePaid], [AmountPaid], [Concession], [ParentId], [TotalAmountDue]) VALUES (2, CAST(N'2018-08-17' AS Date), 2000.0000, 0.0000, 6, 2280.0000)
INSERT [dbo].[PARENT_MONTHLY_FEE] ([PFeeId], [DatePaid], [AmountPaid], [Concession], [ParentId], [TotalAmountDue]) VALUES (3, CAST(N'2018-08-17' AS Date), 80.0000, 0.0000, 6, 280.0000)
INSERT [dbo].[PARENT_MONTHLY_FEE] ([PFeeId], [DatePaid], [AmountPaid], [Concession], [ParentId], [TotalAmountDue]) VALUES (10002, CAST(N'2018-08-18' AS Date), 500.0000, 100.0000, 6, 200.0000)
SET IDENTITY_INSERT [dbo].[PARENT_MONTHLY_FEE] OFF
INSERT [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ([ParentId], [NotificationId], [IsRead]) VALUES (2, 20010, 0)
INSERT [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ([ParentId], [NotificationId], [IsRead]) VALUES (6, 2, 1)
INSERT [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ([ParentId], [NotificationId], [IsRead]) VALUES (6, 10006, 0)
INSERT [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ([ParentId], [NotificationId], [IsRead]) VALUES (6, 20010, 0)
INSERT [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ([ParentId], [NotificationId], [IsRead]) VALUES (1002, 20010, 0)
INSERT [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ([ParentId], [NotificationId], [IsRead]) VALUES (1003, 20010, 0)
SET IDENTITY_INSERT [dbo].[PARENTS] ON 

INSERT [dbo].[PARENTS] ([ParentId], [FatherName], [MotherName], [FCNIC], [HomeAddress], [FMCountryCode], [FMCompanyCode], [FMNumber], [MMCountryCode], [MMCompanyCode], [MMNumber], [EmergencyContact], [EligibiltyThreshold]) VALUES (2, N'TestFather', N'TestMother', N'35202-8448219-9', N'98 Dilkusha park rajgarh lahore', N'+92', N'307', N'4172327', N'+92', N'307', N'4179629', N'03324313586    ', 20)
INSERT [dbo].[PARENTS] ([ParentId], [FatherName], [MotherName], [FCNIC], [HomeAddress], [FMCountryCode], [FMCompanyCode], [FMNumber], [MMCountryCode], [MMCompanyCode], [MMNumber], [EmergencyContact], [EligibiltyThreshold]) VALUES (6, N'Afzal Ali', N'Sadia Afzal', N'35101-1234356-8', N'Street No: 12, House no: 91 Ali Town, Lahore', N'+92', N'345', N'5674321', N'+92', N'308', N'4561230', N'+923224313578  ', 50)
INSERT [dbo].[PARENTS] ([ParentId], [FatherName], [MotherName], [FCNIC], [HomeAddress], [FMCountryCode], [FMCompanyCode], [FMNumber], [MMCountryCode], [MMCompanyCode], [MMNumber], [EmergencyContact], [EligibiltyThreshold]) VALUES (1002, N'Muhammad Aslam Iqbal', N'Farzana Iqbal', N'35202-8114327-0', N'109-Dehli Gate Lahore', N'+92', N'321', N'4313589', N'+92', N'308', N'2315643', N'04237156295    ', 40)
INSERT [dbo].[PARENTS] ([ParentId], [FatherName], [MotherName], [FCNIC], [HomeAddress], [FMCountryCode], [FMCompanyCode], [FMNumber], [MMCountryCode], [MMCompanyCode], [MMNumber], [EmergencyContact], [EligibiltyThreshold]) VALUES (1003, N'Muhammad Shahab', N'Rida Latif', N'35202-1324556-8', N'103 loyal street Lahore', N'+92', N'345', N'4394850', N'+92', N'345', N'4394850', N'04237156295    ', 40)
SET IDENTITY_INSERT [dbo].[PARENTS] OFF
SET IDENTITY_INSERT [dbo].[SECTIONS] ON 

INSERT [dbo].[SECTIONS] ([SectionId], [Name], [ClassId]) VALUES (1, N'A    ', 2)
INSERT [dbo].[SECTIONS] ([SectionId], [Name], [ClassId]) VALUES (4003, N'A    ', 3)
INSERT [dbo].[SECTIONS] ([SectionId], [Name], [ClassId]) VALUES (4, N'B    ', 2)
INSERT [dbo].[SECTIONS] ([SectionId], [Name], [ClassId]) VALUES (1005, N'C    ', 2)
SET IDENTITY_INSERT [dbo].[SECTIONS] OFF
SET IDENTITY_INSERT [dbo].[STAFF] ON 

INSERT [dbo].[STAFF] ([StaffId], [Name], [CNIC], [Address], [MCountryCode], [MCompanyCode], [MNumber], [Salary], [Gender], [JoiningDate]) VALUES (3, N'Fareen Anwar', N'35202-4538762-9', N'180-A Edenabad Khayaban-e-Amin Lahore', N'+92', N'320', N'3452195', 9000.0000, 0, CAST(N'2018-07-01' AS Date))
INSERT [dbo].[STAFF] ([StaffId], [Name], [CNIC], [Address], [MCountryCode], [MCompanyCode], [MNumber], [Salary], [Gender], [JoiningDate]) VALUES (1002, N'Muhammad Umair Tahir', N'35202-8448219-9', N'98-Dilkusha Park, Rajgarh Lahore', N'+92', N'307', N'4179629', 15000.0000, 1, CAST(N'2018-07-01' AS Date))
INSERT [dbo].[STAFF] ([StaffId], [Name], [CNIC], [Address], [MCountryCode], [MCompanyCode], [MNumber], [Salary], [Gender], [JoiningDate]) VALUES (1004, N'staff', N'35102-1236547-7', N'12 abcxyz Lahore', N'+92', N'345', N'1234567', 8000.0000, 0, CAST(N'2018-07-01' AS Date))
INSERT [dbo].[STAFF] ([StaffId], [Name], [CNIC], [Address], [MCountryCode], [MCompanyCode], [MNumber], [Salary], [Gender], [JoiningDate]) VALUES (2012, N'Shakeel Afridi', N'34202-8448219-9', N'78 neela gumbad lahore', N'+92', N'300', N'4508967', 10000.0000, 1, CAST(N'2018-07-01' AS Date))
INSERT [dbo].[STAFF] ([StaffId], [Name], [CNIC], [Address], [MCountryCode], [MCompanyCode], [MNumber], [Salary], [Gender], [JoiningDate]) VALUES (2013, N'Demo Teacher', N'12345-1234567-1', N'xxxxxxxx', N'+92', N'321', N'1234567', 0.0000, 0, CAST(N'2018-07-01' AS Date))
SET IDENTITY_INSERT [dbo].[STAFF] OFF
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (3, 10015)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (3, 20015)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1002, 6)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1002, 7)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1002, 8)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1002, 9)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1002, 10016)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1002, 20016)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1004, 4)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1004, 5)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1004, 10014)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (1004, 20014)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (2012, 10013)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (2012, 20013)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (2013, 10012)
INSERT [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] ([StaffId], [AttandanceId]) VALUES (2013, 20012)
SET IDENTITY_INSERT [dbo].[STRUCK-OFF_STUDENTS] ON 

INSERT [dbo].[STRUCK-OFF_STUDENTS] ([SStudentId], [Name], [FatherName], [FatherCellNo], [BoardExamNo], [LastClassName], [DateOfStruck], [BFormNo], [Gender]) VALUES (1, N'Umar Tahir', N'TestFather2', N'+923227861235  ', N'123456', N'One            ', CAST(N'2018-08-17' AS Date), N'35202-4563217-7', 1)
INSERT [dbo].[STRUCK-OFF_STUDENTS] ([SStudentId], [Name], [FatherName], [FatherCellNo], [BoardExamNo], [LastClassName], [DateOfStruck], [BFormNo], [Gender]) VALUES (1002, N'testStudent2', N'Afzal Ali', N'+923455674321  ', N'123456', N'One            ', CAST(N'2018-09-04' AS Date), N'35202-1234451-7', 0)
INSERT [dbo].[STRUCK-OFF_STUDENTS] ([SStudentId], [Name], [FatherName], [FatherCellNo], [BoardExamNo], [LastClassName], [DateOfStruck], [BFormNo], [Gender]) VALUES (1003, N'testStudent', N'TestFather', N'+923074172327  ', N' ', N'One            ', CAST(N'2018-09-04' AS Date), N'35202-1234456-9', 1)
SET IDENTITY_INSERT [dbo].[STRUCK-OFF_STUDENTS] OFF
INSERT [dbo].[STUDENT_GET_TEST_RESULTS] ([StudentId], [TestId], [ObtainedMarks], [TeacherRemarks]) VALUES (1002, 10002, CAST(45.00 AS Decimal(5, 2)), N'excellent')
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (10, 10003)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (10, 20010)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1002, 2)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1002, 3)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1002, 10002)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1002, 10004)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1002, 20011)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1003, 10009)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1003, 20007)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1004, 10010)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1004, 20008)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1005, 10005)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (1005, 20003)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2028, 10006)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2028, 20004)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2029, 10007)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2029, 20005)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2031, 10008)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2031, 20006)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2032, 10011)
INSERT [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] ([StudentId], [AttandanceId]) VALUES (2032, 20009)
SET IDENTITY_INSERT [dbo].[STUDENT_MONTHLY_FEE] ON 

INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (1, 100.0000, 8, 2018, 1002)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (2, 100.0000, 9, 2018, 14)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (3, 500.0000, 9, 2018, 1002)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (4, 100.0000, 9, 2018, 10)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (6, 150.0000, 9, 2018, 1003)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (7, 10.0000, 9, 2018, 1004)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (8, 100.0000, 9, 2018, 1005)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (9, 30.0000, 9, 2018, 2028)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (10, 100.0000, 9, 2018, 2029)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (11, 0.0000, 9, 2018, 2031)
INSERT [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId], [Fine], [Month], [Year], [StudentId]) VALUES (12, 103.0000, 9, 2018, 2032)
SET IDENTITY_INSERT [dbo].[STUDENT_MONTHLY_FEE] OFF
INSERT [dbo].[STUDENT_PURCHASES_ITEMS] ([StudentId], [ItemId], [Quantity], [Date], [PurchasedPrice]) VALUES (16, 1, 2, CAST(N'2018-08-16T00:00:00.000' AS DateTime), 50.0000)
INSERT [dbo].[STUDENT_PURCHASES_ITEMS] ([StudentId], [ItemId], [Quantity], [Date], [PurchasedPrice]) VALUES (16, 1, 1, CAST(N'2018-08-16T22:31:06.500' AS DateTime), 50.0000)
INSERT [dbo].[STUDENT_PURCHASES_ITEMS] ([StudentId], [ItemId], [Quantity], [Date], [PurchasedPrice]) VALUES (1002, 1, 3, CAST(N'2018-08-17T15:28:25.973' AS DateTime), 100.0000)
SET IDENTITY_INSERT [dbo].[STUDENTS] ON 

INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (10, N'35202-1234456-7', N'testStudent2', N'A-124', CAST(N'1995-12-10' AS Date), CAST(N'2018-08-16' AS Date), N'', 1500.0000, 2, 4003, 101, 0)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (1002, N'35202-4563217-7', N'Muhammad Umar Tahir', N'B-126', CAST(N'2002-12-11' AS Date), CAST(N'2018-08-17' AS Date), N'Nobel High School', 1800.0000, 6, 4003, 103, 1)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (1003, N'35202-4538234-0', N'Muhammad Atif Aslam', N'B-146', CAST(N'2001-10-26' AS Date), CAST(N'2018-08-31' AS Date), N'Nobel High School', 1200.0000, 6, 1005, 104, 0)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (1004, N'               ', N'Test Test', N'C-21', CAST(N'2010-07-14' AS Date), CAST(N'2018-08-31' AS Date), N'', 800.0000, 2, 1005, 105, 0)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (1005, N'35202-4538234-0', N'Muhammad Tayyab Tahir', N'B-123', CAST(N'2018-08-31' AS Date), CAST(N'2018-08-31' AS Date), N'', 1300.0000, 6, 1, 106, 1)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (2028, N'               ', N'Muhammad Arsalan', N'A-98', CAST(N'2012-03-06' AS Date), CAST(N'2018-09-05' AS Date), N'', 1200.0000, 1003, 1, 107, 1)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (2029, N'               ', N'Kinza Shahab', N'B-302', CAST(N'2013-08-06' AS Date), CAST(N'2018-09-05' AS Date), N'', 1200.0000, 1003, 4, 108, 0)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (2031, N'               ', N'Muhammad Dawood', N'A-193', CAST(N'2014-01-09' AS Date), CAST(N'2018-09-05' AS Date), N'', 1000.0000, 1003, 4, 109, 1)
INSERT [dbo].[STUDENTS] ([StudentId], [BFormNumber], [Name], [AdmissionNo], [DOB], [DOA], [PrevInstitute], [MonthlyFee], [ParentId], [SectionId], [RollNo], [Gender]) VALUES (2032, N'               ', N'Kaneez Fatima', N'456', CAST(N'2016-10-19' AS Date), CAST(N'2018-09-05' AS Date), N'', 1000.0000, 1003, 1005, 110, 0)
SET IDENTITY_INSERT [dbo].[STUDENTS] OFF
SET IDENTITY_INSERT [dbo].[SUBJECTS] ON 

INSERT [dbo].[SUBJECTS] ([SubjectId], [Name], [ClassId]) VALUES (1, N'Engslish A', 2)
INSERT [dbo].[SUBJECTS] ([SubjectId], [Name], [ClassId]) VALUES (1003, N'ENGLISH-B', 2)
INSERT [dbo].[SUBJECTS] ([SubjectId], [Name], [ClassId]) VALUES (1004, N'URDU-A', 2)
INSERT [dbo].[SUBJECTS] ([SubjectId], [Name], [ClassId]) VALUES (1005, N'URDU-B', 2)
INSERT [dbo].[SUBJECTS] ([SubjectId], [Name], [ClassId]) VALUES (2002, N'ENGLISH-A', 3)
SET IDENTITY_INSERT [dbo].[SUBJECTS] OFF
INSERT [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION] ([StaffId], [SubjectId], [SectionId]) VALUES (3, 1, 1)
INSERT [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION] ([StaffId], [SubjectId], [SectionId]) VALUES (2013, 1003, 1)
INSERT [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION] ([StaffId], [SubjectId], [SectionId]) VALUES (2013, 1005, 1)
SET IDENTITY_INSERT [dbo].[TEACHERS_QUALIFICATIONS] ON 

INSERT [dbo].[TEACHERS_QUALIFICATIONS] ([QId], [StaffId], [Degree], [Year]) VALUES (1007, 3, N'FSc (Pre-Med)', 2015)
INSERT [dbo].[TEACHERS_QUALIFICATIONS] ([QId], [StaffId], [Degree], [Year]) VALUES (1008, 3, N'Matric', 2013)
INSERT [dbo].[TEACHERS_QUALIFICATIONS] ([QId], [StaffId], [Degree], [Year]) VALUES (1009, 1002, N'Matric', 2010)
INSERT [dbo].[TEACHERS_QUALIFICATIONS] ([QId], [StaffId], [Degree], [Year]) VALUES (1010, 1002, N'FSc (Pre-Med)', 2015)
INSERT [dbo].[TEACHERS_QUALIFICATIONS] ([QId], [StaffId], [Degree], [Year]) VALUES (1011, 1002, N'BSCS (continue)', 2018)
SET IDENTITY_INSERT [dbo].[TEACHERS_QUALIFICATIONS] OFF
INSERT [dbo].[TEACHING_STAFF] ([StaffId]) VALUES (3)
INSERT [dbo].[TEACHING_STAFF] ([StaffId]) VALUES (1002)
INSERT [dbo].[TEACHING_STAFF] ([StaffId]) VALUES (2013)
SET IDENTITY_INSERT [dbo].[TESTS] ON 

INSERT [dbo].[TESTS] ([TestId], [TotalMarks], [HeldDate], [SubjectId]) VALUES (1, CAST(40.00 AS Decimal(5, 2)), CAST(N'2018-08-16' AS Date), 1)
INSERT [dbo].[TESTS] ([TestId], [TotalMarks], [HeldDate], [SubjectId]) VALUES (10002, CAST(50.00 AS Decimal(5, 2)), CAST(N'2018-08-20' AS Date), 1)
SET IDENTITY_INSERT [dbo].[TESTS] OFF
/****** Object:  Index [ONE_DAIRY_AT_A_DATE]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ONE_DAIRY_AT_A_DATE] ON [dbo].[DAILY_DAIRIES]
(
	[Date] ASC,
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UNIQUE_PER_STAFF]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE NONCLUSTERED INDEX [UNIQUE_PER_STAFF] ON [dbo].[MONTHLY_SALARIES]
(
	[StaffId] ASC,
	[Month] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNIQUE_CNIC]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNIQUE_CNIC] ON [dbo].[PARENTS]
(
	[FCNIC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNIQUE_SECTION_FOR_CLASS]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNIQUE_SECTION_FOR_CLASS] ON [dbo].[SECTIONS]
(
	[Name] ASC,
	[ClassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNIQUE_CNIC]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNIQUE_CNIC] ON [dbo].[STAFF]
(
	[CNIC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNIQUE_BFORM]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNIQUE_BFORM] ON [dbo].[STRUCK-OFF_STUDENTS]
(
	[BFormNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UNIQUE_FEE_MONTH]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNIQUE_FEE_MONTH] ON [dbo].[STUDENT_MONTHLY_FEE]
(
	[StudentId] ASC,
	[Month] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UNIQUE_ADDMISSION_NUMBER]    Script Date: 3/28/2019 1:40:51 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UNIQUE_ADDMISSION_NUMBER] ON [dbo].[STUDENTS]
(
	[AdmissionNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[APPLICATIONS] ADD  CONSTRAINT [DF_APPLICATIONS_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[ATTANDANCES] ADD  CONSTRAINT [DF_ATTANDANCES_IsAbsent]  DEFAULT ((0)) FOR [IsAbsent]
GO
ALTER TABLE [dbo].[EXAMINATIONS] ADD  CONSTRAINT [DF_EXAMINATIONS_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
GO
ALTER TABLE [dbo].[NOTIFICATIONS] ADD  CONSTRAINT [DF_NOTIFICATIONS_IsSMS]  DEFAULT ((0)) FOR [IsSMS]
GO
ALTER TABLE [dbo].[NOTIFICATIONS] ADD  CONSTRAINT [DF_NOTIFICATIONS_IsWeb]  DEFAULT ((0)) FOR [IsWeb]
GO
ALTER TABLE [dbo].[NOTIFICATIONS] ADD  CONSTRAINT [DF_NOTIFICATIONS_ParentFlag]  DEFAULT ((0)) FOR [ParentFlag]
GO
ALTER TABLE [dbo].[NOTIFICATIONS] ADD  CONSTRAINT [DF_NOTIFICATIONS_TeacherFlag]  DEFAULT ((0)) FOR [TeacherFlag]
GO
ALTER TABLE [dbo].[PARENT_HAS_ACCOUNT] ADD  CONSTRAINT [DF_PARENT_HAS_ACCOUNT_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PARENT_MONTHLY_FEE] ADD  CONSTRAINT [DF_PARENT_MONTHLY_FEE_AmountPaid]  DEFAULT ((0)) FOR [AmountPaid]
GO
ALTER TABLE [dbo].[PARENT_MONTHLY_FEE] ADD  CONSTRAINT [DF_PARENT_MONTHLY_FEE_Concession]  DEFAULT ((0)) FOR [Concession]
GO
ALTER TABLE [dbo].[PARENT_MONTHLY_FEE] ADD  CONSTRAINT [DF_PARENT_MONTHLY_FEE_TotalAmountDue]  DEFAULT ((0)) FOR [TotalAmountDue]
GO
ALTER TABLE [dbo].[PARENT_RECEIVES_NOTIFICATIONS] ADD  CONSTRAINT [DF_PARENT_RECEIVES_NOTIFICATIONS_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[TEACHER_HAS_ACCOUNT] ADD  CONSTRAINT [DF_TEACHER_HAS_ACCOUNT_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TEACHER_RECEIVES_NOTIFICATIONS] ADD  CONSTRAINT [DF_TEACHER_RECEIVES_NOTIFICATIONS_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE]  WITH CHECK ADD  CONSTRAINT [FK_ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE_ADDITIONAL_FUNDS] FOREIGN KEY([FundId])
REFERENCES [dbo].[ADDITIONAL_FUNDS] ([FundId])
GO
ALTER TABLE [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] CHECK CONSTRAINT [FK_ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE_ADDITIONAL_FUNDS]
GO
ALTER TABLE [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE]  WITH CHECK ADD  CONSTRAINT [FK_ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE_STUDENT_MONTHLY_FEE] FOREIGN KEY([SFeeId])
REFERENCES [dbo].[STUDENT_MONTHLY_FEE] ([SFeeId])
GO
ALTER TABLE [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] CHECK CONSTRAINT [FK_ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE_STUDENT_MONTHLY_FEE]
GO
ALTER TABLE [dbo].[APPLICATIONS]  WITH CHECK ADD  CONSTRAINT [PARENT_SENDS_APPLICATIONS] FOREIGN KEY([ParentId])
REFERENCES [dbo].[PARENTS] ([ParentId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[APPLICATIONS] CHECK CONSTRAINT [PARENT_SENDS_APPLICATIONS]
GO
ALTER TABLE [dbo].[CLASSES]  WITH CHECK ADD  CONSTRAINT [CLASS_HAS_INCHARGE] FOREIGN KEY([InchargeId])
REFERENCES [dbo].[TEACHING_STAFF] ([StaffId])
GO
ALTER TABLE [dbo].[CLASSES] CHECK CONSTRAINT [CLASS_HAS_INCHARGE]
GO
ALTER TABLE [dbo].[DAILY_DAIRIES]  WITH CHECK ADD  CONSTRAINT [SUBJECT_HAS_DAILY_DAIRIES] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[SUBJECTS] ([SubjectId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DAILY_DAIRIES] CHECK CONSTRAINT [SUBJECT_HAS_DAILY_DAIRIES]
GO
ALTER TABLE [dbo].[DOWNLOADABLES]  WITH CHECK ADD  CONSTRAINT [CLASS_HAS_DOWNLOADABLES] FOREIGN KEY([ClassId])
REFERENCES [dbo].[CLASSES] ([ClassId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DOWNLOADABLES] CHECK CONSTRAINT [CLASS_HAS_DOWNLOADABLES]
GO
ALTER TABLE [dbo].[INVENTORY_ITEMS]  WITH CHECK ADD  CONSTRAINT [ITEMS_BELONG_TO_CATEGORY] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[INVENTORY_CATEGORIES] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[INVENTORY_ITEMS] CHECK CONSTRAINT [ITEMS_BELONG_TO_CATEGORY]
GO
ALTER TABLE [dbo].[MONTHLY_SALARIES]  WITH CHECK ADD  CONSTRAINT [STAFF_TAKES_MONTHLY_SALARY] FOREIGN KEY([StaffId])
REFERENCES [dbo].[STAFF] ([StaffId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MONTHLY_SALARIES] CHECK CONSTRAINT [STAFF_TAKES_MONTHLY_SALARY]
GO
ALTER TABLE [dbo].[NON-TEACHING_STAFF]  WITH CHECK ADD  CONSTRAINT [FK_NON-TEACHING_STAFF_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[STAFF] ([StaffId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NON-TEACHING_STAFF] CHECK CONSTRAINT [FK_NON-TEACHING_STAFF_STAFF]
GO
ALTER TABLE [dbo].[PARENT_HAS_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_PARENT_HAS_ACCOUNT_PARENTS] FOREIGN KEY([ParentId])
REFERENCES [dbo].[PARENTS] ([ParentId])
GO
ALTER TABLE [dbo].[PARENT_HAS_ACCOUNT] CHECK CONSTRAINT [FK_PARENT_HAS_ACCOUNT_PARENTS]
GO
ALTER TABLE [dbo].[PARENT_MONTHLY_FEE]  WITH CHECK ADD  CONSTRAINT [PARENT_PAYS_MONTHLY_FEE] FOREIGN KEY([ParentId])
REFERENCES [dbo].[PARENTS] ([ParentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PARENT_MONTHLY_FEE] CHECK CONSTRAINT [PARENT_PAYS_MONTHLY_FEE]
GO
ALTER TABLE [dbo].[PARENT_RECEIVES_NOTIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_PARENT_RECEIVES_NOTIFICATIONS_NOTIFICATIONS] FOREIGN KEY([NotificationId])
REFERENCES [dbo].[NOTIFICATIONS] ([NotificationId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PARENT_RECEIVES_NOTIFICATIONS] CHECK CONSTRAINT [FK_PARENT_RECEIVES_NOTIFICATIONS_NOTIFICATIONS]
GO
ALTER TABLE [dbo].[PARENT_RECEIVES_NOTIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_PARENT_RECEIVES_NOTIFICATIONS_PARENTS] FOREIGN KEY([ParentId])
REFERENCES [dbo].[PARENTS] ([ParentId])
GO
ALTER TABLE [dbo].[PARENT_RECEIVES_NOTIFICATIONS] CHECK CONSTRAINT [FK_PARENT_RECEIVES_NOTIFICATIONS_PARENTS]
GO
ALTER TABLE [dbo].[SECTIONS]  WITH CHECK ADD  CONSTRAINT [CLASSES_HAS_SECTIONS] FOREIGN KEY([ClassId])
REFERENCES [dbo].[CLASSES] ([ClassId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SECTIONS] CHECK CONSTRAINT [CLASSES_HAS_SECTIONS]
GO
ALTER TABLE [dbo].[STAFF_GIVES_DAILY_ATTANDANCE]  WITH CHECK ADD  CONSTRAINT [FK_STAFF_GIVES_DAILY_ATTANDANCE_ATTANDANCES] FOREIGN KEY([AttandanceId])
REFERENCES [dbo].[ATTANDANCES] ([AttandanceId])
GO
ALTER TABLE [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] CHECK CONSTRAINT [FK_STAFF_GIVES_DAILY_ATTANDANCE_ATTANDANCES]
GO
ALTER TABLE [dbo].[STAFF_GIVES_DAILY_ATTANDANCE]  WITH CHECK ADD  CONSTRAINT [FK_STAFF_GIVES_DAILY_ATTANDANCE_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[STAFF] ([StaffId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STAFF_GIVES_DAILY_ATTANDANCE] CHECK CONSTRAINT [FK_STAFF_GIVES_DAILY_ATTANDANCE_STAFF]
GO
ALTER TABLE [dbo].[STUDENT_GET_TEST_RESULTS]  WITH CHECK ADD  CONSTRAINT [FK_STUDENT_GET_TEST_RESULTS_STUDENTS] FOREIGN KEY([StudentId])
REFERENCES [dbo].[STUDENTS] ([StudentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENT_GET_TEST_RESULTS] CHECK CONSTRAINT [FK_STUDENT_GET_TEST_RESULTS_STUDENTS]
GO
ALTER TABLE [dbo].[STUDENT_GET_TEST_RESULTS]  WITH CHECK ADD  CONSTRAINT [FK_STUDENT_GET_TEST_RESULTS_TESTS] FOREIGN KEY([TestId])
REFERENCES [dbo].[TESTS] ([TestId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENT_GET_TEST_RESULTS] CHECK CONSTRAINT [FK_STUDENT_GET_TEST_RESULTS_TESTS]
GO
ALTER TABLE [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE]  WITH CHECK ADD  CONSTRAINT [FK_STUDENT_GIVES_DAILY_ATTANDANCE_ATTANDANCES] FOREIGN KEY([AttandanceId])
REFERENCES [dbo].[ATTANDANCES] ([AttandanceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] CHECK CONSTRAINT [FK_STUDENT_GIVES_DAILY_ATTANDANCE_ATTANDANCES]
GO
ALTER TABLE [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE]  WITH CHECK ADD  CONSTRAINT [FK_STUDENT_GIVES_DAILY_ATTANDANCE_STUDENTS] FOREIGN KEY([StudentId])
REFERENCES [dbo].[STUDENTS] ([StudentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENT_GIVES_DAILY_ATTANDANCE] CHECK CONSTRAINT [FK_STUDENT_GIVES_DAILY_ATTANDANCE_STUDENTS]
GO
ALTER TABLE [dbo].[STUDENT_MONTHLY_FEE]  WITH NOCHECK ADD  CONSTRAINT [STUDENT_HAS_MONTHLY_FEE] FOREIGN KEY([StudentId])
REFERENCES [dbo].[STUDENTS] ([StudentId])
GO
ALTER TABLE [dbo].[STUDENT_MONTHLY_FEE] NOCHECK CONSTRAINT [STUDENT_HAS_MONTHLY_FEE]
GO
ALTER TABLE [dbo].[STUDENT_PURCHASES_ITEMS]  WITH CHECK ADD  CONSTRAINT [FK_STUDENT_PURCHASES_ITEMS_INVENTORY_ITEMS] FOREIGN KEY([ItemId])
REFERENCES [dbo].[INVENTORY_ITEMS] ([ItemId])
GO
ALTER TABLE [dbo].[STUDENT_PURCHASES_ITEMS] CHECK CONSTRAINT [FK_STUDENT_PURCHASES_ITEMS_INVENTORY_ITEMS]
GO
ALTER TABLE [dbo].[STUDENT_PURCHASES_ITEMS]  WITH NOCHECK ADD  CONSTRAINT [FK_STUDENT_PURCHASES_ITEMS_STUDENTS] FOREIGN KEY([StudentId])
REFERENCES [dbo].[STUDENTS] ([StudentId])
GO
ALTER TABLE [dbo].[STUDENT_PURCHASES_ITEMS] NOCHECK CONSTRAINT [FK_STUDENT_PURCHASES_ITEMS_STUDENTS]
GO
ALTER TABLE [dbo].[STUDENTS]  WITH CHECK ADD  CONSTRAINT [STUDENT_BELONGS_TO_PARENT] FOREIGN KEY([ParentId])
REFERENCES [dbo].[PARENTS] ([ParentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENTS] CHECK CONSTRAINT [STUDENT_BELONGS_TO_PARENT]
GO
ALTER TABLE [dbo].[STUDENTS]  WITH CHECK ADD  CONSTRAINT [STUDENTS_READ_IN_SECTION] FOREIGN KEY([SectionId])
REFERENCES [dbo].[SECTIONS] ([SectionId])
GO
ALTER TABLE [dbo].[STUDENTS] CHECK CONSTRAINT [STUDENTS_READ_IN_SECTION]
GO
ALTER TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS]  WITH CHECK ADD  CONSTRAINT [FK_STUDENTS_GET_EXAM_RESULTS_EXAMINATIONS] FOREIGN KEY([ExamId])
REFERENCES [dbo].[EXAMINATIONS] ([ExamId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS] CHECK CONSTRAINT [FK_STUDENTS_GET_EXAM_RESULTS_EXAMINATIONS]
GO
ALTER TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS]  WITH CHECK ADD  CONSTRAINT [FK_STUDENTS_GET_EXAM_RESULTS_STUDENTS] FOREIGN KEY([StudentId])
REFERENCES [dbo].[STUDENTS] ([StudentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS] CHECK CONSTRAINT [FK_STUDENTS_GET_EXAM_RESULTS_STUDENTS]
GO
ALTER TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS]  WITH CHECK ADD  CONSTRAINT [FK_STUDENTS_GET_EXAM_RESULTS_SUBJECTS] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[SUBJECTS] ([SubjectId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STUDENTS_GET_EXAM_RESULTS] CHECK CONSTRAINT [FK_STUDENTS_GET_EXAM_RESULTS_SUBJECTS]
GO
ALTER TABLE [dbo].[SUBJECTS]  WITH CHECK ADD  CONSTRAINT [CLASS_HAS_SUBJECTS] FOREIGN KEY([ClassId])
REFERENCES [dbo].[CLASSES] ([ClassId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SUBJECTS] CHECK CONSTRAINT [CLASS_HAS_SUBJECTS]
GO
ALTER TABLE [dbo].[TEACHER_HAS_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_TEACHER_HAS_ACCOUNT_TEACHING_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[TEACHING_STAFF] ([StaffId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHER_HAS_ACCOUNT] CHECK CONSTRAINT [FK_TEACHER_HAS_ACCOUNT_TEACHING_STAFF]
GO
ALTER TABLE [dbo].[TEACHER_RECEIVES_NOTIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_TEACHER_RECEIVES_NOTIFICATIONS_NOTIFICATIONS] FOREIGN KEY([NotificationId])
REFERENCES [dbo].[NOTIFICATIONS] ([NotificationId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHER_RECEIVES_NOTIFICATIONS] CHECK CONSTRAINT [FK_TEACHER_RECEIVES_NOTIFICATIONS_NOTIFICATIONS]
GO
ALTER TABLE [dbo].[TEACHER_RECEIVES_NOTIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_TEACHER_RECEIVES_NOTIFICATIONS_TEACHING_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[TEACHING_STAFF] ([StaffId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHER_RECEIVES_NOTIFICATIONS] CHECK CONSTRAINT [FK_TEACHER_RECEIVES_NOTIFICATIONS_TEACHING_STAFF]
GO
ALTER TABLE [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION]  WITH CHECK ADD  CONSTRAINT [FK_TEACHER_TEACHES_SUBJECT_OF_A_SECTION_SUBJECTS] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[SUBJECTS] ([SubjectId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION] CHECK CONSTRAINT [FK_TEACHER_TEACHES_SUBJECT_OF_A_SECTION_SUBJECTS]
GO
ALTER TABLE [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION]  WITH CHECK ADD  CONSTRAINT [FK_TEACHER_TEACHES_SUBJECT_OF_A_SECTION_TEACHING_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[TEACHING_STAFF] ([StaffId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHER_TEACHES_SUBJECT_OF_A_SECTION] CHECK CONSTRAINT [FK_TEACHER_TEACHES_SUBJECT_OF_A_SECTION_TEACHING_STAFF]
GO
ALTER TABLE [dbo].[TEACHERS_QUALIFICATIONS]  WITH CHECK ADD  CONSTRAINT [FK_TEACHERS_QUALIFICATIONS_TEACHING_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[TEACHING_STAFF] ([StaffId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHERS_QUALIFICATIONS] CHECK CONSTRAINT [FK_TEACHERS_QUALIFICATIONS_TEACHING_STAFF]
GO
ALTER TABLE [dbo].[TEACHING_STAFF]  WITH CHECK ADD  CONSTRAINT [FK_TEACHING_STAFF_STAFF] FOREIGN KEY([StaffId])
REFERENCES [dbo].[STAFF] ([StaffId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TEACHING_STAFF] CHECK CONSTRAINT [FK_TEACHING_STAFF_STAFF]
GO
ALTER TABLE [dbo].[TESTS]  WITH CHECK ADD  CONSTRAINT [SUBJECT_HAS_TESTS] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[SUBJECTS] ([SubjectId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TESTS] CHECK CONSTRAINT [SUBJECT_HAS_TESTS]
GO
/****** Object:  StoredProcedure [dbo].[AddNewParent]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddNewParent] 
	-- Add the parameters for the stored procedure here
	@fName varchar(max) , 
	@motherName varchar(max),
	@fCNIC nchar(15),
	@homeAddress varchar(max),
	@fmCountryCode nchar(3),
	@fmCompanyCode nchar(3),
	@fmNumber nchar(7),
	@mmCountryCode nchar(3),
	@mmCompanyCode nchar(3),
	@mmNumber nchar(7),
	@emergencyContact nchar(15),
	@eThreshold int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @id int;
    -- Insert statements for procedure here
	INSERT INTO [dbo].[PARENTS]
           ([FatherName]
           ,[MotherName]
           ,[FCNIC]
           ,[HomeAddress]
           ,[FMCountryCode]
           ,[FMCompanyCode]
           ,[FMNumber]
           ,[MMCountryCode]
           ,[MMCompanyCode]
           ,[MMNumber]
           ,[EmergencyContact]
           ,[EligibiltyThreshold])
     VALUES
           (@fName
           ,@motherName
           ,@fCNIC
           ,@homeAddress
           ,@fmCountryCode
           ,@fmCompanyCode
           ,@fmNumber
           ,@mmCountryCode
           ,@mmCompanyCode
           ,@mmNumber
           ,@emergencyContact
           ,@eThreshold);
	
	Select MAX(ParentId) from PARENTS;
END
GO
/****** Object:  StoredProcedure [dbo].[AddNonStaff]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddNonStaff]
	@name varchar(MAX),
	@CNIC nchar(15),
	@address varchar(MAX),
	@mCountryCode nchar(3),
	@mCompanyCode nchar(3),
	@mNumber nchar(7),
	@salary money,
	@jobType varchar(50),
	@gender bit,
	@joiningDate date
AS
BEGIN
	INSERT INTO [STAFF]
           ([Name]
           ,[CNIC]
           ,[Address]
           ,[MCountryCode]
           ,[MCompanyCode]
           ,[MNumber]
           ,[Salary]
		   ,[Gender]
		   ,[JoiningDate])
     VALUES
           (@name
           ,@CNIC
           ,@address
           ,@mCountryCode
           ,@mCompanyCode
           ,@mNumber
           ,@salary
		   ,@gender
		   ,@joiningDate);
	DECLARE @id int;
	SELECT @id=MAX(StaffId) FROM STAFF;
	INSERT INTO [NON-TEACHING_STAFF]
           ([StaffId]
           ,[JobType])
     VALUES
           (@id
           ,@jobType);
	SELECT @id;
END
GO
/****** Object:  StoredProcedure [dbo].[AddStruckOffStudent]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddStruckOffStudent]
	@studentId int,
	@dateOfStruck date,
	@boardRoll varchar(50)
AS
BEGIN
	DECLARE @name varchar(MAX), @fName varchar(MAX), @fCellNo nchar(15), @className varchar(50), @bForm nchar(15), @gender bit, @secId int, @parentId int;
	--Attributes from student Table
	SELECT @name=[Name], @bForm=BFormNumber, @gender=Gender, @parentId=ParentId, @secId=SectionId FROM STUDENTS WHERE StudentId=@studentId;
	--Attribites from parnt Table
	SELECT @fName=FatherName, @fCellNo=FMCountryCode+FMCompanyCode+FMNumber FROM PARENTS WHERE ParentId=@parentId;
	--Class Name
	SELECT @className=[Name] FROM CLASSES WHERE ClassId=(SELECT ClassId FROM SECTIONS WHERE SectionId=@secId); 
	--Inserting Data
	INSERT INTO [dbo].[STRUCK-OFF_STUDENTS]
           ([Name]
           ,[FatherName]
           ,[FatherCellNo]
           ,[BoardExamNo]
           ,[LastClassName]
           ,[DateOfStruck]
           ,[BFormNo]
           ,[Gender])
     VALUES
           (@name
           ,@fName
           ,@fCellNo
           ,@boardRoll
           ,@className
           ,@dateOfStruck
           ,@bForm
           ,@gender);
	--Delete Student
	DELETE STUDENTS WHERE StudentId=@studentId;
END
GO
/****** Object:  StoredProcedure [dbo].[AddStudent]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddStudent] 
	@name varchar(MAX),
	@bForm nchar(15),
	@addNo varchar(50),
	@dob date,
	@doa date,
	@prevInst varchar(MAX),
	@monthlyFee money,
	@parentId int,
	@sectionId int,
	@gender bit
	
AS
BEGIN
DECLARE @classId int,
	@id int,
	@rollNo int;
--Getting ClassId from SectionId to calculate rollNo--
	Select @classId = ClassId from SECTIONS where SectionId=@sectionId;
	--If there is no Record exists for the class, setting the roll no to the Roll Number Index of the Class--
	if not exists(Select s.StudentId from STUDENTS s, (select SectionId from SECTIONS where ClassId=@classId) as sec where s.SectionId=sec.SectionId)
		BEGIN
			Select @rollNo=RollNoIndex from CLASSES where ClassId=@classId;
		END
	--But if there is already students entered in that class then get the maximum roll no and add 1 to that--
	else
		BEGIN
			Select @rollNo= MAX(s.RollNo)+1 from STUDENTS s,(select SectionId from SECTIONS where ClassId=@classId) as sec where s.SectionId=sec.SectionId;
		END
	--Inserting all the data--
	INSERT INTO [dbo].[STUDENTS]
           ([BFormNumber]
           ,[Name]
           ,[AdmissionNo]
           ,[DOB]
           ,[DOA]
           ,[PrevInstitute]
           ,[MonthlyFee]
           ,[ParentId]
           ,[SectionId]
           ,[RollNo]
           ,[Gender])
     VALUES
           (@bForm
           ,@name
           ,@addNo
           ,@dob
           ,@doa
           ,@prevInst
           ,@monthlyFee
           ,@parentId
           ,@sectionId
           ,@rollNo
           ,@gender);
	--Getting  Id of the data inserted--
	SELECT @id=MAX(StudentId) FROM STUDENTS;
	DECLARE @ViewTbl Table(
	id int,
	roll int);
	INSERT INTO @ViewTbl
	VALUES(@id,@rollNo);
	SELECT *
	FROM @ViewTbl;
END
GO
/****** Object:  StoredProcedure [dbo].[GetMonthlyProgress]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMonthlyProgress] 
	@monthStart date,
	@monthEnd date,
	@month int,
	@year int,
	@sectionId int
AS
BEGIN
--declare a new table to store the testIds of all the subjects of the section
	DECLARE @Tests Table(
		testId bigint
	);
	--the the testIds into our table and select the data between the month
	INSERT INTO @Tests
	SELECT t.TestId
	FROM TESTS t, (
		SELECT sub.SubjectId
		FROM SECTIONS s, SUBJECTS sub
		WHERE s.ClassId=sub.ClassId AND s.SectionId=@sectionId) as sub
	WHERE t.SubjectId=sub.SubjectId AND t.[HeldDate] BETWEEN @monthStart AND @monthEnd;
	--same as tests now declare a table for getting the examinationIds
	DECLARE @Examinations Table(
		examId int
	);
	--Inserting values into the table
	INSERT INTO @Examinations
	SELECT ExamId
	FROM EXAMINATIONS
	WHERE [Month]=@month AND [Year]=@year;
	--Declaring variables which will store the sum of tests marks
	DECLARE @testObtained decimal,
	@testTotal decimal;
	--Query to get tests total
	if EXISTS (SELECT st.ObtainedMarks
	FROM STUDENT_GET_TEST_RESULTS st, STUDENTS s, @Tests t
	WHERE st.StudentId=s.StudentId AND s.SectionId=@sectionId AND t.testId=st.TestId)
	BEGIN --If the Sum is not null
		SELECT @testObtained = SUM(st.ObtainedMarks), @testTotal=SUM(test.TotalMarks)
		FROM STUDENT_GET_TEST_RESULTS st, STUDENTS s, @Tests t, TESTS test
		WHERE st.StudentId=s.StudentId AND s.SectionId=@sectionId AND t.testId=st.TestId AND st.TestId=t.testId;
	END
	ELSE
	BEGIN
	--use 0 if the summ is null
		SET @testTotal=0;
		SET @testObtained=0;
	END
	--Declaring Variables for examinationResult
	DECLARE @marksTotal decimal,
	 @marksObtained decimal
	 IF EXISTS(SELECT sr.ObtainedMarks
		FROM @Examinations e, STUDENTS_GET_EXAM_RESULTS sr, STUDENTS s
		WHERE e.ExamId=sr.ExamId AND sr.StudentId=s.StudentId AND s.SectionId=@sectionId)
	BEGIN
		SELECT @marksObtained=SUM(sr.ObtainedMarks), @marksTotal=SUM(sr.TotalMarks)
		FROM @Examinations e, STUDENTS_GET_EXAM_RESULTS sr, STUDENTS s
		WHERE e.ExamId=sr.ExamId AND sr.StudentId=s.StudentId AND s.SectionId=@sectionId;
	END
	ELSE
	BEGIN
		SET @marksObtained=0;
		SET @marksTotal=0;
	END

	--Getting Result
	SELECT (@testObtained+@marksObtained) as ObtainedMarks, (@testTotal+@marksTotal) as TotalMarks
END
GO
/****** Object:  StoredProcedure [dbo].[GetParentMonthFee]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetParentMonthFee] 
	-- Add the parameters for the stored procedure here
	@startdate date,
	@endDate date,
	@month smallint,
	@year smallint,
	@parentId int
AS
BEGIN
	SET NOCOUNT ON;
	--decalre variables to get values
	DECLARE @studentFee money = 0,
	@studentItemsAmount money = 0,
	@total money = 0,
	@balance money=0;
	--check if Fee record for students of a parent is present or not, if not then the value for @studentFeee will be default as set above
	IF EXISTS (Select * from STUDENT_MONTHLY_FEE sm, STUDENTS s where sm.Month=@month and sm.Year=@year and sm.StudentId=s.StudentId and s.ParentId=@parentId)
	BEGIN
	-- if exists the take the sum of tuition fees for all students
		--check if the fee contain any addtitional funds or not
		IF NOT EXISTS (
		Select adsum.AddSum
		from STUDENT_MONTHLY_FEE sm, STUDENTS s,(
			SELECT SUM(a.Amount) as AddSum , smf.SFeeId as FeeId
			FROM ADDITIONAL_FUNDS a, ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE af, STUDENT_MONTHLY_FEE smf 
			WHERE af.SFeeId=smf.SFeeId AND a.FundId=af.FundId GROUP BY smf.SFeeId) adsum 
		where sm.Month=@month and sm.Year=@year and sm.StudentId=s.StudentId and s.ParentId=@parentId and adsum.FeeId=sm.SFeeId)
		BEGIN
		--if not then sum only the tution fee and fine
			Select @studentFee= SUM(s.MonthlyFee+sm.Fine)
			from STUDENT_MONTHLY_FEE sm, STUDENTS s
			where sm.Month=@month and sm.Year=@year and sm.StudentId=s.StudentId and s.ParentId=@parentId;
		END
		ELSE
		BEGIN
		--if added then also add the sum of additional funds added to the student(s) monthly fee
			Select @studentFee= SUM(s.MonthlyFee+sm.Fine+adsum.AddSum)
			from STUDENT_MONTHLY_FEE sm, STUDENTS s,(
				SELECT SUM(a.Amount) as AddSum , smf.SFeeId as FeeId
				FROM ADDITIONAL_FUNDS a, ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE af, STUDENT_MONTHLY_FEE smf 
				WHERE af.SFeeId=smf.SFeeId AND a.FundId=af.FundId GROUP BY smf.SFeeId) adsum 
			where sm.Month=@month and sm.Year=@year and sm.StudentId=s.StudentId and s.ParentId=@parentId and adsum.FeeId=sm.SFeeId;
		END
	END
	--check if student have purchased any item from inventory, if not the use the default value
	IF EXISTS (Select * from STUDENT_PURCHASES_ITEMS sp, STUDENTS s where s.ParentId=@parentId and sp.StudentId=s.StudentId and (sp.Date between @startdate and @endDate))
	BEGIN
		Select @studentItemsAmount=SUM(sp.PurchasedPrice*sp.Quantity) from STUDENT_PURCHASES_ITEMS sp, STUDENTS s where s.ParentId=@parentId and sp.StudentId=s.StudentId and (sp.Date between @startdate and @endDate);
	END
	--check if parent have submitted any dues in the given month , if yes then use the sum of amount paid in that month , if not then calculate the balance from the last fee record
	IF NOT EXISTS(SELECT * FROM PARENT_MONTHLY_FEE WHERE (DatePaid BETWEEN @startdate AND @endDate) AND ParentId=@parentId)
	BEGIN
		Select @balance=(TotalAmountDue-Concession-AmountPaid) from PARENT_MONTHLY_FEE where PFeeId=(Select MAX(PFeeId) from PARENT_MONTHLY_FEE where ParentId=@parentId);
	END
	ELSE
	BEGIN
		SELECT @balance=-(SUM(AmountPaid+Concession)) FROM PARENT_MONTHLY_FEE WHERE (DatePaid BETWEEN @startdate AND @endDate) AND ParentId=@parentId;
	END
	--return the sum
	SELECT @studentFee+@studentItemsAmount+@balance;
END
GO
/****** Object:  StoredProcedure [dbo].[GetStaffAttendance]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStaffAttendance]
	@staffId int,
	@date date
AS
BEGIN
	IF NOT EXISTS(SELECT *
		FROM ATTANDANCES a, STAFF_GIVES_DAILY_ATTANDANCE st
		WHERE a.AttandanceId=st.AttandanceId AND st.StaffId=@staffId AND a.[Date]=@date)
	BEGIN
		SELECT 0;
	END
	ELSE
	BEGIN
		SELECT a.IsAbsent
		FROM ATTANDANCES a, STAFF_GIVES_DAILY_ATTANDANCE st
		WHERE a.AttandanceId=st.AttandanceId AND st.StaffId=@staffId AND a.[Date]=@date;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[GetStudentAttendance]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStudentAttendance]
	@studentId int,
	@date date
AS
BEGIN
	IF NOT EXISTS(SELECT *
		FROM ATTANDANCES a, STUDENT_GIVES_DAILY_ATTANDANCE st
		WHERE a.AttandanceId=st.AttandanceId AND st.StudentId=@studentId AND a.[Date]=@date)
	BEGIN
		SELECT 0;
	END
	ELSE
	BEGIN
		SELECT a.IsAbsent
		FROM ATTANDANCES a, STUDENT_GIVES_DAILY_ATTANDANCE st
		WHERE a.AttandanceId=st.AttandanceId AND st.StudentId=@studentId AND a.[Date]=@date;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[GetStudentDairy]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentDairy]
	@studentId int,
	@date date
AS
BEGIN
	--Declare a Table to get subjects of class of the given Student--
	DECLARE @subjects Table(
		SubjectId int NOT NULL
		);
	--Get classId from StudentId--
	DECLARE @classId int;
	SELECT @classId=s.ClassId
	FROM SECTIONS s, STUDENTS st
	WHERE s.SectionId=st.SectionId AND st.StudentId=@studentId;
	--Insert Subjects into the Table--
	INSERT INTO @subjects(SubjectId)
	SELECT SubjectId
	FROM SUBJECTS
	WHERE ClassId=@classId;
	--Main SELECT Query to get the desired result--
	SELECT d.Content, d.SubjectId 
	FROM DAILY_DAIRIES d,(SELECT SubjectId FROM @subjects) s
	WHERE [Date]=@date AND d.SubjectId=s.SubjectId;
																			
END
GO
/****** Object:  StoredProcedure [dbo].[GetStudentMonthlyProgress]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentMonthlyProgress]
	@monthStart date,
	@monthEnd date,
	@month int,
	@year int,
	@studentId int
AS
BEGIN
	DECLARE @testTotal decimal,
			@testObtained decimal,
			@resultTotal decimal,
			@resultObtained decimal;

	SET @testObtained=0;
	SET @testTotal=0;
	SET @resultObtained=0;
	SET @resultTotal=0;
	IF EXISTS (SELECT st.StudentId
				FROM STUDENT_GET_TEST_RESULTS st, TESTS t
				WHERE st.TestId=t.TestId AND st.StudentId=@studentId AND t.HeldDate BETWEEN @monthStart AND @monthEnd)
	BEGIN
		SELECT @testObtained =SUM(st.ObtainedMarks),@testTotal= SUM( t.TotalMarks)
		FROM STUDENT_GET_TEST_RESULTS st, TESTS t
		WHERE st.TestId=t.TestId AND st.StudentId=@studentId AND t.HeldDate BETWEEN @monthStart AND @monthEnd;

	END

	IF EXISTS (SELECT ste.StudentId
				FROM STUDENTS_GET_EXAM_RESULTS ste, EXAMINATIONS e
				WHERE ste.ExamId=e.ExamId AND e.[Month]=@month AND e.[Year]=@year AND ste.StudentId=@studentId)
	BEGIN
		SELECT @resultObtained= SUM( ste.ObtainedMarks), @resultTotal= SUM( ste.TotalMarks)
		FROM STUDENTS_GET_EXAM_RESULTS ste, EXAMINATIONS e
		WHERE ste.ExamId=e.ExamId AND e.[Month]=@month AND e.[Year]=@year AND ste.StudentId=@studentId;
	END

	IF((@resultTotal+@testTotal)!=0)
	BEGIN
		SELECT ((@resultObtained+@testObtained)/(@resultTotal+@testTotal))*100 as Progress;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[GetTotalDue]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTotalDue]
	@monthStart date,
	@monthEnd date
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @totalStu money=0,
	@totalItem money=0;
	Select @totalStu=SUM(MonthlyFee) from STUDENTS;
	IF EXISTS(Select sp.Quantity,sp.PurchasedPrice from STUDENT_PURCHASES_ITEMS sp where sp.Date between @monthStart and @monthEnd)
		Select @totalItem=SUM(sp.PurchasedPrice) from STUDENT_PURCHASES_ITEMS sp where sp.Date between @monthStart and @monthEnd;
	SELECT @totalItem+@totalStu;
END
GO
/****** Object:  StoredProcedure [dbo].[MonthSoFar]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MonthSoFar]
	-- Add the parameters for the stored procedure here
	@dateStart date,
	@dateEnd date
AS
BEGIN
	Declare @studentsAdmit int=0,
	@testTaken int=0,
	@itemsSold int=0,
	@notificationsSent int=0,
	@applicationsReceived int=0,
	@paymentsReceived int=0

	SELECT @studentsAdmit = COUNT(*) FROM STUDENTS WHERE [DOA]  BETWEEN @dateStart AND @dateEnd;

	SELECT @testTaken = COUNT(*) FROM TESTS WHERE [HeldDate]  BETWEEN @dateStart AND @dateEnd;

	SELECT @itemsSold = COUNT(*) FROM STUDENT_PURCHASES_ITEMS WHERE [Date]  BETWEEN @dateStart AND @dateEnd;

	SELECT @notificationsSent = COUNT(*) FROM NOTIFICATIONS WHERE [Date]  BETWEEN @dateStart AND @dateEnd;

	SELECT @applicationsReceived = COUNT(*) FROM APPLICATIONS WHERE [Date]  BETWEEN @dateStart AND @dateEnd;

	SELECT @paymentsReceived = COUNT(*) FROM PARENT_MONTHLY_FEE WHERE [DatePaid]  BETWEEN @dateStart AND @dateEnd;

	DECLARE @view Table(
		StudentsAdmit int,
		TestTaken int,
		ItemsSold int,
		NotificationsSent int,
		ApplicationsReceived int,
		PaymentsReceived int
		);
	INSERT INTO @view VALUES(@studentsAdmit ,@testTaken ,@itemsSold ,@notificationsSent ,@applicationsReceived ,@paymentsReceived);

	SELECT * FROM @view;
END
GO
/****** Object:  StoredProcedure [dbo].[SellItem]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SellItem]
	@itemId int,
	@studentId int,
	@quantity int,
	@date dateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Declare a variable to get item's current quantity
	DECLARE @itemQuantity int, @itemPrice money ;
	SELECT @itemQuantity = Quantity, @itemPrice=Price FROM INVENTORY_ITEMS WHERE ItemId=@itemId;
	--Check wheather the item has enough quantity to be purchased
	if(@quantity<@itemQuantity OR @quantity=@itemQuantity)
	BEGIN
	--Insert Values into student Table Which means the student has purchased the item
		INSERT INTO [STUDENT_PURCHASES_ITEMS]
           ([StudentId]
           ,[ItemId]
           ,[Quantity]
           ,[Date]
		   ,[PurchasedPrice])
		 VALUES
           (@studentId
           ,@itemId
           ,@quantity
           ,@date
		   ,@itemPrice);
		   --update the stock according to the item(s) Purchased
		   UPDATE INVENTORY_ITEMS SET Quantity=@itemQuantity-@quantity WHERE ItemId=@itemId;
		   SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SetSalary]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetSalary]
	@month int,
	@year int,
	@staffId int,
	@perAbsent money,
	@salary money
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT * FROM MONTHLY_SALARIES WHERE [Month]=@month AND [Year]=@year AND [StaffId]=@staffId)
	BEGIN
		INSERT INTO [dbo].[MONTHLY_SALARIES]
           ([Month]
           ,[Year]
           ,[StaffId]
           ,[PerAbsentDeduction]
		   ,[Salary])
		VALUES
           (@month
           ,@year
           ,@staffId
           ,@perAbsent
		   ,@salary);
		   SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SetStaffAttandance]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetStaffAttandance] 
	@staffId int,
	@isAbsent bit,
	@date date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Check if the same data is already present or not
    IF NOT EXISTS(SELECT * FROM ATTANDANCES a, STAFF_GIVES_DAILY_ATTANDANCE sa WHERE a.[Date]=@date AND sa.AttandanceId=a.AttandanceId AND sa.StaffId=@staffId)
	BEGIN
	--If not, Insert new data 
		INSERT INTO [ATTANDANCES]
           ([Date]
           ,[IsAbsent])
		VALUES
           (@date
           ,@isAbsent);
		--Get id of the newest data inserted
		DECLARE @newId bigint;
		SELECT @newId= MAX(AttandanceId) FROM ATTANDANCES;
		--Insert into constrain table 
		INSERT INTO [STAFF_GIVES_DAILY_ATTANDANCE]
           ([StaffId]
           ,[AttandanceId])
		VALUES
           (@staffId
           ,@newId)
	END
	ELSE
	BEGIN
	--If exists then update that record
		DECLARE @atId bigint;
		--Get Id of the record to be updated
		SELECT @atId=AttandanceId FROM STAFF_GIVES_DAILY_ATTANDANCE WHERE StaffId=@staffId;
		UPDATE ATTANDANCES SET IsAbsent=@isAbsent WHERE AttandanceId=@atId;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SetStudentAttandance]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetStudentAttandance] 
	@studentId int,
	@isAbsent bit,
	@date date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Check if the same data is already present or not
    IF NOT EXISTS(SELECT * FROM ATTANDANCES a, STUDENT_GIVES_DAILY_ATTANDANCE sa WHERE a.[Date]=@date AND sa.AttandanceId=a.AttandanceId AND sa.StudentId=@studentId)
	BEGIN
	--If not Insert new data 
		INSERT INTO [ATTANDANCES]
           ([Date]
           ,[IsAbsent])
		VALUES
           (@date
           ,@isAbsent);
		--Get id of the newest data inserted
		DECLARE @newId bigint;
		SELECT @newId= MAX(AttandanceId) FROM ATTANDANCES;
		--Insert into constrain table 
		INSERT INTO [STUDENT_GIVES_DAILY_ATTANDANCE]
			([AttandanceId]
			,[StudentId])
		VALUES
			(@newId
			, @studentId);
	END
	ELSE
	BEGIN
	--If exists then update that record
		DECLARE @atId bigint;
		--Get Id of the record to be updated
		SELECT @atId=AttandanceId FROM STUDENT_GIVES_DAILY_ATTANDANCE WHERE StudentId=@studentId;
		UPDATE ATTANDANCES SET IsAbsent=@isAbsent WHERE AttandanceId=@atId;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[StudentExamResult]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[StudentExamResult] 
	@studentId int,
	@examId int

AS
BEGIN
	DECLARE 
		@classId int,
		@rank int;
	--Make a new table which will contain only those students who are in same class--
	Declare @rankTbl Table(
		stuId int NOT NULL,
		[percentage] decimal NOT NULL
		);
	--Getting classId from studentId--
	SELECT @classId=s.ClassId 
	FROM SECTIONS s, STUDENTS st 
	WHERE s.SectionId=st.SectionId AND st.StudentId=@studentId;
	--Inserting the percentages(on which the student will be ranked) into the table made above--
	INSERT INTO @rankTbl(stuId, [percentage])
	SELECT ex.StudentId, (SUM(ex.ObtainedMarks)/SUM(ex.TotalMarks))*100
	FROM STUDENTS_GET_EXAM_RESULTS ex, (
		SELECT st.StudentId
		FROM STUDENTS st, SECTIONS s
		WHERE st.SectionId=s.SectionId AND s.ClassId=@classId) s
	WHERE ExamId=@examId AND ex.StudentId=s.StudentId
	GROUP BY ex.StudentId;
	--Getting final result with the rank of the student--
	SELECT e.SubjectId, e.ObtainedMarks, e.TotalMarks, e.TeacherRemarks, r.[rank]
	FROM STUDENTS_GET_EXAM_RESULTS e, (SELECT stuId, DENSE_RANK() OVER (ORDER BY [percentage] DESC) as [rank] FROM @rankTbl) r
	WHERE e.ExamId=@examId AND e.StudentId=@studentId AND r.stuId=@studentId;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateStaffCNIC]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateStaffCNIC]
	@cnic nchar(15),
	@staffId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM STAFF WHERE CNIC=@cnic)
	BEGIN
		UPDATE STAFF SET [CNIC]=@cnic WHERE StaffId=@staffId	
	END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateStudentSection]    Script Date: 3/28/2019 1:40:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateStudentSection] 
	@studentId int,
	@sectionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Update Section
	update STUDENTS set SectionId= @sectionId where StudentId=@studentId;

	DECLARE @classId int, @rollNo int, @sClassId int;
    --Getting ClassId from SectionId to calculate rollNo--
	Select @classId = ClassId from SECTIONS where SectionId=@sectionId;
	--Getting Class Id from where the student belongs--
	SELECT @sClassId=s.ClassId FROM SECTIONS s, STUDENTS st WHERE s.SectionId=st.SectionId AND st.StudentId=@studentId; 
	--Checking weather if the previous and current sections belong to the same class--
	IF(@classId !=@sClassId)
	BEGIN
		--If there is no Record exists for the class, setting the roll no to the Roll Number Index of the Class--
		if not exists(Select StudentId from STUDENTS where SectionId=(select SectionId from SECTIONS where ClassId=@classId))
			BEGIN
				Select @rollNo=RollNoIndex from CLASSES where ClassId=@classId;
			END
		--But if there is already students entered in that class then get the maximum roll no and add 1 to that--
		else
			BEGIN
				Select @rollNo=MAX(RollNo)+1 from STUDENTS where SectionId=(select SectionId from SECTIONS where ClassId=@classId);
			END

		--Updating Roll NO
		UPDATE STUDENTS SET RollNo=@rollNo where StudentId=@studentId;
	END
END
GO
USE [master]
GO
ALTER DATABASE [SMSDatabase] SET  READ_WRITE 
GO
