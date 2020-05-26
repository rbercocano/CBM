using EF = Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Charcutarie.Models.Entities;
using Charcutarie.Repository.DbContext.Mapping;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace Charcutarie.Repository.DbContext
{
    public class CharcuterieDbContext : EF.DbContext
    {
        public CharcuterieDbContext(DbContextOptions<CharcuterieDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
           .Entries()
           .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    var property = entityEntry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedOn");
                    if (property != null)
                        entityEntry.Property("CreatedOn").CurrentValue = DateTime.Now;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    var property = entityEntry.Properties.FirstOrDefault(p => p.Metadata.Name == "LastUpdated");
                    if (property != null)
                        entityEntry.Property("LastUpdated").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CorpClient> CorpClients { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PersonCustomer> PersonCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SystemModule> SystemModules { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemStatus> OrderItemStatus { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<OrderSummary> OrderSummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CorpClientConfiguration());
            modelBuilder.ApplyConfiguration(new MeasureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyCustomerConfiguration());
            modelBuilder.ApplyConfiguration(new PersonCustomerConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new SystemModuleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleModuleConfiguration());
            modelBuilder.ApplyConfiguration(new ContactTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerContactConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderSummaryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemStatusConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentStatusConfiguration());


        }
    }
}
