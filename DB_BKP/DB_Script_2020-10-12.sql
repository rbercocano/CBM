USE [master]
GO
/****** Object:  Database [DB_A625FD_rbercocano]    Script Date: 10/12/2020 8:35:54 PM ******/
CREATE DATABASE [DB_A625FD_rbercocano]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_A625FD_rbercocano_Data', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\\DB_A625FD_rbercocano_DATA.mdf' , SIZE = 8192KB , MAXSIZE = 2048000KB , FILEGROWTH = 10%)
 LOG ON 
( NAME = N'DB_A625FD_rbercocano_Log', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DB_A625FD_rbercocano_Log.LDF' , SIZE = 3072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_A625FD_rbercocano].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET  MULTI_USER 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET QUERY_STORE = OFF
GO
USE [DB_A625FD_rbercocano]
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertMeasure]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ConvertMeasure] 
(
	@SourceMeasureUnitId INT,
	@Quantity Decimal(18,2),
	@DestinationMeasureUnitId INT
) RETURNS Decimal(18,4)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result Decimal(18,4);
	DECLARE @ResultInUnits Decimal(18,4);
	IF(@SourceMeasureUnitId = @DestinationMeasureUnitId)
		RETURN @Quantity;

	IF @SourceMeasureUnitId = 1
		SET @ResultInUnits = @Quantity;
	ELSE IF @SourceMeasureUnitId = 2
		SET @ResultInUnits = @Quantity * 453.592;
	ELSE IF (@SourceMeasureUnitId = 3 OR @SourceMeasureUnitId = 5)
		SET @ResultInUnits = @Quantity * 0.001;
	ELSE IF (@SourceMeasureUnitId = 4 OR @SourceMeasureUnitId = 6)
		SET @ResultInUnits = @Quantity * 1000;	
	ELSE IF @SourceMeasureUnitId = 7
		SET @ResultInUnits = @Quantity * 28.3495;

	
	IF @DestinationMeasureUnitId = 1
		SET @Result = @ResultInUnits;
	ELSE IF @DestinationMeasureUnitId = 2
		SET @Result = @ResultInUnits / 453.592;
	ELSE IF (@DestinationMeasureUnitId = 3 OR @DestinationMeasureUnitId = 5)
		SET @Result = @ResultInUnits / 0.001;
	ELSE IF (@DestinationMeasureUnitId = 4 OR @DestinationMeasureUnitId = 6)
		SET @Result = @ResultInUnits / 1000;
	ELSE IF @DestinationMeasureUnitId = 7
		SET @Result = @ResultInUnits / 28.3495;

	RETURN @Result

