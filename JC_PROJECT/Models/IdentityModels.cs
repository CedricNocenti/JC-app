using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JC_PROJECT.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant d'autres propriétés à votre classe ApplicationUser. Pour en savoir plus, consultez https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
       

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("OracleAuthDBContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Permet de récupérer toutes les informations concernant l'authentification
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("AUTH_DEV"); //uppercase  
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.UserName).HasColumnName("UserName");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.AccessFailedCount).HasColumnName("AccessFailedCount");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.LockoutEnabled).HasColumnName("LockoutEnabled");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.SecurityStamp).HasColumnName("SecurityStamp");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.PasswordHash).HasColumnName("PasswordHash");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.EmailConfirmed).HasColumnName("EmailConfirmed");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.Email).HasColumnName("Email");
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers").Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<IdentityUserRole>()
            .ToTable("AspNetUserRoles").Property(p => p.RoleId).HasColumnName("RoleId");
            modelBuilder.Entity<IdentityUserRole>()
            .ToTable("AspNetUserRoles").Property(p => p.UserId).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserLogin>()
            .ToTable("AspNetUserLogins").Property(p => p.UserId).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserLogin>()
            .ToTable("AspNetUserLogins").Property(p => p.ProviderKey).HasColumnName("ProviderKey");
            modelBuilder.Entity<IdentityUserLogin>()
            .ToTable("AspNetUserLogins").Property(p => p.LoginProvider).HasColumnName("LoginProvider");
            modelBuilder.Entity<IdentityUserClaim>()
            .ToTable("AspNetUserClaims").Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<IdentityUserClaim>()
            .ToTable("AspNetUserClaims").Property(p => p.UserId).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserClaim>()
            .ToTable("AspNetUserClaims").Property(p => p.ClaimType).HasColumnName("ClaimType");
            modelBuilder.Entity<IdentityUserClaim>()
            .ToTable("AspNetUserClaims").Property(p => p.ClaimValue).HasColumnName("ClaimValue");
            modelBuilder.Entity<IdentityRole>()
            .ToTable("AspNetRoles").Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<IdentityRole>()
            .ToTable("AspNetRoles").Property(p => p.Name).HasColumnName("Name");
        }
    }
}
