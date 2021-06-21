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
using System.Windows;
using System.Data;

namespace MPUS_STUDY.Pages
{
    public partial class UsersList : System.Web.UI.Page
    {
        //переменная для бд
        private string QR = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            QR = DBConnection.qrUsers;
            if (!IsPostBack)
            {
                //проверка авторизации и вывод данных в лейбл
                DBConnection connection = new DBConnection();
                if (DBConnection.idUser != 0)
                {
                    gvFill(QR);
                    ddlPostFill();

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
        //очистка полей в обратной связи
        protected void Cleaner1()
        {

            tbNameHelp.Text = string.Empty;
            tbMailHelp.Text = string.Empty;
            tbHelp.Text = string.Empty;

        }
        //очистка полей на странице после действия
        protected void Cleaner()
        {
            DBConnection.IDUsers = 0;
            tbSurname.Text = string.Empty;
            tbName.Text = string.Empty;
            tbMiddlename.Text = string.Empty;
            tbLogin.Text = string.Empty;
            tbPassword.Text = string.Empty;
            ddlPost.SelectedIndex = 0;
            tbDecrypt.Text = string.Empty;
        }
        //заполнение таблицы
        private void gvFill(string qr)
        {
            sdsUsers.ConnectionString =
                DBConnection.connection.ConnectionString.ToString();
            sdsUsers.SelectCommand = qr;
            sdsUsers.DataSourceMode = SqlDataSourceMode.DataReader;
            gvUsers.DataSource = sdsUsers;
            gvUsers.DataBind();
        }
        //заполнение выпадающего списка
        private void ddlPostFill()
        {
            sdsPost.ConnectionString = DBConnection.connection.ConnectionString.ToString();
            sdsPost.SelectCommand = DBConnection.qrPostView;
            sdsPost.DataSourceMode = SqlDataSourceMode.DataReader;
            ddlPost.DataSource = sdsPost;
            ddlPost.DataTextField = "Должность";
            ddlPost.DataValueField = "ID_post";
            ddlPost.DataBind();
        }
        //добавление записи в бд
        protected void btInsert_Click(object sender, EventArgs e)
        {
            DBConnection connection = new DBConnection();
            //Проверка уникальности логина
            if (connection.LoginCheck(tbLogin.Text) > 0 & tbLogin.Text != "")
            {
                lblLoginCheck.Visible = true;
            }
            else
            {
                try
                {
                    DBConnection.IDUsers = 0;
                    DBProcedures procedures = new DBProcedures();
                    procedures.spUsers_insert(Convert.ToString(tbLogin.Text), Convert.ToString(tbPassword.Text), Convert.ToString(tbSurname.Text), Convert.ToString(tbName.Text), Convert.ToString(tbMiddlename.Text), Convert.ToInt32(ddlPost.SelectedValue));
                    Cleaner();
                    gvFill(QR);
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Не удалось добавить запись :(')", true);
                }              
                
            }

        }
        //изменение записи в бд
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DBProcedures procedures = new DBProcedures();
                procedures.spUsers_update(DBConnection.IDUsers, Convert.ToString(tbSurname.Text), Convert.ToString(tbName.Text), Convert.ToString(tbMiddlename.Text), Convert.ToInt32(ddlPost.SelectedValue), Convert.ToString(tbLogin.Text), Convert.ToString(tbPassword.Text));            
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
        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[8].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvUsers, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Нажмите, чтобы выбрать запись";
            }
        }
        //удаление записи в бд
        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {            
            try
            {
                int Index = Convert.ToInt32(e.RowIndex);
                DBProcedures procedures = new DBProcedures();
                GridViewRow rows = gvUsers.SelectedRow;                
                DBConnection.IDUsers = Convert.ToInt32(gvUsers.Rows[Index].Cells[1].Text.ToString());
                //Проверка выбранного значения на удаление
                if (DBConnection.IDUsers != 1)
                {
                    procedures.spUsers_delete(DBConnection.IDUsers, DBConnection.IDAuth);
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
        //заполнение текстбоксов по выбранной строке
        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvUsers.Rows)
            {
                if (row.RowIndex == gvUsers.SelectedIndex)
                {
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.ToolTip = "Нажмите, чтобы выбрать запись";
                }
            }
            GridViewRow roww = gvUsers.SelectedRow;
            tbSurname.Text = roww.Cells[2].Text.ToString();
            tbName.Text = roww.Cells[3].Text.ToString();
            tbMiddlename.Text = roww.Cells[4].Text.ToString();
            tbLogin.Text = roww.Cells[6].Text.ToString();
            tbPassword.Text = roww.Cells[7].Text.ToString();
            ddlPost.SelectedValue = roww.Cells[8].Text.ToString();
            DBConnection.IDUsers = Convert.ToInt32(roww.Cells[1].Text.ToString());
        }
        //сортировка по столбцу
        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
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
                case ("Логин"):
                    e.SortExpression = "[Login]";
                    break;
                case ("Пароль"):
                    e.SortExpression = "[Password]";
                    break;
                case ("Должность"):
                    e.SortExpression = "[Name_post]";
                    break;
            }
            sortGridView(gvUsers, e, out sortDirection, out strField);
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
                foreach (GridViewRow row in gvUsers.Rows)
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

        //Экспорт в эксель
        protected void btCreateExcel_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = DESKTOP-T819KVA\SQLEXPRESS; " +
              " Initial Catalog = MPUS_STUDY_DataBase; Persist Security Info = true;" +
              " User ID = sa; Password = \"psl14082001\""))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(DBConnection.qrUsersExport, connection);

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
                Response.AddHeader("Content-Disposition", "attachment; filename=Список пользователей на " + time + ".xls");
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

        protected void btDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                lblDecrypt.Visible = true;
                lblDecrypt.Text = ("Отлично, вот ваш расшифрованный пароль. | Пароль: ") + Encryption.Decrypt(tbDecrypt.Text);
                Cleaner();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Вы ввели некорректный шифр, проверьте написание :(')", true);
            }           
           
        }
    }
}