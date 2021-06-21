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
    public partial class Students_disciplines : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrStudents_disciplines;
            if (!IsPostBack)
            {
                //проверка авторизации пользователя заполнение его данных в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    gvFill(QR);
                    ddlGroupNumberFill();
                    ddlTeachersFill();
                    ddlDisciplineFill();
                    lblProfile.Text = connection.Profile(Convert.ToString(DBConnection.idUser));
                }

                else
                {
                    Response.Redirect("Authorization.aspx");
                }
                //проверка роли пользователя скрытие страниц администратора
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
        //очистка полей в обратной связи
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //очистка полей на странице после действий
        protected void Cleaner()
        {
            DBConnection.IDStudentsDisciplines = 0;
            tbAttesstationForm.Text = string.Empty;
            ddlGroupNumber.SelectedIndex = 0;
            ddlTeachers.SelectedIndex = 0;
            ddlDiscipline.SelectedIndex = 0;
            tbSemestrNumber.Text = string.Empty;

        }
        //заполнение выпадающего списка значениями
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
        //заполнение выпадающего списка значениями
        private void ddlTeachersFill()
        {
            sdsddlTeachers.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsddlTeachers.SelectCommand = DBConnection.qrTeachersFio;
            sdsddlTeachers.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlTeachers.DataSource = sdsddlTeachers;
            ddlTeachers.DataTextField = "ФИО преподавателя";
            ddlTeachers.DataValueField = "ID_teachers";
            ddlTeachers.DataBind();
        }
        //заполнение выпадающего списка значениями
        private void ddlDisciplineFill()
        {
            sdsddlDiscipline.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsddlDiscipline.SelectCommand = DBConnection.qrDisciplines_View;
            sdsddlDiscipline.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlDiscipline.DataSource = sdsddlDiscipline;
            ddlDiscipline.DataTextField = "Дисциплина";
            ddlDiscipline.DataValueField = "ID_disciplines";
            ddlDiscipline.DataBind();
        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsStudentsDisciplines.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsStudentsDisciplines.SelectCommand = qr;
            sdsStudentsDisciplines.DataSourceMode = SqlDataSourceMode.DataReader;
            gvStudentsDisciplines.DataSource = sdsStudentsDisciplines;
            gvStudentsDisciplines.DataBind();
        }


        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDStudentsDisciplines = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spStudents_disciplines_insert(Convert.ToString(tbSemestrNumber.Text), Convert.ToString(tbAttesstationForm.Text), Convert.ToInt32(ddlGroupNumber.SelectedValue), Convert.ToInt32(ddlDiscipline.SelectedValue), Convert.ToInt32(ddlTeachers.SelectedValue));
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
                procedures.spStudents_disciplines_update(DBConnection.IDStudentsDisciplines, Convert.ToString(tbSemestrNumber.Text), Convert.ToString(tbAttesstationForm.Text), Convert.ToInt32(ddlGroupNumber.SelectedValue), Convert.ToInt32(ddlDiscipline.SelectedValue), Convert.ToInt32(ddlTeachers.SelectedValue));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }

        }
        //скрытие столбцов выбор строки
        protected void gvStudentsDisciplines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[8].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvStudentsDisciplines, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //удаление записи из бд
        protected void gvStudentsDisciplines_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvStudentsDisciplines.SelectedRow;
                DBConnection.IDStudentsDisciplines = Convert.ToInt32(gvStudentsDisciplines.Rows[Index].Cells[1].Text.ToString());
                procedures.spStudents_disciplines_delete(DBConnection.IDStudentsDisciplines);
                gvFill(QR);
                Cleaner();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось удалить запись :(')", true);
            }
        }
        //заполнение текстбоксов значениями из таблицы
        protected void gvStudentsDisciplines_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvStudentsDisciplines.Rows)
            {
                if (row.RowIndex == gvStudentsDisciplines.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvStudentsDisciplines.SelectedRow;
            tbSemestrNumber.Text = roww.Cells[2].Text.ToString();
            tbAttesstationForm.Text = roww.Cells[3].Text.ToString();
            ddlGroupNumber.SelectedValue = roww.Cells[4].Text.ToString();
            ddlDiscipline.SelectedValue = roww.Cells[6].Text.ToString();
            ddlTeachers.SelectedValue = roww.Cells[8].Text.ToString();
            DBConnection.IDStudentsDisciplines = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }
        //сортировка по заголовкам столбцов
        protected void gvStudentsDisciplines_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("Номер семестра"):
                    e.SortExpression = "[Number_semester]";
                    break;
                case ("Форма аттестации"):
                    e.SortExpression = "[Attestation_form]";
                    break;
                case ("Номер группы"):
                    e.SortExpression = "[Number_groups]";
                    break;
                case ("Тип дисциплины"):
                    e.SortExpression = "[Disciplines_type]";
                    break;
                case ("Код предмета"):
                    e.SortExpression = "[Subject_code]";
                    break;
                case ("Название дисциплины"):
                    e.SortExpression = "[Name_disciplines]";
                    break;
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
            sortGridView(gvStudentsDisciplines, e, out sortDirection, out strField);
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
        //поиск
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                foreach (GridViewRow row in gvStudentsDisciplines.Rows)
                {
                    if (row.Cells[2].Text.Equals(tbSearch.Text) ||
                        row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[5].Text.Equals(tbSearch.Text) ||
                        row.Cells[7].Text.Equals(tbSearch.Text) ||
                        row.Cells[9].Text.Equals(tbSearch.Text))
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