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
    public partial class Students_list : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //проверка авторизации пользователя и отображение его данных в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    QR = DBConnection.qrStudents_list;
                    gvFill(QR);
                    ddlGroupNumberFill();
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
        //отправка письма на почту в окне обратной связи
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
        //очистка полей на странице после действий
        protected void Cleaner()
        {
            DBConnection.IDStudentsList = 0;
            tbSurname.Text = string.Empty;
            tbName.Text = string.Empty;
            tbMiddlename.Text = string.Empty;
            ddlGroupNumber.SelectedIndex = 0;

        }
        //заполнение выпадающего списка 
        private void ddlGroupNumberFill()
        {
            sdsddlGroupNumber.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsddlGroupNumber.SelectCommand = DBConnection.qrGroupView;
            sdsddlGroupNumber.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlGroupNumber.DataSource = sdsddlGroupNumber;
            ddlGroupNumber.DataTextField = "Группа";
            ddlGroupNumber.DataValueField = "ID_groups";
            ddlGroupNumber.DataBind();
        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsStudents_list.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsStudents_list.SelectCommand = qr;
            sdsStudents_list.DataSourceMode = SqlDataSourceMode.DataReader;
            gvStudents.DataSource = sdsStudents_list;
            gvStudents.DataBind();
        }


        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDStudentsList = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spStudents_list_insert(Convert.ToString(tbSurname.Text), Convert.ToString(tbName.Text), Convert.ToString(tbMiddlename.Text), Convert.ToInt32(ddlGroupNumber.SelectedValue));
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
                procedures.spStudents_list_update(DBConnection.IDStudentsList, Convert.ToString(tbSurname.Text), Convert.ToString(tbName.Text), Convert.ToString(tbMiddlename.Text), Convert.ToInt32(ddlGroupNumber.SelectedValue));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
            
        }
        //скрытие столбцов и выбор записи при нажатии
        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[5].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvStudents, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //удаление строки
        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvStudents.SelectedRow;
                DBConnection.IDStudentsList = Convert.ToInt32(gvStudents.Rows[Index].Cells[1].Text.ToString());
                procedures.spStudents_list_delete(DBConnection.IDStudentsList);
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
        protected void gvStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvStudents.Rows)
            {
                if (row.RowIndex == gvStudents.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvStudents.SelectedRow;
            tbSurname.Text = roww.Cells[2].Text.ToString();
            tbName.Text = roww.Cells[3].Text.ToString();
            tbMiddlename.Text = roww.Cells[4].Text.ToString();
            ddlGroupNumber.SelectedValue = roww.Cells[5].Text.ToString();
            DBConnection.IDStudentsList = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }
        //сортировка по выбранному столбцу
        protected void gvStudents_Sorting(object sender, GridViewSortEventArgs e)
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
                case ("Номер группы"):
                    e.SortExpression = "[Number_groups]";
                    break;
            }
            sortGridView(gvStudents, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvStudents.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[4].Text.Equals(tbSearch.Text) ||
                        row.Cells[6].Text.Equals(tbSearch.Text))
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
