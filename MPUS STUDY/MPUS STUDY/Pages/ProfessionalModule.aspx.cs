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
    public partial class ProfessionalModule : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrProfessional_module;
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
                //проверка роли пользователя и скрытие страниц администратора
                if (connection.Role(DBConnection.idUser) != 1)
                {
                    admin.Visible = false;
                }
                else
                {
                    admin.Visible = true;
                }
                //проверка таблицы и добавление пустого значения по умолчанию
                if (connection.ProfIDCheck() < 1)
                {
                    string CodePM = "Нет";
                    string NamePM = "Нет";
                    DBProcedures procedures = new DBProcedures();
                    procedures.spProfessional_module_insert(CodePM, NamePM);
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
        //очистка полей в обратной связи
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsProfessionalModule.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsProfessionalModule.SelectCommand = qr;
            sdsProfessionalModule.DataSourceMode = SqlDataSourceMode.DataReader;
            gvProfModule.DataSource = sdsProfessionalModule;
            gvProfModule.DataBind();
        }
        //очистка полей на странице после действия 
        protected void Cleaner()
        {
            DBConnection.IDProfModule = 0;
            tbCodeProfModule.Text = string.Empty;
            tbNameProfModule.Text = string.Empty;

        }
        //удаление записи в бд
        protected void gvProfModule_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvProfModule.SelectedRow;
                DBConnection.IDProfModule = Convert.ToInt32(gvProfModule.Rows[Index].Cells[1].Text.ToString());
                if (DBConnection.IDProfModule != 1)
                {
                    procedures.spProfessional_module_delete(DBConnection.IDProfModule);
                    gvFill(QR);
                    Cleaner();
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                else
                {
                    lblError.Visible = true;
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }

        }
        //скрытие столбцов
        protected void gvProfModule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvProfModule, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDProfModule = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spProfessional_module_insert(Convert.ToString(tbCodeProfModule.Text), Convert.ToString(tbNameProfModule.Text));
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
                procedures.spProfessional_module_update(DBConnection.IDProfModule, Convert.ToString(tbCodeProfModule.Text), Convert.ToString(tbNameProfModule.Text));
                if (DBConnection.IDProfModule != 1)
                {                    
                    Cleaner();
                    gvFill(QR);
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                else
                {
                    lblError.Visible = true;
                }
               
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
         
        }
        //заполнение текстбоксов выбранными значениями
        protected void gvProfModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvProfModule.Rows)
            {
                if (row.RowIndex == gvProfModule.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvProfModule.SelectedRow;
            tbCodeProfModule.Text = roww.Cells[2].Text.ToString();
            tbNameProfModule.Text = roww.Cells[3].Text.ToString();
            DBConnection.IDProfModule = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }
        //сортировка по столбцу
        protected void gvProfModule_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Код профессионального модуля"):
                    e.SortExpression = "[Code_professional_module]";
                    break;
                case ("Название профессионального модуля"):
                    e.SortExpression = "[Name_professional_module]";
                    break;
            }
            sortGridView(gvProfModule, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvProfModule.Rows)
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