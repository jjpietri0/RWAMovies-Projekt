using AdminModule.Dal;
using AdminModule.Properties;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.Configure<Api>(builder.Configuration.GetSection("Api"));
builder.Services.Configure<Admin>(builder.Configuration.GetSection("AdminCreds"));

builder.Services.AddHttpClient<VideoService>();
builder.Services.AddHttpClient<CountryService>();
builder.Services.AddHttpClient<TagService>();
builder.Services.AddHttpClient<GenreService>();
builder.Services.AddHttpClient<UserService>();

builder.Services.AddTransient<VideoService>();
builder.Services.AddTransient<TagService>();


var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
