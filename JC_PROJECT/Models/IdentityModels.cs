using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JC_PROJECT.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant d'autres propriétés à votre classe ApplicationUser. Pour en savoir plus, consultez https://go.microsoft.com/fwlink/?LinkID=317594.
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
   
    public class ApplicationUser : IdentityUser<int, CustomUserLogin , CustomUserRole, CustomUserClaim>
    {
 
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("OracleAuthDBContext")
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
            modelBuilder.HasDefaultSchema("AUTH_DEV");
            modelBuilder
                .Properties()
                .Where(p => p.PropertyType == typeof(string) &&
                            !p.Name.Contains("Id") &&
                            !p.Name.Contains("Provider"))
                .Configure(p => p.HasMaxLength(256));

            modelBuilder.Entity<CustomRole>().ToTable("AspNetRoles").Property(c => c.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers").Property(c => c.UserName).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers").Property(c => c.Email).HasMaxLength(128).IsRequired();


            modelBuilder.Entity<CustomRole>()
                .HasKey(r => new { r.Id })
                .ToTable("AspNetRoles");
            modelBuilder.Entity<CustomUserClaim>()
                .HasKey(r => new { r.Id })
                .ToTable("AspNetUserClaims");
            modelBuilder.Entity<CustomUserRole>()
            .HasKey(r => new { r.UserId, r.RoleId })
            .ToTable("AspNetUserRoles");
            modelBuilder.Entity<CustomUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey })
                .ToTable("AspNetUserLogins");


            modelBuilder.Entity<CustomUserRole>().ToTable("AspNetUserRoles");

            modelBuilder.Entity<CustomUserLogin>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<CustomUserClaim>().ToTable("AspNetUserClaims");
        }
    }
}
