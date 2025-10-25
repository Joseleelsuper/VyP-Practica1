using Database;
using Logica.Models;
using System;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
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
    }
}
