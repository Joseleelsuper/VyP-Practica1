using Database;
using Logica.Models;
using Logica.Utils;
using System;
using System.Linq;
using System.Web.UI; // for DataBinder
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public partial class Dashboard : System.Web.UI.Page
{
    private void ShowToast(string type, string title, string message)
    {
        var payload = new { type = type, title = title, message = message };
        var ser = new JavaScriptSerializer();
        hfToast.Value = ser.Serialize(payload);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Ensure language every request
        EnsureLanguage();

        // Enforce authentication on every request (not only first load)
        var email = Session["userEmail"] as string;
        if (string.IsNullOrEmpty(email))
        {
            Response.Redirect(ResolveUrl("~/src/aspx/session/Login.aspx"));
            return;
        }

        var data = DataStore.Instance;
        var user = data.LeeUsuario(email);
        if (user == null)
        {
            Response.Redirect(ResolveUrl("~/src/aspx/session/Login.aspx"));
            return;
        }

        if (!IsPostBack)
        {
            ApplyTranslations(user, data);
            BindData(user, data);
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userEmail"] = null;
        Response.Redirect(ResolveUrl("~/src/aspx/session/Login.aspx"));
    }

    protected void btnToggleLang_Click(object sender, EventArgs e)
    {
        var lang = Session["lang"] as string;
        if (string.Equals(lang, "EN_en", StringComparison.OrdinalIgnoreCase))
            lang = "ES_es";
        else
            lang = "EN_en";
        Session["lang"] = lang;
        TranslationService.SetLanguage(lang);

        var email = Session["userEmail"] as string;
        var data = DataStore.Instance;
        var user = string.IsNullOrEmpty(email) ? null : data.LeeUsuario(email);
        ApplyTranslations(user, data);
        BindData(user, data);
    }

    // Activities grid events
    protected void gvMyActivities_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        var data = DataStore.Instance;
        var email = Session["userEmail"] as string;
        var user = data.LeeUsuario(email);
        if (user == null) return;

        int id;
        if (int.TryParse((string)e.CommandArgument, out id))
        {
            if (e.CommandName == "DeleteActivity")
            {
                var act = data.LeeActividad(id);
                if (act != null && act.UserId == user.Id)
                {
                    data.EliminaActividad(id);
                    ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.activityDeleted"));
                }
                ClearActivityInputs();
                hfEditingId.Value = string.Empty;
                btnAddActivity.Text = TranslationService.Get("dashboard.btnAddActivity");
                BindData(user, data);
            }
            else if (e.CommandName == "EditActivity")
            {
                var act = data.LeeActividad(id);
                if (act != null && act.UserId == user.Id)
                {
                    txtName.Text = act.Name;
                    txtDescription.Text = act.Description;
                    txtDate.Text = act.Date.ToString("yyyy-MM-dd");
                    txtDuration.Text = act.Duration.ToString();
                    txtDistance.Text = act.Distance.ToString();
                    hfEditingId.Value = id.ToString();
                    btnAddActivity.Text = TranslationService.Get("dashboard.btnSaveActivity");
                    ShowToast("info", TranslationService.Get("dashboard.toast.info"), TranslationService.Get("dashboard.toast.editMode"));
                }
            }
        }
    }

    protected void btnAddActivity_Click(object sender, EventArgs e)
    {
        var data = DataStore.Instance;
        var email = Session["userEmail"] as string;
        var user = data.LeeUsuario(email);
        if (user == null) return;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            ShowToast("error", TranslationService.Get("dashboard.toast.error"), TranslationService.Get("dashboard.toast.missingName"));
            return;
        }

        double distance;
        TimeSpan duration;
        DateTime date;
        if (!double.TryParse(txtDistance.Text, out distance)) distance = 0;
        if (!TimeSpan.TryParse(txtDuration.Text, out duration)) duration = TimeSpan.Zero;
        if (!DateTime.TryParse(txtDate.Text, out date)) date = DateTime.Now;

        int editingId;
        if (int.TryParse(hfEditingId.Value, out editingId) && editingId > 0)
        {
            var act = data.LeeActividad(editingId);
            if (act != null && act.UserId == user.Id)
            {
                act.Name = txtName.Text;
                act.Description = txtDescription.Text;
                act.Date = date;
                act.Duration = duration;
                act.Distance = distance;
                ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.activityUpdated"));
            }
            hfEditingId.Value = string.Empty;
            btnAddActivity.Text = TranslationService.Get("dashboard.btnAddActivity");
        }
        else
        {
            var act = new Activity(txtName.Text, txtDescription.Text, date, duration, user.Id, distance);
            data.GuardaActividad(act);
            ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.activityCreated"));
        }

        ClearActivityInputs();
        BindData(user, data);
    }

    // Admin events
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var role = DataBinder.Eval(e.Row.DataItem, "Role") as string;
            var status = DataBinder.Eval(e.Row.DataItem, "Status") as string;
            var ddlRole = (DropDownList)e.Row.FindControl("ddlRole");
            var ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
            if (ddlRole != null)
            {
                ddlRole.SelectedValue = role;
                ddlRole.Items[0].Text = TranslationService.Get("dashboard.role.user");
                ddlRole.Items[1].Text = TranslationService.Get("dashboard.role.admin");
            }
            if (ddlStatus != null)
            {
                ddlStatus.SelectedValue = status;
                ddlStatus.Items[0].Text = TranslationService.Get("dashboard.status.active");
                ddlStatus.Items[1].Text = TranslationService.Get("dashboard.status.inactive");
                ddlStatus.Items[2].Text = TranslationService.Get("dashboard.status.banned");
            }
        }
    }

    protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        var data = DataStore.Instance;
        if (e.CommandName == "DeleteUser")
        {
            var email = (string)e.CommandArgument;
            data.EliminaUsuario(email);
            var me = data.LeeUsuario(Session["userEmail"] as string);
            BindData(me, data);
            ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.userDeleted"));
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ddl = (DropDownList)sender;
        var row = (GridViewRow)ddl.NamingContainer;
        var email = (string)gvUsers.DataKeys[row.RowIndex].Value;
        var data = DataStore.Instance;
        Status nuevo;
        if (Enum.TryParse<Status>(ddl.SelectedValue, out nuevo))
        {
            data.CambiaEstadoUsuario(email, nuevo);
            ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.statusChanged"));
        }
        var me = data.LeeUsuario(Session["userEmail"] as string);
        BindData(me, data);
        ApplyTranslations(me, data);
    }

    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ddl = (DropDownList)sender;
        var row = (GridViewRow)ddl.NamingContainer;
        var email = (string)gvUsers.DataKeys[row.RowIndex].Value;
        var data = DataStore.Instance;
        Role nuevo;
        if (Enum.TryParse<Role>(ddl.SelectedValue, out nuevo))
        {
            data.CambiaRolUsuario(email, nuevo);
            ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.roleChanged"));
        }
        var me = data.LeeUsuario(Session["userEmail"] as string);
        BindData(me, data);
        ApplyTranslations(me, data);
    }

    protected void rptActivitiesByUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var gv = (GridView)e.Item.FindControl("gvActs");
            var litUser = (Literal)e.Item.FindControl("litGroupUser");
            if (gv != null)
            {
                var activities = ((dynamic)e.Item.DataItem).Activities;
                gv.DataSource = activities;
                gv.DataBind();

                if (gv.HeaderRow != null && gv.HeaderRow.Cells.Count >= 6)
                {
                    gv.HeaderRow.Cells[0].Text = TranslationService.Get("dashboard.colId");
                    gv.HeaderRow.Cells[1].Text = TranslationService.Get("dashboard.colName");
                    gv.HeaderRow.Cells[2].Text = TranslationService.Get("dashboard.colDescription");
                    gv.HeaderRow.Cells[3].Text = TranslationService.Get("dashboard.colDate");
                    gv.HeaderRow.Cells[4].Text = TranslationService.Get("dashboard.colDuration");
                    gv.HeaderRow.Cells[5].Text = TranslationService.Get("dashboard.colDistance");
                }
            }
            if (litUser != null)
            {
                var u = ((dynamic)e.Item.DataItem).User as User;
                if (u != null)
                {
                    litUser.Text = u.Name + " " + u.Surname + " (" + u.Email + ")";
                }
            }
        }
    }

    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        var data = DataStore.Instance;
        var me = data.LeeUsuario(Session["userEmail"] as string);
        if (me == null || me.Role != Role.ADMIN) return;

        var errors = new System.Collections.Generic.List<string>();
        string nif = txtNewNif.Text;
        string name = txtNewName.Text;
        string surname = txtNewSurname.Text;
        string email = txtNewEmail.Text;
        string password = txtNewPassword.Text;
        string telf = txtNewTelf.Text;

        if (string.IsNullOrWhiteSpace(name)) errors.Add(TranslationService.Get("dashboard.user.validation.nameRequired"));
        if (string.IsNullOrWhiteSpace(surname)) errors.Add(TranslationService.Get("dashboard.user.validation.surnameRequired"));
        if (!Logica.Utils.Validate.NIF(nif)) errors.Add(TranslationService.Get("dashboard.user.validation.nifInvalid"));
        if (!Logica.Utils.Validate.Email(email)) errors.Add(TranslationService.Get("dashboard.user.validation.emailInvalid"));
        if (!Logica.Utils.Validate.Telf(telf)) errors.Add(TranslationService.Get("dashboard.user.validation.telfInvalid"));
        if (!Logica.Utils.Validate.Password(password)) errors.Add(TranslationService.Get("dashboard.user.validation.passwordInvalid"));
        if (data.LeeUsuario(email) != null) errors.Add(TranslationService.Get("dashboard.user.validation.emailExists"));
        if (data.GetUsuarios().Any(u => u.NIF == nif)) errors.Add(TranslationService.Get("dashboard.user.validation.nifExists"));

        if (errors.Count > 0)
        {
            ShowToast("error", TranslationService.Get("dashboard.toast.error"), string.Join("\n", errors.ToArray()));
            return;
        }

        try
        {
            var newUser = new User(nif, name, surname, email, password, 18, telf, Gender.NOT_SPECIFIED, 70f);
            if (!data.GuardaUsuario(newUser))
            {
                ShowToast("error", TranslationService.Get("dashboard.toast.error"), TranslationService.Get("dashboard.toast.invalidUser"));
                return;
            }
            ShowToast("success", TranslationService.Get("dashboard.toast.ok"), TranslationService.Get("dashboard.toast.userCreated"));
        }
        catch
        {
            ShowToast("error", TranslationService.Get("dashboard.toast.error"), TranslationService.Get("dashboard.toast.invalidUser"));
        }
        BindData(me, data);
    }

    private void EnsureLanguage()
    {
        var lang = Session["lang"] as string;
        if (string.IsNullOrEmpty(lang)) lang = "ES_es";
        Session["lang"] = lang;
        TranslationService.SetLanguage(lang);
    }

    private void ApplyTranslations(User user, CapaDatos data)
    {
        if (user == null) return;
        var welcomeFormat = TranslationService.Get("dashboard.welcome");
        var statsFormat = TranslationService.Get("dashboard.stats");
        var title = TranslationService.Get("dashboard.title");

        litDashboardTitle.Text = title;
        lblWelcome.Text = string.Format(welcomeFormat, user.Name, user.Surname, user.Email);
        lblStats.Text = string.Format(statsFormat, data.NumUsuarios(), data.NumUsuariosActivos(), data.NumActividades(user.Id));

        btnLogout.Text = TranslationService.Get("dashboard.logout");
        btnToggleLang.Text = TranslationService.Get("dashboard.toggleLang");

        // Section titles and labels
        litMyActivitiesTitle.Text = TranslationService.Get("dashboard.myActivities");
        litAddActivityTitle.Text = TranslationService.Get("dashboard.addActivity");
        lblActName.Text = TranslationService.Get("dashboard.actName");
        lblActDesc.Text = TranslationService.Get("dashboard.actDesc");
        lblActDate.Text = TranslationService.Get("dashboard.actDate");
        lblActDuration.Text = TranslationService.Get("dashboard.actDuration");
        lblActDistance.Text = TranslationService.Get("dashboard.actDistance");
        btnAddActivity.Text = string.IsNullOrEmpty(hfEditingId.Value) ? TranslationService.Get("dashboard.btnAddActivity") : TranslationService.Get("dashboard.btnSaveActivity");

        pnlAdmin.Visible = user.Role == Role.ADMIN;
        if (pnlAdmin.Visible)
        {
            litAdminTitle.Text = TranslationService.Get("dashboard.adminTitle");
            litUsersTitle.Text = TranslationService.Get("dashboard.usersTitle");
            litCreateUserTitle.Text = TranslationService.Get("dashboard.createUserTitle");
            litActivitiesByUserTitle.Text = TranslationService.Get("dashboard.activitiesByUserTitle");
            lblNewName.Text = TranslationService.Get("dashboard.newName");
            lblNewSurname.Text = TranslationService.Get("dashboard.newSurname");
            lblNewEmail.Text = TranslationService.Get("dashboard.newEmail");
            lblNewPassword.Text = TranslationService.Get("dashboard.newPassword");
            lblNewNif.Text = TranslationService.Get("dashboard.newNif");
            lblNewTelf.Text = TranslationService.Get("dashboard.newTelf");
            btnCreateUser.Text = TranslationService.Get("dashboard.btnCreateUser");
        }

        SetGridHeaders();
    }

    private void SetGridHeaders()
    {
        // My activities headers
        if (gvMyActivities.Columns.Count >= 6)
        {
            gvMyActivities.Columns[0].HeaderText = TranslationService.Get("dashboard.colId");
            gvMyActivities.Columns[1].HeaderText = TranslationService.Get("dashboard.colName");
            gvMyActivities.Columns[2].HeaderText = TranslationService.Get("dashboard.colDescription");
            gvMyActivities.Columns[3].HeaderText = TranslationService.Get("dashboard.colDate");
            gvMyActivities.Columns[4].HeaderText = TranslationService.Get("dashboard.colDuration");
            gvMyActivities.Columns[5].HeaderText = TranslationService.Get("dashboard.colDistance");
        }
        // Users headers
        if (gvUsers.Columns.Count >= 5)
        {
            gvUsers.Columns[0].HeaderText = TranslationService.Get("dashboard.colName");
            gvUsers.Columns[1].HeaderText = TranslationService.Get("dashboard.colSurname");
            gvUsers.Columns[2].HeaderText = TranslationService.Get("dashboard.colEmail");
            // Template fields for Role and Status
            gvUsers.Columns[3].HeaderText = TranslationService.Get("dashboard.colRole");
            gvUsers.Columns[4].HeaderText = TranslationService.Get("dashboard.colStatus");
        }
    }

    private void BindData(User user, CapaDatos data)
    {
        // My activities
        gvMyActivities.DataSource = data.GetActividadesUsuario(user.Id)
            .Select(a => new { a.Id, a.Name, a.Description, Date = a.Date.ToString("yyyy-MM-dd"), Duration = a.Duration.ToString(), a.Distance }).ToList();
        gvMyActivities.DataBind();

        // Admin
        pnlAdmin.Visible = user.Role == Role.ADMIN;
        if (pnlAdmin.Visible)
        {
            gvUsers.DataSource = data.GetUsuarios().Select(u => new { u.Name, u.Surname, u.Email, Role = u.Role.ToString(), Status = u.Status.ToString() }).ToList();
            gvUsers.DataBind();
            // After binding, ensure headers are set
            SetGridHeaders();

            var grouped = data.GetTodasActividades()
                .GroupBy(a => a.UserId)
                .Select(g => new
                {
                    User = data.GetUsuarios().Find(delegate(User u) { return u.Id == g.Key; }),
                    Activities = g.Select(a => new { a.Id, a.Name, a.Description, Date = a.Date.ToString("yyyy-MM-dd"), Duration = a.Duration.ToString(), a.Distance }).ToList()
                })
                .ToList();
            rptActivitiesByUser.DataSource = grouped;
            rptActivitiesByUser.DataBind();
        }
    }

    private void ClearActivityInputs()
    {
        txtName.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtDate.Text = string.Empty;
        txtDuration.Text = string.Empty;
        txtDistance.Text = string.Empty;
    }
}
