go 
set ansi_padding on
go
set quoted_identifier on
go
set ansi_nulls on
go
create database [MPUS_STUDY_DataBase]
go
use [MPUS_STUDY_DataBase]
go
--Таблица специальность
create table [dbo].[Specialization]
(
[ID_specialization] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Number_specialty] VARCHAR(8)  NOT NULL check ([Number_specialty] like  '[0-9][0-9].[0-9][0-9].[0-9][0-9]'),
[Name_specialty] VARCHAR(200)  NOT NULL check ([Name_specialty] like  '%[А-Я, а-я, ‘,’, ‘.’, ‘(’, ‘)’, ‘-’]%'),
)
go

--Процедуры для специальности
create procedure [dbo].[Specialization_insert]
@Number_specialty [VARCHAR](8), @Name_specialty [VARCHAR](200)
as
	insert into [dbo].[Specialization] ([Number_specialty] , [Name_specialty])
					  	 values (@Number_specialty, @Name_specialty)
go
create procedure [dbo].[Specialization_update]
@ID_specialization [int], @Number_specialty [VARCHAR](8), @Name_specialty [VARCHAR](200)
as
	update [dbo].[Specialization] set 
	[Number_specialty] = @Number_specialty ,
	[Name_specialty] = @Name_specialty
	where [ID_specialization] = @ID_specialization
go
create procedure [dbo].[Specialization_delete]
@ID_specialization [int]
as
	delete from [dbo].[Specialization] 
	where [ID_specialization] = @ID_specialization

go

--Таблица группы
create table [dbo].[Groups]
(
[ID_groups] INT NOT NULL PRIMARY KEY clustered identity(1,1), 
[Number_groups] VARCHAR(10)  NOT NULL check ([Number_groups] like  '%[А-Я, 0-9,‘-’]%'), 
[Number_course] VARCHAR(1)  NOT NULL  check ([Number_course] like  '[1-4]'),
[Specialization_ID] INT NULL foreign key references [dbo].[Specialization] ([ID_specialization]) on update cascade on delete cascade,
)
go

--Процедуры для групп
create procedure [dbo].[Groups_insert]
@Number_groups [VARCHAR](10), @Number_course [VARCHAR](1), @Specialization_ID [int]
as
	insert into [dbo].[Groups] ([Number_groups] , [Number_course], [Specialization_ID])
					  	 values (@Number_groups, @Number_course, @Specialization_ID)
go
create procedure [dbo].[Groups_update]
@ID_groups [int], @Number_groups [VARCHAR](10), @Number_course [VARCHAR](1), @Specialization_ID [int]
as
	update [dbo].[Groups] set 
	[Number_groups] = @Number_groups ,
	[Number_course] = @Number_course,
	[Specialization_ID] = @Specialization_ID
	where [ID_groups] = @ID_groups
go
create procedure [dbo].[Groups_delete]
@ID_groups [int]
as
	delete from [dbo].[Groups] 
	where [ID_groups] = @ID_groups

go

--Таблица профессиональный модуль
create table [dbo].[Professional_module]
(
[ID_professional_module] int NOT NULL PRIMARY KEY clustered identity(1,1),
[Code_professional_module] VARCHAR(5)  NOT NULL check ([Code_professional_module] like  '%[А-Я, 0-9, ‘.’]%'),
[Name_professional_module] VARCHAR(200)  NOT NULL check ([Name_professional_module] like  '%[А-Я, а-я, ‘,’, ‘.’, ‘(’, ‘)’, ‘-’]%'),
)
go

--Процедуры для профессионального модуля
create procedure [dbo].[Professional_module_insert]
@Code_professional_module [VARCHAR](5), @Name_professional_module [VARCHAR](200)
as
	insert into [dbo].[Professional_module] ([Code_professional_module] , [Name_professional_module])
					  	 values (@Code_professional_module, @Name_professional_module)
go
create procedure [dbo].[Professional_module_update]
@ID_professional_module [int], @Code_professional_module [VARCHAR](5), @Name_professional_module [VARCHAR](200)
as
	update [dbo].[Professional_module] set 
	[Code_professional_module] = @Code_professional_module,
	[Name_professional_module] = @Name_professional_module
	where [ID_professional_module] = @ID_professional_module
