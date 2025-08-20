using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using DevExpress.ExpressApp.EFCore.Updating;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DXApplication4.Module.BusinessObjects; 


namespace DXApplication4.Module.BusinessObjects
{
    [TypesInfoInitializer(typeof(DbContextTypesInfoInitializer<DXApplication4EFCoreDbContext>))]
    public class DXApplication4EFCoreDbContext : DbContext
    {
        public DXApplication4EFCoreDbContext(DbContextOptions<DXApplication4EFCoreDbContext> options) : base(options)
        {
        }
        //public DbSet<ModuleInfo> ModulesInfo { get; set; }
        public DbSet<ModelDifference> ModelDifferences { get; set; }
        public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
        public DbSet<PermissionPolicyRole> Roles { get; set; }
        public DbSet<DXApplication4.Module.BusinessObjects.ApplicationUser> Users { get; set; }
        public DbSet<DXApplication4.Module.BusinessObjects.ApplicationUserLoginInfo> UserLoginsInfo { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Loan> Loans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseDeferredDeletion(this);
            modelBuilder.UseOptimisticLock();
            modelBuilder.SetOneToManyAssociationDeleteBehavior(DeleteBehavior.SetNull, DeleteBehavior.Cascade);
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
            modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
            modelBuilder.Entity<DXApplication4.Module.BusinessObjects.ApplicationUserLoginInfo>(b =>
            {
                b.HasIndex(nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName), nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
            });
            modelBuilder.Entity<ModelDifference>()
                .HasMany(t => t.Aspects)
                .WithOne(t => t.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Book>(b =>
            {
                b.HasIndex(x => x.ISBN).IsUnique(); 
            });
            modelBuilder.Entity<Member>(b =>
            {
                b.HasIndex(x => x.Email)
                 .IsUnique()
                 .HasFilter("[Email] IS NOT NULL"); 
            });
            modelBuilder.Entity<Loan>(b =>
            {
                b.HasOne(l => l.Kitap)
                 .WithMany(k => k.Loans)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(l => l.Uye)
                 .WithMany(m => m.Loans)
                 .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
