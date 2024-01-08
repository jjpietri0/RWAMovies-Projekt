using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using IntegrationModule.JWTServices;
using IntegrationModule.Models;
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


builder.Services.AddDbContext<ProjectDBContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection"));
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//var tokenHandler = new JwtSecurityTokenHandler();
//var key = Encoding.ASCII.GetBytes("edbXe3uNTBm0zjxf/OvnVKbVq1KmKmnVvqxWK6JfeKU=");
//var tokenDescriptor = new SecurityTokenDescriptor
//{
//    Subject = new ClaimsIdentity(new Claim[]
//    {
//        new(ClaimTypes.Name, "Pero")
//    }),
//    Expires = DateTime.UtcNow.AddDays(7),
//    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//    Issuer = "localhost",
//    Audience = "localhost"
//};
//var token = tokenHandler.CreateToken(tokenDescriptor);
//var tokenString = tokenHandler.WriteToken(token);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IUserGenRepository, UserGenRepository>();

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



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
               builder => builder.WithOrigins("https://localhost:7035", "http://localhost:5280")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(c =>
{
    var swaggerSettings = builder.Configuration.GetSection("Swagger");

    c.SwaggerEndpoint(swaggerSettings["Endpoint"], swaggerSettings["Title"]); 
    c.RoutePrefix = swaggerSettings["RoutePrefix"];
});

app.UseCors("AllowOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();