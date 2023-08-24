using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using MultiAuthBug;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate()
    .AddMicrosoftIdentityWebApp(cfg =>
    {
        cfg.TenantId = "CHANGE ME";
        cfg.ClientId = "CHANGE ME";
        cfg.Instance = "https://login.microsoftonline.com/";
        cfg.CallbackPath = "/signin-oidc";
        cfg.SignedOutCallbackPath = "/signout-oidc";
    });
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents();
builder.Services.AddAuthorization(cfg =>
{
    var entra = new AuthorizationPolicyBuilder(OpenIdConnectDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
    cfg.AddPolicy("Entra", entra);
    cfg.FallbackPolicy = cfg.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>();

app.Run();
