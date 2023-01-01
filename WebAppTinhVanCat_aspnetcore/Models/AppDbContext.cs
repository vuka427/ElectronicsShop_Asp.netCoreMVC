using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTinhVanCat_aspnetcore.Models.Contacts;

namespace WebAppTinhVanCat_aspnetcore.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<Category> Categories { set; get; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            // Bỏ tiền tố AspNet của các bảng: mặc định
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.Entity<Category>(entity => { // dánh chỉ mục trường slug
                entity.HasIndex(p => p.Slug)
                .IsUnique(); //isUnipue : có giá trị duy nhất 
            });

            builder.Entity<PostCategory>( entity=> { // PostID và CategoryID là khóa chính tạo quan hệ nhiều - nhiều cho bản post và category
                entity.HasKey(k => new {k.CategoryID, k.PostID });

            });

            builder.Entity<Post>(entity => { // dánh chỉ mục trường slug
                entity.HasIndex(p => p.Slug)
                .IsUnique(); //isUnipue : có giá trị duy nhất 
            });
        }
    }
}
