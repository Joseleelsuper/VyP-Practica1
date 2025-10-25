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
            <asp:HiddenField ID="hfEditingId" runat="server" />
            <asp:HiddenField ID="hfToast" runat="server" />
            <div class="card shadow">
                <h1 class="title"><asp:Literal ID="litDashboardTitle" runat="server" /></h1>
                <asp:Label ID="lblWelcome" runat="server" CssClass="lead" />
                <div class="stats">
                    <asp:Label ID="lblStats" runat="server" CssClass="muted" />
                </div>
            </div>

            <!-- My activities section -->
            <section class="card shadow" style="margin-top:16px;">
                <h2 class="subtitle"><asp:Literal ID="litMyActivitiesTitle" runat="server" /></h2>
                <asp:GridView ID="gvMyActivities" runat="server" AutoGenerateColumns="false" CssClass="grid" OnRowCommand="gvMyActivities_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="Nombre" />
                        <asp:BoundField DataField="Description" HeaderText="Descripción" />
                        <asp:BoundField DataField="Date" HeaderText="Fecha" />
                        <asp:BoundField DataField="Duration" HeaderText="Duración" />
                        <asp:BoundField DataField="Distance" HeaderText="Distancia (m)" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditActivity" runat="server" CommandName="EditActivity" CommandArgument='<%# Eval("Id") %>' CssClass="link">
                                    <%# TranslationService.Get("dashboard.edit") %>
                                </asp:LinkButton>
                                &nbsp;|&nbsp;
                                <asp:LinkButton ID="btnDeleteActivity" runat="server" CommandName="DeleteActivity" CommandArgument='<%# Eval("Id") %>' CssClass="link danger">
                                    <%# TranslationService.Get("dashboard.delete") %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>

            <!-- Add activity section -->
            <section class="card shadow" style="margin-top:16px;">
                <h2 class="subtitle"><asp:Literal ID="litAddActivityTitle" runat="server" /></h2>
                <div class="form-grid">
                    <asp:Label ID="lblActName" runat="server" AssociatedControlID="txtName" />
                    <asp:TextBox ID="txtName" runat="server" CssClass="input" />

                    <asp:Label ID="lblActDesc" runat="server" AssociatedControlID="txtDescription" />
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="input" />

                    <asp:Label ID="lblActDate" runat="server" AssociatedControlID="txtDate" />
                    <asp:TextBox ID="txtDate" runat="server" CssClass="input" />

                    <asp:Label ID="lblActDuration" runat="server" AssociatedControlID="txtDuration" />
                    <asp:TextBox ID="txtDuration" runat="server" CssClass="input" />

                    <asp:Label ID="lblActDistance" runat="server" AssociatedControlID="txtDistance" />
                    <asp:TextBox ID="txtDistance" runat="server" CssClass="input" />
                </div>
                <asp:Button ID="btnAddActivity" runat="server" CssClass="btn btn-primary" Text="Añadir" OnClick="btnAddActivity_Click" />
            </section>

            <!-- Admin section -->
            <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
                <section class="card shadow" style="margin-top:16px;">
                    <h2 class="subtitle"><asp:Literal ID="litAdminTitle" runat="server" /></h2>

                    <!-- Users management -->
                    <h3 class="subtitle-small"><asp:Literal ID="litUsersTitle" runat="server" /></h3>
                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="grid" DataKeyNames="Email" OnRowCommand="gvUsers_RowCommand" OnRowDataBound="gvUsers_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Nombre" />
                            <asp:BoundField DataField="Surname" HeaderText="Apellido" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Rol">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                                        <asp:ListItem Value="USER" Text="User" />
                                        <asp:ListItem Value="ADMIN" Text="Admin" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                        <asp:ListItem Value="ACTIVE" Text="Active" />
                                        <asp:ListItem Value="INACTIVE" Text="Inactive" />
                                        <asp:ListItem Value="BANNED" Text="Banned" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteUser" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("Email") %>' CssClass="link danger">
                                        <%# TranslationService.Get("dashboard.delete") %>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <!-- Create user -->
                    <h3 class="subtitle-small" style="margin-top:12px;"><asp:Literal ID="litCreateUserTitle" runat="server" /></h3>
                    <div class="form-grid">
                        <asp:Label ID="lblNewName" runat="server" AssociatedControlID="txtNewName" />
                        <asp:TextBox ID="txtNewName" runat="server" CssClass="input" />

                        <asp:Label ID="lblNewSurname" runat="server" AssociatedControlID="txtNewSurname" />
                        <asp:TextBox ID="txtNewSurname" runat="server" CssClass="input" />

                        <asp:Label ID="lblNewEmail" runat="server" AssociatedControlID="txtNewEmail" />
                        <asp:TextBox ID="txtNewEmail" runat="server" CssClass="input" />

                        <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="txtNewPassword" />
                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="input" />

                        <asp:Label ID="lblNewNif" runat="server" AssociatedControlID="txtNewNif" />
                        <asp:TextBox ID="txtNewNif" runat="server" CssClass="input" />

                        <asp:Label ID="lblNewTelf" runat="server" AssociatedControlID="txtNewTelf" />
                        <asp:TextBox ID="txtNewTelf" runat="server" CssClass="input" />
                    </div>
                    <asp:Button ID="btnCreateUser" runat="server" CssClass="btn btn-primary" OnClick="btnCreateUser_Click" />

                    <!-- Activities by user -->
                    <h3 class="subtitle-small" style="margin-top:12px;"><asp:Literal ID="litActivitiesByUserTitle" runat="server" /></h3>
                    <asp:Repeater ID="rptActivitiesByUser" runat="server" OnItemDataBound="rptActivitiesByUser_ItemDataBound">
                        <ItemTemplate>
                            <div class="card" style="margin-top:8px;">
                                <div class="muted"><asp:Literal ID="litGroupUser" runat="server" /></div>
                                <asp:GridView ID="gvActs" runat="server" AutoGenerateColumns="false" CssClass="grid-small">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="Name" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Description" HeaderText="Descripción" />
                                        <asp:BoundField DataField="Date" HeaderText="Fecha" />
                                        <asp:BoundField DataField="Duration" HeaderText="Duración" />
                                        <asp:BoundField DataField="Distance" HeaderText="Distancia (m)" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </section>
            </asp:Panel>
        </main>
        <script src="../../js/toast.js"></script>
        <script src="../../js/toast-hydrator.js"></script>
        <script type="text/javascript">
            window.toastHydrator && window.toastHydrator.run('<%= hfToast.ClientID %>');
        </script>
    </form>
</body>
</html>
