﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAppTinhVanCat_aspnetcore.Models;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<string>("Slug")
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.HasKey("PostId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.PostCategory", b =>
                {
                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<int>("PostID")
                        .HasColumnType("int");

                    b.HasKey("CategoryID", "PostID");

                    b.HasIndex("PostID");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Contacts.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Massage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.CategoryProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.OrderItem", b =>
                {
                    b.Property<int>("OrderItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemID"));

                    b.Property<decimal>("CurentPrice")
                        .HasColumnType("decimal(10,0)");

                    b.Property<int>("GTGT")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<string>("ProductTitle")
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Reviews")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.HasKey("OrderItemID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.OrderModel", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomCancelReason")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("CustomNote")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Finished")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsReviews")
                        .HasColumnType("bit");

                    b.Property<string>("OrderCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,0)");

                    b.Property<string>("ShopCancelReason")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShopNote")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("TransportCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.HasIndex("OrderCode")
                        .IsUnique()
                        .HasFilter("[OrderCode] IS NOT NULL");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.ProductModel", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Feature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GTGT")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,0)");

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("QuantityRating")
                        .HasColumnType("int");

                    b.Property<int>("QuantitySold")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<int>("Sold")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("nvarchar(160)");

                    b.Property<int>("TradeMarkId")
                        .HasColumnType("int");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.Property<int>("Warranty")
                        .HasColumnType("int");

                    b.Property<float>("rating")
                        .HasColumnType("real");

                    b.HasKey("ProductId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.HasIndex("TradeMarkId");

                    b.HasIndex("Unit");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.ProductPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductPhoto");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.TradeMarkModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TradeMark");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.UnitProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UnitProduct");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.Category", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Blog.Category", "ParentCategory")
                        .WithMany("CategoryChildren")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.Post", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.PostCategory", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Blog.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Blog.Post", "Post")
                        .WithMany("PostCategories")
                        .HasForeignKey("PostID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.CategoryProduct", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Product.CategoryProduct", "ParentCategory")
                        .WithMany("CategoryChildren")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.OrderItem", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Product.OrderModel", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.ProductModel", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Product.CategoryProduct", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Product.TradeMarkModel", "TradeMark")
                        .WithMany("Products")
                        .HasForeignKey("TradeMarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Product.UnitProduct", "UnitProduct")
                        .WithMany("Products")
                        .HasForeignKey("Unit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");

                    b.Navigation("TradeMark");

                    b.Navigation("UnitProduct");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.ProductPhoto", b =>
                {
                    b.HasOne("WebAppTinhVanCat_aspnetcore.Models.Product.ProductModel", "Product")
                        .WithMany("Photos")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.Category", b =>
                {
                    b.Navigation("CategoryChildren");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Blog.Post", b =>
                {
                    b.Navigation("PostCategories");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.CategoryProduct", b =>
                {
                    b.Navigation("CategoryChildren");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.OrderModel", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.ProductModel", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.TradeMarkModel", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebAppTinhVanCat_aspnetcore.Models.Product.UnitProduct", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
