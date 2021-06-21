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
    public partial class Disciplines : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrDisciplines;
            if (!IsPostBack)
            {
                //проверка на авторизацию пользователя отображение его данных в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    QR = DBConnection.qrDisciplines;
                    gvFill(QR);
                    ddlProfessionalModuleFill();
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
        //очистка полей страницы после действий
        protected void Cleaner()
        {
            DBConnection.IDDisciplines = 0;
            tbTypeDisciplines.Text = string.Empty;
            tbSubjectCode.Text = string.Empty;
            tbNameDisciplines.Text = string.Empty;
            ddlProfessionalModule.SelectedIndex = 0;

        }
        //заполнение выпадающего списка значениями из бд
        private void ddlProfessionalModuleFill()
        {
            sdsddl.ConnectionString =
            DBConnection.connection.ConnectionString.ToString();
            sdsddl.SelectCommand = DBConnection.qrProfessional_moduleView;
            sdsddl.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlProfessionalModule.DataSource = sdsddl;
            ddlProfessionalModule.DataTextField = "Профессиональный модуль";
            ddlProfessionalModule.DataValueField = "ID_professional_module";
            ddlProfessionalModule.DataBind();
        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsDisciplines.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsDisciplines.SelectCommand = qr;
            sdsDisciplines.DataSourceMode = SqlDataSourceMode.DataReader;
            gvDisciplines.DataSource = sdsDisciplines;
            gvDisciplines.DataBind();
        }


        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDDisciplines = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spDisciplines_insert(Convert.ToString(tbTypeDisciplines.Text), Convert.ToString(tbSubjectCode.Text), Convert.ToString(tbNameDisciplines.Text), Convert.ToString(ddlProfessionalModule.SelectedValue));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
            
        }
        //изменине записи в бд
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DBProcedures procedures = new DBProcedures();
                procedures.spDisciplines_update(DBConnection.IDDisciplines, Convert.ToString(tbTypeDisciplines.Text), Convert.ToString(tbSubjectCode.Text), Convert.ToString(tbNameDisciplines.Text), Convert.ToString(ddlProfessionalModule.SelectedValue));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
            
        }
        //отображение столбцов в таблице и выбор строки по нажатию
        protected void gvDisciplines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[5].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvDisciplines, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //удаление строки в таблице 
        protected void gvDisciplines_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvDisciplines.SelectedRow;
                DBConnection.IDDisciplines = Convert.ToInt32(gvDisciplines.Rows[Index].Cells[1].Text.ToString());
                procedures.spDisciplines_delete(DBConnection.IDDisciplines);
                gvFill(QR);
                Cleaner();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }

        //выбор записи в таблице и заполнение текстбоксов выбранными значениями
        protected void gvDisciplines_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvDisciplines.Rows)
            {
                if (row.RowIndex == gvDisciplines.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvDisciplines.SelectedRow;
            tbTypeDisciplines.Text = roww.Cells[2].Text.ToString();
            tbSubjectCode.Text = roww.Cells[3].Text.ToString();
            tbNameDisciplines.Text = roww.Cells[4].Text.ToString();
            ddlProfessionalModule.SelectedValue = roww.Cells[5].Text.ToString();
            DBConnection.IDDisciplines = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }

        // сортировка по столбцу
        protected void gvDisciplines_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Тип дисциплины"):
                    e.SortExpression = "[Disciplines_type]";
                    break;
                case ("Код предмета"):
                    e.SortExpression = "[Subject_code]";
                    break;
                case ("Название дисциплины"):
                    e.SortExpression = "[Name_disciplines]";
                    break;
                case ("Код профессионального модуля"):
                    e.SortExpression = "[Code_professional_module]";
                    break;
                case ("Название профессионального модуля"):
                    e.SortExpression = "[Name_professional_module]";
                    break;
            }
            sortGridView(gvDisciplines, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvDisciplines.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[4].Text.Equals(tbSearch.Text) ||
                        row.Cells[6].Text.Equals(tbSearch.Text) ||
                        row.Cells[7].Text.Equals(tbSearch.Text))
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