go
create procedure [dbo].[Professional_module_delete]
@ID_professional_module [int]
as
	delete from [dbo].[Professional_module] 
	where [ID_professional_module] = @ID_professional_module

go

--Таблица дисциплины
create table [dbo].[Disciplines]
(
[ID_disciplines] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Disciplines_type] varchar(10) not null check ([Disciplines_type] like  '%[А-Я, а-я]%'),
[Subject_code] varchar(10) null check ([Subject_code] like  '%[0-9, ‘.’]%'),
[Name_disciplines] varchar(150) not null check ([Name_disciplines] like  '%[А-Я, а-я, ‘,’, ‘.’, ‘(’, ‘)’, ‘-’]%'),
[Professional_module_ID] int NOT NULL foreign key references [dbo].[Professional_module] ([ID_professional_module]) on update cascade on delete cascade,
)
go

--Процедуры для дисциплин
create procedure [dbo].[Disciplines_insert]
@Disciplines_type [VARCHAR](10), @Subject_code [VARCHAR](10), @Name_disciplines [VARCHAR](150), @Professional_module_ID [int]
as
	insert into [dbo].[Disciplines] ([Disciplines_type] , [Subject_code], [Name_disciplines], [Professional_module_ID])
					  	 values (@Disciplines_type, @Subject_code, @Name_disciplines, @Professional_module_ID)
go
create procedure [dbo].[Disciplines_update]
@ID_disciplines [int], @Disciplines_type [VARCHAR](10), @Subject_code [VARCHAR](10), @Name_disciplines [VARCHAR](150), @Professional_module_ID [int]
as
	update [dbo].[Disciplines] set 
	[Disciplines_type] = @Disciplines_type ,
	[Subject_code] = @Subject_code,
	[Name_disciplines] = @Name_disciplines,
	[Professional_module_ID] = @Professional_module_ID
	where [ID_disciplines] = @ID_disciplines
go
create procedure [dbo].[Disciplines_delete]
@ID_disciplines [int]
as
	delete from [dbo].[Disciplines] 
	where [ID_disciplines] = @ID_disciplines
go

--Таблица преподаватели
create table [dbo].[Teachers]
(
[ID_teachers] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Surname] VARCHAR(20)  NOT NULL check ([Surname] like  '%[А-Я, а-я]%'),
[Name] VARCHAR(20)  NOT NULL check ([Name] like  '%[А-Я, а-я]%'),
[Middlename] VARCHAR(20)  NOT NULL check ([Middlename] like  '%[А-Я, а-я]%'), 
)
go

--Процедуры для преподавателей
create procedure [dbo].[Teachers_insert]
@Surname [VARCHAR](20), @Name [VARCHAR](20), @Middlename [VARCHAR](20)
as
	insert into [dbo].[Teachers] ([Surname] , [Name], [Middlename])
					  	 values (@Surname, @Name, @Middlename)
go
create procedure [dbo].[Teachers_update]
@ID_teachers [int], @Surname [VARCHAR](20), @Name [VARCHAR](20), @Middlename [VARCHAR](20)
as
	update [dbo].[Teachers] set 
	[Surname] = @Surname ,
	[Name] = @Name,
	[Middlename] = @Middlename
	where [ID_teachers] = @ID_teachers
go
create procedure [dbo].[Teachers_delete]
@ID_teachers [int]
as
	delete from [dbo].[Teachers] 
	where [ID_teachers] = @ID_teachers
go

--Таблица должности
create table [dbo].[Post]
(
[ID_post] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Name_post] VARCHAR(50)  NOT NULL unique check ([Name_post] like  '%[А-Я, а-я]%'),
)
go

--Процедуры для должности
create procedure [dbo].[Post_insert]
@Name_post [VARCHAR](50)
as
	insert into [dbo].[Post] ([Name_post])
					  	 values (@Name_post)
go
create procedure [dbo].[Post_update]
@ID_post [int], @Name_post [VARCHAR](50)
as
	update [dbo].[Post] set 
	[Name_post] = @Name_post
	where [ID_post] = @ID_post
go
create procedure [dbo].[Post_delete]
@ID_post [int]
as
	delete from [dbo].[Post] 
	where [ID_post] = @ID_post
go

