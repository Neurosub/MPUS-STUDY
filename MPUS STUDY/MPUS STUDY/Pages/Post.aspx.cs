using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using MPUS_STUDY.Classes;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.Windows;

namespace MPUS_STUDY.Pages
{
    public partial class Post : System.Web.UI.Page
    {
        //переменная для заполнения таблицы
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //присваивание значения переменной
            QR = DBConnection.qrPost;
            if (!IsPostBack)
            {
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    //заполнение таблицы и вывод аккаунта пользователя в lbl
                    gvFill(QR);
                    lblProfile.Text = connection.Profile(Convert.ToString(DBConnection.idUser));

                }
                else
                {
                    Response.Redirect("Authorization.aspx");
                }
                //определение роли и скрытие страниц администратора
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
        //отправка письма на почту
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
        //очистка полей модальной формы
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //заполнение таблицы данными из бд
        private void gvFill(string qr)
        {
            sdsPost.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsPost.SelectCommand = qr;
            sdsPost.DataSourceMode = SqlDataSourceMode.DataReader;
            gvPost.DataSource = sdsPost;
            gvPost.DataBind();
        }
        //очистка полей и id
        protected void Cleaner()
        {
            DBConnection.IDPost = 0;
            tbNamePost.Text = string.Empty;

        }

        //удаление строки из таблицы
        protected void gvPost_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvPost.SelectedRow;
                DBConnection.IDPost = Convert.ToInt32(gvPost.Rows[Index].Cells[1].Text.ToString());
                procedures.spPost_delete(DBConnection.IDPost);
                gvFill(QR);
                Cleaner();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }

        }

        //скрытие столбцов и выбор строк из таблицы
        protected void gvPost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvPost, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }


        //добавление записи
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDPost = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spPost_insert(Convert.ToString(tbNamePost.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
            
        }

        //изменение записи
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DBProcedures procedures = new DBProcedures();
                procedures.spPost_update(DBConnection.IDPost, Convert.ToString(tbNamePost.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
            
        }

        //выбор строк из таблицы
        protected void gvPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvPost.Rows)
            {
                if (row.RowIndex == gvPost.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvPost.SelectedRow;
            tbNamePost.Text = roww.Cells[2].Text.ToString();
            DBConnection.IDPost = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }


        //сортировка таблицы по заголовкам
        protected void gvPost_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Название должности"):
                    e.SortExpression = "[Name_post]";
                    break;
            }
            sortGridView(gvPost, e, out sortDirection, out strField);
            string strDirection = sortDirection
                == SortDirection.Ascending ? "ASC" : "DESC";
            gvFill(QR + " order by " + e.SortExpression + " " + strDirection);
        }
        private void sortGridView(GridView gridView,
         GridViewSortEventArgs e,
         out SortDirection sortDirection,
         out string strSortField)
        {
            strSortField = e.SortExpression;
            sortDirection = e.SortDirection;

            if (gridView.Attributes["CurrentSortField"] != null &&
                gridView.Attributes["CurrentSortDirection"] != null)
            {
                if (strSortField ==
                    gridView.Attributes["CurrentSortField"])
                {
                    if (gridView.Attributes["CurrentSortDirection"]
                        == "ASC")
                    {
                        sortDirection = SortDirection.Descending;
                    }
                    else
                    {
                        sortDirection = SortDirection.Ascending;
                    }
                }
            }
            gridView.Attributes["CurrentSortField"] = strSortField;
            gridView.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC"
                : "DESC");
        }

        //Поиск
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                foreach (GridViewRow row in gvPost.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text))
                        row.BackColor = ColorTranslator.FromHtml("#495057");
                }
            }
            btCancel.Visible = true;
            Cleaner();
        }

        //отмена поиска
        protected void btCanсel_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
            btCancel.Visible = false;
            Cleaner();
            gvFill(QR);
        }

        //выход из аккаунта
        protected void btExit_Click(object sender, EventArgs e)
        {
            DBConnection.idUser = 0;
            Response.Redirect("Authorization.aspx");

        }
    }
}