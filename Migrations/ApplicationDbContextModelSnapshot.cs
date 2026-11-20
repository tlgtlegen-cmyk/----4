using AishaTulegen.ViewModelValidation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace AishaTulegen.ViewModelValidation.Migrations;

[DbContext(typeof(ApplicationDbContext))]
public sealed class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.22")
            .HasAnnotation("Relational:MaxIdentifierLength", 64);

        modelBuilder.Entity("AishaTulegen.ViewModelValidation.Models.User", entity =>
        {
            entity.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");

            entity.Property<int>("Age")
                .HasColumnType("INTEGER");

            entity.Property<DateTime>("CreatedAt")
                .HasColumnType("TEXT");

            entity.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(254)
                .HasColumnType("TEXT");

            entity.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("TEXT");

            entity.Property<string>("PasswordHash")
                .IsRequired()
                .HasColumnType("TEXT");

            entity.HasKey("Id");
            entity.HasIndex("Email").IsUnique();
            entity.ToTable("Users");
        });
    }
}
