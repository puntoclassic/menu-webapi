using System.Net;
using System.Net.Mail;
using System.Text;
using FluentEmail.Core.Interfaces;
using MenuBackend.Models;
using MenuBackend.Models.Auth;
using MenuBackend.Models.Auth.Policies;
using MenuBackend.Models.Data;
using MenuBackend.Models.Options;
using MenuBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



//Aggiunge l'autenticazione JWT a swagger
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token: add you token with prefix Bearer",
    });
    o.AddSecurityRequirement(
        new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
        },
        new string[] {}
    }
});
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<EmailService>();
builder.Services.Configure<FrontendOptions>(builder.Configuration.GetSection("Frontend"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<EmailOptionsModel>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IAuthorizationHandler, OrderBelongsToUserHandler>();
JwtOptions? jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();


builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<MenuBackend.Services.TokenService, MenuBackend.Services.TokenService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        string ValidIssuer = jwtOptions?.ValidIssuer ?? "menuBackend";
        string ValidAudience = jwtOptions?.ValidAudience ?? "menuBackend";
        string Key = jwtOptions?.Key ?? "fdc94cca304ff508ca4791c1c56f307e4a9d2b5377e9cf1174a4182d7e664385";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = ValidIssuer,
            ValidAudience = ValidAudience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Key)
            ),
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("token"))
                {
                    context.Token = context.Request.Cookies["token"];
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrderBelongsToUser", policy => policy.AddRequirements(new OrderBelongsToUserRequirement()));
});

builder.Services.AddAutoMapper(typeof(MenuProfile));


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("myCorsPolicy");
app.UsePathBase("/Api");
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        await context.Response.WriteAsJsonAsync(new
        {
            Message = "Login required"
        });
    }
    if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        await context.Response.WriteAsJsonAsync(new
        {
            Message = "Permission required"
        });
    }
});
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
