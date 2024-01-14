using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using IntegrationModule.JWTServices;
using IntegrationModule.Models;
using IntegrationModule.Properties;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

//var client = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
//var secretConnstring = (await client.GetSecretAsync("RWAConnectionString")).Value.Value;
//builder.Configuration["ConnectionStrings:AzureConnection"] = secretConnstring;

//var tokenHandler = new JwtSecurityTokenHandler();
//var key = Encoding.ASCII.GetBytes("edbXe3uNTBm0zjxf/OvnVKbVq1KmKmnVvqxWK6JfeKU=");
//var tokenDescriptor = new SecurityTokenDescriptor
//{
//    Subject = new ClaimsIdentity(new Claim[]
//    {
//        new(ClaimTypes.Name, "admin"),
//        new(ClaimTypes.Role, "admin")
//    }),
//    Expires = DateTime.UtcNow.AddDays(365),
//    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//    Issuer = "localhost",
//    Audience = "localhost"
//};
//var token = tokenHandler.CreateToken(tokenDescriptor);
//var tokenString = tokenHandler.WriteToken(token);

//get smtp settings from confgiration "SmtpClientSettings" 
builder.Services.Configure<SmtpClientSettings>(builder.Configuration.GetSection("SmtpClientSettings"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntegrationModule"});

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.Value))
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddSingleton<IUserGenRepository, UserGenRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApi",
               builder => builder.WithOrigins("http://localhost:5280","http://localhost:5182")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

builder.Services.AddDbContext<ProjectDBContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection"));
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseSwaggerUI(c =>
{
    var swaggerSettings = builder.Configuration.GetSection("Swagger");

    c.SwaggerEndpoint(swaggerSettings["Endpoint"], swaggerSettings["Title"]); 
    c.RoutePrefix = swaggerSettings["RoutePrefix"];
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowApi");
app.MapControllers();

app.UseStaticFiles();


app.Run();