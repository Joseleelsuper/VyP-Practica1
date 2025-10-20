<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>
    <link rel="stylesheet" href="../../css/site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card shadow">
                <h1 class="title"><asp:Literal ID="litLoginTitle" runat="server" /></h1>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                <div class="form-group">
                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Email" CssClass="label" />
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="correo@dominio.com"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Contraseña" CssClass="label" />
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
                </div>
                <div class="actions">
                    <asp:Button ID="btnLogin" runat="server" Text="Entrar" OnClick="btnLogin_Click" CssClass="btn btn-primary btn-block" />
                </div>
                <div class="sample-users">
                    <span class="muted small" style="margin-right:8px;"><asp:Literal ID="litSampleUsers" runat="server" /></span>
                    <a href="#" class="chip" onclick="return quickLogin('admin@admin.net','AdminQ_123');"><asp:Literal ID="litSampleAdmin" runat="server" /></a>
                    <a href="#" class="chip" onclick="return quickLogin('juanp1025@alu.ubu.es','HolaQ_123');"><asp:Literal ID="litSampleJuan" runat="server" /></a>
                    <a href="#" class="chip" onclick="return quickLogin('mariag1002@gmail.com','AdiósQ_123');"><asp:Literal ID="litSampleMaria" runat="server" /></a>
                </div>
                <div class="actions" style="margin-top:16px;text-align:right;">
                    <asp:Button ID="btnToggleLangLogin" runat="server" Text="ES/EN" OnClick="btnToggleLangLogin_Click" CssClass="btn btn-secondary" />
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function quickLogin(email, pass) {
                var emailInput = document.getElementById('<%= txtEmail.ClientID %>');
                var passInput = document.getElementById('<%= txtPassword.ClientID %>');
                if (!emailInput || !passInput) return false;
                emailInput.value = email;
                passInput.value = pass;
                try { passInput.focus(); } catch (e) { }
                return false;
            }
        </script>
    </form>
</body>
</html>
