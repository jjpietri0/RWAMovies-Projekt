using AdminModule.Dal;
using AdminModule.Properties;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<IntegrationModule.Models.ProjectDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddHttpClient<CountryService>();
builder.Services.AddHttpClient<VideoService>();
builder.Services.AddHttpClient<TagService>();
//builder.Services.AddHttpClient<GenreService>();
//builder.Services.AddHttpClient<UserService>();
builder.Services.Configure<Api>(builder.Configuration.GetSection("Api"));

builder.Services.AddTransient<VideoService>();
builder.Services.AddTransient<TagService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