--Таблица авторизация
create table [dbo].[Authorization]
(
[ID_Authorization] int not null identity(1,1) primary key,
[Login] varchar(30) not null unique check(len([Login]) >= 5),
[Password] varchar(max) not null check(len([Password]) >= 5),
)
go

create procedure [dbo].[Authorization_insert]
 @Login [varchar] (30), @Password [varchar] (max)
as
	insert into [dbo].[Authorization] ([Login], [Password] )
	values (@Login, @Password)
go
create procedure [dbo].[Authorization_update]
@ID_Authorization [int],  @Login [varchar] (30), @Password [varchar] (max)
as
	update [dbo].[Authorization] set 
	[Login] = @Login,
	[Password] = @Password
	where [ID_Authorization] = @ID_Authorization
go
create procedure [dbo].[Authorization_delete]
@ID_Authorization [int]
as
	delete from [dbo].[Authorization] 
	where [ID_Authorization] = @ID_Authorization
go

--Таблица пользователи
create table [dbo].[Users]
(
[ID_users] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Surname] VARCHAR(20)  NOT NULL check ([Surname] like  '%[А-Я, а-я]%'),
[Name] VARCHAR(20)  NOT NULL check ([Name] like  '%[А-Я, а-я]%'),
[Middlename] VARCHAR(20)  NOT NULL check ([Middlename] like  '%[А-Я, а-я]%'),
[Authorization_ID] INT NULL foreign key references [dbo].[Authorization] ([ID_authorization]) on update cascade on delete cascade,
[Post_ID] INT NULL foreign key references [dbo].[Post] ([ID_post])	on update cascade on delete cascade,
)
go
--Представление для авторизации
create view [Authorization_View]
("Post_ID", "Status")
as
	select [Post_ID], 'Администратор' from [Users]
	union
	select [Post_ID], 'Делопроизводитель' from [Users]
go
--Регистрация администратора
create procedure [dbo].[Admin_register]
@Login [varchar] (30), @Password [varchar] (max), @Name_post [VARCHAR](50), @Surname [VARCHAR](20), @Name [VARCHAR](20), @MiddleName [VARCHAR](20)
 as
    insert into [dbo].[Authorization] ([Login], [Password] )
    values (@Login, @Password)
    select @@identity
	insert into [dbo].[Post] ([Name_post])
    values (@Name_post)
    select @@identity
    insert into [dbo].[Users] ([Surname], [Name], [MiddleName], [Authorization_ID],  [Post_ID])
    values (@Surname, @Name, @MiddleName, @@identity,  @@identity)
go
--Добавление пользователя
create procedure [dbo].[Users_insert]
@Login [varchar] (30), @Password [varchar] (max),  @Post_ID [int], @Surname [VARCHAR](20), @Name [VARCHAR](20), @MiddleName [VARCHAR](20)
 as
    insert into [dbo].[Authorization] ([Login], [Password] )
    values (@Login, @Password)
    select @@identity
    insert into [dbo].[Users] ([Surname], [Name], [MiddleName], [Authorization_ID],  [Post_ID])
    values (@Surname, @Name, @MiddleName, @@identity,  @Post_ID)
go

--Редактирование пользователя
go
create procedure [dbo].[Users_update]
@ID_users [int], @Surname [VARCHAR](20), @Name [VARCHAR](20), @MiddleName [VARCHAR](20), @Post_ID [int], @Login [varchar] (30), @Password [varchar] (max)
 as   
	update [dbo].[Users] set 
	[Surname] = @Surname ,
	[Name] = @Name,
	[MiddleName] = @MiddleName,
	[Post_ID] = @Post_ID
	where [ID_users] = @ID_users
	declare @ID_Authorization int = (select [Authorization_ID] from [Users] where [ID_users] = @ID_users)
	update [dbo].[Authorization] set
	[Login] = @Login,
	[Password] = @Password
go
--Удаление пользователя
create procedure [dbo].[Users_delete]
@ID_users [int], @Authorization_ID [int]
as
	delete from [dbo].[Users]
	where [ID_users] = @ID_users
	delete from [dbo].[Authorization]
	where [ID_Authorization] = @Authorization_ID
