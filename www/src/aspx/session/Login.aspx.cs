using Database;
using System;
using System.Web.Script.Serialization;

public partial class Login : System.Web.UI.Page
{
    private void ShowToast(string type, string title, string message)
    {
        var payload = new { type = type, title = title, message = message };
        var ser = new JavaScriptSerializer();
        hfToastLogin.Value = ser.Serialize(payload);
    }

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
            ShowToast("error", TranslationService.Get("login.toast.error"), TranslationService.Get("login.toast.missing"));
            return;
        }

        var data = DataStore.Instance;
        if (data.ValidaUsuario(email, password))
        {
            Session["userEmail"] = email;

            ShowToast("success", TranslationService.Get("login.toast.ok"), TranslationService.Get("login.toast.signedIn"));

            Response.Redirect("../application/Dashboard.aspx");
        }
        else
        {
            ShowToast("error", TranslationService.Get("login.toast.error"), TranslationService.Get("login.toast.invalidCreds"));
        }
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
        ShowToast("info", TranslationService.Get("login.toast.info"), TranslationService.Get("login.toast.langChanged"));
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
        litLoginTitle.Text = TranslationService.Get("login.title");
        lblEmail.Text = TranslationService.Get("login.email");
        lblPassword.Text = TranslationService.Get("login.password");
        btnLogin.Text = TranslationService.Get("login.signIn");
        btnToggleLangLogin.Text = TranslationService.Get("login.toggleLang");
        litSampleUsers.Text = TranslationService.Get("login.sampleUsers");
        litSampleAdmin.Text = TranslationService.Get("login.sampleAdmin");
        litSampleJuan.Text = TranslationService.Get("login.sampleJuan");
        litSampleMaria.Text = TranslationService.Get("login.sampleMaria");
    }
}