END
GO
/****** Object:  Table [dbo].[DataSheetItem]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataSheetItem](
	[DataSheetItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[DataSheetId] [bigint] NOT NULL,
	[RawMaterialId] [bigint] NOT NULL,
	[Percentage] [decimal](9, 5) NOT NULL,
	[AdditionalInfo] [varchar](200) NULL,
	[IsBaseItem] [bit] NOT NULL,
 CONSTRAINT [PK_DataSheetItem] PRIMARY KEY CLUSTERED 
(
	[DataSheetItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[MeasureUnitId] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[ActiveForSale] [bit] NOT NULL,
	[CorpClientId] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RawMaterial]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RawMaterial](
	[RawMaterialId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Price] [decimal](18, 4) NOT NULL,
	[CorpClientId] [int] NOT NULL,
	[MeasureUnitId] [int] NOT NULL,
 CONSTRAINT [PK_RawMaterial] PRIMARY KEY CLUSTERED 
(
	[RawMaterialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataSheet]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataSheet](
	[DataSheetId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[ProcedureDescription] [varchar](max) NULL,
	[WeightVariationPercentage] [decimal](4, 2) NOT NULL,
	[IncreaseWeight] [bit] NOT NULL,
 CONSTRAINT [PK_DataSheet] PRIMARY KEY CLUSTERED 
(
	[DataSheetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwProductCost]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwProductCost]
AS
SELECT 
	X.ProductId,
	ROUND(SUM(X.Cost),2) AS Cost
FROM 
	(SELECT
		P.ProductId,
	CASE 
		WHEN (R.MeasureUnitId = 5 OR  R.MeasureUnitId = 6)
			THEN R.Price / dbo.ConvertMeasure(R.MeasureUnitId,1,5) 
		ELSE R.Price / dbo.ConvertMeasure(R.MeasureUnitId,1,1)
	END * 
	CASE 
		WHEN D.IncreaseWeight = 1 THEN
		(dbo.ConvertMeasure(P.MeasureUnitId,1,1) / (1+(CAST(D.WeightVariationPercentage as DECIMAL(4,2))/100))) * I.Percentage/100
		ELSE
		(dbo.ConvertMeasure(P.MeasureUnitId,1,1) / (1-(CAST(D.WeightVariationPercentage as DECIMAL(4,2))/100))) * I.Percentage/100
		END AS Cost
FROM Product P
JOIN DataSheet D ON P.ProductId = D.ProductId
JOIN DataSheetItem I ON D.DataSheetId = I.DataSheetId
JOIN RawMaterial R ON I.RawMaterialId = R.RawMaterialId
) X 
GROUP BY 
	X.ProductId


GO
/****** Object:  Table [dbo].[ContactType]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactType](
	[ContactTypeId] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Icon] [varchar](20) NOT NULL,
 CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED 
(
	[ContactTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CorpClient]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CorpClient](
	[CorpClientId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[DBAName] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Email] [varchar](200) NOT NULL,
	[Mobile] [varchar](20) NOT NULL,
	[Currency] [varchar](4) NOT NULL,
 CONSTRAINT [PK_CorpClient] PRIMARY KEY CLUSTERED 
(
	[CorpClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerTypeId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdated] [datetime] NULL,
	[CorpClientId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[DBAName] [varchar](100) NULL,
	[CPF] [varchar](14) NULL,
	[CNPJ] [varchar](18) NULL,
	[LastName] [varchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerContact]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerContact](
	[CustomerContactId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[Contact] [varchar](300) NOT NULL,
	[AdditionalInfo] [varchar](200) NULL,
	[CustomerId] [bigint] NULL,
 CONSTRAINT [PK_CustomerContact] PRIMARY KEY CLUSTERED 
(
	[CustomerContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerType]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerType](
	[CustomerTypeId] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED 
(
	[CustomerTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeasureUnit]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasureUnit](
	[MeasureUnitId] [int] NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[ShortName] [varchar](10) NOT NULL,
	[MeasureUnitTypeId] [int] NOT NULL,
 CONSTRAINT [PK_MeasureUnit] PRIMARY KEY CLUSTERED 
(
	[MeasureUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeasureUnitType]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasureUnitType](
	[MeasureUnitTypeId] [int] NOT NULL,
	[MeasureUnitType] [varchar](20) NOT NULL,
 CONSTRAINT [PK_MeasureUnitType] PRIMARY KEY CLUSTERED 
(
	[MeasureUnitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[CustomerId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdated] [datetime] NULL,
	[CompleteBy] [datetime] NOT NULL,
	[OrderStatusId] [int] NOT NULL,
	[PaymentStatusId] [int] NOT NULL,
	[FreightPrice] [decimal](18, 2) NOT NULL,
	[PaidOn] [datetime] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[OrderItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[ItemNumber] [int] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Quantity] [decimal](10, 2) NOT NULL,
	[OrderItemStatusId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdated] [datetime] NULL,
	[AdditionalInfo] [varchar](200) NULL,
	[OriginalPrice] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[PriceAfterDiscount] [decimal](18, 2) NOT NULL,
	[MeasureUnitId] [int] NOT NULL,
	[ProductPrice] [decimal](18, 2) NOT NULL,
	[LastStatusDate] [datetime] NULL,
	[Cost] [decimal](18, 2) NULL,
	[Profit] [decimal](18, 2) NULL,
	[ProfitPercentage] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItemStatus]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItemStatus](
	[OrderItemStatusId] [int] NOT NULL,
	[Description] [varchar](30) NOT NULL,
 CONSTRAINT [PK_OrderItemStatus] PRIMARY KEY CLUSTERED 
(
	[OrderItemStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[OrderStatusId] [int] NOT NULL,
	[Description] [varchar](30) NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[OrderStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentStatus]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentStatus](
	[PaymentStatusId] [int] NOT NULL,
	[Description] [varchar](30) NOT NULL,
 CONSTRAINT [PK_PaymentStatus] PRIMARY KEY CLUSTERED 
(
	[PaymentStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleModule]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleModule](
	[RoleModuleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[SystemModuleId] [int] NOT NULL,
 CONSTRAINT [PK_RoleModule] PRIMARY KEY CLUSTERED 
(
	[RoleModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemModule]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemModule](
	[SystemModuleId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [varchar](50) NOT NULL,
	[Route] [varchar](100) NULL,
	[Active] [bit] NOT NULL,
	[Order] [int] NOT NULL,
	[IsMenu] [bit] NOT NULL,
 CONSTRAINT [PK_SystemModule] PRIMARY KEY CLUSTERED 
(
	[SystemModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[Active] [bit] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NULL,
	[RoleId] [int] NOT NULL,
	[CorpClientId] [int] NULL,
	[Password] [varchar](50) NOT NULL,
	[Email] [varchar](200) NULL,
	[HomePhone] [varchar](20) NULL,
	[Mobile] [varchar](20) NULL,
	[DateOfBirth] [date] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUpdated] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserToken]    Script Date: 10/12/2020 8:35:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserToken](
	[UserId] [bigint] NOT NULL,
	[Token] [varchar](max) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserTokens_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[ContactType] ([ContactTypeId], [Description], [Icon]) VALUES (1, N'WhatsApp', N'whatsapp_60px.png')
INSERT [dbo].[ContactType] ([ContactTypeId], [Description], [Icon]) VALUES (2, N'E-Mail', N'email_60px.png')
INSERT [dbo].[ContactType] ([ContactTypeId], [Description], [Icon]) VALUES (3, N'Telefone', N'phone_60px.png')
INSERT [dbo].[ContactType] ([ContactTypeId], [Description], [Icon]) VALUES (4, N'Celular', N'mobile_60px.png')
GO
SET IDENTITY_INSERT [dbo].[CorpClient] ON 

INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (1, N'Charcutaria Amaral', N'Charcutaria Amaral', 1, CAST(N'2020-05-01T08:38:34.707' AS DateTime), N'rbercocano@gmail.com', N'+1 916 402 6099', N'$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (2, N'Curato', N'Curato', 1, CAST(N'2020-06-02T00:00:00.000' AS DateTime), N'navarro@curato.com.br', N'+55 11 97336-9793', N'R$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (3, N'Hugo Sobrinho', N'PorcoPuto', 0, CAST(N'2020-06-04T00:00:00.000' AS DateTime), N'hugolsobrinho@gmail.com', N'+55 11 95606-2949', N'R$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (4, N'Donanna Salumeria Familiare', N'Donanna Salumeria Familiare', 1, CAST(N'2020-06-05T00:00:00.000' AS DateTime), N'donannasalumeria@gmail.com', N'+55 19 99719-0813', N'R$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (6, N'ColdSmoke Charcutaria', N'ColdSmoke Charcutaria', 1, CAST(N'2020-06-08T00:00:00.000' AS DateTime), N'cesar@coldsmoke.com.br', N'+55 19 98338-3464', N'R$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (7, N'Chef aqui em Casa', N'Chef aqui em Casa', 1, CAST(N'2020-06-12T00:00:00.000' AS DateTime), N'chef@chefaquiemcasa.com.br', N'+55 19 98342-7905', N'R$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (9, N'Ricardo Giannoccaro 
Salumi ', N'Ricardo Giannoccaro 
Salumi ', 1, CAST(N'2020-06-12T00:00:00.000' AS DateTime), N'Ricardo.gianno@gmail.com', N'+55 11 98929-4331', N'R$')
INSERT [dbo].[CorpClient] ([CorpClientId], [Name], [DBAName], [Active], [CreatedOn], [Email], [Mobile], [Currency]) VALUES (10, N'Fit Sou', N'Fit Sou', 1, CAST(N'2020-09-24T16:28:37.663' AS DateTime), N'jeh.maia@gmail.com', N'+1 916 365 7408', N'$')
SET IDENTITY_INSERT [dbo].[CorpClient] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (1, 1, CAST(N'2020-05-02T15:12:23.363' AS DateTime), CAST(N'2020-05-21T09:38:54.267' AS DateTime), 1, N'Marcus', NULL, N'', NULL, N'', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (2, 1, CAST(N'2020-05-16T06:33:46.677' AS DateTime), CAST(N'2020-05-26T09:44:39.943' AS DateTime), 1, N'Daniele', NULL, N'', NULL, N'Lopes', CAST(N'2010-06-01T07:00:00.000' AS DateTime))
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (3, 1, CAST(N'2020-05-16T06:41:06.823' AS DateTime), CAST(N'2020-05-16T06:41:17.017' AS DateTime), 1, N'Michelle', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (4, 2, CAST(N'2020-05-16T18:37:51.780' AS DateTime), NULL, 1, N'XXX', N'123', NULL, N'12.311.111/1111-11', NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (5, 1, CAST(N'2020-05-16T18:45:46.777' AS DateTime), NULL, 1, N'Angélica', NULL, N'', NULL, N'Mota', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (6, 1, CAST(N'2020-05-16T18:55:31.597' AS DateTime), CAST(N'2020-06-05T08:09:07.230' AS DateTime), 1, N'Victor', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (7, 1, CAST(N'2020-05-16T18:55:57.170' AS DateTime), CAST(N'2020-05-26T10:01:52.513' AS DateTime), 1, N'Flávia', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (8, 1, CAST(N'2020-05-16T18:57:54.990' AS DateTime), NULL, 1, N'Emmerson', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (9, 2, CAST(N'2020-05-16T18:59:00.547' AS DateTime), NULL, 1, N'Curato', N'Curato Charcutaria Artesanal', NULL, N'12.312.211/1111-11', NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (10, 1, CAST(N'2020-05-21T08:33:19.783' AS DateTime), CAST(N'2020-05-25T17:16:11.337' AS DateTime), 1, N'Julien', NULL, N'123.123.123-12', NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (11, 1, CAST(N'2020-05-21T11:16:05.343' AS DateTime), NULL, 1, N'Damaris', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (12, 1, CAST(N'2020-05-23T08:50:23.760' AS DateTime), NULL, 1, N'Aline', NULL, NULL, NULL, N'Lopes', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (13, 1, CAST(N'2020-05-26T09:57:09.867' AS DateTime), NULL, 1, N'Thania', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (14, 1, CAST(N'2020-05-26T10:02:58.343' AS DateTime), CAST(N'2020-05-26T10:03:25.053' AS DateTime), 1, N'Juliana', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (15, 1, CAST(N'2020-05-26T21:14:01.430' AS DateTime), NULL, 1, N'Cassio', NULL, N'991.839.123-12', NULL, N'Zacarias', CAST(N'2000-05-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (16, 1, CAST(N'2020-06-03T14:14:03.193' AS DateTime), NULL, 1, N'Rubens', NULL, NULL, NULL, N'Bueno', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (17, 1, CAST(N'2020-06-06T08:20:10.420' AS DateTime), NULL, 1, N'Jô', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (18, 1, CAST(N'2020-06-07T13:19:33.987' AS DateTime), NULL, 1, N'Vanessa', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (19, 2, CAST(N'2020-06-08T15:53:36.047' AS DateTime), NULL, 6, N'Coldsmoke Charcutaria', N'Coldsmoke Fabricação de Produtos de Carne Eireli', NULL, N'33.143.067/0001-34', NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (20, 1, CAST(N'2020-06-09T09:07:01.050' AS DateTime), NULL, 1, N'Wellington', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (21, 1, CAST(N'2020-06-09T09:07:31.757' AS DateTime), NULL, 1, N'Atila', NULL, NULL, NULL, N'Santana', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (22, 1, CAST(N'2020-06-15T07:25:39.073' AS DateTime), NULL, 1, N'Patricia', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (23, 1, CAST(N'2020-06-15T07:26:12.627' AS DateTime), NULL, 1, N'Ericka', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (24, 1, CAST(N'2020-06-19T11:17:21.733' AS DateTime), NULL, 1, N'Maristela', NULL, NULL, NULL, N'D`Best', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (25, 1, CAST(N'2020-06-25T09:11:38.637' AS DateTime), NULL, 1, N'Cleidson', NULL, NULL, NULL, N'Ramos', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (26, 1, CAST(N'2020-06-25T09:11:59.980' AS DateTime), NULL, 1, N'Indy', NULL, NULL, NULL, N'Camargo', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (27, 1, CAST(N'2020-06-25T11:40:59.337' AS DateTime), NULL, 1, N'Bruna', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (28, 1, CAST(N'2020-06-27T16:38:41.657' AS DateTime), NULL, 1, N'Guto', NULL, NULL, NULL, N'', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (29, 1, CAST(N'2020-07-06T07:48:42.173' AS DateTime), NULL, 1, N'Iany', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (30, 1, CAST(N'2020-07-10T06:56:30.667' AS DateTime), NULL, 1, N'Carlos', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (31, 1, CAST(N'2020-07-12T11:28:23.327' AS DateTime), NULL, 1, N'Andreia', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (32, 1, CAST(N'2020-07-14T13:02:32.907' AS DateTime), NULL, 1, N'Jessica', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (33, 1, CAST(N'2020-07-18T12:35:07.837' AS DateTime), NULL, 1, N'Claudine', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (34, 1, CAST(N'2020-07-30T12:50:17.693' AS DateTime), NULL, 1, N'Debora', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (35, 1, CAST(N'2020-08-02T17:09:33.340' AS DateTime), NULL, 1, N'Dorinha', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (36, 1, CAST(N'2020-08-02T17:10:02.770' AS DateTime), NULL, 1, N'Katia', NULL, NULL, NULL, N'Emilia', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (37, 1, CAST(N'2020-08-14T08:57:02.427' AS DateTime), NULL, 1, N'Adam', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (38, 1, CAST(N'2020-09-08T16:08:49.087' AS DateTime), NULL, 1, N'Michele', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (39, 1, CAST(N'2020-09-17T07:06:55.860' AS DateTime), NULL, 1, N'Ramon', NULL, NULL, NULL, N'Corralez', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (40, 1, CAST(N'2020-09-20T07:40:08.857' AS DateTime), CAST(N'2020-09-20T07:40:28.477' AS DateTime), 1, N'Thais', NULL, NULL, NULL, N'Sato', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (41, 1, CAST(N'2020-09-22T16:33:14.560' AS DateTime), NULL, 1, N'Luciana', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (42, 1, CAST(N'2020-09-22T17:13:15.927' AS DateTime), NULL, 1, N'Almendes', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (43, 1, CAST(N'2020-09-22T18:08:04.627' AS DateTime), NULL, 1, N'Andrea', NULL, NULL, NULL, N'Harrington', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (44, 1, CAST(N'2020-09-27T16:19:30.833' AS DateTime), NULL, 10, N'Dani', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (45, 1, CAST(N'2020-09-30T11:14:44.493' AS DateTime), NULL, 1, N'Eunice', NULL, NULL, NULL, N'Lemos', NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (46, 1, CAST(N'2020-10-02T20:28:01.473' AS DateTime), NULL, 1, N'Giuliane', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([CustomerId], [CustomerTypeId], [CreatedOn], [LastUpdated], [CorpClientId], [Name], [DBAName], [CPF], [CNPJ], [LastName], [DateOfBirth]) VALUES (47, 1, CAST(N'2020-10-12T07:32:40.040' AS DateTime), NULL, 1, N'Esther', NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerContact] ON 

INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (13, 1, N'+1 (650) 276-8859', N'', 1)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (14, 1, N'+1 (314) 570-9491', NULL, 8)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (15, 1, N'12312312', NULL, 9)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (16, 1, N'+1 (916) 534-9377', NULL, 3)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (17, 1, N'+1 (415) 216-7875', NULL, 10)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (18, 1, N'+1 (916) 254-1972', NULL, 11)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (19, 1, N'+1 (916) 696-1460', NULL, 5)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (21, 1, N'+1 (916) 718-5852', NULL, 2)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (22, 1, N'+1 (707) 365-3372', NULL, 13)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (23, 1, N'+1 (916) 737-6437', NULL, 7)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (24, 1, N'+1 (916) 578-7937', NULL, 14)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (25, 1, N'+55 11 98401-5727', NULL, 15)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (26, 2, N'cfernandes@gmail.com', NULL, 15)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (27, 4, N'+55 11 98989 9999', N'Pessoal', 15)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (28, 1, N'+1 (916) 997-1393', NULL, 16)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (29, 1, N'+1 (908) 922-6780', NULL, 6)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (30, 1, N'+1 (916) 934-6095', NULL, 18)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (31, 1, N'César', N'19 983383464', 19)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (32, 1, N'+1 (916) 895-3453', NULL, 20)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (33, 1, N'+1 (916) 621-7886', NULL, 21)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (34, 1, N'+1 (916) 953-4110', NULL, 22)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (35, 1, N'+1 (415) 622-5363', NULL, 23)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (36, 1, N'+1 (415) 410-4140', NULL, 24)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (37, 1, N'+1 (707) 474-2093', NULL, 25)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (38, 1, N'+1 (916) 559-1001', NULL, 26)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (39, 1, N'+1 (510) 253-7592', NULL, 27)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (40, 1, N'+1 (786) 604-4494', NULL, 28)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (41, 1, N'+1 (774) 946-7267', NULL, 29)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (42, 1, N'+1 (530) 219-3006', NULL, 30)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (43, 1, N'+1 (214) 5017-5774', NULL, 31)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (44, 1, N'+1 (510) 334-8723', NULL, 32)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (45, 1, N'(916) 412-5936', NULL, 33)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (46, 1, N'+1 (279) 333-6012', NULL, 34)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (47, 1, N'+1 (916) 804-8720', NULL, 35)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (48, 1, N'+1 (207) 641-7637', NULL, 36)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (50, 1, N'+1 (916) 585-0023', NULL, 37)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (51, 1, N'+1 (916) 882-5581', NULL, 38)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (52, 1, N'+1 (916) 695-5830', NULL, 39)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (53, 1, N'+1 (510) 666-7695', NULL, 40)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (54, 1, N'+1 (408) 614-8356', NULL, 41)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (55, 1, N'+1 (916) 548-3310', NULL, 42)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (56, 1, N'+1 (585) 469-9336', NULL, 43)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (57, 1, N'(916)718-5852', NULL, 44)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (58, 1, N'+1 (530) 701-3485', NULL, 45)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (59, 1, N'+1 (415) 225-7262', NULL, 46)
INSERT [dbo].[CustomerContact] ([CustomerContactId], [ContactTypeId], [Contact], [AdditionalInfo], [CustomerId]) VALUES (60, 1, N'+1 (415) 810-3649', NULL, 47)
SET IDENTITY_INSERT [dbo].[CustomerContact] OFF
GO
INSERT [dbo].[CustomerType] ([CustomerTypeId], [Description]) VALUES (1, N'Pessoa Física')
INSERT [dbo].[CustomerType] ([CustomerTypeId], [Description]) VALUES (2, N'Pessoa Jurídica')
GO
SET IDENTITY_INSERT [dbo].[DataSheet] ON 

INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (1, 11, N'<h3>Bacon Defumado</h3><p><strong>Para esta receita utilizamos a técnica de salga seca (Dry Rub)</strong></p><p><strong>Modo de Preparo:</strong></p><ol><li>Faça a toilet da carne retirando membranas, nervuras e dando formato a peça;</li><li>Comece polvilhando o sal de cura pela peça massageando-a. Não é necessário passar na parte do couro;</li><li>Em seguida faça o mesmo com o sal de cozinha;</li><li>Embale a peça em à vacuo ou em um zip lock retirando o máximo de ar possível da embalagem;</li><li>Guarde a peça na geladeira por pelo menos 7 dias ( 7 dias para cada polegada de altura da barriga );</li><li>Após o período de cura, retire-a da geladeira e lave-a bem com água filtrada;</li><li>Aplique a fumaça líquida;</li><li>Submeta a peça ao processo de rampa de cozimento até que a mesma atinja a temperatura interna de 72 graus internamente.</li></ol><p><strong>Rampa de Cozimento</strong></p><ol><li>Primera hora com forno em 60 graus celsius;</li><li>Segunda hora com forno em 70 graus celsius;</li><li>Terceira hora em diante com forno em 80 graus celsius até atinjir a temperatura desejada.</li></ol>', CAST(13.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (2, 10, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (3, 9, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (4, 8, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (5, 14, N'<h3>Panceta Curada</h3>', CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (6, 16, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (7, 15, NULL, CAST(50.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (8, 12, N'<p>Paio Defumado</p>', CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (9, 13, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (10, 18, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (11, 19, NULL, CAST(0.00 AS Decimal(4, 2)), 1)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (12, 20, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (13, 21, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (14, 22, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (15, 23, N'<p><strong>Penaut Butter Cookie</strong></p><p>&nbsp;</p><p>- 70 gr penaut butter (5 tbsp)</p><p>- A large egg - 50gr</p><p>- 5ml 1 tsp vanilla extract</p><p>- 14gr (1 tbsp) melted coconut oil</p><p>- 112gr (1 cup) almond flour</p><p>- 42gr (3 tbsp) sweetner</p><p>- ¼ cup (35 gr) chocolate chips</p><p>- ½ tsp baking soda 2.5gr</p><p>- ¼ tsp baking powder 1.25gr</p><p>- pinch of salt</p><p>&nbsp;</p><p>Mix everything and bake.</p><ul><li>Peso total dos ingredientes: 331,75 gr</li></ul><p>Rendimento por receita: 300 gr de cookies ( 12 unidades no total)</p><p>&nbsp;</p>', CAST(9.57 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (16, 24, N'<h3><strong>Chicken Pie</strong></h3><p><strong>Massa</strong>:</p><ul><li>3 ovos</li><li>200g (1 xíc) de creme de leite (ou <a href="https://www.senhortanquinho.com/leite-coco-farinha-coco-como-fazer-caseiro/"><strong>leite de coco</strong></a>)</li><li>150g (1,5 xic) de <a href="https://www.senhortanquinho.com/farinha-low-carb-qual-quando-usar-receitas-amendoas-amendoim-berinjela-castanha-coco-frango-linhaca-maracuja-nozes-aveia-banana-grao-de-bico/"><strong>farinha low-carb</strong></a> (recomenda-se amêndoas)</li><li>100g (4 cs) de <a href="https://www.senhortanquinho.com/gorduras-oleos-para-cozinhar-melhores-piores-saude/"><strong>manteiga ou óleo de coco</strong></a> amolecidos</li><li>1 colher de sopa de fermento químico</li><li>Sal a gosto</li><li>&nbsp;</li><li><strong>Sugestão de recheio (veja outras abaixo):</strong><ul><li>300g de <a href="https://www.senhortanquinho.com/dieta-low-carb-variacoes-qual-melhor-opcao/"><strong>frango</strong></a> cozido e desfiado</li><li>200g de <a href="https://www.senhortanquinho.com/requeijao-low-carb-caseiro-receita-cetogenica/"><strong>requeijão</strong></a></li><li>200g de palmito em cubos</li><li><a href="https://www.senhortanquinho.com/low-carb-vegetariano-lista-de-compras/"><strong>Azeitona</strong></a> a gosto</li><li>Cebola picada a gosto</li><li>Salsinha, cebolinha, curcuma, páprica defumada, e sal a gosto</li><li>Fatias de <strong>mussarela</strong> (para cobrir a torta e gratinar)</li><li>Queijo parmesão ralado a gosto (para cobrir)</li></ul></li></ul><h4>&nbsp;</h4><h4>Preparo:</h4><ul><li><strong>Massa:</strong><ul><li>Em um liquidificador, misturar todos os ingredientes da “massa” até que esteja homogêneo;</li><li>Despejar metade da massa em uma assadeira untada ou antiaderente;</li><li>Distribuir o recheio por cima dessa primeira camada de massa;</li><li>Cobrir o recheio com o restante da massa;</li><li>Cobrir a torta com fatias de muçarela e queijo parmesão ralado;</li><li>Levar ao forno pre aquecido a 180ºC por cerca de 40 minutos (sempre verificando para evitar que queime);</li><li>Retirar, fatiar, e servir.</li><li>&nbsp;</li></ul></li><li><strong>Recheio:</strong><ul><li>Misturar todos os ingredientes em uma panela em fogo baixo;</li><li>Reservar até a hora de utilizar na torta.</li></ul></li></ul>', CAST(0.00 AS Decimal(4, 2)), 1)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (17, 25, N'<h3><strong>Garlic Flatbread</strong></h3><p>&nbsp;</p><p><strong>Receita</strong></p><p>170 g de mussarela ralada<br>85 g de farinha de amêndoas<br>60 g de creme de ricota<br>½ col de sopa de fermento químico<br>1 ovo<br>1 colher de chá de goma xantana<br>1 dente de alho espremido<br>salsinha a gosto<br>1 col chá de sal<br>cobertura: azeite qto baste 1 alho espremido salsinha a gosto sal e pimenta do reino a gosto</p><p>&nbsp;</p><p><strong>Preparo</strong></p><p>Coloque todos os ingredientes num recipiente de vidro e leve ao microondas para derreter por 1 minuto. Depois adicione 1 ovo até obter uma massa homogênea. Abra a massa sobre tapete de silicone, e faça recortes com uma faca. Depois misture o azeite, o olho e a salsinha e tempere. Pincele o pão , pode polvilhar queijo ralado por cima e asse em forno quente 180 graus por aproximadamente 20 minutos.</p>', CAST(0.00 AS Decimal(4, 2)), 1)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (18, 26, N'<p>Pizza</p><h3>Ingredients</h3><h4>&nbsp;</h4><ul><li>1½ cups shredded mozzarella - 360gr</li><li>2 tbs cream cheese - 30gr</li><li>1 cup almond flour - 240 gr</li><li>1 egg, beaten - 50 gr</li><li>&nbsp;</li></ul><h3>Instructions</h3><ol><li>Place the mozzarella and cream cheese in a medium size microwaveable bowl.</li><li>Microwave for 1 minute, stir and then cook for another 30 seconds.</li><li>Stir in the almond flour and beaten egg.</li><li>Let the dough cool slightly, then knead until smooth. Add a little extra almond flour if the dough is too sticky, then knead again.</li></ol><h3>Notes</h3><p>&nbsp;</p><p>4g net carbs per serving (one quarter of the recipe)</p><p>For information on how to cook, please see one of the specific recipes.</p><p>&nbsp;</p><p>Peso total dos ingredientes: 680 gramas</p><p>Rendimento: 164gr ( 2 unidades de 7 inches) - 10 dólares 2 unidades</p>', CAST(51.76 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (19, 27, N'<h3>Fudge Keto Brownie</h3><ul><li>&nbsp;</li><li><strong>Ingredients</strong></li><li>&nbsp;</li><li>1/2 cup <strong>Butter - 120gr</strong></li><li>4 oz <strong>Unsweetened baking chocolate - 120gr</strong></li><li>3/4 cup <strong>Almond Flour - 180gr</strong></li><li>2/3 cup <strong>Besti Powdered Allulose</strong> (or powdered erythritol) - 160gr</li><li>2 tbsp <strong>Cocoa powder - 30gr</strong></li><li>2 large <strong>Eggs</strong> (at room temperature) - 100gr</li><li>1 tsp <strong>Vanilla extract</strong> (optional) - 5ml</li><li>1/4 tsp <strong>Sea salt</strong> (only if using unsalted butter) - 1,25gr</li><li>1/4 cup <strong>Walnuts</strong> (optional, chopped) - 60gr</li></ul><p>&nbsp;</p><p><strong>Instructions</strong></p><p>Preheat the oven to 350 degrees F (177 degrees C). Line an 8x8 in (20x20 cm) pan with parchment paper, with the edges of the paper over the sides.</p><p>Melt the butter and chocolate together in a double boiler, stirring occasionally, until smooth. Remove from heat.<br>&nbsp;</p><p>Stir in the vanilla extract.<br>&nbsp;</p><p>Add the almond flour, powdered sweetener, cocoa powder, sea salt, and eggs. Stir together until uniform. The batter will be a little grainy looking.<br>&nbsp;</p><p>Transfer the batter to the lined pan. Smooth the top with a spatula or the back of a spoon. If desired, sprinkle with chopped walnuts and press into the top.</p><p>Bake for about <a href="https://www.wholesomeyum.com/best-fudgy-keto-brownies-recipe/#"><strong>13-18 minutes</strong></a>, until an inserted toothpick comes out almost clean with just a little batter on it that balls up between your fingers. (Do NOT wait for it to come out totally clean, and don''t worry about any butter pooled on top - just watch the actual brownie part to be super soft but not fluid.)</p><p>&nbsp;</p><p>Cool completely before moving or cutting. There may be some butter pooled on top - do not drain it, it will absorb back in after cooling.</p><p>&nbsp;</p><p>Peso total dos ingredientes: 776,25gr</p><p>Rendimento: 560gr</p><p>Uma receita dá 2 unidades ( o valor de cada uma será $8.00)</p>', CAST(27.85 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (20, 28, N'<h4>Banana Bread</h4><p>&nbsp;</p><p><strong>Ingredients</strong></p><ul><li>1 1/4 cups mashed banana (about 3 medium ripe bananas, mashed) - 300gr</li><li>1 teaspoon vanilla extract 5 ml</li><li>1/4 cup almond butter - 60gr</li><li>2 eggs, at room temperature - 100gr</li><li>1/2 cup coconut flour - 120gr</li><li>3/4 teaspoon baking soda* - 3,75gr</li><li>1/2 teaspoon cinnamon - 2,5gr</li><li>1/4 teaspoon salt - 1,25gr</li><li>1/2 cup chocolate chips, dairy free if desired plus extra for sprinkling on top - 120gr</li><li>&nbsp;</li></ul><p><strong>Instructions</strong></p><p>Preheat oven to 350 degrees F. Line a 8x4 inch or 9x5 inch loaf pan with parchment paper and spray with nonstick cooking spray.</p><p>In the bowl of an electric mixer or in a regular bowl, combine bananas, vanilla and almond butter; mix until well combined, smooth, and creamy.&nbsp;</p><p>Add in eggs, one at a time and mix until combined. With the mixer on medium-low speed, add in coconut flour, baking soda, cinnamon and salt; mix again until just combined. Gently fold in chocolate chips.</p><p>Pour batter into prepared pan and smooth top. Sprinkle a few extra chocolate chips on top. Bake for 25-35 minutes or until tester inserted into center comes out clean.&nbsp;</p><p>Remove from oven and place on wire rack to cool for 20 minutes, then carefully invert, remove bread from pan and place back on wire rack to cool completely. Cut into 12 slices.</p><p>&nbsp;</p><p>Peso total dos ingredientes: 712.5gr</p><p>Rendimento da receita - 690gr (1.5lb)</p>', CAST(3.80 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (21, 29, N'<h2><strong>Keto Blueberry Lemon Cheesecake Bars</strong></h2><p>&nbsp;</p><p><strong>Ingredients</strong></p><p><strong>Almond Flour Crust</strong></p><ul><li>8 tablespoons butter - 113gr</li><li>1 1/4 cup almond flour - 155gr</li><li>2 tablespoons swerve sweetener - 25gr</li></ul><p>&nbsp;</p><p><strong>Low Carb Blueberry Sauce</strong></p><ul><li>1 1/2 cup blueberries - 250gr</li><li>1/4 cup water - 42gr</li><li>1/3 cup of confectioners swerve sweetener - 65gr</li></ul><p>&nbsp;</p><p><strong>Lemon Cheesecake Layer</strong></p><ul><li>1 (8 ounce) block cream cheese - 230gr</li><li>1 egg yolk - 19gr</li><li>1/3 cup confectioners swerve - 65gr</li><li>2 tablespoon lemon juice - 20gr</li><li>1 teaspoon lemon zest, tightly packed - 10gr</li><li>1 teaspoon vanilla extract - 5gr</li></ul><p><strong>Coconut Crumble Topping</strong></p><ul><li>2 tablespoons butter - 26gr</li><li>1/4 cup almond flour - 23gr</li><li>1/4 cup unsweetened coconut flakes - 20gr</li><li>1 tablespoons swerve sweetener - 15gr</li></ul><p>&nbsp;</p><p><strong>Instructions</strong></p><p><strong>Blueberry sauce</strong>:&nbsp;</p><p>Add blueberries, swerve sweetener and water. Allow mixutre to simmer until it becomes thick, approximately 10-15 minutes. Set aside.</p><p><strong>Crust</strong>:&nbsp;</p><p>Preheat oven to 350 degrees.<br>Line an 8x8 pan with foil or parchment paper.<br>Combine the melted butter, almond flour and swerve in a small bowl and press into the foil lined pan.<br>Prebake crust for 7 minutes, it should not be firm just slightly beginning to brown around the edges.<br>Remove crust and <strong>allow it to cool. DO NOT add the cheesecake layer while it is hot.</strong></p><p>&nbsp;</p><p><strong>Lemon Cheesecake layer:&nbsp;</strong></p><p>Using an electric mixer or small blender combine the cream cheese, egg yolk, sweetener and lemon juice, zest and extract until smooth.<br><br>Spread the cheesecake layer evenly over the crust.</p><p>&nbsp;</p><p><strong>Blueberry layer</strong>:&nbsp;</p><p>Spread the prepared low carb blueberry sauce over the cheesecake mixture.<br>&nbsp;</p><p><strong>Crumble:&nbsp;</strong></p><p>Combine the butter, almond flour, unsweetened coconut and sweetener in a small blender or food processor and pulse until it resembles a crumb like misture.<br><br>Sprinkle over the blueberry layer.<br><br>Bake 18-20 minutes until the top is lightly browned.<br>Allow bars to cool completely before slicing.</p><p>&nbsp;</p><p><strong>Notes</strong>: Place the bars in the freezer for 15 minutes before slicing to get nice clean slices.</p><p>&nbsp;</p><p>Peso dos ingredientes: 1083gr</p><p>Rendimento: 848gr ( 2 unidades de 424gr) valor unitário:&nbsp;</p>', CAST(21.70 AS Decimal(4, 2)), 0)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (22, 30, N'<h3>Sex in a Pan</h3><p>&nbsp;</p><p><strong>Ingredients</strong></p><p><strong>Chocolate Crust:</strong></p><ul><li>1 ¼ cups almond flour,&nbsp;</li><li>¼ cup cocoa powder,</li><li>¼ cup powdered sweetener</li><li>¼ tsp salt</li><li>¼ cup butter, melted</li><li>1 tbsp water</li></ul><p>&nbsp;</p><p><strong>Cheesecake Layer:</strong></p><ul><li>¼ cup Heavy cream</li><li>¼ cup powdered sweetener,&nbsp;</li><li>½ tsp Vanilla extract,&nbsp;</li><li>8 oz Cream cheese, softened</li></ul><p>&nbsp;</p><p><strong>Chocolate Pudding Layer:</strong></p><ul><li>1 cup Heavy cream</li><li>1 cup unsweetened almond milk,&nbsp;</li><li>½ cup powdered sweetener,&nbsp;</li><li>4 large egg yolks</li><li>½ tsp xanthan gum,&nbsp;</li><li>? cup cocoa powder,&nbsp;</li><li>3 tbsp butter,&nbsp;</li><li>½ tsp Vanilla extract,&nbsp;</li></ul><p>&nbsp;</p><p><strong>Whipped Cream Topping:</strong></p><ul><li>1½ cup Heavy cream</li><li>2 tbsp Powdered erythritol,&nbsp;</li><li>½ tsp Vanilla extract,&nbsp;</li><li>1 oz Sugar-free dark chocolate,&nbsp;</li></ul><p>&nbsp;</p><p>&nbsp;</p><h2>Instructions</h2><p><strong>Crust:</strong></p><ol><li>Preheat the oven to 325°F. Line a 9x9" baking pan with parchment paper.</li><li>In a medium bowl, whisk together the almond flour, cocoa powder, sweetener, and salt. Add the melted butter and water and stir until the mixture begins to clump together.</li><li>Press the mixture Press evenly into the lined baking pan and bake about 15 minutes, or until golden and just firm to the touch. Set aside to cool.</li></ol><p><strong>Cream Cheese Layer:</strong></p><ol><li>While the crust is baking, make the cream cheese layer. Use a hand mixer to beat the cream, vanilla extract, and powdered erythritol together, until stiff peaks form. Gradually beat in the softened cream cheese, a bit at a time, until well combined.</li><li>Once the crust has cooled, spread the cream cheese mixture evenly over it.</li></ol><p><strong>Chocolate Pudding Layer:</strong></p><ol><li>While the crust is cooling, make the chocolate pudding layer.</li><li>In a medium saucepan over medium heat, combine almond milk, whipping cream and sweetener. Bring to a simmer, stirring to dissolve the sweetener.</li><li>In a medium bowl, whisk the egg yolks until smooth. Slowly whisk about 1/2 cup of the hot cream mixture into the yolks to temper. Then slowly whisk tempered yolks back into the saucepan.</li><li>Reduce heat to medium-low and sprinkle the surface with the xanthan gum, whisking vigorously to combine. Whisk in the cocoa powder and cook until thickened, about 3 or 4 minutes. Remove from heat, then whisk in the vanilla extract and butter pieces. Whisk until smooth.</li><li>Cool the chocolate pudding for 15 minutes and then spread over the cheesecake layer. Refrigerate at least 2 hours.</li></ol><p><strong>Whipped Cream Topping:</strong></p><ol><li>Use a hand mixer to beat the cream with sweetener and vanilla extract until stiff peaks form. Spread the whipped cream over the chocolate layer.&nbsp;Dust with cocoa powder (this works best through a fine sieve). Use a vegetable peeler or cheese slicer to shave chocolate over top.</li><li>Refrigerate for 1-2 more hours (or as long as needed until serving) to fully set.</li></ol><p>&nbsp;</p><p><strong>Notes</strong></p><p><strong>Tip:</strong> In Crust you can also add 1/2&nbsp;cup&nbsp;<a href="https://amzn.to/2BV3ttn">pecan meal&nbsp;</a>(or make it by grinding pecans in a food processor) or coarsely chopped if you like.</p>', CAST(0.00 AS Decimal(4, 2)), 1)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (23, 31, N'<h3>Pinnaple Dessert</h3><p>&nbsp;</p><p><strong>Almond flour cake base:</strong></p><ul><li>¾ cup almond flour - 180gr</li><li>1 cup coconut flakes - 240gr</li><li>6 tbsp erythritol - 90gr</li><li>3 eggs - 150gr</li><li>¾ cup vegetable milk (almond, cashew, etc) - 180gr</li><li>2 tbsp coconut oil - 30gr</li><li>1tbsp baking powder - 15gr</li></ul><p>&nbsp;</p><p><strong>Cream </strong>:</p><ul><li>250 ml &nbsp;heavy whipping cream</li><li>250 ml water ( em temperatura ambiente)</li><li>1 tsp vanilla extract - 5gr</li><li>7 egg yolks &nbsp;(120g)</li><li>1 cup erythitol ( 125g)</li><li>1 tsp de psyllium (5g) or ½ tsp xantham gum (2.5gr)</li></ul><p>&nbsp;</p><p><strong>&nbsp;Whipped Cream Topping:</strong></p><ul><li>1½ cup Heavy cream -360gr</li><li>2 tbsp Powdered erythritol, 30gr</li><li>½ tsp Vanilla extract, I use this brand - 2.5gr</li></ul><p>&nbsp;</p><p><strong>Extras:</strong></p><p>1 Pinnaple in cubes - 500gr</p><p>1 cup of coconut flakes - 240gr</p><p>1 cup of coconut milk - 240gr</p><p>&nbsp;</p><p><strong>Instructions</strong></p><p><strong>Almond Flour Cake</strong></p><p>No liquidificador <strong>bata os ovos por 1-2 minutos</strong>, até espumar bem - isso é importante para incorporar ar neles, que ajuda a conseguir um bolo mais leve e fofinho. Em seguida adicione o óleo de coco e a bebida vegetal e bata para misturar.</p><p><i><strong>Dica:</strong> A bebida vegetal pode ser de soja, de amêndoas, de arroz ou outra. Se você tiver intolerância ao glúten, não use bebida de aveia.</i></p><p>&nbsp;</p><p>Numa tigela <strong>misture os ingredientes secos</strong>: a farinha de amêndoas, o coco ralado e o adoçante</p><p>Adicione a mistura do liquidificador nos ingredientes secos e misture com a ajuda de uma espátula ou batedor de arame (fouet).<strong> Deixe a mistura repousar por 10-15 minutos</strong>, desse jeito a farinha de amêndoas e o coco ralado irão absorver os líquidos e a massa do bolo de amêndoas ficará mais consistente.</p><p><i><strong>Dica:</strong> Nesse momento pré-aqueça o forno nos 180ºC.</i></p><p>Por fim <strong>coloque o fermento</strong> e, se quiser, adicione chocolate picado também. Misture para incorporar.</p><p><i><strong>Dica:</strong> O chocolate picado ajuda a conseguir um bolo de amêndoas mais gostoso! No entanto, se você for intolerante à lactose, lembre de usar um chocolate sem leite.</i></p><p>Unte com óleo de coco uma forma de bolo &nbsp;Polvilhe coco ralado e <strong>coloque a massa do bolo</strong> nela.</p><p>Leve seu <strong>bolo fit com farinha de amêndoas</strong> para assar no forno nos 180ºC por 40 minutos ou até passar no <a href="https://www.tudoreceitas.com/artigo-como-fazer-o-teste-do-palito-no-bolo-4654.html">teste do palito</a>. Retire e deixe esfriar completamente antes de desenformar, para evitar que quebre.</p><p>&nbsp;</p><p>&nbsp;</p><p><strong>Cream:</strong></p><ol><li>Em uma panela grande, misture o creme de leite, a essência de baunilha e a água;</li><li>Leve ao fogo médio e deixe ferver por uns cinco minutos;</li><li>Enquanto isso, junte as gemas e adicione o adoçante em outro recipiente;</li><li>Com a ajuda de um batedor de claras, bata essa mistura de gemas com adoçante, até que vire uma mistura homogênea e clarinha;</li><li>Dica: Pode até ser batido na batedeira, mas em potencia baixa. Essa mistura deve ficar apenas mais clara e fofinha.</li><li>Agora pegue uma pequena porção da mistura de creme de leite, despeje dentro da mistura de gemas sempre mexendo;</li><li>Esse processo deve ser feito para que a gemas não talhem.</li><li>Junte tudo dentro da panela, leve a fogo médio e misture até que tudo vire um creme;</li><li>Dica: Se o seu fogo médio ainda for alto, leve a mistura ao fogo baixo.</li><li>Se por acaso o creme talhar, não tem problema, bata no liquidificador até que tudo fique bem homogêneo;</li><li>Adicione o psyllium e bata por 3 minutos em velocidade média;</li><li>Volte a mistura do creme para a panela, leve ao fogo baixinho e mexa por mais 5 minutos;O seu creme está pronto, você pode usa-lo quente, frio ou adicionar gelatina para que fique bem firminho!</li></ol><p>&nbsp;</p><p><strong>Whipped cream topping</strong></p><p>Use a hand mixer to beat&nbsp;</p><p>the cream with sweetener and vanilla extract until stiff peaks form. Spread the whipped cream over the cream layer.</p><p>&nbsp;</p><p><strong>Montagem:</strong></p><p>1- Colocar o bolo na forma e molhar com o leite de coco;</p><p>2- Colocar um pouco do abacaxi sobre o bolo, depois o creme e o restante do abacaxi;</p><p>3- Polvilhar o coco ralado sobre o creme</p><p>4- Acrescentar o chantilly e polvilhar coco ralado;</p><p>5- Levar para gelar</p>', CAST(0.00 AS Decimal(4, 2)), 1)
INSERT [dbo].[DataSheet] ([DataSheetId], [ProductId], [ProcedureDescription], [WeightVariationPercentage], [IncreaseWeight]) VALUES (24, 32, NULL, CAST(0.00 AS Decimal(4, 2)), 0)
SET IDENTITY_INSERT [dbo].[DataSheet] OFF
GO
SET IDENTITY_INSERT [dbo].[DataSheetItem] ON 

INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (1, 1, 6, CAST(100.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (5, 1, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (6, 1, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (7, 1, 8, CAST(2.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (10, 2, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (11, 2, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (12, 2, 8, CAST(2.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (13, 3, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (15, 3, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (16, 3, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (17, 3, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (18, 3, 8, CAST(2.50000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (19, 3, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (20, 3, 2, CAST(0.09000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (21, 3, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (22, 4, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (24, 4, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (25, 4, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (26, 4, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (27, 4, 8, CAST(2.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (28, 4, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (29, 4, 2, CAST(0.07000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (30, 4, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (31, 3, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (32, 4, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (33, 3, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (35, 3, 12, CAST(0.03900 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (36, 4, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (38, 7, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (39, 7, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (40, 7, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (41, 7, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (42, 7, 8, CAST(2.50000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (43, 7, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (44, 7, 2, CAST(0.09000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (45, 7, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (46, 7, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (47, 7, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (48, 8, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (49, 8, 9, CAST(8.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (50, 8, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (51, 8, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (52, 8, 8, CAST(1.70000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (53, 8, 10, CAST(0.40000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (54, 8, 2, CAST(0.00500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (55, 8, 1, CAST(0.03000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (56, 8, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (57, 8, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (58, 8, 12, CAST(0.03900 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (59, 8, 14, CAST(0.03000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (60, 8, 13, CAST(0.10000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (61, 8, 15, CAST(0.03000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (62, 8, 16, CAST(0.02500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (63, 8, 17, CAST(0.01500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (64, 8, 18, CAST(2.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (65, 5, 6, CAST(100.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (66, 5, 9, CAST(200.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (67, 1, 12, CAST(0.04000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (68, 2, 19, CAST(100.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (69, 2, 12, CAST(0.04000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (70, 9, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (71, 9, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (72, 9, 8, CAST(4.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (73, 9, 8, CAST(4.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (74, 9, 8, CAST(4.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (75, 9, 20, CAST(100.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (76, 10, 21, CAST(80.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (77, 10, 26, CAST(0.15000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (78, 10, 24, CAST(0.50000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (79, 10, 25, CAST(0.15000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (80, 10, 27, CAST(20.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (81, 11, 29, CAST(75.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (82, 11, 28, CAST(25.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (83, 11, 8, CAST(1.50000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (84, 12, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (85, 12, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (86, 12, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (87, 12, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (88, 12, 8, CAST(2.50000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (89, 12, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (90, 12, 2, CAST(0.09000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (91, 12, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (92, 12, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (93, 12, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (94, 13, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (95, 13, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (96, 13, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (97, 13, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (98, 13, 8, CAST(2.50000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (99, 13, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (100, 13, 2, CAST(0.09000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (101, 13, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (102, 13, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (103, 13, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (104, 14, 6, CAST(30.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (105, 14, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (106, 14, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (107, 14, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (108, 14, 8, CAST(2.00000 AS Decimal(9, 5)), NULL, 0)
GO
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (109, 14, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (111, 14, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (112, 14, 7, CAST(70.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (113, 14, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (114, 15, 30, CAST(33.76000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (116, 15, 38, CAST(0.38000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (117, 15, 35, CAST(21.10000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (118, 15, 32, CAST(10.55000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (119, 15, 37, CAST(0.75000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (120, 15, 36, CAST(4.22000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (121, 15, 31, CAST(12.66000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (122, 15, 33, CAST(15.07000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (123, 15, 34, CAST(1.51000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (124, 16, 33, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (125, 16, 39, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (126, 16, 30, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (127, 16, 47, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (128, 16, 38, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (129, 16, 40, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (130, 16, 42, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (131, 16, 41, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (132, 16, 43, CAST(0.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (133, 16, 44, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (134, 16, 45, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (135, 16, 46, CAST(0.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (136, 17, 45, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (137, 17, 30, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (138, 17, 48, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (139, 17, 38, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (140, 17, 33, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (141, 17, 49, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (142, 18, 45, CAST(52.94000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (143, 18, 50, CAST(4.41000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (144, 18, 30, CAST(35.29000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (145, 18, 33, CAST(7.35000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (146, 19, 47, CAST(15.46000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (147, 19, 32, CAST(15.46000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (148, 19, 30, CAST(23.19000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (149, 19, 31, CAST(20.61000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (150, 19, 33, CAST(12.88000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (151, 19, 34, CAST(0.64000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (152, 19, 51, CAST(3.86000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (153, 20, 34, CAST(0.70000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (154, 20, 33, CAST(14.04000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (155, 20, 57, CAST(42.11000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (156, 20, 59, CAST(8.42000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (157, 20, 52, CAST(16.84000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (158, 20, 37, CAST(0.53000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (159, 20, 54, CAST(0.35000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (160, 20, 32, CAST(16.84000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (161, 20, 60, CAST(0.18000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (162, 16, 60, CAST(0.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (163, 19, 60, CAST(0.16000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (164, 19, 61, CAST(7.73000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (165, 17, 60, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (166, 21, 47, CAST(12.83000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (167, 21, 30, CAST(16.44000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (168, 21, 31, CAST(15.70000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (169, 21, 55, CAST(23.08000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (170, 21, 50, CAST(21.24000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (171, 21, 33, CAST(1.75000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (172, 21, 58, CAST(2.77000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (173, 21, 34, CAST(0.46000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (174, 21, 53, CAST(1.85000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (175, 22, 30, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (176, 22, 51, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (177, 22, 31, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (178, 22, 60, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (179, 22, 47, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (180, 22, 39, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (181, 22, 34, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (182, 22, 50, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (183, 22, 63, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (184, 22, 33, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (185, 22, 49, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (187, 22, 62, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (188, 23, 30, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (189, 23, 63, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (190, 23, 38, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (191, 23, 53, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (192, 23, 31, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (193, 23, 33, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (194, 23, 36, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (195, 23, 39, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (196, 23, 34, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (197, 23, 49, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (198, 23, 64, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (199, 23, 65, CAST(0.00000 AS Decimal(9, 5)), NULL, 1)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (202, 24, 9, CAST(10.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (203, 24, 4, CAST(0.25600 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (204, 24, 5, CAST(0.06300 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (205, 24, 8, CAST(2.00000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (206, 24, 10, CAST(0.18000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (207, 24, 2, CAST(0.02250 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (208, 24, 1, CAST(0.04500 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (210, 24, 11, CAST(0.02000 AS Decimal(9, 5)), NULL, 0)
INSERT [dbo].[DataSheetItem] ([DataSheetItemId], [DataSheetId], [RawMaterialId], [Percentage], [AdditionalInfo], [IsBaseItem]) VALUES (211, 24, 67, CAST(100.00000 AS Decimal(9, 5)), NULL, 1)
SET IDENTITY_INSERT [dbo].[DataSheetItem] OFF
GO
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (1, N'Grama', N'g', 1)
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (2, N'Libra', N'lb', 1)
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (3, N'Miligrama', N'mg', 1)
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (4, N'Kilo', N'kg', 1)
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (5, N'Mililitro', N'ml', 2)
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (6, N'Litro', N'l', 2)
INSERT [dbo].[MeasureUnit] ([MeasureUnitId], [Description], [ShortName], [MeasureUnitTypeId]) VALUES (7, N'Onça', N'oz', 1)
GO
INSERT [dbo].[MeasureUnitType] ([MeasureUnitTypeId], [MeasureUnitType]) VALUES (1, N'Massa')
INSERT [dbo].[MeasureUnitType] ([MeasureUnitTypeId], [MeasureUnitType]) VALUES (2, N'Volume')
INSERT [dbo].[MeasureUnitType] ([MeasureUnitTypeId], [MeasureUnitType]) VALUES (3, N'Cumprimento')
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (2, 1, 10, CAST(N'2020-05-23T20:16:14.963' AS DateTime), CAST(N'2020-06-12T14:05:14.133' AS DateTime), CAST(N'2020-05-30T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-05-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (3, 2, 11, CAST(N'2020-05-23T20:17:22.280' AS DateTime), CAST(N'2020-06-07T08:13:41.023' AS DateTime), CAST(N'2020-05-30T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-03T00:00:00.000' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (4, 3, 12, CAST(N'2020-05-23T20:18:24.383' AS DateTime), CAST(N'2020-06-21T19:30:28.140' AS DateTime), CAST(N'2020-06-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-21T19:30:27.803' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (5, 4, 5, CAST(N'2020-05-23T20:19:24.933' AS DateTime), CAST(N'2020-06-14T20:08:56.663' AS DateTime), CAST(N'2020-06-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:08:56.663' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (6, 5, 7, CAST(N'2020-05-27T15:07:57.440' AS DateTime), CAST(N'2020-06-07T08:13:03.477' AS DateTime), CAST(N'2020-06-07T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-06T18:38:15.150' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (9, 6, 16, CAST(N'2020-06-03T14:36:24.367' AS DateTime), CAST(N'2020-06-07T13:27:45.527' AS DateTime), CAST(N'2020-06-07T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-07T13:27:42.197' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (10, 7, 1, CAST(N'2020-06-03T18:09:25.427' AS DateTime), CAST(N'2020-06-19T14:42:40.957' AS DateTime), CAST(N'2020-06-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-19T14:42:05.383' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (11, 8, 11, CAST(N'2020-06-06T08:17:19.730' AS DateTime), CAST(N'2020-06-07T08:10:18.797' AS DateTime), CAST(N'2020-06-07T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-06T12:54:40.387' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (12, 9, 13, CAST(N'2020-06-06T18:22:04.887' AS DateTime), CAST(N'2020-06-18T07:19:21.013' AS DateTime), CAST(N'2020-06-17T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-18T06:45:05.763' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (13, 10, 18, CAST(N'2020-06-07T13:20:19.447' AS DateTime), CAST(N'2020-06-11T18:24:45.223' AS DateTime), CAST(N'2020-06-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-11T18:24:41.063' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (14, 1, 19, CAST(N'2020-06-08T15:54:43.203' AS DateTime), NULL, CAST(N'2020-06-11T20:00:00.000' AS DateTime), 1, 1, CAST(20.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (15, 11, 20, CAST(N'2020-06-09T09:08:25.283' AS DateTime), CAST(N'2020-06-14T12:04:09.297' AS DateTime), CAST(N'2020-06-16T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-14T12:04:06.933' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (16, 12, 21, CAST(N'2020-06-09T09:10:29.687' AS DateTime), CAST(N'2020-06-16T13:46:30.363' AS DateTime), CAST(N'2020-06-16T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-16T13:46:30.113' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (17, 13, 8, CAST(N'2020-06-10T10:15:33.260' AS DateTime), CAST(N'2020-06-18T08:57:16.617' AS DateTime), CAST(N'2020-06-20T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:12.307' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (18, 14, 7, CAST(N'2020-06-11T20:43:03.243' AS DateTime), CAST(N'2020-06-13T08:37:42.157' AS DateTime), CAST(N'2020-06-13T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-13T07:59:39.103' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (19, 15, 17, CAST(N'2020-06-13T12:23:14.690' AS DateTime), CAST(N'2020-06-14T20:09:11.120' AS DateTime), CAST(N'2020-06-20T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:09:11.117' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (20, 16, 14, CAST(N'2020-06-13T16:23:49.267' AS DateTime), CAST(N'2020-06-14T11:48:59.110' AS DateTime), CAST(N'2020-06-13T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-14T11:48:54.240' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (21, 17, 22, CAST(N'2020-06-15T07:38:45.597' AS DateTime), CAST(N'2020-06-25T09:10:05.767' AS DateTime), CAST(N'2020-06-24T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-25T09:10:05.763' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (22, 18, 23, CAST(N'2020-06-15T07:40:02.907' AS DateTime), CAST(N'2020-06-24T05:59:03.447' AS DateTime), CAST(N'2020-06-24T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-24T05:58:55.200' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (23, 19, 16, CAST(N'2020-06-19T10:58:03.947' AS DateTime), CAST(N'2020-06-19T15:47:15.570' AS DateTime), CAST(N'2020-06-19T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-19T15:47:15.567' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (24, 20, 24, CAST(N'2020-06-19T11:17:44.560' AS DateTime), CAST(N'2020-06-23T15:44:27.560' AS DateTime), CAST(N'2020-06-23T00:00:00.000' AS DateTime), 4, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (25, 21, 2, CAST(N'2020-06-25T09:14:25.923' AS DateTime), CAST(N'2020-06-28T08:57:45.907' AS DateTime), CAST(N'2020-06-26T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-28T08:57:43.153' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (26, 22, 25, CAST(N'2020-06-25T09:15:18.620' AS DateTime), CAST(N'2020-06-27T16:38:18.950' AS DateTime), CAST(N'2020-06-25T00:00:00.000' AS DateTime), 4, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (27, 23, 26, CAST(N'2020-06-25T09:16:23.703' AS DateTime), CAST(N'2020-06-29T07:16:16.703' AS DateTime), CAST(N'2020-06-28T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-29T07:16:13.750' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (28, 24, 27, CAST(N'2020-06-25T11:41:49.167' AS DateTime), CAST(N'2020-06-26T07:49:02.360' AS DateTime), CAST(N'2020-06-26T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-26T07:48:59.143' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (29, 25, 28, CAST(N'2020-06-27T16:39:47.100' AS DateTime), CAST(N'2020-06-28T08:57:26.110' AS DateTime), CAST(N'2020-06-27T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-06-28T08:57:25.853' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (30, 26, 10, CAST(N'2020-07-05T10:29:09.290' AS DateTime), CAST(N'2020-07-13T09:14:12.490' AS DateTime), CAST(N'2020-07-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-06T10:24:06.170' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (31, 27, 3, CAST(N'2020-07-05T10:29:44.697' AS DateTime), CAST(N'2020-07-16T14:40:57.123' AS DateTime), CAST(N'2020-07-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-16T14:30:01.507' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (32, 28, 29, CAST(N'2020-07-06T07:49:16.003' AS DateTime), CAST(N'2020-07-15T15:09:32.553' AS DateTime), CAST(N'2020-07-19T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-15T15:09:29.770' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (33, 29, 27, CAST(N'2020-07-06T19:58:23.467' AS DateTime), CAST(N'2020-07-10T11:08:53.427' AS DateTime), CAST(N'2020-07-10T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-10T11:08:50.633' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (34, 30, 18, CAST(N'2020-07-08T10:43:09.927' AS DateTime), CAST(N'2020-08-04T12:19:37.253' AS DateTime), CAST(N'2020-07-31T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-04T12:19:34.737' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (35, 31, 16, CAST(N'2020-07-08T15:15:18.210' AS DateTime), CAST(N'2020-07-19T11:57:18.393' AS DateTime), CAST(N'2020-07-17T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-17T09:33:56.697' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (36, 32, 2, CAST(N'2020-07-09T10:21:36.297' AS DateTime), CAST(N'2020-07-26T08:40:44.573' AS DateTime), CAST(N'2020-07-25T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-26T08:40:44.570' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (37, 33, 30, CAST(N'2020-07-10T06:57:17.133' AS DateTime), CAST(N'2020-07-17T12:25:14.240' AS DateTime), CAST(N'2020-07-18T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-17T12:25:10.727' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (38, 34, 31, CAST(N'2020-07-12T15:27:29.137' AS DateTime), CAST(N'2020-07-25T08:17:05.373' AS DateTime), CAST(N'2020-07-25T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-24T15:39:05.910' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (39, 35, 7, CAST(N'2020-07-13T07:46:56.130' AS DateTime), CAST(N'2020-07-25T08:16:48.143' AS DateTime), CAST(N'2020-07-23T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:16:37.607' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (40, 36, 32, CAST(N'2020-07-14T13:03:47.163' AS DateTime), CAST(N'2020-07-20T09:55:05.277' AS DateTime), CAST(N'2020-07-17T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-20T09:55:01.577' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (41, 37, 33, CAST(N'2020-07-18T12:36:59.537' AS DateTime), CAST(N'2020-07-18T14:49:50.793' AS DateTime), CAST(N'2020-07-18T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-18T14:48:01.270' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (42, 38, 1, CAST(N'2020-07-25T09:00:17.293' AS DateTime), CAST(N'2020-08-06T15:16:27.333' AS DateTime), CAST(N'2020-08-01T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-06T15:16:24.967' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (43, 39, 34, CAST(N'2020-07-30T12:51:07.050' AS DateTime), CAST(N'2020-07-30T12:51:14.623' AS DateTime), CAST(N'2020-07-30T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-07-30T12:51:12.527' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (44, 40, 3, CAST(N'2020-07-30T12:51:39.283' AS DateTime), CAST(N'2020-08-14T08:55:23.063' AS DateTime), CAST(N'2020-08-07T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-14T08:55:20.197' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (45, 41, 36, CAST(N'2020-08-02T17:10:49.517' AS DateTime), CAST(N'2020-08-28T08:04:10.287' AS DateTime), CAST(N'2020-08-22T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-28T08:04:06.950' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (46, 42, 35, CAST(N'2020-08-02T17:11:19.773' AS DateTime), CAST(N'2020-08-14T08:55:09.087' AS DateTime), CAST(N'2020-08-08T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-14T08:55:05.850' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (47, 43, 37, CAST(N'2020-08-14T08:58:01.277' AS DateTime), CAST(N'2020-08-17T09:03:05.323' AS DateTime), CAST(N'2020-08-15T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-08-17T09:02:51.587' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (48, 44, 10, CAST(N'2020-08-29T15:07:21.757' AS DateTime), CAST(N'2020-09-17T07:08:43.350' AS DateTime), CAST(N'2020-09-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-05T18:27:21.240' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (49, 45, 5, CAST(N'2020-08-29T15:08:08.890' AS DateTime), CAST(N'2020-09-15T16:08:42.547' AS DateTime), CAST(N'2020-09-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-15T16:08:39.390' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (50, 46, 37, CAST(N'2020-08-30T08:07:06.863' AS DateTime), CAST(N'2020-09-17T21:18:08.533' AS DateTime), CAST(N'2020-09-12T00:00:00.000' AS DateTime), 4, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (51, 47, 16, CAST(N'2020-09-06T07:53:24.967' AS DateTime), CAST(N'2020-09-06T07:53:34.540' AS DateTime), CAST(N'2020-09-12T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-06T07:53:31.817' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (52, 48, 38, CAST(N'2020-09-08T16:22:12.300' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(N'2020-09-18T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:18.923' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (53, 49, 37, CAST(N'2020-09-10T22:25:24.160' AS DateTime), CAST(N'2020-09-19T14:57:10.003' AS DateTime), CAST(N'2020-09-19T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:04.597' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (54, 50, 31, CAST(N'2020-09-13T09:49:43.173' AS DateTime), CAST(N'2020-09-27T16:32:22.803' AS DateTime), CAST(N'2020-09-19T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-20T07:42:14.733' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (55, 51, 2, CAST(N'2020-09-13T09:50:05.620' AS DateTime), CAST(N'2020-09-20T07:42:02.463' AS DateTime), CAST(N'2020-09-19T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-20T07:41:59.560' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (56, 52, 3, CAST(N'2020-09-17T07:05:24.590' AS DateTime), CAST(N'2020-09-27T10:53:31.827' AS DateTime), CAST(N'2020-09-26T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-27T10:53:28.590' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (57, 53, 39, CAST(N'2020-09-17T07:07:49.243' AS DateTime), CAST(N'2020-09-25T15:13:45.160' AS DateTime), CAST(N'2020-09-26T00:00:00.000' AS DateTime), 4, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (58, 54, 39, CAST(N'2020-09-17T20:00:52.813' AS DateTime), CAST(N'2020-09-22T16:35:49.623' AS DateTime), CAST(N'2020-09-26T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-22T16:35:47.623' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (59, 55, 40, CAST(N'2020-09-20T07:41:36.577' AS DateTime), CAST(N'2020-09-25T10:47:14.627' AS DateTime), CAST(N'2020-09-20T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-25T10:47:14.627' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (60, 56, 41, CAST(N'2020-09-22T16:34:49.140' AS DateTime), CAST(N'2020-10-02T15:25:27.870' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-10-02T15:25:18.230' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (61, 57, 2, CAST(N'2020-09-22T16:36:49.487' AS DateTime), CAST(N'2020-09-27T16:32:39.130' AS DateTime), CAST(N'2020-09-26T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-27T16:32:34.390' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (62, 58, 18, CAST(N'2020-09-22T17:02:05.277' AS DateTime), CAST(N'2020-10-02T16:45:32.523' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:32.520' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (63, 59, 42, CAST(N'2020-09-22T17:13:48.617' AS DateTime), CAST(N'2020-10-01T15:41:52.723' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-29T20:43:29.877' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (64, 60, 27, CAST(N'2020-09-22T17:17:54.190' AS DateTime), CAST(N'2020-09-30T20:02:02.920' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-30T20:01:58.953' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (65, 61, 43, CAST(N'2020-09-22T18:08:45.147' AS DateTime), CAST(N'2020-09-29T12:55:08.183' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-29T12:55:04.213' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (66, 62, 1, CAST(N'2020-09-24T07:28:07.810' AS DateTime), CAST(N'2020-09-28T16:02:17.510' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-09-28T16:02:12.643' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (67, 63, 7, CAST(N'2020-09-27T20:40:11.937' AS DateTime), CAST(N'2020-10-02T20:27:31.683' AS DateTime), CAST(N'2020-09-28T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-10-02T20:27:28.630' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (68, 64, 45, CAST(N'2020-09-30T11:15:19.063' AS DateTime), CAST(N'2020-10-01T17:06:35.140' AS DateTime), CAST(N'2020-10-01T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-10-01T17:06:32.147' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (69, 65, 46, CAST(N'2020-10-02T20:28:42.823' AS DateTime), CAST(N'2020-10-03T12:10:23.580' AS DateTime), CAST(N'2020-10-03T00:00:00.000' AS DateTime), 3, 2, CAST(0.00 AS Decimal(18, 2)), CAST(N'2020-10-03T12:10:12.943' AS DateTime))
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (70, 1, 44, CAST(N'2020-10-04T17:03:07.583' AS DateTime), NULL, CAST(N'2020-10-16T00:00:00.000' AS DateTime), 1, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (71, 66, 8, CAST(N'2020-10-12T07:16:09.823' AS DateTime), NULL, CAST(N'2020-10-24T00:00:00.000' AS DateTime), 1, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (72, 67, 34, CAST(N'2020-10-12T07:29:43.390' AS DateTime), NULL, CAST(N'2020-10-24T00:00:00.000' AS DateTime), 1, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[Order] ([OrderId], [OrderNumber], [CustomerId], [CreatedOn], [LastUpdated], [CompleteBy], [OrderStatusId], [PaymentStatusId], [FreightPrice], [PaidOn]) VALUES (73, 68, 47, CAST(N'2020-10-12T10:05:25.170' AS DateTime), NULL, CAST(N'2020-10-24T00:00:00.000' AS DateTime), 1, 1, CAST(0.00 AS Decimal(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItem] ON 

INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (2, 2, 1, 11, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:16:14.967' AS DateTime), CAST(N'2020-06-12T14:05:13.737' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-12T14:05:13.737' AS DateTime), CAST(17.00 AS Decimal(18, 2)), CAST(19.00 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (3, 2, 2, 8, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:16:14.967' AS DateTime), CAST(N'2020-06-12T14:05:13.737' AS DateTime), N'Sem Pimenta', CAST(32.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(32.00 AS Decimal(18, 2)), 2, CAST(8.00 AS Decimal(18, 2)), CAST(N'2020-06-12T14:05:13.737' AS DateTime), CAST(10.95 AS Decimal(18, 2)), CAST(21.05 AS Decimal(18, 2)), CAST(192.24 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (4, 2, 3, 9, CAST(6.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:16:14.967' AS DateTime), CAST(N'2020-06-12T14:05:13.737' AS DateTime), N'Sem Pimenta', CAST(54.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(54.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-12T14:05:13.737' AS DateTime), CAST(16.43 AS Decimal(18, 2)), CAST(37.57 AS Decimal(18, 2)), CAST(228.67 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (5, 2, 4, 12, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:16:14.967' AS DateTime), CAST(N'2020-06-12T14:05:13.737' AS DateTime), N'', CAST(18.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-12T14:05:13.737' AS DateTime), CAST(5.59 AS Decimal(18, 2)), CAST(12.41 AS Decimal(18, 2)), CAST(222.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (6, 2, 5, 10, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:16:14.967' AS DateTime), CAST(N'2020-06-12T14:05:13.737' AS DateTime), N'', CAST(18.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-12T14:05:13.737' AS DateTime), CAST(6.88 AS Decimal(18, 2)), CAST(11.12 AS Decimal(18, 2)), CAST(161.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (7, 3, 1, 10, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:17:22.280' AS DateTime), CAST(N'2020-06-07T08:13:41.007' AS DateTime), N'', CAST(18.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-06T12:40:04.047' AS DateTime), CAST(6.88 AS Decimal(18, 2)), CAST(11.12 AS Decimal(18, 2)), CAST(161.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (8, 4, 1, 11, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:18:24.387' AS DateTime), CAST(N'2020-06-14T12:04:27.450' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-14T12:04:27.367' AS DateTime), CAST(17.00 AS Decimal(18, 2)), CAST(19.00 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (9, 4, 2, 9, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:18:24.387' AS DateTime), CAST(N'2020-06-07T08:13:47.943' AS DateTime), N'Sem Pimenta', CAST(36.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-05T08:21:18.757' AS DateTime), CAST(10.95 AS Decimal(18, 2)), CAST(25.05 AS Decimal(18, 2)), CAST(228.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (11, 5, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:19:24.933' AS DateTime), CAST(N'2020-06-14T12:05:05.853' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-05T08:21:48.080' AS DateTime), CAST(4.25 AS Decimal(18, 2)), CAST(4.75 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (12, 5, 2, 10, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:19:24.933' AS DateTime), CAST(N'2020-06-14T12:05:05.853' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-05T08:21:55.010' AS DateTime), CAST(3.44 AS Decimal(18, 2)), CAST(5.56 AS Decimal(18, 2)), CAST(161.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (13, 5, 3, 12, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:19:24.933' AS DateTime), CAST(N'2020-06-14T12:05:05.853' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-14T12:05:05.853' AS DateTime), CAST(2.79 AS Decimal(18, 2)), CAST(6.21 AS Decimal(18, 2)), CAST(222.58 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (14, 5, 4, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-23T20:19:24.933' AS DateTime), CAST(N'2020-06-14T12:05:05.853' AS DateTime), N'Sem Pimenta', CAST(18.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-05T08:21:58.457' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(12.52 AS Decimal(18, 2)), CAST(228.47 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (15, 6, 1, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-27T15:07:57.443' AS DateTime), CAST(N'2020-06-07T08:12:59.753' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-06T18:38:18.163' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(6.26 AS Decimal(18, 2)), CAST(228.47 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (16, 6, 2, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-27T15:07:57.443' AS DateTime), CAST(N'2020-06-07T08:13:01.510' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-06T18:38:18.163' AS DateTime), CAST(4.25 AS Decimal(18, 2)), CAST(4.75 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (17, 6, 3, 10, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-05-27T15:07:57.443' AS DateTime), CAST(N'2020-06-07T08:13:03.463' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-06T18:38:18.163' AS DateTime), CAST(13.76 AS Decimal(18, 2)), CAST(22.24 AS Decimal(18, 2)), CAST(161.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (22, 9, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-03T14:36:24.377' AS DateTime), CAST(N'2020-06-07T13:27:45.540' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-07T13:27:45.540' AS DateTime), CAST(4.25 AS Decimal(18, 2)), CAST(4.75 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (23, 9, 2, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-03T14:36:24.377' AS DateTime), CAST(N'2020-06-07T13:27:45.540' AS DateTime), N'', CAST(16.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(16.00 AS Decimal(18, 2)), 2, CAST(8.00 AS Decimal(18, 2)), CAST(N'2020-06-05T08:18:27.570' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(10.53 AS Decimal(18, 2)), CAST(192.50 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (24, 10, 1, 11, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-03T18:09:25.437' AS DateTime), CAST(N'2020-06-19T14:42:40.953' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-19T14:42:40.953' AS DateTime), CAST(17.00 AS Decimal(18, 2)), CAST(19.00 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (25, 10, 2, 12, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-03T18:09:25.440' AS DateTime), CAST(N'2020-06-19T14:42:40.953' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-19T14:42:40.953' AS DateTime), CAST(11.18 AS Decimal(18, 2)), CAST(24.82 AS Decimal(18, 2)), CAST(222.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (26, 11, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-06T08:17:19.737' AS DateTime), CAST(N'2020-06-07T08:10:18.217' AS DateTime), N'', CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), 2, CAST(9.00 AS Decimal(18, 2)), CAST(N'2020-06-06T12:40:57.980' AS DateTime), CAST(4.25 AS Decimal(18, 2)), CAST(4.75 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (27, 12, 1, 11, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-06T18:22:04.893' AS DateTime), CAST(N'2020-06-18T07:19:20.453' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-18T07:19:20.453' AS DateTime), CAST(8.50 AS Decimal(18, 2)), CAST(13.50 AS Decimal(18, 2)), CAST(158.82 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (28, 12, 2, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-06T18:22:04.897' AS DateTime), CAST(N'2020-06-18T07:19:20.453' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-18T07:19:20.453' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (29, 12, 3, 14, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-06T18:22:04.897' AS DateTime), CAST(N'2020-06-18T07:19:20.453' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-18T07:19:20.453' AS DateTime), CAST(8.40 AS Decimal(18, 2)), CAST(13.60 AS Decimal(18, 2)), CAST(161.90 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (30, 12, 4, 12, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-06T18:22:04.897' AS DateTime), CAST(N'2020-06-18T07:19:20.453' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-18T07:19:20.453' AS DateTime), CAST(5.59 AS Decimal(18, 2)), CAST(14.41 AS Decimal(18, 2)), CAST(257.78 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (31, 13, 1, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-07T13:20:19.447' AS DateTime), CAST(N'2020-06-11T18:24:45.120' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-11T18:24:45.120' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (32, 9, 3, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-07T13:26:43.547' AS DateTime), CAST(N'2020-06-07T13:27:45.540' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-07T13:26:43.543' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (33, 14, 1, 17, CAST(25.00 AS Decimal(10, 2)), 1, CAST(N'2020-06-08T15:54:43.207' AS DateTime), NULL, N'', CAST(1750.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1750.00 AS Decimal(18, 2)), 4, CAST(70.00 AS Decimal(18, 2)), CAST(N'2020-06-08T15:54:43.013' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (34, 15, 1, 8, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:08:25.283' AS DateTime), CAST(N'2020-06-14T12:04:09.317' AS DateTime), N'', CAST(40.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-14T12:04:09.317' AS DateTime), CAST(10.95 AS Decimal(18, 2)), CAST(29.05 AS Decimal(18, 2)), CAST(265.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (35, 15, 2, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:08:25.287' AS DateTime), CAST(N'2020-06-14T12:04:09.317' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-14T12:04:09.317' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (36, 16, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:10:29.687' AS DateTime), CAST(N'2020-06-14T20:07:51.850' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:07:51.850' AS DateTime), CAST(4.25 AS Decimal(18, 2)), CAST(6.75 AS Decimal(18, 2)), CAST(158.82 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (37, 16, 2, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:10:29.687' AS DateTime), CAST(N'2020-06-14T20:07:51.850' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:07:51.850' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (38, 16, 3, 8, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:10:29.687' AS DateTime), CAST(N'2020-06-14T20:07:51.850' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:07:51.850' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (39, 16, 4, 12, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:10:29.687' AS DateTime), CAST(N'2020-06-14T20:07:51.850' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:07:51.850' AS DateTime), CAST(2.79 AS Decimal(18, 2)), CAST(7.21 AS Decimal(18, 2)), CAST(258.42 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (40, 16, 5, 13, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-09T09:10:29.687' AS DateTime), CAST(N'2020-06-14T20:07:51.850' AS DateTime), N'', CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-06-14T20:07:51.850' AS DateTime), CAST(10.88 AS Decimal(18, 2)), CAST(13.12 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (41, 17, 1, 11, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:15:33.273' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(8.50 AS Decimal(18, 2)), CAST(13.50 AS Decimal(18, 2)), CAST(158.82 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (42, 17, 2, 10, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:15:33.277' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(33.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(33.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(10.32 AS Decimal(18, 2)), CAST(22.68 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (43, 17, 3, 13, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:15:33.277' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(10.88 AS Decimal(18, 2)), CAST(13.12 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (44, 17, 4, 12, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:15:33.277' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(8.38 AS Decimal(18, 2)), CAST(21.62 AS Decimal(18, 2)), CAST(258.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (46, 17, 6, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:15:33.277' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (47, 17, 7, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:15:33.277' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (48, 17, 8, 14, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-10T10:16:19.810' AS DateTime), CAST(N'2020-06-18T08:57:16.527' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-18T08:57:16.527' AS DateTime), CAST(8.40 AS Decimal(18, 2)), CAST(13.60 AS Decimal(18, 2)), CAST(161.90 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (49, 18, 1, 13, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-11T20:43:03.253' AS DateTime), CAST(N'2020-06-13T08:37:41.833' AS DateTime), N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-06-12T11:15:27.920' AS DateTime), CAST(5.44 AS Decimal(18, 2)), CAST(6.56 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (50, 19, 1, 11, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-13T12:23:14.697' AS DateTime), CAST(N'2020-06-14T12:04:52.980' AS DateTime), N'', CAST(44.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), CAST(36.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-14T12:04:52.980' AS DateTime), CAST(17.00 AS Decimal(18, 2)), CAST(19.00 AS Decimal(18, 2)), CAST(111.76 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (51, 20, 1, 8, CAST(800.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-13T16:23:49.270' AS DateTime), CAST(N'2020-06-14T11:48:59.020' AS DateTime), N'', CAST(17.64 AS Decimal(18, 2)), CAST(0.64 AS Decimal(18, 2)), CAST(17.00 AS Decimal(18, 2)), 1, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-14T11:48:59.020' AS DateTime), CAST(4.83 AS Decimal(18, 2)), CAST(12.17 AS Decimal(18, 2)), CAST(251.97 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (52, 21, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:38:45.600' AS DateTime), CAST(N'2020-06-25T09:09:57.120' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-25T09:09:57.120' AS DateTime), CAST(4.25 AS Decimal(18, 2)), CAST(6.75 AS Decimal(18, 2)), CAST(158.82 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (53, 21, 2, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:38:45.600' AS DateTime), CAST(N'2020-06-25T09:09:57.120' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-25T09:09:57.120' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (54, 21, 3, 8, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:38:45.600' AS DateTime), CAST(N'2020-06-25T09:09:57.120' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-25T09:09:57.120' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (55, 21, 4, 12, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:38:45.600' AS DateTime), CAST(N'2020-06-25T09:09:57.120' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-25T09:09:57.120' AS DateTime), CAST(2.79 AS Decimal(18, 2)), CAST(7.21 AS Decimal(18, 2)), CAST(258.42 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (56, 22, 1, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:40:02.907' AS DateTime), CAST(N'2020-06-24T05:59:03.360' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-24T05:59:03.360' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (57, 22, 2, 12, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:40:02.907' AS DateTime), CAST(N'2020-06-24T05:59:03.360' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-24T05:59:03.360' AS DateTime), CAST(5.59 AS Decimal(18, 2)), CAST(14.41 AS Decimal(18, 2)), CAST(257.78 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (58, 22, 3, 10, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-15T07:40:02.907' AS DateTime), CAST(N'2020-06-24T05:59:03.360' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-24T05:59:03.360' AS DateTime), CAST(6.88 AS Decimal(18, 2)), CAST(15.12 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (59, 23, 1, 8, CAST(3.50 AS Decimal(10, 2)), 5, CAST(N'2020-06-19T10:58:03.957' AS DateTime), CAST(N'2020-06-19T15:47:08.837' AS DateTime), N'', CAST(35.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(35.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-19T15:47:08.837' AS DateTime), CAST(9.58 AS Decimal(18, 2)), CAST(25.42 AS Decimal(18, 2)), CAST(265.34 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (60, 24, 1, 8, CAST(1.00 AS Decimal(10, 2)), 3, CAST(N'2020-06-19T11:17:44.560' AS DateTime), CAST(N'2020-06-23T15:44:17.950' AS DateTime), N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-23T15:44:17.870' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (61, 24, 2, 13, CAST(1.50 AS Decimal(10, 2)), 3, CAST(N'2020-06-19T11:39:39.520' AS DateTime), CAST(N'2020-06-23T15:44:21.847' AS DateTime), N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-06-23T15:44:21.847' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (62, 24, 3, 11, CAST(3.00 AS Decimal(10, 2)), 3, CAST(N'2020-06-19T13:44:12.583' AS DateTime), CAST(N'2020-06-23T15:44:25.240' AS DateTime), N'', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-23T15:44:25.237' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (63, 25, 1, 11, CAST(1.50 AS Decimal(10, 2)), 5, CAST(N'2020-06-25T09:14:25.923' AS DateTime), CAST(N'2020-06-28T08:57:45.827' AS DateTime), N'', CAST(16.50 AS Decimal(18, 2)), CAST(1.50 AS Decimal(18, 2)), CAST(15.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-28T08:57:45.827' AS DateTime), CAST(7.33 AS Decimal(18, 2)), CAST(7.67 AS Decimal(18, 2)), CAST(104.64 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (64, 25, 2, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-25T09:14:25.927' AS DateTime), CAST(N'2020-06-28T08:57:45.827' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-28T08:57:45.827' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(5.11 AS Decimal(18, 2)), CAST(104.50 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (65, 26, 1, 13, CAST(3.00 AS Decimal(10, 2)), 3, CAST(N'2020-06-25T09:15:18.620' AS DateTime), CAST(N'2020-06-27T16:38:19.197' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(35.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-06-27T16:38:19.197' AS DateTime), CAST(16.32 AS Decimal(18, 2)), CAST(18.68 AS Decimal(18, 2)), CAST(114.46 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (66, 26, 2, 9, CAST(2.00 AS Decimal(10, 2)), 3, CAST(N'2020-06-25T09:15:18.620' AS DateTime), CAST(N'2020-06-27T16:38:19.197' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-27T16:38:19.197' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (67, 27, 1, 8, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-25T09:16:23.703' AS DateTime), CAST(N'2020-06-29T07:16:16.620' AS DateTime), N'', CAST(40.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-29T07:16:16.620' AS DateTime), CAST(10.95 AS Decimal(18, 2)), CAST(29.05 AS Decimal(18, 2)), CAST(265.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (68, 28, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-25T11:41:49.170' AS DateTime), CAST(N'2020-06-26T07:49:02.280' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-26T07:49:02.280' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (69, 25, 3, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-26T07:51:20.133' AS DateTime), CAST(N'2020-06-28T08:57:45.827' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-28T08:57:45.827' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(5.11 AS Decimal(18, 2)), CAST(104.50 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (70, 29, 1, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-27T16:39:47.100' AS DateTime), CAST(N'2020-06-27T17:23:20.477' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-27T17:23:20.477' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (71, 29, 2, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-27T16:39:47.100' AS DateTime), CAST(N'2020-06-27T17:23:20.477' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-06-27T17:23:20.477' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(5.11 AS Decimal(18, 2)), CAST(104.50 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (72, 29, 3, 13, CAST(1.50 AS Decimal(10, 2)), 5, CAST(N'2020-06-27T16:39:47.100' AS DateTime), CAST(N'2020-06-27T17:23:20.477' AS DateTime), N'', CAST(18.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(15.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-06-27T17:23:20.477' AS DateTime), CAST(8.16 AS Decimal(18, 2)), CAST(6.84 AS Decimal(18, 2)), CAST(83.82 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (73, 29, 4, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-06-27T16:39:47.100' AS DateTime), CAST(N'2020-06-27T17:23:20.477' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-06-27T17:23:20.477' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (74, 30, 1, 9, CAST(10.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-05T10:29:09.297' AS DateTime), CAST(N'2020-07-13T09:14:12.103' AS DateTime), N'', CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-13T09:14:12.103' AS DateTime), CAST(27.38 AS Decimal(18, 2)), CAST(72.62 AS Decimal(18, 2)), CAST(265.23 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (75, 30, 2, 11, CAST(10.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-05T10:29:09.297' AS DateTime), CAST(N'2020-07-13T09:14:12.103' AS DateTime), N'', CAST(110.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(110.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-13T09:14:12.103' AS DateTime), CAST(48.86 AS Decimal(18, 2)), CAST(61.14 AS Decimal(18, 2)), CAST(125.13 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (76, 31, 1, 11, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-05T10:29:44.697' AS DateTime), CAST(N'2020-07-16T14:40:57.010' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-16T14:40:57.010' AS DateTime), CAST(9.77 AS Decimal(18, 2)), CAST(10.23 AS Decimal(18, 2)), CAST(104.71 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (77, 31, 2, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-05T10:29:44.697' AS DateTime), CAST(N'2020-07-16T14:40:57.010' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-16T14:40:57.010' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (78, 32, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-06T07:49:16.003' AS DateTime), CAST(N'2020-07-15T15:09:32.473' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-15T15:09:32.473' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (79, 32, 2, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-06T07:49:16.007' AS DateTime), CAST(N'2020-07-15T15:09:32.473' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-15T15:09:32.473' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (80, 33, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-06T19:58:23.473' AS DateTime), CAST(N'2020-07-10T11:08:53.323' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-10T11:08:53.323' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (81, 34, 1, 15, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-08T10:43:09.933' AS DateTime), CAST(N'2020-08-04T12:19:37.177' AS DateTime), N'', CAST(15.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(15.00 AS Decimal(18, 2)), CAST(N'2020-08-04T12:19:37.177' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(6.53 AS Decimal(18, 2)), CAST(119.38 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (82, 35, 1, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-08T15:15:18.217' AS DateTime), CAST(N'2020-07-19T11:57:18.207' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-19T11:57:18.207' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (83, 36, 1, 11, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-09T10:21:36.307' AS DateTime), CAST(N'2020-07-26T08:40:37.980' AS DateTime), N'', CAST(44.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-26T08:40:37.980' AS DateTime), CAST(19.54 AS Decimal(18, 2)), CAST(20.46 AS Decimal(18, 2)), CAST(104.71 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (85, 36, 2, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-09T10:22:14.607' AS DateTime), CAST(N'2020-07-26T08:40:37.980' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-26T08:40:37.980' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (86, 37, 1, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-10T06:57:17.137' AS DateTime), CAST(N'2020-07-17T12:25:14.137' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-17T12:25:14.137' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (87, 37, 2, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-10T06:57:17.137' AS DateTime), CAST(N'2020-07-17T12:25:14.137' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-17T12:25:14.137' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (88, 31, 3, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-10T23:33:34.480' AS DateTime), CAST(N'2020-07-16T14:40:57.010' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-16T14:40:57.010' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (89, 38, 1, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-12T15:27:29.147' AS DateTime), CAST(N'2020-07-25T08:17:05.403' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:17:05.403' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (90, 38, 2, 13, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-12T15:27:29.147' AS DateTime), CAST(N'2020-07-25T08:17:05.403' AS DateTime), N'', CAST(36.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:17:05.403' AS DateTime), CAST(16.32 AS Decimal(18, 2)), CAST(13.68 AS Decimal(18, 2)), CAST(83.82 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (91, 39, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-13T07:46:56.137' AS DateTime), CAST(N'2020-07-25T08:16:48.047' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:16:48.047' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (92, 39, 2, 10, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-13T07:46:56.140' AS DateTime), CAST(N'2020-07-25T08:16:48.047' AS DateTime), N'', CAST(44.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(44.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:16:48.047' AS DateTime), CAST(13.76 AS Decimal(18, 2)), CAST(30.24 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (93, 39, 3, 13, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-13T07:46:56.140' AS DateTime), CAST(N'2020-07-25T08:16:48.047' AS DateTime), N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:16:48.047' AS DateTime), CAST(5.44 AS Decimal(18, 2)), CAST(6.56 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (94, 39, 4, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-13T07:46:56.140' AS DateTime), CAST(N'2020-07-25T08:16:48.047' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-25T08:16:48.047' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (95, 40, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-14T13:03:47.167' AS DateTime), CAST(N'2020-07-20T09:55:05.190' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-20T09:55:05.190' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (96, 40, 2, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-14T13:03:47.167' AS DateTime), CAST(N'2020-07-20T09:55:05.190' AS DateTime), N'Sem pimenta', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-20T09:55:05.190' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (97, 40, 3, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-14T13:03:47.167' AS DateTime), CAST(N'2020-07-20T09:55:05.190' AS DateTime), N'Sem pimenta', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-20T09:55:05.190' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (98, 41, 1, 8, CAST(3.50 AS Decimal(10, 2)), 5, CAST(N'2020-07-18T12:36:59.537' AS DateTime), CAST(N'2020-07-18T14:49:50.670' AS DateTime), N'', CAST(35.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(35.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-07-18T14:49:50.670' AS DateTime), CAST(9.58 AS Decimal(18, 2)), CAST(25.42 AS Decimal(18, 2)), CAST(265.34 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (99, 42, 1, 12, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-25T09:00:17.300' AS DateTime), CAST(N'2020-08-06T15:16:27.247' AS DateTime), N'', CAST(40.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-08-06T15:16:27.247' AS DateTime), CAST(11.18 AS Decimal(18, 2)), CAST(28.82 AS Decimal(18, 2)), CAST(257.78 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (100, 43, 1, 11, CAST(1.50 AS Decimal(10, 2)), 5, CAST(N'2020-07-30T12:51:07.053' AS DateTime), CAST(N'2020-07-30T12:51:14.510' AS DateTime), N'', CAST(16.50 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(16.50 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-30T12:51:14.510' AS DateTime), CAST(7.33 AS Decimal(18, 2)), CAST(9.17 AS Decimal(18, 2)), CAST(125.10 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (101, 43, 2, 10, CAST(1.50 AS Decimal(10, 2)), 5, CAST(N'2020-07-30T12:51:07.057' AS DateTime), CAST(N'2020-07-30T12:51:14.510' AS DateTime), N'', CAST(16.50 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(16.50 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-07-30T12:51:14.510' AS DateTime), CAST(5.16 AS Decimal(18, 2)), CAST(11.34 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (102, 44, 1, 14, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-07-30T12:51:39.283' AS DateTime), CAST(N'2020-08-14T08:55:23.103' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-08-14T08:55:23.103' AS DateTime), CAST(8.40 AS Decimal(18, 2)), CAST(11.60 AS Decimal(18, 2)), CAST(138.10 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (103, 45, 1, 8, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-02T17:10:49.520' AS DateTime), CAST(N'2020-08-28T08:04:10.197' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-08-28T08:04:10.197' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (104, 46, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-02T17:11:19.773' AS DateTime), CAST(N'2020-08-14T08:55:09.017' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-08-14T08:55:09.017' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (105, 46, 2, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-02T17:11:19.773' AS DateTime), CAST(N'2020-08-14T08:55:09.017' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-08-14T08:55:09.017' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (106, 47, 1, 13, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-14T08:58:01.280' AS DateTime), CAST(N'2020-08-17T09:03:05.333' AS DateTime), N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-08-17T09:03:05.333' AS DateTime), CAST(5.44 AS Decimal(18, 2)), CAST(6.56 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (107, 47, 2, 19, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-14T08:58:01.280' AS DateTime), CAST(N'2020-08-17T09:03:05.333' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-08-17T09:03:05.333' AS DateTime), CAST(10.03 AS Decimal(18, 2)), CAST(9.97 AS Decimal(18, 2)), CAST(99.40 AS Decimal(18, 2)))
GO
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (108, 47, 3, 14, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-14T08:58:01.280' AS DateTime), CAST(N'2020-08-17T09:03:05.333' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-08-17T09:03:05.333' AS DateTime), CAST(4.20 AS Decimal(18, 2)), CAST(6.80 AS Decimal(18, 2)), CAST(161.90 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (109, 48, 1, 11, CAST(8.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-29T15:07:21.767' AS DateTime), CAST(N'2020-09-17T07:08:43.250' AS DateTime), N'', CAST(88.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(88.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-17T07:08:43.250' AS DateTime), CAST(39.09 AS Decimal(18, 2)), CAST(48.91 AS Decimal(18, 2)), CAST(125.12 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (110, 48, 2, 8, CAST(8.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-29T15:07:21.770' AS DateTime), CAST(N'2020-09-17T07:08:43.250' AS DateTime), N'', CAST(80.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(80.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-17T07:08:43.250' AS DateTime), CAST(21.90 AS Decimal(18, 2)), CAST(58.10 AS Decimal(18, 2)), CAST(265.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (111, 49, 1, 11, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-29T15:08:08.890' AS DateTime), CAST(N'2020-09-15T16:08:42.533' AS DateTime), N'', CAST(44.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(44.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-15T16:08:42.533' AS DateTime), CAST(19.54 AS Decimal(18, 2)), CAST(24.46 AS Decimal(18, 2)), CAST(125.18 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (112, 49, 2, 9, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-08-29T15:08:08.890' AS DateTime), CAST(N'2020-09-15T16:08:42.533' AS DateTime), N'', CAST(40.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-15T16:08:42.533' AS DateTime), CAST(10.95 AS Decimal(18, 2)), CAST(29.05 AS Decimal(18, 2)), CAST(265.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (113, 50, 1, 8, CAST(2.00 AS Decimal(10, 2)), 3, CAST(N'2020-08-30T08:07:06.873' AS DateTime), CAST(N'2020-09-17T21:18:08.983' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-17T21:18:08.983' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (114, 51, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-06T07:53:24.973' AS DateTime), CAST(N'2020-09-06T07:53:34.447' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-06T07:53:34.447' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (115, 52, 1, 11, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-08T16:22:12.303' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(9.77 AS Decimal(18, 2)), CAST(12.23 AS Decimal(18, 2)), CAST(125.18 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (117, 52, 3, 12, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-08T16:22:12.307' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(5.59 AS Decimal(18, 2)), CAST(14.41 AS Decimal(18, 2)), CAST(257.78 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (118, 52, 4, 13, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-08T16:22:12.307' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), N'', CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(10.88 AS Decimal(18, 2)), CAST(13.12 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (119, 52, 5, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-08T16:22:12.307' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (120, 52, 6, 8, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-08T16:22:12.307' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (121, 53, 1, 10, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-10T22:25:24.170' AS DateTime), CAST(N'2020-09-19T14:57:09.900' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:09.900' AS DateTime), CAST(3.44 AS Decimal(18, 2)), CAST(7.56 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (122, 53, 2, 13, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-10T22:25:24.173' AS DateTime), CAST(N'2020-09-19T14:57:09.900' AS DateTime), N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:09.900' AS DateTime), CAST(5.44 AS Decimal(18, 2)), CAST(6.56 AS Decimal(18, 2)), CAST(120.59 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (123, 53, 3, 8, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-10T22:25:24.173' AS DateTime), CAST(N'2020-09-19T14:57:09.900' AS DateTime), N'Com queijo', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:09.900' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (124, 54, 1, 19, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-13T09:49:43.180' AS DateTime), CAST(N'2020-09-27T16:32:22.777' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-20T07:42:32.210' AS DateTime), CAST(10.03 AS Decimal(18, 2)), CAST(9.97 AS Decimal(18, 2)), CAST(99.40 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (125, 54, 2, 13, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-13T09:49:43.183' AS DateTime), CAST(N'2020-09-27T16:32:22.777' AS DateTime), N'', CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-09-27T16:32:22.777' AS DateTime), CAST(6.70 AS Decimal(18, 2)), CAST(17.30 AS Decimal(18, 2)), CAST(258.21 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (126, 55, 1, 19, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-13T09:50:05.620' AS DateTime), CAST(N'2020-09-20T07:42:02.423' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-20T07:42:02.423' AS DateTime), CAST(10.03 AS Decimal(18, 2)), CAST(9.97 AS Decimal(18, 2)), CAST(99.40 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (127, 52, 7, 10, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-14T10:52:10.360' AS DateTime), CAST(N'2020-09-19T14:57:21.367' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-19T14:57:21.367' AS DateTime), CAST(6.88 AS Decimal(18, 2)), CAST(15.12 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (129, 57, 1, 8, CAST(1.00 AS Decimal(10, 2)), 3, CAST(N'2020-09-17T07:07:49.243' AS DateTime), CAST(N'2020-09-17T20:01:09.270' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-17T20:01:09.270' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (130, 57, 2, 9, CAST(2.00 AS Decimal(10, 2)), 3, CAST(N'2020-09-17T07:07:49.243' AS DateTime), CAST(N'2020-09-17T20:01:09.270' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-17T20:01:09.270' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (132, 50, 2, 10, CAST(1.00 AS Decimal(10, 2)), 3, CAST(N'2020-09-17T13:23:09.243' AS DateTime), CAST(N'2020-09-17T21:18:08.983' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-17T21:18:08.983' AS DateTime), CAST(3.44 AS Decimal(18, 2)), CAST(7.56 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (133, 56, 2, 10, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-17T19:57:51.380' AS DateTime), CAST(N'2020-09-27T10:53:31.790' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(14.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-27T10:53:31.790' AS DateTime), CAST(6.88 AS Decimal(18, 2)), CAST(1.12 AS Decimal(18, 2)), CAST(16.28 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (134, 56, 3, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-17T19:57:55.290' AS DateTime), CAST(N'2020-09-27T10:53:31.790' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-27T10:53:31.790' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (135, 58, 1, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-17T20:00:52.813' AS DateTime), CAST(N'2020-09-22T16:35:49.457' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-22T16:35:49.457' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (137, 58, 3, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-17T20:00:52.813' AS DateTime), CAST(N'2020-09-22T16:35:49.457' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-22T16:35:49.457' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (138, 59, 1, 8, CAST(3.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-20T07:41:36.580' AS DateTime), CAST(N'2020-09-25T10:47:08.140' AS DateTime), N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-25T10:47:08.140' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (139, 59, 2, 13, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-20T11:19:32.183' AS DateTime), CAST(N'2020-09-25T10:47:08.140' AS DateTime), N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-09-25T10:47:03.483' AS DateTime), CAST(3.35 AS Decimal(18, 2)), CAST(8.65 AS Decimal(18, 2)), CAST(258.21 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (140, 60, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T16:34:49.140' AS DateTime), CAST(N'2020-10-02T15:25:27.743' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T15:25:27.743' AS DateTime), CAST(5.46 AS Decimal(18, 2)), CAST(14.54 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (141, 60, 2, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T16:34:49.143' AS DateTime), CAST(N'2020-10-02T15:25:27.743' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T15:25:27.743' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (142, 60, 3, 11, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T16:34:49.143' AS DateTime), CAST(N'2020-10-02T15:25:27.743' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-02T15:25:27.743' AS DateTime), CAST(9.77 AS Decimal(18, 2)), CAST(12.23 AS Decimal(18, 2)), CAST(125.18 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (143, 60, 4, 13, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T16:34:49.143' AS DateTime), CAST(N'2020-10-02T15:25:27.743' AS DateTime), N'', CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-10-02T15:25:27.743' AS DateTime), CAST(6.70 AS Decimal(18, 2)), CAST(17.30 AS Decimal(18, 2)), CAST(258.21 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (144, 60, 5, 12, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T16:34:49.143' AS DateTime), CAST(N'2020-10-02T15:25:27.743' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T15:25:27.743' AS DateTime), CAST(5.58 AS Decimal(18, 2)), CAST(14.42 AS Decimal(18, 2)), CAST(258.42 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (145, 61, 1, 13, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T16:36:49.487' AS DateTime), CAST(N'2020-09-27T16:32:39.107' AS DateTime), N'', CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-09-27T16:32:39.107' AS DateTime), CAST(6.70 AS Decimal(18, 2)), CAST(17.30 AS Decimal(18, 2)), CAST(258.21 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (146, 62, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T17:02:05.283' AS DateTime), CAST(N'2020-10-02T16:45:26.867' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:26.867' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (147, 62, 2, 10, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T17:02:05.287' AS DateTime), CAST(N'2020-10-02T16:45:26.867' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:26.867' AS DateTime), CAST(3.44 AS Decimal(18, 2)), CAST(7.56 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (148, 62, 3, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T17:02:05.287' AS DateTime), CAST(N'2020-10-02T16:45:26.867' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:26.867' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (151, 63, 1, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T17:13:48.620' AS DateTime), CAST(N'2020-10-01T15:41:52.133' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-01T15:41:52.133' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (154, 64, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T17:17:54.190' AS DateTime), CAST(N'2020-09-30T20:02:02.783' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-30T20:02:02.783' AS DateTime), CAST(5.46 AS Decimal(18, 2)), CAST(14.54 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (155, 56, 4, 12, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T18:04:32.667' AS DateTime), CAST(N'2020-09-27T10:53:31.790' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-27T10:53:31.790' AS DateTime), CAST(5.58 AS Decimal(18, 2)), CAST(14.42 AS Decimal(18, 2)), CAST(258.42 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (156, 56, 5, 10, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T18:05:54.077' AS DateTime), CAST(N'2020-09-27T10:53:31.790' AS DateTime), N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-09-27T10:53:31.790' AS DateTime), CAST(6.88 AS Decimal(18, 2)), CAST(15.12 AS Decimal(18, 2)), CAST(219.77 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (157, 65, 1, 8, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T18:08:45.147' AS DateTime), CAST(N'2020-09-29T12:55:08.043' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-29T12:55:08.043' AS DateTime), CAST(2.73 AS Decimal(18, 2)), CAST(7.27 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (159, 65, 2, 20, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T20:58:03.447' AS DateTime), CAST(N'2020-09-29T12:55:08.043' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-29T12:55:08.043' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (160, 63, 2, 21, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T20:58:25.747' AS DateTime), CAST(N'2020-10-01T15:41:52.133' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-01T15:41:52.133' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (161, 62, 7, 21, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T20:58:55.620' AS DateTime), CAST(N'2020-10-02T16:45:26.867' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:26.867' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (162, 62, 8, 20, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T20:59:36.210' AS DateTime), CAST(N'2020-10-02T16:45:26.867' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:26.867' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (163, 62, 9, 20, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-22T20:59:37.200' AS DateTime), CAST(N'2020-10-02T16:45:26.867' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T16:45:26.867' AS DateTime), CAST(5.47 AS Decimal(18, 2)), CAST(14.53 AS Decimal(18, 2)), CAST(265.63 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (164, 66, 1, 22, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-24T07:28:07.820' AS DateTime), CAST(N'2020-09-28T16:02:17.390' AS DateTime), N'', CAST(40.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-09-28T16:02:17.390' AS DateTime), CAST(10.88 AS Decimal(18, 2)), CAST(29.12 AS Decimal(18, 2)), CAST(267.65 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (165, 67, 1, 11, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-27T20:40:11.943' AS DateTime), CAST(N'2020-10-02T20:27:31.540' AS DateTime), N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-02T20:27:31.540' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (166, 67, 2, 13, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-27T20:40:11.947' AS DateTime), CAST(N'2020-10-02T20:27:31.540' AS DateTime), N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(12.00 AS Decimal(18, 2)), CAST(N'2020-10-02T20:27:31.540' AS DateTime), CAST(3.35 AS Decimal(18, 2)), CAST(8.65 AS Decimal(18, 2)), CAST(258.21 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (167, 67, 3, 9, CAST(1.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-27T20:40:11.947' AS DateTime), CAST(N'2020-10-02T20:27:31.540' AS DateTime), N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-02T20:27:31.540' AS DateTime), CAST(2.74 AS Decimal(18, 2)), CAST(7.26 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (168, 67, 4, 10, CAST(4.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-27T20:40:11.947' AS DateTime), CAST(N'2020-10-02T20:27:31.540' AS DateTime), N'', CAST(44.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-02T20:27:31.540' AS DateTime), CAST(13.76 AS Decimal(18, 2)), CAST(26.24 AS Decimal(18, 2)), CAST(190.70 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (169, 68, 1, 9, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-30T11:15:19.063' AS DateTime), CAST(N'2020-10-01T17:06:34.993' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-01T17:06:34.993' AS DateTime), CAST(5.48 AS Decimal(18, 2)), CAST(14.52 AS Decimal(18, 2)), CAST(264.96 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (170, 68, 2, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-09-30T11:15:19.063' AS DateTime), CAST(N'2020-10-01T17:06:34.993' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-01T17:06:34.993' AS DateTime), CAST(5.46 AS Decimal(18, 2)), CAST(14.54 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (171, 69, 1, 8, CAST(2.00 AS Decimal(10, 2)), 5, CAST(N'2020-10-02T20:28:42.823' AS DateTime), CAST(N'2020-10-03T12:10:23.443' AS DateTime), N'', CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-03T12:10:23.443' AS DateTime), CAST(5.46 AS Decimal(18, 2)), CAST(14.54 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (172, 70, 1, 28, CAST(1.50 AS Decimal(10, 2)), 1, CAST(N'2020-10-04T17:03:07.590' AS DateTime), NULL, N'', CAST(12.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), 2, CAST(8.00 AS Decimal(18, 2)), CAST(N'2020-10-04T17:03:07.373' AS DateTime), CAST(4.94 AS Decimal(18, 2)), CAST(7.06 AS Decimal(18, 2)), CAST(142.91 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (173, 71, 1, 11, CAST(3.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:16:09.823' AS DateTime), NULL, N'', CAST(33.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(33.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:16:09.717' AS DateTime), CAST(14.66 AS Decimal(18, 2)), CAST(18.34 AS Decimal(18, 2)), CAST(125.10 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (174, 71, 2, 32, CAST(3.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:16:09.823' AS DateTime), NULL, N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:16:09.717' AS DateTime), CAST(4.06 AS Decimal(18, 2)), CAST(25.94 AS Decimal(18, 2)), CAST(638.92 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (175, 71, 3, 8, CAST(3.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:16:09.823' AS DateTime), NULL, N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:16:09.717' AS DateTime), CAST(8.19 AS Decimal(18, 2)), CAST(21.81 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (176, 71, 4, 12, CAST(3.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:16:09.823' AS DateTime), NULL, N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:16:09.717' AS DateTime), CAST(8.37 AS Decimal(18, 2)), CAST(21.63 AS Decimal(18, 2)), CAST(258.42 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (177, 71, 5, 21, CAST(3.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:16:09.823' AS DateTime), NULL, N'', CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:16:09.717' AS DateTime), CAST(8.21 AS Decimal(18, 2)), CAST(21.79 AS Decimal(18, 2)), CAST(265.41 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (178, 72, 1, 32, CAST(1.50 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:29:43.390' AS DateTime), NULL, N'', CAST(15.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(15.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:29:43.380' AS DateTime), CAST(2.03 AS Decimal(18, 2)), CAST(12.97 AS Decimal(18, 2)), CAST(638.92 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (179, 72, 2, 11, CAST(2.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T07:29:43.390' AS DateTime), NULL, N'', CAST(22.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(22.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-12T07:29:43.380' AS DateTime), CAST(9.77 AS Decimal(18, 2)), CAST(12.23 AS Decimal(18, 2)), CAST(125.18 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (180, 73, 1, 11, CAST(1.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T10:05:25.170' AS DateTime), NULL, N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-12T10:05:25.153' AS DateTime), CAST(4.89 AS Decimal(18, 2)), CAST(6.11 AS Decimal(18, 2)), CAST(124.95 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (181, 73, 2, 14, CAST(1.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T10:05:25.170' AS DateTime), NULL, N'', CAST(11.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), 2, CAST(11.00 AS Decimal(18, 2)), CAST(N'2020-10-12T10:05:25.153' AS DateTime), CAST(4.20 AS Decimal(18, 2)), CAST(6.80 AS Decimal(18, 2)), CAST(161.90 AS Decimal(18, 2)))
INSERT [dbo].[OrderItem] ([OrderItemId], [OrderId], [ItemNumber], [ProductId], [Quantity], [OrderItemStatusId], [CreatedOn], [LastUpdated], [AdditionalInfo], [OriginalPrice], [Discount], [PriceAfterDiscount], [MeasureUnitId], [ProductPrice], [LastStatusDate], [Cost], [Profit], [ProfitPercentage]) VALUES (182, 73, 3, 8, CAST(1.00 AS Decimal(10, 2)), 1, CAST(N'2020-10-12T10:05:25.170' AS DateTime), NULL, N'', CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, CAST(10.00 AS Decimal(18, 2)), CAST(N'2020-10-12T10:05:25.153' AS DateTime), CAST(2.73 AS Decimal(18, 2)), CAST(7.27 AS Decimal(18, 2)), CAST(266.30 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[OrderItem] OFF
GO
INSERT [dbo].[OrderItemStatus] ([OrderItemStatusId], [Description]) VALUES (1, N'Aguardando Produção')
INSERT [dbo].[OrderItemStatus] ([OrderItemStatusId], [Description]) VALUES (2, N'Pronto para Entrega')
INSERT [dbo].[OrderItemStatus] ([OrderItemStatusId], [Description]) VALUES (3, N'Cancelado')
INSERT [dbo].[OrderItemStatus] ([OrderItemStatusId], [Description]) VALUES (4, N'Em Andamento')
INSERT [dbo].[OrderItemStatus] ([OrderItemStatusId], [Description]) VALUES (5, N'Entregue')
GO
INSERT [dbo].[OrderStatus] ([OrderStatusId], [Description]) VALUES (1, N'Criado')
INSERT [dbo].[OrderStatus] ([OrderStatusId], [Description]) VALUES (2, N'Em Andamento')
INSERT [dbo].[OrderStatus] ([OrderStatusId], [Description]) VALUES (3, N'Finalizado')
INSERT [dbo].[OrderStatus] ([OrderStatusId], [Description]) VALUES (4, N'Cancelado')
INSERT [dbo].[OrderStatus] ([OrderStatusId], [Description]) VALUES (5, N'Aguardando Entrega')
GO
INSERT [dbo].[PaymentStatus] ([PaymentStatusId], [Description]) VALUES (1, N'Pendente')
INSERT [dbo].[PaymentStatus] ([PaymentStatusId], [Description]) VALUES (2, N'Pago')
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (8, N'Linguiça Calabresa Fresca', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (9, N'Linguiça Calabresa Defumada', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (10, N'Costela Defumada', 2, CAST(11.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (11, N'Bacon Defumado', 2, CAST(11.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (12, N'Paio', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (13, N'Jabá', 2, CAST(12.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (14, N'Panceta Curada', 2, CAST(11.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (15, N'Salamela', 2, CAST(20.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (16, N'Bacon ', 4, CAST(70.00 AS Decimal(10, 2)), 1, 3)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (17, N'Bacon Fatiado', 4, CAST(70.00 AS Decimal(10, 2)), 1, 6)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (18, N'Salame tartufo nero', 4, CAST(280.00 AS Decimal(10, 2)), 1, 9)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (19, N'Sirloin & Bacon Prime Burguer', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (20, N'Linguiça Cuiabana Fresca', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (21, N'Linguiça Caprese Fresca', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (22, N'Linguiça Toscana', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (23, N'Peanut butter cookie (KETO)', 2, CAST(24.20 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (24, N'Chicken Pie (Low Carb / Keto)', 2, CAST(20.00 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (25, N'Garlic Flatbread', 2, CAST(10.00 AS Decimal(10, 2)), 0, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (26, N'Pizza', 2, CAST(13.88 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (27, N'Fudge Keto Brownie', 2, CAST(13.00 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (28, N'Banana Bread', 2, CAST(8.00 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (29, N'Blueberry Lemon Cheesecake Bar', 2, CAST(13.97 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (30, N'Sex in a pan', 2, CAST(10.00 AS Decimal(10, 2)), 0, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (31, N'Pinnaple Dessert', 2, CAST(10.00 AS Decimal(10, 2)), 1, 10)
INSERT [dbo].[Product] ([ProductId], [Name], [MeasureUnitId], [Price], [ActiveForSale], [CorpClientId]) VALUES (32, N'Linguiça de Frango', 2, CAST(10.00 AS Decimal(10, 2)), 1, 1)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[RawMaterial] ON 

INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (1, N'Pimenta do Reino', CAST(0.0353 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (2, N'Pimenta Calabresa', CAST(0.0298 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (4, N'Sal de Cura #1', CAST(0.0106 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (5, N'Eritorbato de Sódio', CAST(0.0617 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (6, N'Pork Belly', CAST(4.2000 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (7, N'Pork Shoulder Butt', CAST(2.0000 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (8, N'Sal', CAST(1.0000 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (9, N'Água', CAST(0.0000 AS Decimal(18, 4)), 1, 5)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (10, N'Alho', CAST(0.0000 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (11, N'Tripa Suína 28mm - 32mm', CAST(0.0291 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (12, N'WRIGHTS HICKORY LIQUID SMOKE', CAST(0.0035 AS Decimal(18, 4)), 1, 5)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (13, N'Flavor Enhancer', CAST(0.0282 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (14, N'Pimenta Cayenne', CAST(0.0249 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (15, N'Nóz Moscada', CAST(0.0391 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (16, N'Oregano', CAST(0.0368 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (17, N'MCCORMICK GROUND CORIANDER', CAST(0.0316 AS Decimal(18, 4)), 1, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (18, N'Cabernet Sauvignon', CAST(0.0053 AS Decimal(18, 4)), 1, 5)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (19, N'Baby Back Ribs', CAST(3.3900 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (20, N'Eye of Round', CAST(3.2000 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (21, N'Copa lombo', CAST(14.9500 AS Decimal(18, 4)), 9, 4)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (22, N'Pernil suíno ', CAST(13.2500 AS Decimal(18, 4)), 9, 4)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (23, N'Barriga suína ', CAST(15.4000 AS Decimal(18, 4)), 9, 4)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (24, N'Trufa negra', CAST(5.0000 AS Decimal(18, 4)), 9, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (25, N'Pimenta do reino ', CAST(40.0000 AS Decimal(18, 4)), 9, 4)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (26, N'Canela em pau', CAST(50.0000 AS Decimal(18, 4)), 9, 4)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (27, N'Retalho bovino', CAST(20.9500 AS Decimal(18, 4)), 9, 4)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (28, N'Bacon Artesanal', CAST(11.0000 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (29, N'Top Sirloin', CAST(3.0000 AS Decimal(18, 4)), 1, 2)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (30, N'Almond Flour', CAST(0.0099 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (31, N'Erythritol', CAST(0.0132 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (32, N'Dark Chocolate Chips', CAST(0.0169 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (33, N'Egg', CAST(0.0059 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (34, N'Vanilla Extract', CAST(0.0847 AS Decimal(18, 4)), 10, 5)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (35, N'Peanut Butter', CAST(0.0088 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (36, N'Coconut oil', CAST(0.0067 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (37, N'Baking Soda', CAST(0.0022 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (38, N'Baking Powder', CAST(0.0071 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (39, N'Heavy Whipping cream', CAST(0.0034 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (40, N'Chicken breast', CAST(0.0044 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (41, N'Hearts of palm', CAST(0.0050 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (42, N'Cheese Spread / Crema Mexicana', CAST(0.0082 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (43, N'Olives', CAST(0.0265 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (44, N'Onion', CAST(0.0022 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (45, N'Mozzarella cheese', CAST(0.0088 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (46, N'Parmesan Cheese', CAST(0.0154 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (47, N'Butter', CAST(0.0066 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (48, N'Ricotta', CAST(0.0045 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (49, N'Xanthan Gum', CAST(0.0423 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (50, N'Cream Cheese', CAST(0.0044 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (51, N'Cocoa Powder', CAST(0.0132 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (52, N'Coconut Flour', CAST(0.0049 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (53, N'Coconut Flakes', CAST(0.0035 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (54, N'Cinnamon', CAST(0.0141 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (55, N'Blueberries', CAST(0.0051 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (56, N'Strawberries', CAST(0.0044 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (57, N'Banana', CAST(0.0015 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (58, N'Lemon', CAST(0.0031 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (59, N'Almond butter', CAST(0.0147 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (60, N'Salt', CAST(0.0022 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (61, N'Walnuts', CAST(0.0120 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (62, N'Chocolate Baking Bar', CAST(0.0353 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (63, N'Almond Milk', CAST(0.0011 AS Decimal(18, 4)), 10, 5)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (64, N'Coconut Milk', CAST(0.0049 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (65, N'Pinnaple', CAST(0.0019 AS Decimal(18, 4)), 10, 1)
INSERT [dbo].[RawMaterial] ([RawMaterialId], [Name], [Price], [CorpClientId], [MeasureUnitId]) VALUES (67, N'Coxa / Sobrecoxa de Frango', CAST(1.2900 AS Decimal(18, 4)), 1, 2)
SET IDENTITY_INSERT [dbo].[RawMaterial] OFF
GO
INSERT [dbo].[Role] ([RoleId], [Name]) VALUES (1, N'SysAdmin')
INSERT [dbo].[Role] ([RoleId], [Name]) VALUES (2, N'Customer')
GO
SET IDENTITY_INSERT [dbo].[RoleModule] ON 

INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (1, 1, 1)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (2, 1, 2)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (3, 1, 3)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (4, 1, 4)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (5, 1, 5)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (6, 1, 6)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (7, 1, 7)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (8, 1, 8)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (9, 1, 9)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (10, 1, 10)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (11, 1, 15)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (12, 1, 16)
INSERT [dbo].[RoleModule] ([RoleModuleId], [RoleId], [SystemModuleId]) VALUES (14, 1, 19)
SET IDENTITY_INSERT [dbo].[RoleModule] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemModule] ON 

INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (1, NULL, N'Sistema', NULL, 0, 2, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (2, 1, N'Gerenciar Módulos', N'/System/Modules', 0, 3, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (3, NULL, N'Dashboard', N'/dashboard', 1, 1, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (4, NULL, N'Cadastro', NULL, 1, 4, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (5, 4, N'Produtos', N'/product', 1, 5, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (6, 4, N'Clientes', N'/customer', 1, 6, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (7, NULL, N'Pedidos', NULL, 1, 7, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (8, 7, N'Gerar Pedido', N'/order/new', 1, 8, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (9, 7, N'Pesquisar', N'/order/search', 1, 9, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (10, 4, N'Matéria Prima', N'/raw-material', 1, 10, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (15, NULL, N'Relatórios', NULL, 1, 11, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (16, 15, N'Status', N'/report/status-report', 1, 12, 1)
INSERT [dbo].[SystemModule] ([SystemModuleId], [ParentId], [Name], [Route], [Active], [Order], [IsMenu]) VALUES (19, 4, N'Ficha Técnica', N'/product/datasheet/details', 1, 14, 1)
SET IDENTITY_INSERT [dbo].[SystemModule] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (1, N'rbercocano', 1, N'Rodrigo', N'Berçocano do Amaral', 1, 1, N'K454fzDF/DmXOWB4P/pVUpytj7QrHU/VveO2RoIE9Nk=', N'rbercocano@gmail.com', N'+1(916)402-6099', N'+1(916)402-6099', CAST(N'1985-09-13' AS Date), CAST(N'2020-05-06T17:31:14.990' AS DateTime), CAST(N'2020-10-12T06:36:05.073' AS DateTime))
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (2, N'navarro', 1, N'Navarro', NULL, 1, 2, N'2AG0Pc/5PFZoL6dDRoGRjWUJzQcOgIaioOixz2qFndY=', N'navarro@curato.com.br', NULL, NULL, NULL, CAST(N'2020-06-01T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (3, N'hugosobrinho', 1, N'Hugo', N'Sobrinho', 1, 3, N'k6d2eaxfmOJQCgzXFxjqJUMl0/kOQ2DWW6tDWeYxud4=', N'hugolsobrinho@gmail.com', NULL, NULL, NULL, CAST(N'2020-06-04T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (6, N'fernandoferro', 1, N'Fernando', N'Ferro', 1, 4, N'9kFJ6U7Q7oRuj0sRO0u35up8KBzDd8gavsdX24cSlKs=', N'donannasalumeria@gmail.com', NULL, NULL, NULL, CAST(N'2020-06-05T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (8, N'cstoer', 1, N'César', N'Stoer', 1, 6, N'blQZ/OIJ4zYedWCqKujV8Q==', N'cesar@coldsmoke.com.br', NULL, NULL, NULL, CAST(N'2020-06-08T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (9, N'cfalcao', 1, N'Carlos', N'Henrique Teixeira Falcão', 1, 7, N'WZGQMu1T6cjrtDVGYIZ8WQ==', N'chef@chefaquiemcasa.com.br', NULL, NULL, NULL, CAST(N'2020-06-12T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (10, N'rgiannoccaro', 1, N'Ricardo', N'Giannoccaro', 1, 9, N'+NIZ40yZqs92DAXLpd6sDg==', N'Ricardo.gianno@gmail.com', NULL, NULL, NULL, CAST(N'2020-06-12T00:00:00.000' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (11, N'jessica', 1, N'Jéssica', N'Cristina Maia do Amaral', 1, 10, N'h7FIcB6D5UepUtSi8FgXig==', N'jeh.maia@gmail.com', NULL, NULL, NULL, CAST(N'2020-09-24T16:31:28.070' AS DateTime), NULL)
INSERT [dbo].[User] ([UserId], [Username], [Active], [Name], [LastName], [RoleId], [CorpClientId], [Password], [Email], [HomePhone], [Mobile], [DateOfBirth], [CreatedOn], [LastUpdated]) VALUES (12, N'andreia', 1, N'Andréia', N'Braga', 1, 10, N'dr5wrc/FeivKL6a2b1Xycg==', N'andreiabraga73@hotmail.com', NULL, NULL, NULL, CAST(N'2020-09-24T16:32:20.697' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (1, N'SqxHsCX/hcb/1HEdZC+Hllm+gQJBp7xYeIFpd4LyoNM=', CAST(N'2020-10-11T06:43:17.140' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (2, N'jfGEMThQdqOhSexXGuL5z3CpxBPFht1fN3+kdgwHK2M=', CAST(N'2020-06-02T18:44:17.073' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (3, N'2h0C9MGQNLqpYupOLNoX68vx9ts8H30+edQwrxkcbls=', CAST(N'2020-06-04T14:28:27.063' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (6, N'HOePseq2iKqAkeUFXEsVTstxH47mUvmwRpSEjG/Gurg=', CAST(N'2020-06-05T07:13:40.740' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (8, N'exx2V/NR/XBUnOcpNfqNSzmAauL2kdBhgzOA8G0R7IA=', CAST(N'2020-06-08T18:26:41.137' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (9, N'6H4I9B7QK3ueQzPABIFw76LrRx+Ik0dMX0ya6UHfE2E=', CAST(N'2020-07-20T05:10:25.423' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (10, N'eY/MDLtHzE6E3qNbNGTcWZA/QAZbNNXcOOocuLrlD+0=', CAST(N'2020-06-22T11:08:40.537' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (11, N'8PPgNzpUDtgeE3jFeMbXHZJAMY2Zx0J8LYj+mRxmLGw=', CAST(N'2020-09-24T16:44:42.110' AS DateTime))
INSERT [dbo].[UserToken] ([UserId], [Token], [CreatedOn]) VALUES (12, N'gsHNhbFXaHxZhsVuNOLGPDpbNoNc2KgsL7cVc7pMknM=', CAST(N'2020-10-10T14:58:19.813' AS DateTime))
GO
/****** Object:  Index [UN_PROD_ID]    Script Date: 10/12/2020 8:35:59 PM ******/
ALTER TABLE [dbo].[DataSheet] ADD  CONSTRAINT [UN_PROD_ID] UNIQUE NONCLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CorpClient] ADD  DEFAULT ('R$') FOR [Currency]
GO
ALTER TABLE [dbo].[DataSheet] ADD  CONSTRAINT [DF__DataSheet__Weigh__76619304]  DEFAULT ((0)) FOR [WeightVariationPercentage]
GO
ALTER TABLE [dbo].[DataSheet] ADD  DEFAULT ((0)) FOR [IncreaseWeight]
GO
ALTER TABLE [dbo].[SystemModule] ADD  CONSTRAINT [DF_SystemModule_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__User__Email__74794A92]  DEFAULT ('') FOR [Email]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_CorpClient] FOREIGN KEY([CorpClientId])
REFERENCES [dbo].[CorpClient] ([CorpClientId])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_CorpClient]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_CustomerType] FOREIGN KEY([CustomerTypeId])
REFERENCES [dbo].[CustomerType] ([CustomerTypeId])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_CustomerType]
GO
ALTER TABLE [dbo].[CustomerContact]  WITH CHECK ADD  CONSTRAINT [FK_CustomerContact_ContactType] FOREIGN KEY([ContactTypeId])
REFERENCES [dbo].[ContactType] ([ContactTypeId])
GO
ALTER TABLE [dbo].[CustomerContact] CHECK CONSTRAINT [FK_CustomerContact_ContactType]
GO
ALTER TABLE [dbo].[CustomerContact]  WITH CHECK ADD  CONSTRAINT [FK_CustomerContact_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[CustomerContact] CHECK CONSTRAINT [FK_CustomerContact_Customer]
GO
ALTER TABLE [dbo].[DataSheet]  WITH CHECK ADD  CONSTRAINT [FK_DataSheet_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[DataSheet] CHECK CONSTRAINT [FK_DataSheet_Product]
GO
ALTER TABLE [dbo].[DataSheetItem]  WITH CHECK ADD  CONSTRAINT [FK_DataSheetItem_DataSheet] FOREIGN KEY([DataSheetId])
REFERENCES [dbo].[DataSheet] ([DataSheetId])
GO
ALTER TABLE [dbo].[DataSheetItem] CHECK CONSTRAINT [FK_DataSheetItem_DataSheet]
GO
ALTER TABLE [dbo].[DataSheetItem]  WITH CHECK ADD  CONSTRAINT [FK_DataSheetItem_RawMaterial] FOREIGN KEY([RawMaterialId])
REFERENCES [dbo].[RawMaterial] ([RawMaterialId])
GO
ALTER TABLE [dbo].[DataSheetItem] CHECK CONSTRAINT [FK_DataSheetItem_RawMaterial]
GO
ALTER TABLE [dbo].[MeasureUnit]  WITH CHECK ADD  CONSTRAINT [FK_MeasureUnit_MeasureUnitType] FOREIGN KEY([MeasureUnitTypeId])
REFERENCES [dbo].[MeasureUnitType] ([MeasureUnitTypeId])
GO
ALTER TABLE [dbo].[MeasureUnit] CHECK CONSTRAINT [FK_MeasureUnit_MeasureUnitType]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderStatus] FOREIGN KEY([OrderStatusId])
REFERENCES [dbo].[OrderStatus] ([OrderStatusId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderStatus]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_PaymentStatus] FOREIGN KEY([PaymentStatusId])
REFERENCES [dbo].[PaymentStatus] ([PaymentStatusId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_PaymentStatus]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_CorpClient] FOREIGN KEY([CorpClientId])
REFERENCES [dbo].[CorpClient] ([CorpClientId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_CorpClient]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_MeasureUnit] FOREIGN KEY([MeasureUnitId])
REFERENCES [dbo].[MeasureUnit] ([MeasureUnitId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_MeasureUnit]
GO
ALTER TABLE [dbo].[RawMaterial]  WITH CHECK ADD  CONSTRAINT [FK_RawMaterial_CorpClient] FOREIGN KEY([CorpClientId])
REFERENCES [dbo].[CorpClient] ([CorpClientId])
GO
ALTER TABLE [dbo].[RawMaterial] CHECK CONSTRAINT [FK_RawMaterial_CorpClient]
GO
ALTER TABLE [dbo].[RawMaterial]  WITH CHECK ADD  CONSTRAINT [FK_RawMaterial_MeasureUnit] FOREIGN KEY([MeasureUnitId])
REFERENCES [dbo].[MeasureUnit] ([MeasureUnitId])
GO
ALTER TABLE [dbo].[RawMaterial] CHECK CONSTRAINT [FK_RawMaterial_MeasureUnit]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_CorpClient] FOREIGN KEY([CorpClientId])
REFERENCES [dbo].[CorpClient] ([CorpClientId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_CorpClient]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserToken]  WITH CHECK ADD  CONSTRAINT [FK_UserTokens_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserToken] CHECK CONSTRAINT [FK_UserTokens_User]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ContactType"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 148
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwProductCost'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwProductCost'
GO
USE [master]
GO
ALTER DATABASE [DB_A625FD_rbercocano] SET  READ_WRITE 
GO
