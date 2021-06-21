<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Authorization.aspx.cs" Inherits="MPUS_STUDY.Pages.Authorization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../Content/bootstrap-grid.min.css" />
    <link rel="stylesheet" href="../Content/Styles.css" />
    <script src="https://unpkg.com/@popperjs/core@2</script"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>




    <title>Авторизация</title>
    <style>
        body {
            background: #222222;
            font-family: 'Century Gothic';
            margin-top: 200px;
        }

        .Helper {
            font-size: 23px;
            color: black;
        }

        .Helper2 {
            font-size: 15px;
            color: black;
        }

        .title {
            text-align: center;
            color: white;
        }

        .form-control {
            font-size: 14px;
            border-radius: 10px;
            background: #D8BFD8;
        }

            .form-control:hover {
                background: White;
                border-color: darkred;
                box-shadow: 0 0 10px 0 darkred;
                -webkit-transition: 0.3s ease-out;
                -webkit-transition-delay: 0.1s;
                -o-transition: 0.3s ease-out;
                -o-transition-delay: 0.1s;
                -moz-transition: 0.3s ease-out;
                -moz-transition-delay: 0.1s;
                transition: 0.3s ease-out;
                transition-delay: 0.1s;
            }

        .form-group {
            text-align: center;
        }

        .error {
            color: red;
        }

        .btn-outline-danger {
            color: black;
            background: #D8BFD8;
            border-color: white;
        }

        #btEnter {
            color: black;
            border-radius: 150px;
            background: #D8BFD8;
            color: #eeebf5;
            border: none;
            transition: 0.5s;
            font-size: 26px;
            padding-left: 20px;
            padding-right: 20px;
            margin-top: 40px;
        }

        #btEnter {
            color: black;
            border-radius: 30px;
            background: #A9A9A9;
        }

            #btEnter:hover {
                background: purple;
                border-color: darkorchid;
                box-shadow: 0 0 10px 0 darkorchid;
                -webkit-transition: 0.3s ease-out;
                -webkit-transition-delay: 0.1s;
                -o-transition: 0.3s ease-out;
                -o-transition-delay: 0.1s;
                -moz-transition: 0.3s ease-out;
                -moz-transition-delay: 0.1s;
                transition: 0.3s ease-out;
                transition-delay: 0.1s;
            }

        .Succes {
            color: green;
        }

        #btEnter:focus {
            background: purple;
        }

        .alert {
            width: 1000px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="sign-form">
                <h1 class="title">Авторизация</h1>
                <asp:Label runat="server" ID="lblAuthorization" Text="Неверный логин или пароль" Visible="false" CssClass="Error"></asp:Label>
                <div class="form-group">
                    <label for="formGroupExampleInput" class="lblTitle" style="float: left">Логин</label>
                    <asp:TextBox ID="tbLogin" runat="server" type="text" class="form-control mt-2 mb-3"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="2" ErrorMessage="Введите логин" Display="Dynamic" ControlToValidate="tbLogin" CssClass="Error"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label for="formGroupExampleInput2" class="lblTitle" style="float: left">Пароль</label>
                    <asp:TextBox ID="tbPassword" runat="server" type="text" class="form-control mt-2 mb-3" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="2" ErrorMessage="Введите пароль" Display="Dynamic" ControlToValidate="tbPassword" CssClass="Error"></asp:RequiredFieldValidator>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:Button runat="server" ID="btEnter" ValidationGroup="2" Text="ВОЙТИ" Class="btEnter mt-2 mb-3" OnClick="btEnter_Click" />
                    </div>
                </div>
                <button type="button" class="btn btn-secondary mt-2 mb-3" data-container="body" data-toggle="modal" data-target="#myModal" data-placement="top">Проблемы с доступом? </button>
                <div class="col">
                    <asp:Button runat="server" ID="btAdminRegister" Text="Регистрация администратора" Class="btEnter mt-2 mb-3" OnClick="btAdminRegister_Click" />
                    <br />
                    <asp:Label runat="server" ID="lblAccountSucces" Class="Succes mt-2 mb-3" Text="Отлично, а теперь войдите в систему. Данные от учётной записи: Логин: Admin | Пароль: Admin" Visible="false"></asp:Label>
                </div>
                <!-- Модальное окно -->
                <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title" id="myModalLabel">Поддержка <i class="fa fa-commenting" aria-hidden="true"></i></h4>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body row justify-content-center">
                                <div class="form-group col-12">
                                    <label class="Helper ">Имя</label>
                                    <asp:TextBox class="form-control" ID="tbNameHelp" runat="server" type="search"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="tbNameHelp1" runat="server" ValidationGroup="1" ErrorMessage="Поле не должно быть пустым" class="error" ControlToValidate="tbNameHelp" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-12">
                                    <label class="Helper">Почта</label>
                                    <asp:TextBox class="form-control" ID="tbMailHelp" runat="server" type="search"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="tbMailHelp1" runat="server" ValidationGroup="1" ErrorMessage="Поле не должно быть пустым" class="error" ControlToValidate="tbMailHelp" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-12">
                                    <label class="Helper">Описание проблемы</label>
                                    <asp:TextBox class="form-control" ID="tbHelp" runat="server" type="search"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="tbHelp1" runat="server" ValidationGroup="1" ErrorMessage="Поле не должно быть пустым" class="error" ControlToValidate="tbHelp" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-12">
                                    <asp:Label ID="Info" runat="server" class="Helper">Контактная информация <i class="fa fa-book" aria-hidden="true"></i> </asp:Label>
                                </div>
                                <div class="form-group col-12">
                                    <asp:Label ID="lblmailinfo" runat="server" class="Helper2">Почта для обратной связи: i_a.s.popov@mpt.ru</asp:Label>
                                </div>
                                <div class="form-group col-12">
                                    <asp:Label ID="lblnumberinfo" runat="server" class="Helper2">Номер телефона: 8 (915) 335-68-45 </asp:Label>
                                </div>
                                <div class="form-group col-12">
                                    <asp:Label ID="lbladressinfo" runat="server" class="Helper2">Адрес: Нежинская д. 7</asp:Label>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button type="button" runat="server" class="btn btn-secondary" data-dismiss="modal" CausesValidation="false" Text="Отмена"></asp:Button>
                                <asp:Button type="button" runat="server" class="btn btn-primary" ValidationGroup="1" OnClick="btSendHelp_Click" Text="Отправить"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
