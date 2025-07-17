using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StocksWebApi.Models;
using System.Security.Cryptography.X509Certificates;

namespace StocksWebApi.Data
{
    /// <summary>
    /// Контекст базы данных с поддержкой Identity (роли, пользователи).
    /// </summary>
    public class StockDBContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// Конструктор контекста. Принимает опции конфигурации БД.
        /// </summary>
        /// <param name="options">Опции DbContext</param>
        public StockDBContext(DbContextOptions options) : base(options)
        {
            // Конструктор передаёт настройки в базовый класс
        }

        /// <summary>
        /// DbSet для таблицы акций.
        /// </summary>
        public DbSet<Stock> Stocks { get; set; }

        /// <summary>
        /// DbSet для таблицы комментариев.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// DbSet для таблицы портфелей.
        /// </summary>
        public DbSet<Portfolio> Portfolios { get; set; }

        /// <summary>
        /// Настройка модели данных при создании миграций.
        /// </summary>
        /// <param name="builder">Модель-билдер для настройки схемы БД</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Настройка составного ключа для Portfolio (AppUserId + StockId)
            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            // Связь Portfolio ↔ AppUser (один ко многим)
            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            // Связь Portfolio ↔ Stock (один ко многим)
            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);

            // Загрузка статических ролей при миграции
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            // Вставка начальных ролей в БД
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}