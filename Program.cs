using StocksWebApi.Data;
using Microsoft.EntityFrameworkCore;
using StocksWebApi.Interfaces;
using StocksWebApi.Repository;
using StocksWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StocksWebApi.Service;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер внедрения зависимостей

builder.Services.AddControllers(); // Регистрация контроллеров для API
builder.Services.AddEndpointsApiExplorer(); // Поддержка генерации OpenAPI документации
builder.Services.AddSwaggerGen(); // Генерация Swagger документации

// Настройка Swagger для поддержки JWT авторизации
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// Настройка JSON сериализации для игнорирования циклических ссылок
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Настройка контекста базы данных с использованием SQL Server
builder.Services.AddDbContext<StockDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Настройка Identity для управления пользователями и ролями
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true; // Пароль должен содержать цифры
    options.Password.RequireLowercase = true; // Пароль должен содержать строчные буквы
    options.Password.RequireUppercase = true; // Пароль должен содержать заглавные буквы
    options.Password.RequireNonAlphanumeric = true; // Пароль должен содержать специальные символы
    options.Password.RequiredLength = 12; // Минимальная длина пароля
})
.AddEntityFrameworkStores<StockDBContext>(); // Использование Entity Framework для хранения данных пользователей

// Настройка аутентификации через JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Проверка издателя токена
        ValidIssuer = builder.Configuration["JWT:Issuer"], // Издатель из конфигурации
        ValidateAudience = true, // Проверка аудитории токена
        ValidAudience = builder.Configuration["JWT:Audience"], // Аудитория из конфигурации
        ValidateIssuerSigningKey = true, // Проверка ключа подписи
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]) // Ключ подписи из конфигурации
        )
    };
});

// Регистрация репозиториев и сервисов
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();

// Создание приложения
var app = builder.Build();

// Настройка конвейера обработки HTTP-запросов

if (app.Environment.IsDevelopment()) // Включение Swagger только в режиме разработки
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Перенаправление HTTP на HTTPS

app.UseAuthentication(); // Включение аутентификации
app.UseAuthorization(); // Включение авторизации

app.MapControllers(); // Маршрутизация к контроллерам

app.Run(); // Запуск приложения