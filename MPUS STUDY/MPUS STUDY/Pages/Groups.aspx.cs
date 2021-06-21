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
    public partial class Groups : System.Web.UI.Page
    {
        //объявление переменной для БД
        private string QR = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //проверка на авторизацию пользователя и вывод его данные в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    QR = DBConnection.qrGroups;
                    gvFill(QR);
                    ddlSpecializationFill();
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
        //очистка текстбоксов в окне обратной связи
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //заполнение выпадающего списка со специальностями
        private void ddlSpecializationFill()
        {
            sdsddl.ConnectionString =
            DBConnection.connection.ConnectionString.ToString();
            sdsddl.SelectCommand = DBConnection.qrSpecializationView;
            sdsddl.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlSpecialization.DataSource = sdsddl;
            ddlSpecialization.DataTextField = "Специальность";
            ddlSpecialization.DataValueField = "ID_specialization";
            ddlSpecialization.DataBind();
        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsGroups.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsGroups.SelectCommand = qr;
            sdsGroups.DataSourceMode = SqlDataSourceMode.DataReader;
            gvGroups.DataSource = sdsGroups;
            gvGroups.DataBind();
        }
        //очистка полей страницы после действий
        protected void Cleaner()
        {
            DBConnection.IDDisciplines = 0;
            tbGroupNumber.Text = string.Empty;
            tbCourseNumber.Text = string.Empty;
            ddlSpecialization.SelectedIndex = 0;

        }
        //удаление строк в таблице
        protected void gvGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvGroups.SelectedRow;
                DBConnection.IDGroups = Convert.ToInt32(gvGroups.Rows[Index].Cells[1].Text.ToString());
                procedures.spGroups_delete(DBConnection.IDGroups);
                gvFill(QR);
                Cleaner();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }

        }
        //отображение столбцов в таблице
        protected void gvGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvGroups, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }


        //добавление новой записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDGroups = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spGroups_insert(Convert.ToString(tbGroupNumber.Text), Convert.ToString(tbCourseNumber.Text), Convert.ToInt32(ddlSpecialization.SelectedValue));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }
            
        }

        //изменение существующей записи в бд
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DBProcedures procedures = new DBProcedures();
                procedures.spGroups_update(DBConnection.IDGroups, Convert.ToString(tbGroupNumber.Text), Convert.ToString(tbCourseNumber.Text), Convert.ToInt32(ddlSpecialization.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }
            
        }

        //выбор записи в таблице и заполнение текстбоксов выбранными значениями
        protected void gvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvGroups.Rows)
            {
                if (row.RowIndex == gvGroups.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvGroups.SelectedRow;
            tbGroupNumber.Text = roww.Cells[2].Text.ToString();
            tbCourseNumber.Text = roww.Cells[3].Text.ToString();
            ddlSpecialization.SelectedValue = roww.Cells[4].Text.ToString();
            DBConnection.IDGroups = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }


        //сортировка по столбцу
        protected void gvGroups_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Номер группы"):
                    e.SortExpression = "[Number_groups]";
                    break;
                case ("Номер курса"):
                    e.SortExpression = "[Number_course]";
                    break;
                case ("Номер специальности"):
                    e.SortExpression = "[Number_specialty]";
                    break;
                case ("Наименование специальности"):
                    e.SortExpression = "[Name_specialty]";
                    break;
            }
            sortGridView(gvGroups, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvGroups.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[5].Text.Equals(tbSearch.Text) ||
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