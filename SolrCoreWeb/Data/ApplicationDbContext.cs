namespace SolrCoreWeb.Data
{
    using Duende.IdentityServer.EntityFramework.Entities;
    using Duende.IdentityServer.EntityFramework.Options;
    using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Models;

    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("asp_net_users");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");
            modelBuilder.Entity<IdentityRole>().ToTable("asp_net_roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
            modelBuilder.Entity<DeviceFlowCodes>().ToTable("device_codes");
            modelBuilder.Entity<Key>().ToTable("keys");
            modelBuilder.Entity<PersistedGrant>().ToTable("persisted_grants");
        }
    }
}