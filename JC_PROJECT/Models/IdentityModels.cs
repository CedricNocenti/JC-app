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
            modelBuilder.HasDefaultSchema("AUTH_DEV"); //uppercase  
                                                       //modelBuilder.Entity<IdentityUserLogin>()
                                                       //.HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                                                       //.ToTable("AspNetUserLogins");
                                                       //modelBuilder.Entity<IdentityUserRole>()
                                                       //.HasKey(r => new { r.UserId, r.RoleId })
                                                       //.ToTable("AspNetUserRoles");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.UserName).HasColumnName("UserName");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.AccessFailedCount).HasColumnName("AccessFailedCount");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.LockoutEnabled).HasColumnName("LockoutEnabled");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.SecurityStamp).HasColumnName("SecurityStamp");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.PasswordHash).HasColumnName("PasswordHash");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.EmailConfirmed).HasColumnName("EmailConfirmed");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.Email).HasColumnName("Email");
                                                       //modelBuilder.Entity<ApplicationUser>()
                                                       //.ToTable("AspNetUsers").Property(p => p.Id).HasColumnName("Id");
                                                       //modelBuilder.Entity<CustomUserRole>()
                                                       //.ToTable("AspNetUserRoles").Property(p => p.RoleId).HasColumnName("RoleId");
                                                       //modelBuilder.Entity<CustomUserRole>()
                                                       //.ToTable("AspNetUserRoles").Property(p => p.UserId).HasColumnName("UserId");
                                                       //modelBuilder.Entity<CustomUserLogin>()
                                                       //.ToTable("AspNetUserLogins").Property(p => p.UserId).HasColumnName("UserId");
                                                       //modelBuilder.Entity<CustomUserLogin>()
                                                       //.ToTable("AspNetUserLogins").Property(p => p.ProviderKey).HasColumnName("ProviderKey");
                                                       //modelBuilder.Entity<CustomUserLogin>()
                                                       //.ToTable("AspNetUserLogins").Property(p => p.LoginProvider).HasColumnName("LoginProvider");
                                                       //modelBuilder.Entity<CustomUserClaim>()
                                                       //.ToTable("AspNetUserClaims").Property(p => p.Id).HasColumnName("Id");
                                                       //modelBuilder.Entity<CustomUserClaim>()
                                                       //.ToTable("AspNetUserClaims").Property(p => p.UserId).HasColumnName("UserId");
                                                       //modelBuilder.Entity<IdentityUserClaim>()
                                                       //.ToTable("AspNetUserClaims").Property(p => p.ClaimType).HasColumnName("ClaimType");
                                                       //modelBuilder.Entity<CustomUserClaim>()
                                                       //.ToTable("AspNetUserClaims").Property(p => p.ClaimValue).HasColumnName("ClaimValue");
                                                       //modelBuilder.Entity<IdentityRole>()
                                                       //.ToTable("AspNetRoles").Property(p => p.Id).HasColumnName("Id");
                                                       //modelBuilder.Entity<IdentityRole>()
                                                       //.ToTable("AspNetRoles").Property(p => p.Name).HasColumnName("Name");

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<CustomUserRole>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<CustomUserLogin>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<CustomUserClaim>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<CustomRole>().ToTable("AspNetRoles");
        }
    }
}
