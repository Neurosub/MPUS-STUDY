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


namespace MPUS_STUDY.Pages
{
    public partial class TeachersList : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrTeachers;
            if (!IsPostBack)
            {
                //проверка авторизации пользователя и вывод данных в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    gvFill(QR);
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
        //отправка письма в окне обратной связи
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
        //очистка полей в окне обратной связи
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //очистка полей после действия
        protected void Cleaner()
        {
            DBConnection.IDTeachers = 0;
            tbSurname.Text = string.Empty;
            tbName.Text = string.Empty;
            tbMiddlename.Text = string.Empty;

        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsTeachers.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsTeachers.SelectCommand = qr;
            sdsTeachers.DataSourceMode = SqlDataSourceMode.DataReader;
            gvTeachers.DataSource = sdsTeachers;
            gvTeachers.DataBind();
        }
        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDTeachers = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spTeachers_insert(Convert.ToString(tbSurname.Text), Convert.ToString(tbName.Text), Convert.ToString(tbMiddlename.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
            
        }
        //изменение записи в бд
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DBProcedures procedures = new DBProcedures();
                procedures.spTeachers_update(DBConnection.IDTeachers, Convert.ToString(tbSurname.Text), Convert.ToString(tbName.Text), Convert.ToString(tbMiddlename.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
            
        }
        //скрытие столбцов
        protected void gvTeachers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvTeachers, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //удаление записи в бд
        protected void gvTeachers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvTeachers.SelectedRow;
                DBConnection.IDTeachers = Convert.ToInt32(gvTeachers.Rows[Index].Cells[1].Text.ToString());
                procedures.spTeachers_delete(DBConnection.IDTeachers);
                gvFill(QR);
                Cleaner();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }
        //заполнение текстбоксов по выбранной строке
        protected void gvTeachers_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvTeachers.Rows)
            {
                if (row.RowIndex == gvTeachers.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvTeachers.SelectedRow;
            tbSurname.Text = roww.Cells[2].Text.ToString();
            tbName.Text = roww.Cells[3].Text.ToString();
            tbMiddlename.Text = roww.Cells[4].Text.ToString();
            DBConnection.IDTeachers = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }
        //сортировка по столбцу
        protected void gvTeachers_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Фамилия"):
                    e.SortExpression = "[Surname]";
                    break;
                case ("Имя"):
                    e.SortExpression = "[Name]";
                    break;
                case ("Отчество"):
                    e.SortExpression = "[Middlename]";
                    break;
            }
            sortGridView(gvTeachers, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvTeachers.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[4].Text.Equals(tbSearch.Text))
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


    }
}
