using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SchoolProject.Models
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<SchoolEntities> SchoolEntitiess { get; set; }
        public DbSet<DepartmentEntities> DepartmentEntitiess { get; set; }
        public DbSet<ClassEntities> ClassEntitiess { get; set; }
        public DbSet<UserDetail> UserDetail { get; set; }
        public DbSet<ManageEntities> ManageEntities { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<RoleGroup> RoleGroup { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.Entity<Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin>().HasKey(e => e.UserId);
            modelBuilder.Entity<Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole>().HasKey(e => new { e.UserId ,e.RoleId});
            modelBuilder.Entity<UserDetail>().HasKey(e => e.UserId);
            modelBuilder.Entity<RoleGroup>().HasKey(e => new { e.GroupId });
            modelBuilder.Entity<UserGroup>().HasNoKey();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var parameter = Expression.Parameter(entityType.ClrType);
                //var deletedCheck = Expression.Lambda(Expression.Equal(Expression.Property(parameter, "IsDeleted"), Expression.Constant(false)), parameter);
                //modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }
        }


        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{
        //    OnBeforeSaving();
        //    return base.SaveChanges(acceptAllChangesOnSuccess);
        //}

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    OnBeforeSaving();
        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

        //private void OnBeforeSaving()
        //{
        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.CurrentValues["IsDeleted"] = false;
        //                break;

        //            case EntityState.Deleted:
        //                entry.State = EntityState.Modified;
        //                entry.CurrentValues["IsDeleted"] = true;
        //                break;
        //        }
        //    }
        //}
    }
}
