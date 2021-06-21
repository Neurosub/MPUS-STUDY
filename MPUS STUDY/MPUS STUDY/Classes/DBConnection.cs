using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MPUS_STUDY.Classes
{
    public class DBConnection
    {
        //подключение к БД
        public static SqlConnection connection = new SqlConnection(
               @"Data Source = DESKTOP-T819KVA\SQLEXPRESS; " +
              " Initial Catalog = MPUS_STUDY_DataBase; Persist Security Info = true;" +
              " User ID = sa; Password = \"psl14082001\"");

        //Отображение поля ФИО студентов
        public static string qrStudentsFio = "select * from [Fio_Students_View]";
        //Отображение поля ФИО пользователя
        public static string qrUsersFio = "select * from [Fio_Users_View]";
        //Отображение поля ФИО преподавателей
        public static string qrTeachersFio = "select * from [Fio_Teachers_View]";
        //Отображение поля Проф. модуль
        public static string qrProfessional_moduleView = "select * from [Professional_module_View]";
        //Отображение поля Специальность
        public static string qrSpecializationView = "select * from [Specialization_View]";
        //Отображение поля Группы
        public static string qrGroupView = "select * from [Groups_View]";
        //Отображение поля Дисциплина
        public static string qrDisciplines_View = "select * from [Disciplines_View]";
        //Отображение поля Дисциплина студента
        public static string qrStudents_disciplines_View = "select * from [Students_disciplines_View]";
        //Отображение поля Должность
        public static string qrPostView = "select * from [Post_View]";




        //Таблица специальности
        public DataTable dtSpecialization = new DataTable("Specialization");
        public static string qrSpecialization = "SELECT [ID_specialization] as \"Код специальности\", " +
            "[Number_specialty] as \"Номер специальности\", [Name_specialty] as \"Наименование специальности\" FROM [dbo].[Specialization]";
        //Таблица группы
        public DataTable dtGroups = new DataTable("Groups");
        public static string qrGroups = "SELECT [ID_groups] as \"Код группы\", " +
            "[Number_groups] as \"Номер группы\", [Number_course] as \"Номер курса\", " +
            "[dbo].[Groups].[Specialization_ID], [Number_specialty] as \"Номер специальности\", [Name_specialty] as \"Наименование специальности\"  FROM [dbo].[Groups]" +
            " INNER JOIN [dbo].[Specialization] ON [dbo].[Groups].[Specialization_ID] = [dbo].[Specialization].[ID_specialization]";
        //Таблица профессиональный модуль
        public DataTable dtProfessional_module = new DataTable("Professional_module");
        public static string qrProfessional_module = "SELECT [ID_professional_module] as \"Номер профессионального модуля\", " +
            "[Code_professional_module] as \"Код профессионального модуля\", [Name_professional_module] as \"Название профессионального модуля\" FROM [dbo].[Professional_module]";
        //Таблица дисциплины
        public DataTable dtDisciplines = new DataTable("Disciplines");
        public static string qrDisciplines = "SELECT [ID_disciplines] as \"Код дисциплины\", " +
            "[Disciplines_type] as \"Тип дисциплины\", [Subject_code] as \"Код предмета\", [Name_disciplines] as \"Название дисциплины\", " +
            "[dbo].[Disciplines].[Professional_module_ID] as \"Номер профессионального модуля\", [Code_professional_module] + ' ' + [Name_professional_module] as \"Профессиональный модуль\" FROM [dbo].[Disciplines]" +
            "INNER JOIN [dbo].[Professional_module] ON [dbo].[Disciplines].[Professional_module_ID] = [dbo].[Professional_module].[ID_professional_module]";
        //Таблица преподаватели
        public DataTable dtTeachers = new DataTable("Teachers");
        public static string qrTeachers = "SELECT [ID_teachers] as \"Код преподавателя\", " +
            "[dbo].[Teachers].[Surname] as \"Фамилия\", [dbo].[Teachers].[Name] as \"Имя\", [dbo].[Teachers].[Middlename] as \"Отчество\" FROM [dbo].[Teachers]";
        //Таблица должности
        public DataTable dtPost = new DataTable("Post");
        public static string qrPost = "SELECT [ID_post] as \"Код должности\", [Name_post] as \"Название должности\" FROM [dbo].[Post]";
        //Таблица авторизации
        public DataTable dtAuthorization = new DataTable("Authorization");
        public static string qrAuthorization = "SELECT [ID_Authorization] as \"Код авторизации\", [Login] as \"Логин\", [Password] as \"Пароль\" FROM [dbo].[Authorization]";
        //Таблица пользователи
        public DataTable dtUsers = new DataTable("Users");
        public static string qrUsers = "SELECT [ID_users] as \"Код пользователя\", " +
            "[dbo].[Users].[Surname] as \"Фамилия\", [dbo].[Users].[Name] as \"Имя\", [dbo].[Users].[Middlename] as \"Отчество\", " +
            "[dbo].[Users].[Authorization_ID], [Login] as \"Логин\", [Password] as \"Пароль\"," +
            "[dbo].[Users].[Post_ID], [Name_post] as \"Должность\" FROM [dbo].[Users]" +
            "INNER JOIN [dbo].[Authorization] ON [dbo].[Users].[Authorization_ID] = [dbo].[Authorization].[ID_Authorization]" +
            "INNER JOIN [dbo].[Post] ON [dbo].[Users].[Post_ID] = [dbo].[Post].[ID_post]";
        //эскпорт списка пользователей
        public static string qrUsersExport = "SELECT " +
            "[dbo].[Users].[Surname] as \"Фамилия\", [dbo].[Users].[Name] as \"Имя\", [dbo].[Users].[Middlename] as \"Отчество\", " +
            "[Login] as \"Логин\", [Password] as \"Пароль\", [Name_post] as \"Должность\" FROM [dbo].[Users]" +
            "INNER JOIN [dbo].[Authorization] ON [dbo].[Users].[Authorization_ID] = [dbo].[Authorization].[ID_Authorization]" +
            "INNER JOIN [dbo].[Post] ON [dbo].[Users].[Post_ID] = [dbo].[Post].[ID_post]";
        //Таблица список студентов
        public DataTable dtStudents_list = new DataTable("Students_list");
        public static string qrStudents_list = "SELECT [ID_students_list] as \"Код студента\", " +
            "[dbo].[Students_list].[Surname] as \"Фамилия\", [dbo].[Students_list].[Name] as \"Имя\", [dbo].[Students_list].[Middlename] as \"Отчество\", " +
            "[dbo].[Students_list].[Groups_ID], [Number_groups] as \"Номер группы\" FROM [dbo].[Students_list]" +
             "INNER JOIN [dbo].[Groups] ON [dbo].[Students_list].[Groups_ID] = [dbo].[Groups].[ID_groups]";
        //Таблица дисциплины у студентов с объединенными столбцами
        public DataTable dtStudents_disciplines = new DataTable("Students_disciplines");
        public static string qrStudents_disciplines = "SELECT [ID_students_disciplines] as \"Код дисциплины студентов\", " +
            "[Number_semester] as \"Номер семестра\", [Attestation_form] as \"Форма аттестации\", " +
            "[dbo].[Students_disciplines].[Groups_ID], [Number_groups] as \"Номер группы\", " +
            "[dbo].[Students_disciplines].[Disciplines_ID], [Disciplines_type] + ' ' + [Subject_code] + ' ' + [Name_disciplines] as \"Дисциплина\", " +
            "[dbo].[Students_disciplines].[Teachers_ID], [dbo].[Teachers].[Surname] + ' ' + [dbo].[Teachers].[Name] + ' ' + [dbo].[Teachers].[Middlename] as \"Фио преподавателя\" FROM [dbo].[Students_disciplines]" +
            "LEFT JOIN [dbo].[Groups] ON [dbo].[Students_disciplines].[Groups_ID] = [dbo].[Groups].[ID_groups]" +
            "LEFT JOIN [dbo].[Disciplines] ON [dbo].[Students_disciplines].[Disciplines_ID] = [dbo].[Disciplines].[ID_disciplines]" +
            "LEFT JOIN [dbo].[Teachers] ON [dbo].[Students_disciplines].[Teachers_ID] = [dbo].[Teachers].[ID_teachers]";
        //Таблица успеваемость групп
        public DataTable dtGroup_scores = new DataTable("Group_scores");
        public static string qrGroup_scores = "SELECT [ID_group_scores] as \"Код успеваемости\", " +
            "[dbo].[Group_scores].[Students_list_ID], [dbo].[Students_list].[Surname] + ' ' + [dbo].[Students_list].[Name] + ' ' + [dbo].[Students_list].[Middlename] as \"ФИО студента\", " +
            "[dbo].[Group_scores].[Students_disciplines_ID], [Number_semester] as \"Номер семестра\", [Attestation_form] as \"Форма аттестации\", [dbo].[Students_disciplines].[Groups_ID], [Number_groups] as \"Номер группы\", " +
            "[dbo].[Students_disciplines].[Disciplines_ID], [Disciplines_type] + ' ' + [Subject_code] + ' ' + [Name_disciplines] as \"Дисциплина\", " +
            "[dbo].[Students_disciplines].[Teachers_ID], [dbo].[Teachers].[Surname] + ' ' + [dbo].[Teachers].[Name] + ' ' + [dbo].[Teachers].[Middlename] as \"ФИО преподавателя\", " +
            "[dbo].[Group_scores].[Users_ID], [dbo].[Users].[Surname] + ' ' + [dbo].[Users].[Name] + ' ' + [dbo].[Users].[Middlename] as \"ФИО пользователя\", " +
            "[Score] as \"Оценка\" FROM [dbo].[Group_scores] " +
            "INNER JOIN [dbo].[Students_list] ON [dbo].[Group_scores].[Students_list_ID] = [dbo].[Students_list].[ID_students_list]" +
            "INNER JOIN [dbo].[Students_disciplines] ON [dbo].[Group_scores].[Students_disciplines_ID] = [dbo].[Students_disciplines].[ID_students_disciplines]" +
            "INNER JOIN [dbo].[Users] ON [dbo].[Group_scores].[Users_ID] = [dbo].[Users].[ID_users]" +
            "INNER JOIN [dbo].[Groups] ON [dbo].[Students_disciplines].[Groups_ID] = [dbo].[Groups].[ID_groups]" +
            "INNER JOIN [dbo].[Disciplines] ON [dbo].[Students_disciplines].[Disciplines_ID] = [dbo].[Disciplines].[ID_disciplines]" +
            "INNER JOIN [dbo].[Teachers] ON [dbo].[Students_disciplines].[Teachers_ID] = [dbo].[Teachers].[ID_teachers]";
        //запрос на экспорт
        public static string qrExport = "SELECT " +
          "[dbo].[Students_list].[Surname] + ' ' + [dbo].[Students_list].[Name] + ' ' + [dbo].[Students_list].[Middlename] as \"ФИО студента\", " +
          "[Number_semester] as \"Номер семестра\", [Attestation_form] as \"Форма аттестации\", [Number_groups] as \"Номер группы\", " +
          "[Disciplines_type] + ' ' + [Subject_code] + ' ' + [Name_disciplines] as \"Дисциплина\", [Score] as \"Оценка\", " +
          "[dbo].[Teachers].[Surname] + ' ' + [dbo].[Teachers].[Name] + ' ' + [dbo].[Teachers].[Middlename] as \"ФИО преподавателя\", " +
          "[dbo].[Users].[Surname] + ' ' + [dbo].[Users].[Name] + ' ' + [dbo].[Users].[Middlename] as \"ФИО пользователя\" FROM [dbo].[Group_scores] " +
          "INNER JOIN[dbo].[Students_list] ON [dbo].[Group_scores].[Students_list_ID] = [dbo].[Students_list].[ID_students_list] " +
          "INNER JOIN[dbo].[Students_disciplines] ON [dbo].[Group_scores].[Students_disciplines_ID] = [dbo].[Students_disciplines].[ID_students_disciplines] " +
          "INNER JOIN[dbo].[Users] ON [dbo].[Group_scores].[Users_ID] = [dbo].[Users].[ID_users] " +
          "INNER JOIN[dbo].[Groups] ON [dbo].[Students_disciplines].[Groups_ID] = [dbo].[Groups].[ID_groups] " +
          "INNER JOIN[dbo].[Disciplines] ON [dbo].[Students_disciplines].[Disciplines_ID] = [dbo].[Disciplines].[ID_disciplines] " +
          "INNER JOIN[dbo].[Teachers] ON [dbo].[Students_disciplines].[Teachers_ID] = [dbo].[Teachers].[ID_teachers] ORDER BY [Number_groups]";

        private SqlCommand command = new SqlCommand("", connection);
        public static Int32 IDSpecialization, IDGroups, IDProfModule, IDDisciplines, IDTeachers, IDPost, IDAuth, IDUsers, IDStudentsList, IDStudentsDisciplines, IDGroupScores;
        private void dtFill(DataTable table, string query)
        {
            command.CommandText = query;
            connection.Open();
            table.Load(command.ExecuteReader());
            connection.Close();

        }

        public static int idUser,
            idPersonal, //id Сотрудника
            RoleID, //роли
            groupid; //id группы

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <returns>Возвращает id записи</returns>
        public Int32 Authorization(string Login)
        {
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "select [ID_Authorization] from" +
                    "[Authorization] where [Login] = '" + Login + "'";
                DBConnection.connection.Open();
                idUser = Convert.ToInt32(command.ExecuteScalar().ToString());
                idPersonal = idUser;
                return (idUser);

            }
            catch
            {
                idUser = 0;
                idPersonal = idUser;
                return (idUser);
            }
            finally
            {
                DBConnection.connection.Close();
            }
        }

        /// <summary>
        /// id сотрудника
        /// </summary>
        /// <param name="idUser">id из авторизации</param>
        public void getIDPersonal(int idUser)
        {
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "select [ID_users] from [Users] where [Authorization_ID] like '%" + idUser + "%'";
                connection.Open();
                idPersonal = Convert.ToInt32(command.ExecuteScalar().ToString());
            }
            catch
            {
                idPersonal = 0;

            }
            finally
            {
                DBConnection.connection.Close();
            }
        }
        /// <summary>
        /// Роль
        /// </summary>
        /// <param name="userID">id Авторизованного пользователя</param>
        /// <returns>Роль пользователя</returns>
        public string userRole(Int32 userID)
        {
            string RoleID;
            command.CommandText = "select [Post_ID] from [Users] where [ID_users] = '" + userID + "'";
            connection.Open();
            RoleID = command.ExecuteScalar().ToString();
            connection.Close();
            return RoleID;
        }
        /// <summary>
        /// Должность
        /// </summary>
        /// <param name="userID">id Авторизованного пользователя</param>
        /// <returns>Роль пользователя</returns>

        public string userDoljnost(Int32 userID)
        {
            string DoljnostID;
            command.CommandText = "select [Post_ID] from [Users] where [ID_users]  like '%" + userID + "%'";
            connection.Open();
            DoljnostID = command.ExecuteScalar().ToString();
            connection.Close();
            return DoljnostID;
        }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        /// <param name="userID">id Авторизованного пользователя</param>
        /// <returns>ФИО пользователя</returns>
        public string Profile(string userID)
        {
            string Profile;
            command.CommandText = "select [Surname] + ' ' + [Name] + ' ' + [Middlename] + ' (' +[Name_post]+ ') ' FROM [dbo].[Users] INNER JOIN [dbo].[Post] ON [dbo].[Users].[Post_ID] = [dbo].[Post].[ID_post] where [ID_users] like '%" + userID + "%'";
            connection.Open();
            Profile = command.ExecuteScalar().ToString();
            connection.Close();
            return Profile;
        }

        /// <summary>
        /// Проверка уникальности логина
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>Количество найденных пользователей</returns>
        public Int32 LoginCheck(string login)
        {
            int loginCheck;
            command.CommandText = "select count (*) from [Authorization] where Login like '%" + login + "%'";
            connection.Open();
            loginCheck = Convert.ToInt32(command.ExecuteScalar().ToString());
            connection.Close();
            return loginCheck;
        }

        /// <summary>
        /// Проверка количества учетных записей
        /// </summary>
        /// <param name="ID_authorization">id авторизации</param>
        /// <returns>Количество найденных записей</returns>
        public Int32 AccountCheck()
        {
            int AccountCheck;
            command.CommandText = "select count (*) from [Authorization] where [ID_authorization] like [ID_authorization]";
            connection.Open();
            AccountCheck = Convert.ToInt32(command.ExecuteScalar().ToString());
            connection.Close();
            return AccountCheck;
        }
        /// <summary>
        /// Роль
        /// </summary>
        /// <param name="userID">id Авторизованного пользователя</param>
        /// <returns>Роль пользователя</returns>
        public Int32 Role(Int32 userID)
        {
            int Role;
            command.CommandText = "select [Post_ID] from [Users] where [ID_users] = '" + userID + "'";
            connection.Open();
            Role = Convert.ToInt32(command.ExecuteScalar().ToString());
            connection.Close();
            return Role;
        }
        /// <summary>
        /// Проверка количества проф. модулей
        /// </summary>
        /// <param name="ID_professional_module">id проф. модулей</param>
        /// <returns>Количество найденных записей</returns>
        public Int32 ProfIDCheck()
        {
            int ProfIDCheck;
            command.CommandText = "select count (*) from [Professional_module] where [ID_professional_module] like [ID_professional_module]";
            connection.Open();
            ProfIDCheck = Convert.ToInt32(command.ExecuteScalar().ToString());
            connection.Close();
            return ProfIDCheck;
        }
        /// <summary>
        /// Проверка удаления стандартного администратора
        /// </summary>
        /// <param name="ID_professional_module">id пользователя</param>
        /// <returns>ID администратора</returns>
        public Int32 UserNotDelete()
        {
            int UserNotDelete;
            command.CommandText = "select [ID_users] from [Users] where [ID_users] like '" + 1 + "'";
            connection.Open();
            UserNotDelete = Convert.ToInt32(command.ExecuteScalar().ToString());
            connection.Close();
            return UserNotDelete;
        }
        public void SpecializationFill()
        {
            dtFill(dtSpecialization, qrSpecialization);
        }
        public void Professional_moduleFill()
        {
            dtFill(dtProfessional_module, qrProfessional_module);
        }
        public void DisciplinesFill()
        {
            dtFill(dtDisciplines, qrDisciplines);
        }
        public void TeachersFill()
        {
            dtFill(dtTeachers, qrTeachers);
        }
        public void PostFill()
        {
            dtFill(dtPost, qrPost);
        }
        public void AuthorizationFill()
        {
            dtFill(dtAuthorization, qrAuthorization);
        }
        public void UsersFill()
        {
            dtFill(dtUsers, qrUsers);
        }
        public void Students_listFill()
        {
            dtFill(dtStudents_list, qrStudents_list);
        }
        public void Students_disciplinesFill()
        {
            dtFill(dtStudents_disciplines, qrStudents_disciplines);
        }
        public void Group_scoresFill()
        {
            dtFill(dtGroup_scores, qrGroup_scores);
        }
        public void GroupsFill()
        {
            dtFill(dtGroups, qrGroups);
        }
    }
}