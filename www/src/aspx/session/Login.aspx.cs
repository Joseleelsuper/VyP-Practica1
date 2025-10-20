using Database;
using System;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            EnsureLanguage();
            ApplyTranslations();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        var email = (txtEmail.Text == null) ? null : txtEmail.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Introduce el email y la contraseña.");
            return;
        }

        var data = DataStore.Instance;
        if (data.ValidaUsuario(email, password))
        {
            Session["userEmail"] = email;

            lblError.Visible = false;

            Response.Redirect("../application/Dashboard.aspx");
        }
        else
        {
            ShowError("Credenciales inválidas.");
        }
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }

    protected void btnToggleLangLogin_Click(object sender, EventArgs e)
    {
        var lang = Session["lang"] as string;
        if (string.Equals(lang, "EN_en", StringComparison.OrdinalIgnoreCase))
            lang = "ES_es";
        else
            lang = "EN_en";
        Session["lang"] = lang;
        TranslationService.SetLanguage(lang);
        ApplyTranslations();
    }

    private void EnsureLanguage()
    {
        var lang = Session["lang"] as string;
        if (string.IsNullOrEmpty(lang)) lang = "ES_es";
        Session["lang"] = lang;
        TranslationService.SetLanguage(lang);
    }

    private void ApplyTranslations()
    {
        // Asignaciones b�sicas; si existe login.json, tomar� valores de ah�
        litLoginTitle.Text = TranslationService.Get("login.title");
        btnLogin.Text = TranslationService.Get("login.signIn");
        btnToggleLangLogin.Text = TranslationService.Get("login.toggleLang");
        litSampleUsers.Text = TranslationService.Get("login.sampleUsers");
        litSampleAdmin.Text = TranslationService.Get("login.sampleAdmin");
        litSampleJuan.Text = TranslationService.Get("login.sampleJuan");
        litSampleMaria.Text = TranslationService.Get("login.sampleMaria");
    }
}
