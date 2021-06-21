using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using MPUS_STUDY.Classes;

namespace MPUS_STUDY.Pages
{
    public partial class Advantages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //проверка аторизации и вывод в лейбл данных
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    lblProfile.Text = connection.Profile(Convert.ToString(DBConnection.idUser));
                }
                else
                {
                    Response.Redirect("Authorization.aspx");
                }
                //проверка роли и скрытие страниц администратора
                if (connection.Role(DBConnection.idUser) != 1)
                {
                    admin.Visible = false;
                }
                else
                {
                    admin.Visible = true;
                }
            }
        }
        //выход из аккаунта
        protected void btExit_Click(object sender, EventArgs e)
        {
            DBConnection.idUser = 0;
            Response.Redirect("Authorization.aspx");

        }
        //отправка письма на почту в обратной связи
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
    }
}