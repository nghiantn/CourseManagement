CREATE DATABASE CourseDatabase
GO
USE [CourseDatabase]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[idAccount] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](255) NULL,
	[fullname] [varchar](255) NULL,
	[phone] [varchar](50) NULL,
	[idRole] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[idAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Calendars]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calendars](
	[idCalendar] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[startTime] [datetime] NOT NULL,
	[endTime] [datetime] NOT NULL,
	[length] [int] NULL,
	[idCourse] [int] NOT NULL,
	[idTeacher] [int] NOT NULL,
	[slotnow] [int] NOT NULL,
	[slotmax] [int] NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_Calendars] PRIMARY KEY CLUSTERED 
(
	[idCalendar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[idCategory] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[description] [varchar](max) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[idCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[IdContact] [int] IDENTITY(1,1) NOT NULL,
	[phone] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[fullname] [varchar](255) NOT NULL,
	[idCalendar] [int] NOT NULL,
	[idStatus] [int] NOT NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[IdContact] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[idCourse] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[description] [varchar](max) NULL,
	[idCategory] [int] NOT NULL,
	[image] [varchar](max) NULL,
	[price] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[idCourse] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Learn]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Learn](
	[idStudent] [int] NOT NULL,
	[idCalendar] [int] NOT NULL,
	[IdLearn] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Box] PRIMARY KEY CLUSTERED 
(
	[IdLearn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[idRole] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[description] [varchar](255) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 18/11/2022 12:12:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[idStatus] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[idStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 
GO
INSERT [dbo].[Accounts] ([idAccount], [username], [password], [fullname], [phone], [idRole], [Active]) VALUES (1, N'Admin', N'e10adc3949ba59abbe56e057f20f883e', N'Tài khoản quản trị viên', N'0768482178', 1, 1)
GO
INSERT [dbo].[Accounts] ([idAccount], [username], [password], [fullname], [phone], [idRole], [Active]) VALUES (2, N'Teacher', N'e10adc3949ba59abbe56e057f20f883e', N'Tài khoản giáo viên', N'0123456789', 2, 1)
GO
INSERT [dbo].[Accounts] ([idAccount], [username], [password], [fullname], [phone], [idRole], [Active]) VALUES (3, N'Student', N'e10adc3949ba59abbe56e057f20f883e', N'Tài khoản học viên', N'0123456789', 3, 1)
GO
INSERT [dbo].[Accounts] ([idAccount], [username], [password], [fullname], [phone], [idRole], [Active]) VALUES (4, N'Editor', N'e10adc3949ba59abbe56e057f20f883e', N'Biên tập viên', N'0123456789', 4, 1)
GO
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Calendars] ON 
GO
INSERT [dbo].[Calendars] ([idCalendar], [name], [startTime], [endTime], [length], [idCourse], [idTeacher], [slotnow], [slotmax], [active]) VALUES (1, N'Thử', CAST(N'2022-11-14T00:00:00.000' AS DateTime), CAST(N'2022-11-28T00:00:00.000' AS DateTime), 14, 1, 2, 0, 1, 1)
GO
INSERT [dbo].[Calendars] ([idCalendar], [name], [startTime], [endTime], [length], [idCourse], [idTeacher], [slotnow], [slotmax], [active]) VALUES (2, N'Thử 2', CAST(N'2022-12-10T00:00:00.000' AS DateTime), CAST(N'2023-01-07T00:00:00.000' AS DateTime), 28, 2, 2, 0, 2, 0)
GO
SET IDENTITY_INSERT [dbo].[Calendars] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([idCategory], [name], [description]) VALUES (1, N'Web Developer', N'Lập trình viên ứng dụng Web')
GO
INSERT [dbo].[Categories] ([idCategory], [name], [description]) VALUES (2, N'Mobile Developer', N'Lập trình viên ứng dụng di động')
GO
INSERT [dbo].[Categories] ([idCategory], [name], [description]) VALUES (3, N'Android Developer', N'Lập trình viên Android')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Contact] ON 
GO
INSERT [dbo].[Contact] ([IdContact], [phone], [email], [fullname], [idCalendar], [idStatus]) VALUES (1, N'0987654432', N'asddsaasd@gmail.com', N'A B C', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Contact] OFF
GO
SET IDENTITY_INSERT [dbo].[Courses] ON 
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (1, N'HTML', N'HTML', 1, N'html.jpg', 100, 1)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (2, N'CSS', N'CSS', 2, N'default.jpg', 100, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (3, N'Javascript', N'Javascript', 1, N'default.jpg', 100, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (4, N'ReactJS', N'ReactJS', 2, N'default.jpg', 100, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (5, N'NodeJS', N'NodeJS', 1, N'default.jpg', 100, 1)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (6, N'Bootstrap', N'Bootstrap', 2, N'bootstrap.jpg', 100, 1)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (7, N'Test', N'Test', 1, N'test.jpg', 0, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (8, N'Thử nghiệm', N'Thử nghiệm đính kèm ảnh', 1, N'thu-nghiem.jpg', 1111, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (9, N'Thử nghiệm 2 ', N'Đính kèm ảnh', 2, N'thu-nghiem-2.jpg', 133312312, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (10, N'Thử nghiệm 3', N'Đính kèm ảnh', 1, N'thu-nghiem-3.jpg', 100, 1)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (11, N'Thử nghiệm 4', N'Không đính kèm ảnh', 1, N'default.jpg', 123123, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (12, N'Thử nghiệm 5', N'Không có gì', 2, N'thu-nghiem-5.jpg', 99999, 0)
GO
INSERT [dbo].[Courses] ([idCourse], [name], [description], [idCategory], [image], [price], [Active]) VALUES (13, N'Đính kèm ảnh', N'Thử nghiệm', 1, N'dinh-kem-anh.jpg', 99999, 1)
GO
SET IDENTITY_INSERT [dbo].[Courses] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([idRole], [name], [description]) VALUES (1, N'Admin', N'Quản trị viên')
GO
INSERT [dbo].[Roles] ([idRole], [name], [description]) VALUES (2, N'Teacher', N'Giáo viên')
GO
INSERT [dbo].[Roles] ([idRole], [name], [description]) VALUES (3, N'Student', N'Học viên')
GO
INSERT [dbo].[Roles] ([idRole], [name], [description]) VALUES (4, N'Editor', N'Biên tập viên')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 
GO
INSERT [dbo].[Status] ([idStatus], [name], [description]) VALUES (1, N'Mới', N'Vừa mới được chuyển yêu cầu')
GO
INSERT [dbo].[Status] ([idStatus], [name], [description]) VALUES (2, N'Đã duyệt (chờ thanh toán)', N'Yêu cầu đã được xem xét, chờ thanh toán')
GO
INSERT [dbo].[Status] ([idStatus], [name], [description]) VALUES (3, N'Đã duyệt (đã thanh toán)', N'Yêu cầu đang được chờ thực thi')
GO
INSERT [dbo].[Status] ([idStatus], [name], [description]) VALUES (4, N'Đã xác nhận toàn bộ thông tin', N'Xem xét để tự động tạo thông tin cho học viên')
GO
INSERT [dbo].[Status] ([idStatus], [name], [description]) VALUES (5, N'Hoàn tất', N'Yêu cầu đã được hoàn tất xử lý')
GO
INSERT [dbo].[Status] ([idStatus], [name], [description]) VALUES (6, N'Từ chối', N'Yêu cầu bị từ chối')
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Roles] FOREIGN KEY([idRole])
REFERENCES [dbo].[Roles] ([idRole])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Roles]
GO
ALTER TABLE [dbo].[Calendars]  WITH CHECK ADD FOREIGN KEY([idTeacher])
REFERENCES [dbo].[Accounts] ([idAccount])
GO
ALTER TABLE [dbo].[Calendars]  WITH CHECK ADD  CONSTRAINT [FK_Calendars_Courses] FOREIGN KEY([idCourse])
REFERENCES [dbo].[Courses] ([idCourse])
GO
ALTER TABLE [dbo].[Calendars] CHECK CONSTRAINT [FK_Calendars_Courses]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD FOREIGN KEY([idCalendar])
REFERENCES [dbo].[Calendars] ([idCalendar])
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_Status] FOREIGN KEY([idStatus])
REFERENCES [dbo].[Status] ([idStatus])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_Status]
GO
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Categories] FOREIGN KEY([idCategory])
REFERENCES [dbo].[Categories] ([idCategory])
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Categories]
GO
ALTER TABLE [dbo].[Learn]  WITH CHECK ADD FOREIGN KEY([idCalendar])
REFERENCES [dbo].[Calendars] ([idCalendar])
GO
ALTER TABLE [dbo].[Learn]  WITH CHECK ADD FOREIGN KEY([idStudent])
REFERENCES [dbo].[Accounts] ([idAccount])
GO