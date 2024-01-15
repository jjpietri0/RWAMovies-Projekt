using PublicModule.Dal;
using PublicModule.Properties;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<Api>(builder.Configuration.GetSection("Api"));



builder.Services.AddHttpClient<LoginService>();
builder.Services.AddHttpClient<RegisterService>();
builder.Services.AddHttpClient<VideosService>();
builder.Services.AddHttpClient<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
