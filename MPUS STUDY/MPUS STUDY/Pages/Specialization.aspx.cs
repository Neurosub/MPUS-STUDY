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
    public partial class Specialization : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrSpecialization;
            if (!IsPostBack)
            {
                //проверка авторизации пользователя и вывод его данных в лейбл
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
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsSpecialization.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsSpecialization.SelectCommand = qr;
            sdsSpecialization.DataSourceMode = SqlDataSourceMode.DataReader;
            gvSpecialization.DataSource = sdsSpecialization;
            gvSpecialization.DataBind();
        }
        //очистка полей на странице после действия
        protected void Cleaner()
        {
            DBConnection.IDSpecialization = 0;
            tbNameSpecialty.Text = string.Empty;
            tbNumberSpecialty.Text = string.Empty;

        }
        //удаление записи в бд
        protected void gvSpecialization_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvSpecialization.SelectedRow;
                DBConnection.IDSpecialization = Convert.ToInt32(gvSpecialization.Rows[Index].Cells[1].Text.ToString());
                procedures.spSpecialization_delete(DBConnection.IDSpecialization);
                gvFill(QR);
                Cleaner();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }
        //отображение столбцов и выбор записи
        protected void gvSpecialization_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSpecialization, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }


        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDSpecialization = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spSpecialization_insert(Convert.ToString(tbNumberSpecialty.Text), Convert.ToString(tbNameSpecialty.Text));
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
                procedures.spSpecialization_update(DBConnection.IDSpecialization, Convert.ToString(tbNumberSpecialty.Text), Convert.ToString(tbNameSpecialty.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
            
        }
        //заполнение текстбоксов по выбранной строке
        protected void gvSpecialization_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvSpecialization.Rows)
            {
                if (row.RowIndex == gvSpecialization.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvSpecialization.SelectedRow;
            tbNumberSpecialty.Text = roww.Cells[2].Text.ToString();
            tbNameSpecialty.Text = roww.Cells[3].Text.ToString();
            DBConnection.IDSpecialization = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }


        //сортировка по заголовкам
        protected void gvSpecialization_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Номер специальности"):
                    e.SortExpression = "[Number_specialty]";
                    break;
                case ("Наименование специальности"):
                    e.SortExpression = "[Name_specialty]";
                    break;
            }
            sortGridView(gvSpecialization, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvSpecialization.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text))
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