go
--Таблица список студентов
create table [dbo].[Students_list]
(
[ID_students_list] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Surname] VARCHAR(20)  NOT NULL check ([Surname] like  '%[А-Я, а-я]%'),
[Name] VARCHAR(20)  NOT NULL check ([Name] like  '%[А-Я, а-я]%'),
[Middlename] VARCHAR(20)  NOT NULL check ([Middlename] like  '%[А-Я, а-я]%'), 
[Groups_ID] INT NULL foreign key references [dbo].[Groups] ([ID_groups]),
)

go

--Процедуры для списка студентов
create procedure [dbo].[Students_list_insert]
@Surname [VARCHAR](20), @Name [VARCHAR](20), @Middlename [VARCHAR](20), @Groups_ID [int]
as
	insert into [dbo].[Students_list] ([Surname] , [Name], [Middlename], [Groups_ID])
					  	 values (@Surname, @Name, @Middlename, @Groups_ID)
go
create procedure [dbo].[Students_list_update]
@ID_students_list [int], @Surname [VARCHAR](20), @Name [VARCHAR](20), @Middlename [VARCHAR](20), @Groups_ID [int]
as
	update [dbo].[Students_list] set 
	[Surname] = @Surname ,
	[Name] = @Name,
	[Middlename] = @Middlename,
	[Groups_ID] = @Groups_ID
	where [ID_students_list] = @ID_students_list
go
create procedure [dbo].[Students_list_delete]
@ID_students_list [int]
as
	delete from [dbo].[Students_list] 
	where [ID_students_list] = @ID_students_list

go

--Таблица дисциплины у студентов
create table [dbo].[Students_disciplines]
(
[ID_students_disciplines] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Number_semester] VARCHAR(1)  NOT NULL check ([Number_semester] like  '[1-8]'),
[Attestation_form] VARCHAR(20) NULL check ([Attestation_form] like  '%[А-Я,а-я]%'),
[Groups_ID] INT NULL foreign key references [dbo].[Groups] ([ID_groups]),
[Disciplines_ID] INT NULL foreign key references [dbo].[Disciplines] ([ID_disciplines])	on update cascade on delete cascade,
[Teachers_ID] INT NULL foreign key references [dbo].[Teachers] ([ID_teachers]) on update cascade on delete cascade,
)

go

--Процедуры для дисциплин у студентов
create procedure [dbo].[Students_disciplines_insert]
@Number_semester [VARCHAR](1), @Attestation_form [VARCHAR](20), @Groups_ID [int], @Disciplines_ID [int], @Teachers_ID [int]
as
	insert into [dbo].[Students_disciplines] ([Number_semester] , [Attestation_form], [Groups_ID], [Disciplines_ID], [Teachers_ID])
					  	 values (@Number_semester, @Attestation_form, @Groups_ID, @Disciplines_ID, @Teachers_ID)
go
create procedure [dbo].[Students_disciplines_update]
@ID_students_disciplines [int], @Number_semester [VARCHAR](1), @Attestation_form [VARCHAR](20), @Groups_ID [int], @Disciplines_ID [int], @Teachers_ID [int]
as
	update [dbo].[Students_disciplines] set 
	[Number_semester] = @Number_semester ,
	[Attestation_form] = @Attestation_form,
	[Groups_ID] = @Groups_ID,
	[Disciplines_ID] = @Disciplines_ID,
	[Teachers_ID] = @Teachers_ID
	where [ID_students_disciplines] = @ID_students_disciplines
go
create procedure [dbo].[Students_disciplines_delete]
@ID_students_disciplines [int]
as
	delete from [dbo].[Students_disciplines] 
	where [ID_students_disciplines] = @ID_students_disciplines

go
--Таблица успеваемость групп
create table [dbo].[Group_scores]
(
[ID_group_scores] INT NOT NULL PRIMARY KEY clustered identity(1,1),
[Students_list_ID] INT NULL foreign key references [dbo].[Students_list] ([ID_students_list]) on update cascade on delete cascade,
[Students_disciplines_ID] INT NULL foreign key references [dbo].[Students_disciplines] ([ID_students_disciplines]) on update cascade on delete cascade,
[Users_ID] INT NULL foreign key references [dbo].[Users] ([ID_users]) on update cascade on delete no action,
[Score] VARCHAR(1)  NOT NULL check ([Score] like  '[2-5]'),
)
go
--Процедуры для успеваемости групп
create procedure [dbo].[Group_scores_insert]
@Students_list_ID [int], @Students_disciplines_ID [int], @Users_ID [int], @Score [VARCHAR](1)
as
	insert into [dbo].[Group_scores] ([Students_list_ID] , [Students_disciplines_ID], [Users_ID], [Score])
					  	 values (@Students_list_ID, @Students_disciplines_ID, @Users_ID, @Score)
