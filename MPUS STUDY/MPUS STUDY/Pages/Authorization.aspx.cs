using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Text;
using System.Net.Mail;
using System.Net;
using MPUS_STUDY.Classes;

namespace MPUS_STUDY.Pages
{
    public partial class Authorization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConnection connection = new DBConnection();
            //Проверка кол-ва аккаунтов
            if (connection.AccountCheck() > 0)
            {
                btAdminRegister.Visible = false;
            }
        }
        //отправка письма в обратной связи
        protected void btSendHelp_Click(object sender, EventArgs e)
        {
            int port = 587;
            bool enableSSL = true;

            string emailFrom = "bot.mpusstudy@bk.ru"; /*(почта бота)*/
            string password = "Feedback.mpusstudy.Feedback";
            string emailTo = "bot.mpusstudy@bk.ru";
            string subject = tbNameHelp.Text; /*(заголовок сообщения)*/
            string title = "От:  " + tbNameHelp.Text.ToString();  /*это имя отправителя*/
            string from = "Почта:  " + tbMailHelp.Text; /*это почта отправителя*/
            string message = "Возникшая проблема:  " + tbHelp.Text; /*это проблема отправителя*/
            string smtpAddress = "smtp.mail.ru";
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(emailFrom);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = title + "\r\n" + from + "\r\n" + message;
            mail.IsBodyHtml = false;

            using (SmtpClient smtp = new SmtpClient(smtpAddress, port))
            {
                smtp.Credentials = new NetworkCredential(emailFrom, password);
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }

            Cleaner1();
            Response.Redirect(Request.Url.AbsoluteUri);

        }
        //очистка полей обратной связи
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //авторизация
        protected void btEnter_Click(object sender, EventArgs e)
        {
            string Login;
            string Password;
            DBConnection connection = new DBConnection();
            connection.Authorization(tbLogin.Text);
            connection.getIDPersonal(DBConnection.idUser);
            switch (DBConnection.idUser)
            {
                case (0):
                    tbLogin.BackColor = ColorTranslator.FromHtml("#cc0000");
                    tbPassword.BackColor = ColorTranslator.FromHtml("#cc0000");
                    lblAuthorization.Visible = true;
                    break;
                default:
                    //Проверка логина и пароля 
                    SqlCommand command = new SqlCommand("", DBConnection.connection);
                    command.CommandText = "select [Login] from [Authorization] where [ID_Authorization] = '" + DBConnection.idUser + "'";
                    DBConnection.connection.Open();
                    Login = command.ExecuteScalar().ToString(); //Строка (логин) из базы данных
                    DBConnection.connection.Close();
                    if (tbLogin.Text.ToString() == Login)
                    {
                        command.CommandText = "select [Password] from [Authorization] where [ID_Authorization] = '" + DBConnection.idUser + "'";
                        DBConnection.connection.Open();
                        Password = command.ExecuteScalar().ToString(); //Строка (пароль) из базы данных                       
                        DBConnection.connection.Close();
                        if (tbPassword.Text.ToString() == Encryption.Decrypt(Password))
                        {                            
                            switch (connection.userRole(DBConnection.idUser))
                            {
                                case ("1"):
                                    Response.Redirect("UsersList.aspx");
                                    break;
                                case ("2"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("3"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("4"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("5"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("6"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("7"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("8"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("9"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                                case ("10"):
                                    Response.Redirect("Advantages.aspx");
                                    break;
                            }
                        }
                        else
                        {
                            tbLogin.BackColor = ColorTranslator.FromHtml("#cc0000");
                            tbPassword.BackColor = ColorTranslator.FromHtml("#cc0000");
                            lblAuthorization.Visible = true;
                        }
                        break;
                    }
                    else
                    {
                        tbLogin.BackColor = ColorTranslator.FromHtml("#cc0000");
                        tbPassword.BackColor = ColorTranslator.FromHtml("#cc0000");
                        lblAuthorization.Visible = true;
                    }
                    break;
            }
        }
        //регистрация аккаунта администратора
        protected void btAdminRegister_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection connection = new DBConnection();
                //Проверка кол-ва аккаунтов
                if (connection.AccountCheck() > 0)
                {
                    btAdminRegister.Visible = false;
                }
                else
                {
                    string Login = "Admin";
                    string Password = "Admin";
                    string Post = "Администратор";
                    string Surname = "Первоначальный";
                    string Name = "Администратор";
                    string Middlename = "Программы";
                    DBProcedures procedures = new DBProcedures();
                    procedures.spAdmin_register(Login, Password, Post, Surname, Name, Middlename);
                    lblAccountSucces.Visible = true;

                }
            }
            finally
            {
                DBConnection.connection.Close();
            }
           

        }


    }
}