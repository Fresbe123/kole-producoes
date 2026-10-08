using KoleProducoes.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoleProducoes.Infrastructure.Data;

public class KoleDbContext : IdentityDbContext<ApplicationUser>
{
    public KoleDbContext(DbContextOptions<KoleDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Show> Shows => Set<Show>();
    public DbSet<ShowImage> ShowImages => Set<ShowImage>();
    public DbSet<DJ> DJs => Set<DJ>();
    public DbSet<DJAvailability> DJAvailabilities => Set<DJAvailability>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AvailableSlot> AvailableSlots => Set<AvailableSlot>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Show>().HasQueryFilter(s => !s.IsDeleted);
        builder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<DJ>().HasQueryFilter(d => !d.IsDeleted);

        // Precisão dos valores monetários no SQL Server
        builder.Entity<Show>()
            .Property(s => s.StartingPrice)
            .HasPrecision(10, 2);

        builder.Entity<Product>()
            .Property(p => p.DailyRate)
            .HasPrecision(10, 2);

        builder.Entity<DJ>()
            .Property(d => d.StartingPrice)
            .HasPrecision(10, 2);

        // Slugs únicos — garante URLs sem duplicatas
        builder.Entity<Show>()
            .HasIndex(s => s.Slug)
            .IsUnique();

        builder.Entity<Product>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder.Entity<DJ>()
            .HasIndex(d => d.Slug)
            .IsUnique();

        // Relacionamento 1:1 entre AvailableSlot e Appointment
        builder.Entity<AvailableSlot>()
            .HasOne(h => h.Appointment)
            .WithOne(a => a.AvailableSlot)
            .HasForeignKey<Appointment>(a => a.AvailableSlotId);

        // Categorias iniciais
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Stages", Type = CategoryType.Product, Order = 1 },
            new Category { Id = 2, Name = "Sound", Type = CategoryType.Product, Order = 2 },
            new Category { Id = 3, Name = "Lighting", Type = CategoryType.Product, Order = 3 },
            new Category { Id = 4, Name = "DJs", Type = CategoryType.Hiring, Order = 4 },
            new Category { Id = 5, Name = "Sertanejo", Type = CategoryType.Service, Order = 5 },
            new Category { Id = 6, Name = "Rock", Type = CategoryType.Service, Order = 6 }
        );
    }
}

