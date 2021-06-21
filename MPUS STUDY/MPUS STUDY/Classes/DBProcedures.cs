using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace MPUS_STUDY.Classes
{
    public class DBProcedures
    {
        private SqlCommand command = new SqlCommand("", DBConnection.connection);
        private void commandConfig(string config)
        {
            command.CommandType =
                System.Data.CommandType.StoredProcedure;
            command.CommandText = "[dbo].[" + config + "]";
            command.Parameters.Clear();

        }

        //Процедуры для групп
        public void spGroups_insert(string Number_groups, string Number_course, Int32 Specialization_ID)
        {
            commandConfig("Groups_insert");
            command.Parameters.AddWithValue("@Number_groups", Number_groups);
            command.Parameters.AddWithValue("@Number_course", Number_course);
            command.Parameters.AddWithValue("@Specialization_ID", Specialization_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spGroups_update(Int32 ID_groups, string Number_groups, string Number_course, Int32 Specialization_ID)
        {
            commandConfig("Groups_update");
            command.Parameters.AddWithValue("@ID_groups", ID_groups);
            command.Parameters.AddWithValue("@Number_groups", Number_groups);
            command.Parameters.AddWithValue("@Number_course", Number_course);
            command.Parameters.AddWithValue("@Specialization_ID", Specialization_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spGroups_delete(Int32 ID_groups)
        {
            commandConfig("Groups_delete");
            command.Parameters.AddWithValue("@ID_groups", ID_groups);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для преподавателей
        public void spTeachers_insert(string Surname, string Name, string Middlename)
        {
            commandConfig("Teachers_insert");
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spTeachers_update(Int32 ID_teachers, string Surname, string Name, string Middlename)
        {
            commandConfig("Teachers_update");
            command.Parameters.AddWithValue("@ID_teachers", ID_teachers); 
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spTeachers_delete(Int32 ID_teachers)
        {
            commandConfig("Teachers_delete");
            command.Parameters.AddWithValue("@ID_teachers", ID_teachers);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для преподавателей
        public void spPost_insert(string Name_post)
        {
            commandConfig("Post_insert");
            command.Parameters.AddWithValue("@Name_post", Name_post);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spPost_update(Int32 ID_post, string Name_post)
        {
            commandConfig("Post_update");
            command.Parameters.AddWithValue("@ID_post", ID_post);
            command.Parameters.AddWithValue("@Name_post", Name_post);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spPost_delete(Int32 ID_post)
        {
            commandConfig("Post_delete");
            command.Parameters.AddWithValue("@ID_post", ID_post);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для авторизации
        public void spAuthorization_insert(string Login, string Password)
        {
            commandConfig("Authorization_insert");
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spAuthorization_update(Int32 ID_Authorization, string Login, string Password)
        {
            commandConfig("Authorization_update");
            command.Parameters.AddWithValue("@ID_Authorization", ID_Authorization);
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spAuthorization_delete(Int32 ID_Authorization)
        {
            commandConfig("Authorization_delete");
            command.Parameters.AddWithValue("@ID_Authorization", ID_Authorization);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для проф. модуля
        public void spProfessional_module_insert(string Code_professional_module, string Name_professional_module)
        {
            commandConfig("Professional_module_insert");
            command.Parameters.AddWithValue("@Code_professional_module", Code_professional_module);
            command.Parameters.AddWithValue("@Name_professional_module", Name_professional_module);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spProfessional_module_update(Int32 ID_professional_module, string Code_professional_module, string Name_professional_module)
        {
            commandConfig("Professional_module_update");
            command.Parameters.AddWithValue("@ID_professional_module", ID_professional_module);
            command.Parameters.AddWithValue("@Code_professional_module", Code_professional_module);
            command.Parameters.AddWithValue("@Name_professional_module", Name_professional_module);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spProfessional_module_delete(Int32 ID_professional_module)
        {
            commandConfig("Professional_module_delete");
            command.Parameters.AddWithValue("@ID_professional_module", ID_professional_module);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
       
        //Процедура для регистрации администратора
        public void spAdmin_register(string Login, string Password, string Name_post, string Surname, string Name, string Middlename)
        {
            Password = Encryption.Encrypt(Password);
            commandConfig("Admin_register");
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Name_post", Name_post);
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
       
        //Процедуры для пользователей
        public void spUsers_insert(string Login, string Password, string Surname, string Name, string Middlename, Int32 Post_ID)
        {
            Password = Encryption.Encrypt(Password);
            commandConfig("Users_insert");            
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            command.Parameters.AddWithValue("@Post_ID", Post_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();

        }

        public void spUsers_update(Int32 ID_users, string Surname, string Name, string Middlename, Int32 Post_ID, string Login, string Password)
        {
            Password = Encryption.Encrypt(Password);
            commandConfig("Users_update");       
            command.Parameters.AddWithValue("@ID_users", ID_users);            
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            command.Parameters.AddWithValue("@Post_ID", Post_ID);
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spUsers_delete(Int32 ID_users, Int32 Authorization_ID)
        {
            commandConfig("Users_delete");
            command.Parameters.AddWithValue("@ID_users", ID_users);
            command.Parameters.AddWithValue("@Authorization_ID", Authorization_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для дисциплин
        public void spDisciplines_insert(string Disciplines_type, string Subject_code, string Name_disciplines, string Professional_module_ID)
        {
            commandConfig("Disciplines_insert");
            command.Parameters.AddWithValue("@Disciplines_type", Disciplines_type);
            command.Parameters.AddWithValue("@Subject_code", Subject_code);
            command.Parameters.AddWithValue("@Name_disciplines", Name_disciplines);
            command.Parameters.AddWithValue("@Professional_module_ID", Professional_module_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spDisciplines_update(Int32 ID_disciplines, string Disciplines_type, string Subject_code, string Name_disciplines, string Professional_module_ID)
        {
            commandConfig("Disciplines_update");
            command.Parameters.AddWithValue("@ID_disciplines", ID_disciplines);
            command.Parameters.AddWithValue("@Disciplines_type", Disciplines_type);
            command.Parameters.AddWithValue("@Subject_code", Subject_code);
            command.Parameters.AddWithValue("@Name_disciplines", Name_disciplines);
            command.Parameters.AddWithValue("@Professional_module_ID", Professional_module_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spDisciplines_delete(Int32 ID_disciplines)
        {
            commandConfig("Disciplines_delete");
            command.Parameters.AddWithValue("@ID_disciplines", ID_disciplines);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для списка студентов
        public void spStudents_list_insert(string Surname, string Name, string Middlename, Int32 Groups_ID)
        {
            commandConfig("Students_list_insert");
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            command.Parameters.AddWithValue("@Groups_ID", Groups_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spStudents_list_update(Int32 ID_students_list, string Surname, string Name, string Middlename, Int32 Groups_ID)
        {
            commandConfig("Students_list_update");
            command.Parameters.AddWithValue("@ID_students_list", ID_students_list);
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Middlename", Middlename);
            command.Parameters.AddWithValue("@Groups_ID", Groups_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spStudents_list_delete(Int32 ID_students_list)
        {
            commandConfig("Students_list_delete");
            command.Parameters.AddWithValue("@ID_students_list", ID_students_list);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для дисциплин у студентов
        public void spStudents_disciplines_insert(string Number_semester, string Attestation_form, Int32 Groups_ID, Int32 Disciplines_ID, Int32 Teachers_ID)
        {
            commandConfig("Students_disciplines_insert");
            command.Parameters.AddWithValue("@Number_semester", Number_semester);
            command.Parameters.AddWithValue("@Attestation_form", Attestation_form);
            command.Parameters.AddWithValue("@Groups_ID", Groups_ID);
            command.Parameters.AddWithValue("@Disciplines_ID", Disciplines_ID);
            command.Parameters.AddWithValue("@Teachers_ID", Teachers_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spStudents_disciplines_update(Int32 ID_students_disciplines, string Number_semester, string Attestation_form, Int32 Groups_ID, Int32 Disciplines_ID, Int32 Teachers_ID)
        {
            commandConfig("Students_disciplines_update");
            command.Parameters.AddWithValue("@ID_students_disciplines", ID_students_disciplines);
            command.Parameters.AddWithValue("@Number_semester", Number_semester);
            command.Parameters.AddWithValue("@Attestation_form", Attestation_form);
            command.Parameters.AddWithValue("@Groups_ID", Groups_ID);
            command.Parameters.AddWithValue("@Disciplines_ID", Disciplines_ID);
            command.Parameters.AddWithValue("@Teachers_ID", Teachers_ID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spStudents_disciplines_delete(Int32 ID_students_disciplines)
        {
            commandConfig("Students_disciplines_delete");
            command.Parameters.AddWithValue("@ID_students_disciplines", ID_students_disciplines);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для успеваемости групп
        public void spGroup_scores_insert( Int32 Students_list_ID, Int32 Students_disciplines_ID, Int32 Users_ID, string Score)
        {
            commandConfig("Group_scores_insert");
            command.Parameters.AddWithValue("@Students_list_ID", Students_list_ID);
            command.Parameters.AddWithValue("@Students_disciplines_ID", Students_disciplines_ID);
            command.Parameters.AddWithValue("@Users_ID", Users_ID);
            command.Parameters.AddWithValue("@Score", Score);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spGroup_scores_update(Int32 ID_group_scores, Int32 Students_list_ID, Int32 Students_disciplines_ID, Int32 Users_ID, string Score)
        {
            commandConfig("Group_scores_update");
            command.Parameters.AddWithValue("@ID_group_scores", ID_group_scores);
            command.Parameters.AddWithValue("@Students_list_ID", Students_list_ID);
            command.Parameters.AddWithValue("@Students_disciplines_ID", Students_disciplines_ID);
            command.Parameters.AddWithValue("@Users_ID", Users_ID);
            command.Parameters.AddWithValue("@Score", Score);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spGroup_scores_delete(Int32 ID_group_scores)
        {
            commandConfig("Group_scores_delete");
            command.Parameters.AddWithValue("@ID_group_scores", ID_group_scores);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        //Процедуры для специальностей
        public void spSpecialization_insert(string Number_specialty, string Name_specialty)
        {
            commandConfig("Specialization_insert");
            command.Parameters.AddWithValue("@Number_specialty", Number_specialty);
            command.Parameters.AddWithValue("@Name_specialty", Name_specialty);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        public void spSpecialization_update(Int32 ID_specialization, string Number_specialty, string Name_specialty)
        {
            commandConfig("Specialization_update");
            command.Parameters.AddWithValue("@ID_specialization", ID_specialization);
            command.Parameters.AddWithValue("@Number_specialty", Number_specialty);
            command.Parameters.AddWithValue("@Name_specialty", Name_specialty);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

        public void spSpecialization_delete(Int32 ID_specialization)
        {
            commandConfig("Specialization_delete");
            command.Parameters.AddWithValue("@ID_specialization", ID_specialization);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }

    }
}