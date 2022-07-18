using File_Sharing_App.Areas.Admin;
using File_Sharing_App.Areas.Admin.Services;
using File_Sharing_App.Data;
using File_Sharing_App.Helper.Mail;
using File_Sharing_App.Hubs;
using File_Sharing_App.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

 var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddMvcLocalization(op =>
    {
        op.ResourcesPath = "Resources";
    });
builder.Services.AddDbContext<ApplicationDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetction"));
});
builder.Services.AddTransient<IMailHelper, MailHelper>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
}
).AddEntityFrameworkStores<ApplicationDbcontext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromDays(3);
});
builder.Services.AddLocalization();
builder.Services.AddTransient<IUploadService, UploadService>();
builder.Services.AddAdminToSC();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    }
    );
builder.Services.AddSignalR();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var intial = scope.ServiceProvider.GetRequiredService<IUserService>();
    await  intial.IntializeAsync();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
var supportedCulture = new[] { "ar-EG", "en-US" };
app.UseRequestLocalization(r =>
{
    r.AddSupportedCultures(supportedCulture);
    r.AddSupportedUICultures(supportedCulture);
    r.SetDefaultCulture("en-US");
}
);
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
        endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
        endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<NotificationHub>("/Notify");
});
Rotativa.AspNetCore.RotativaConfiguration.Setup(builder.Environment.WebRootPath);
app.Run();