go
create procedure [dbo].[Group_scores_update]
@ID_group_scores [int], @Students_list_ID [int], @Students_disciplines_ID [int], @Users_ID [int], @Score [VARCHAR](1)
as
	update [dbo].[Group_scores] set 
	[Students_list_ID] = @Students_list_ID ,
	[Students_disciplines_ID] = @Students_disciplines_ID,
	[Users_ID] = @Users_ID,
	[Score] = @Score
	where [ID_group_scores] = @ID_group_scores
go
create procedure [dbo].[Group_scores_delete]
@ID_group_scores [int]
as
	delete from [dbo].[Group_scores] 
	where [ID_group_scores] = @ID_group_scores


--Отображение поля ФИО Студентов
go
create view [Fio_Students_View]
as
select [ID_students_list], [Surname] + ' ' + [Name] + ' ' + [Middlename] + ' ' + [Number_groups] as 'ФИО студента' from [dbo].[Students_list]
			LEFT JOIN [dbo].[Groups] ON [dbo].[Students_list].[Groups_ID] = [dbo].[Groups].[ID_groups]
go
--Отображение поля ФИО Пользователя
create view [Fio_Users_View]
as
select [ID_users], [Surname] + ' ' + [Name] + ' ' + [MiddleName] as 'ФИО пользователя' from [dbo].[Users]
go
--Отображение поля ФИО Преподавателей
create view [Fio_Teachers_View]
as
select [ID_teachers], [Surname] + ' ' + [Name] + ' ' + [MiddleName] as 'ФИО преподавателя' from [dbo].[Teachers]
go
--Отображение поля Проф. модуль
create view [Professional_module_View]
as
select [ID_professional_module], [Code_professional_module] + ' ' + [Name_professional_module] as 'Профессиональный модуль' from [dbo].[Professional_module]
go
--Отображение поля Специальность
create view [Specialization_View]
as
select [ID_specialization], [Number_specialty] + ' ' + [Name_specialty] as 'Специальность' from [dbo].[Specialization]
go
--Отображение поля Группа
create view [Groups_View]
as
select [ID_groups], [Number_groups] as 'Группа' from [dbo].[Groups]
go
--Отображение поля Должность
create view [Post_View]
as
select [ID_post], [Name_post] as 'Должность' from [dbo].[Post]
go
--Отображение поля Дисциплина
create view [Disciplines_View]
as
select [ID_disciplines], [Disciplines_type] + ' ' + [Subject_code] + ' ' +  [Name_disciplines] + ' ' +   [Code_professional_module] as 'Дисциплина' from [dbo].[Disciplines]
			LEFT JOIN [dbo].[Professional_module] ON [dbo].[Disciplines].[Professional_module_ID] = [dbo].[Professional_module].[ID_professional_module]
go
--Отображение поля Дисциплина студента
create view [Students_disciplines_View]
as
select [ID_students_disciplines], [Number_semester] + ' семестр. ' + [Attestation_form]+ ' у группы: ' + [Number_groups] + ' по предмету (' + [Disciplines_type] + ' ' + [Subject_code] + ' ' + [Name_disciplines]  + ') ' +  [dbo].[Teachers].[Surname] + ' ' + [dbo].[Teachers].[Name] + ' ' + [dbo].[Teachers].[Middlename] as 'Дисциплина у студента' from [dbo].[Students_disciplines]
            LEFT JOIN [dbo].[Disciplines] ON [dbo].[Students_disciplines].[Disciplines_ID] = [dbo].[Disciplines].[ID_disciplines]
            LEFT JOIN [dbo].[Teachers] ON [dbo].[Students_disciplines].[Teachers_ID] = [dbo].[Teachers].[ID_teachers]
			LEFT JOIN [dbo].[Groups] ON [dbo].[Students_disciplines].[Groups_ID] = [dbo].[Groups].[ID_groups]
go