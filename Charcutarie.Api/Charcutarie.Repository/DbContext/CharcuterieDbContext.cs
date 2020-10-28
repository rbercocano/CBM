using EF = Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Charcutarie.Models.Entities;
using Charcutarie.Repository.DbContext.Mapping;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using System.Data;

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
                if (entityEntry.Entity is UserToken) continue;
                if (entityEntry.State == EntityState.Added)
                {
                    var property = entityEntry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedOn");
                    if (property != null)
                        entityEntry.Property("CreatedOn").CurrentValue = DateTimeOffset.UtcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    var property = entityEntry.Properties.FirstOrDefault(p => p.Metadata.Name == "LastUpdated");
                    if (property != null)
                        entityEntry.Property("LastUpdated").CurrentValue = DateTimeOffset.UtcNow;
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
        public DbSet<DataSheet> DataSheets { get; set; }
        public DbSet<DataSheetItem> DataSheetItems { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        //Custom Queries
        public DbSet<OrderSummary> OrderSummaries { get; set; }
        public DbSet<OrderItemReport> OrderItemReports { get; set; }
        public DbSet<PendingPaymentsSummary> PendingPaymentsSummaries { get; set; }
        public DbSet<SalesSummary> SalesSummaries { get; set; }
        public DbSet<SalesPerMonth> SalesPerMonths { get; set; }
        public DbSet<ProfitSummary> ProfitSummaries { get; set; }
        public DbSet<OrderCountSummary> OrderCountSummaries { get; set; }
        public DbSet<ProductionCostProfit> ProductionCostProfits { get; set; }
        public DbSet<Production> Production { get; set; }
        public DbSet<SummarizedOrderReport> SummarizedOrderReports { get; set; }
        public DbSet<Balance> Balance { get; set; }

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
            modelBuilder.ApplyConfiguration(new RawMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new DataSheetItemConfiguration());
            modelBuilder.ApplyConfiguration(new DataSheetConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemReportConfiguration());
            modelBuilder.ApplyConfiguration(new OrderCountSummaryConfiguration());
            modelBuilder.ApplyConfiguration(new SalesSummaryConfiguration());
            modelBuilder.ApplyConfiguration(new ProfitSummaryConfiguration());
            modelBuilder.ApplyConfiguration(new PendingPaymentsSummaryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductionCostProfitConfiguration());
            modelBuilder.ApplyConfiguration(new ProductionConfiguration());
            modelBuilder.ApplyConfiguration(new SalesPerMonthConfiguration());
            modelBuilder.ApplyConfiguration(new SummarizedOrderReportConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCostConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new BalanceConfiguration());

        }
        public async Task<int> ExecuteScalar(string command)
        {
            using var connection = this.Database.GetDbConnection();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = command;
            if (connection.State.Equals(ConnectionState.Closed)) { connection.Open(); }
            var result = await cmd.ExecuteScalarAsync();
            return result == null ? 0 : (int)result;
        }
    }
}
