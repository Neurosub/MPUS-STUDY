<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Advantages.aspx.cs" Inherits="MPUS_STUDY.Pages.Advantages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Преимущества</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../Content/bootstrap-grid.min.css" />
    <link rel="icon" href="/images/MPUS.png" type="image/x-icon" />
    <link rel="shortcut icon" href="/images/MPUS.png" type="image/x-icon" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="../Scripts/Button.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>
    <style>
        /*Навбар*/
        .navbar {
            margin-bottom: 10px;
        }

        .nav-item {
            font-size: 15px;
            text-align: center;
        }

        .navbar a {
            color: chartreuse;
            text-transform: uppercase;
            text-decoration: none;
            letter-spacing: 0.1em;
            display: inline-block;
            padding: 3px 3px;
            position: relative;
            margin-right: 0px;
        }

            .navbar a:after {
                background: none repeat scroll 0 0 transparent;
                bottom: 0;
                content: "";
                display: block;
                height: 2px;
                left: 50%;
                position: absolute;
                background: chartreuse;
                transition: width 0.3s ease 0s, left 0.3s ease 0s;
                width: 0;
            }

            .navbar a:hover:after {
                width: 100%;
                left: 0;
            }

        .Helper {
            font-size: 23px;
            color: black;
        }

        .Helper2 {
            font-size: 15px;
            color: black;
        }

        /*Навбар*/
        body {
            background: #222222;
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

        table {
            border-color: wheat;
        }

        #lblProfile {
            margin-left: 425px;
        }

        #btExit {
            color: black;
            border-radius: 30px;
            background: #A9A9A9;
        }

            #btExit:hover {
                background: red;
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

            #btExit:focus {
                background: Red;
            }

        .content {
            margin-left: 75px;
            font-size: 20px;
            color: white;
            text-align: center;
        }

        /*Панель обратной связи*/

        .icon-bar {
            position: fixed;
            top: 50%;
            -webkit-transform: translateY(-50%);
            -ms-transform: translateY(-50%);
            transform: translateY(-50%);
        }

            .icon-bar a {
                display: block;
                text-align: center;
                padding: 16px;
                transition: all 0.3s ease;
                color: white;
                font-size: 20px;
            }

                .icon-bar a:hover {
                    background-color: #000;
                }

        .facebook {
            background: #3B5998;
            color: white;
        }

        .vk {
            background: #55ACEE;
        }

        .github {
            background: #161b22;
        }
        /*Панель обратной связи*/

        /*Кнопка поддержка*/
        .animated {
            animation-duration: 3s;
            animation-fill-mode: both;
        }

            .animated.infinite {
                animation-iteration-count: infinite;
                -webkit-animation-iteration-count: infinite;
            }

        @keyframes bounce {
            from, 6%, 17%, 26%, 33% {
                animation-timing-function: cubic-bezier(0.215, 0.610, 0.355, 1.000);
                transform: translate3d(0, 0, 0);
            }

            13%, 14% {
                animation-timing-function: cubic-bezier(0.755, 0.050, 0.855, 0.060);
                transform: translate3d(0, -8px, 0);
            }

            23% {
                animation-timing-function: cubic-bezier(0.755, 0.050, 0.855, 0.060);
                transform: translate3d(0, -4px, 0);
            }

            30% {
                transform: translate3d(0, -2px, 0);
            }

            33%, 100% {
                transform: translate3d(0, 0, 0);
            }
        }

        .bounce {
            animation-name: bounce;
            transform-origin: center bottom;
        }

        .Help {
            position: absolute;
            margin-left: 0%;
            width: 52px;
            height: 52px;
            text-decoration: none;
            color: #a675b3;
            text-align: center;
            display: block;
            font: normal 17px arial;
        }

            .Help:not(.active) {
                box-shadow: inset 0 1px 1px rgba(111, 55, 125, 0.8), inset 0 -1px 0px rgba(63, 59, 113, 0.2), 0 9px 16px 0 rgba(0, 0, 0, 0.3), 0 4px 3px 0 rgba(0, 0, 0, 0.3), 0 0 0 1px #150a1e;
                background-image: linear-gradient(#800080, #271739, #8B008B);
                text-shadow: 0 0 21px rgba(223, 206, 228, 0.5), 0 -1px 0 #311d47;
            }

                .Help:not(.active):hover,
                .Help:not(.active):focus {
                    transition: color 200ms linear, text-shadow 500ms linear;
                    color: #fff;
                    text-shadow: 0 0 21px rgba(223, 206, 228, 0.5), 0 0 10px rgba(223, 206, 228, 0.4), 0 0 2px #2a153c;
                }

            .Help:not(:hover) {
                transition: 0.6s;
            }
        /*Кнопка поддержка*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="icon-bar">
            <a target="_blank" href="https://vk.com/fernandojones" class="vk"><i class="fa fa-vk"></i></a>
            <a target="_blank" href="https://www.facebook.com/profile.php?id=100068757066247" class="facebook"><i class="fa fa-facebook"></i></a>
            <a target="_blank" href="https://github.com/Neurosub/MPUS-STUDY" class="github"><i class="fa fa-github"></i></a>
            <button type="button" class="Help animated infinite bounce" data-toggle="modal" data-target="#myModal"><i class="fa fa-envelope-o" aria-hidden="true"></i></button>
        </div>
        <nav class="navbar navbar-expand-lg navbar-light" style="background-color: #e3f2fd;">
            <a class="navbar-brand nav-link active" href="Advantages.aspx">
                <img src="../Images/NavbarLogo.png" width="150" height="70" class="d-inline-block align-top" alt="" />
            </a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
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
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Справочные списки
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <a class="nav-link " href="Groups.aspx" id="Groups" aria-haspopup="true" aria-expanded="false">Список групп</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link " href="Specialization.aspx" id="Specialization" aria-haspopup="true" aria-expanded="false">Список специальностей</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link " href="Disciplines.aspx" id="Disciplines" aria-haspopup="true" aria-expanded="false">Список дисциплин</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link" href="Students_list.aspx" id="Students_list" aria-haspopup="true" aria-expanded="false">Список студентов</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link" href="TeachersList.aspx" id="TeachersList" aria-haspopup="true" aria-expanded="false">Список преподавателей</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link" href="ProfessionalModule.aspx" id="ProfessionalModule" aria-haspopup="true" aria-expanded="false">Список профессиональных модулей</a>
                        </div>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown2" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Связующие страницы
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <a class="nav-link " href="Students_disciplines.aspx" id="Students_disciplines" aria-haspopup="true" aria-expanded="false">Дисциплины студентов</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link" href="Group_scores.aspx" id="Group_scores" aria-haspopup="true" aria-expanded="false">Успеваемость групп</a>
                        </div>
                    </li>
                    <li runat="server" class="nav-item dropdown" id="admin" visible="true">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown3" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Страницы администратора
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <a class="nav-link" href="Post.aspx" id="Post" aria-haspopup="true" aria-expanded="false">Список должностей</a>
                            <div class="dropdown-divider"></div>
                            <a class="nav-link" href="UsersList.aspx" id="UsersList" aria-haspopup="true" aria-expanded="false">Список пользователей</a>
                        </div>
                    </li>
                    <li>
                        <asp:Label runat="server" class="lblProfile" ID="lblProfile"></asp:Label>
                        <asp:Button ID="btExit" type="button" class="btn" OnClick="btExit_Click" data-toggle="tooltip" data-placement="bottom" title="Выход из учётной записи" RegularExpressionValidator="False" runat="server" Text="ВЫЙТИ" />
                    </li>
                </ul>
            </div>
        </nav>
        <div class="content" style="border-color: white">
            <h3>Добро пожаловать!</h3>
            <p>Здесь вы можете посмотреть карусель изображений и узнать преимущества программного продукта. А также связаться с разработчиком в социальных сетях.</p>
            <p>Программа предназначена для выполнения задач учебно-методического отдела в организации "РЭУ им. Г.В. Плеханова МПТ"</p>
        </div>
        <div class="container">
            <div id="demo" class="carousel slide" data-ride="carousel">

                <!-- Indicators -->
                <ul class="carousel-indicators">
                    <li data-target="#demo" data-slide-to="0" class="active"></li>
                    <li data-target="#demo" data-slide-to="1"></li>
                    <li data-target="#demo" data-slide-to="2"></li>
                    <li data-target="#demo" data-slide-to="3"></li>
                    <li data-target="#demo" data-slide-to="4"></li>
                    <li data-target="#demo" data-slide-to="5"></li>
                    <li data-target="#demo" data-slide-to="6"></li>
                    <li data-target="#demo" data-slide-to="7"></li>
                </ul>

                <!-- The slideshow -->
                <div class="carousel-inner">
                    <div class="carousel-item active">
                        <img src="../Images/Преимущества.png" alt="Advantages" width="1140" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Исправление ошибок.png" alt="Error correction" width="1140" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Понятный интерфейс.png" alt="Intuitive interface" width="1140" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Открытый код.png" alt="Escort" width="1100" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Фильтрация.png" alt="Escort" width="1100" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Экспорт.png" alt="Escort" width="1100" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Скрипты для работы.png" alt="Escort" width="1100" height="370" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Images/Ошибки.png" alt="Escort" width="1100" height="370" />
                    </div>
                </div>
                <a class="carousel-control-prev" href="#demo" data-slide="prev" data-toggle="tooltip" data-placement="bottom" title="Предыдущий слайд">
                    <span class="carousel-control-prev-icon"></span>
                </a>
                <a class="carousel-control-next" href="#demo" data-slide="next" data-toggle="tooltip" data-placement="bottom" title="Следующий слайд">
                    <span class="carousel-control-next-icon"></span>
                </a>
            </div>
            <table class="Table mt-2 mb-3 table table-dark table-condensed table-hover" style="overflow-y: auto; -webkit-overflow-scrolling: touch;">
                <thead>
                    <tr>
                        <th>Разработчик</th>
                        <th>Почта</th>
                        <th>Номер телефона</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Попов Александр Сергеевич</td>
                        <td>i_a.s.popov@mpt.ru</td>
                        <td>8(915)335-68-45</td>
                    </tr>
                    <tr>
                        <th>Репозиторий Github:</th>
                        <th>https://github.com/Neurosub/MPUS-STUDY</th>
                        <th></th>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
