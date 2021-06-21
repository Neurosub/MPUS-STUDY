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
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Data;

namespace MPUS_STUDY.Pages
{
    public partial class Group_scores : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        //подтверждение для экспорта
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //проверка авторизации пользователя и вывод его данных в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    QR = DBConnection.qrGroup_scores + "where [Users_ID] = '" + DBConnection.idPersonal + "'";
                    gvFill(QR);
                    ddlStudentListFill();
                    ddlStudentsDisciplinesFill();
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
        //очистка полей после действия
        protected void Cleaner()
        {
            DBConnection.IDGroupScores = 0;
            ddlStudentList.SelectedIndex = 0;
            ddlStudentsDisciplines.SelectedIndex = 0;
            tbScore.Text = string.Empty;

        }
        //заполнение выпадающего списка
        private void ddlStudentListFill()
        {
            sdsddlStudentList.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsddlStudentList.SelectCommand = DBConnection.qrStudentsFio;
            sdsddlStudentList.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlStudentList.DataSource = sdsddlStudentList;
            ddlStudentList.DataTextField = "ФИО студента";
            ddlStudentList.DataValueField = "ID_students_list";
            ddlStudentList.DataBind();
        }
        //заполнение выпадающего списка
        private void ddlStudentsDisciplinesFill()
        {
            sdsddlStudentsDisciplines.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsddlStudentsDisciplines.SelectCommand = DBConnection.qrStudents_disciplines_View;
            sdsddlStudentsDisciplines.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlStudentsDisciplines.DataSource = sdsddlStudentsDisciplines;
            ddlStudentsDisciplines.DataTextField = "Дисциплина у студента";
            ddlStudentsDisciplines.DataValueField = "ID_students_disciplines";
            ddlStudentsDisciplines.DataBind();
        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsGroupScores.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsGroupScores.SelectCommand = qr;
            sdsGroupScores.DataSourceMode = SqlDataSourceMode.DataReader;
            gvGroupScores.DataSource = sdsGroupScores;
            gvGroupScores.DataBind();
        }
        //добавление в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection.IDGroupScores = 0;
                DBProcedures procedures = new DBProcedures();
                procedures.spGroup_scores_insert(Convert.ToInt32(ddlStudentList.SelectedValue), Convert.ToInt32(ddlStudentsDisciplines.SelectedValue), DBConnection.idPersonal, Convert.ToString(tbScore.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
            }

        }
        //изменение строки в бд
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DBProcedures procedures = new DBProcedures();
                procedures.spGroup_scores_update(DBConnection.IDGroupScores, Convert.ToInt32(ddlStudentList.SelectedValue), Convert.ToInt32(ddlStudentsDisciplines.SelectedValue), DBConnection.idPersonal, Convert.ToString(tbScore.Text));
                Cleaner();
                gvFill(QR);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось изменить запись :(')", true);
            }

        }
        //скрытие столбцов и выбор строки
        protected void gvGroupScores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[14].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvGroupScores, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //удаление строки из бд
        protected void gvGroupScores_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvGroupScores.SelectedRow;
                DBConnection.IDGroupScores = Convert.ToInt32(gvGroupScores.Rows[Index].Cells[1].Text.ToString());
                procedures.spGroup_scores_delete(DBConnection.IDGroupScores);
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
        protected void gvGroupScores_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvGroupScores.Rows)
            {
                if (row.RowIndex == gvGroupScores.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvGroupScores.SelectedRow;
            ddlStudentList.SelectedValue = roww.Cells[2].Text.ToString();
            ddlStudentsDisciplines.SelectedValue = roww.Cells[4].Text.ToString();
            tbScore.Text = roww.Cells[15].Text.ToString();
            DBConnection.IDGroupScores = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }
        //сортировка по заголовкам
        protected void gvGroupScores_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirection = SortDirection.Ascending;
            string strField = string.Empty;
            switch (e.SortExpression)
            {
                case ("ФИО студента"):
                    e.SortExpression = "[Students_list].[Surname] + ' ' + [Students_list].[Name] + ' ' + [Students_list].[Middlename]";
                    break;
                case ("Номер семестра"):
                    e.SortExpression = "[Number_semester]";
                    break;
                case ("Форма аттестации"):
                    e.SortExpression = "[Attestation_form]";
                    break;
                case ("Номер группы"):
                    e.SortExpression = "[Number_groups]";
                    break;
                case ("Дисциплина"):
                    e.SortExpression = "[Disciplines_type] + ' ' + [Subject_code] + ' ' + [Name_disciplines]";
                    break;
                case ("ФИО преподавателя"):
                    e.SortExpression = "[Teachers].[Surname] + ' ' + [Teachers].[Name] + ' ' + [Teachers].[Middlename]";
                    break;
                case ("ФИО пользователя"):
                    e.SortExpression = "[Users].[Surname] + ' ' + [Users].[Name] + ' ' + [Users].[Middlename]";
                    break;
                case ("Оценка"):
                    e.SortExpression = "[Score]";
                    break;
            }
            sortGridView(gvGroupScores, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvGroupScores.Rows)
                {
                    if (row.Cells[3].Text.Equals(tbSearch.Text) ||
                        row.Cells[5].Text.Equals(tbSearch.Text) ||
                        row.Cells[6].Text.Equals(tbSearch.Text) ||
                        row.Cells[8].Text.Equals(tbSearch.Text) ||
                        row.Cells[10].Text.Equals(tbSearch.Text) ||
                        row.Cells[12].Text.Equals(tbSearch.Text) ||
                        row.Cells[15].Text.Equals(tbSearch.Text))
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
        //Экспорт в эксель
        protected void btCreateExcel_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = DESKTOP-T819KVA\SQLEXPRESS; " +
              " Initial Catalog = MPUS_STUDY_DataBase; Persist Security Info = true;" +
              " User ID = sa; Password = \"psl14082001\""))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(DBConnection.qrExport, connection);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                var gvGroupScores = dt;
                var grid = new GridView();
                grid.DataSource = gvGroupScores;
                grid.DataBind();
                List<string> ls = new List<string>();
                string[] sColumnName = { };
                foreach (DataColumn column in dt.Columns)
                {
                    ls.Add(column.ToString());
                }
                sColumnName = ls.ToArray();
                Response.ClearContent();
                Response.Buffer = true;
                string time = DateTime.Now.ToString("dd/MM/yyyy");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=Ведомость успеваемости " + time + ".xls");
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                connection.Close();
            }
        }

    }

}
