using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopDatabaseImplement.Models;

namespace TechiqueShopDatabaseImplement
{
    public class TechiqueShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TechiqueShopDatabase;Integrated Security=False;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provider>().HasIndex(u => u.Telephone).IsUnique();
            modelBuilder.Entity<Provider>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Component>().HasIndex(u => u.ComponentName).IsUnique();
            modelBuilder.Entity<Assembly>().HasIndex(u => u.AssemblyName).IsUnique();
            modelBuilder.Entity<Customer>().HasIndex(u => u.Telephone).IsUnique();
            modelBuilder.Entity<Customer>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Order>().HasIndex(u => u.OrderName);
            modelBuilder.Entity<Supply>().HasIndex(u => u.SupplyName);
        }

        public virtual DbSet<Provider> Providers { set; get; }
        public virtual DbSet<Customer> Customers { set; get; }
        public virtual DbSet<Assembly> Assemblys { set; get; }
        public virtual DbSet<Component> Components { set; get; }
        public virtual DbSet<Delivery> Deliveries { set; get; }
        public virtual DbSet<GetTechnique> GetTechniquies { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Supply> Supplies { set; get; }
        public virtual DbSet<AssemblyComponent> AssemblyComponents { set; get; }
        public virtual DbSet<AssemblyOrder> AssemblyOrders { set; get; }
        public virtual DbSet<DeliveryComponent> DeliveryComponents { set; get; }
        public virtual DbSet<SupplyComponent> SupplyComponents { set; get; }
        public virtual DbSet<SupplyGetTechnique> SupplyGetTechniques { set; get; }
        public virtual DbSet<SupplyOrder> SupplyOrders { set; get; }
    }
}
