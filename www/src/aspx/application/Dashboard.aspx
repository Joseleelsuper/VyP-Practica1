<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Dashboard</title>
    <link rel="stylesheet" href="../../css/site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <header class="topbar">
            <div class="brand">Mi App Fitness</div>
            <div class="spacer"></div>
            <asp:Button ID="btnToggleLang" runat="server" Text="ES/EN" OnClick="btnToggleLang_Click" CssClass="btn btn-secondary" style="margin-right:8px;" />
            <asp:Button ID="btnLogout" runat="server" Text="Cerrar sesión" OnClick="btnLogout_Click" CssClass="btn btn-secondary" />
        </header>
        <main class="container">
            <div class="card shadow">
                <h1 class="title"><asp:Literal ID="litDashboardTitle" runat="server" /></h1>
                <asp:Label ID="lblWelcome" runat="server" CssClass="lead" />
                <div class="stats">
                    <asp:Label ID="lblStats" runat="server" CssClass="muted" />
                </div>
            </div>
        </main>
    </form>
</body>
</html>
