using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using ShortLinkGenerator.Data;
using ShortLinkGenerator.Models;
using ShortLinkGenerator.Services;
using ShortLinkGenerator.ViewModels;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountService, AccountServices>();

builder.Services.AddDbContext<ApplicationDbContext>
            (options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ProjectConnection"));
            });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;

}).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

#region JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
//builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();


// within this section we are configuring the authentication and setting the default scheme
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Key"] ?? "");

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    //ValidIssuer = Configuration["JwtConfig:Issuer"],
    //ValidAudience = Configuration["JwtConfig:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(key)
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameters;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
