using Microsoft.AspNet.Identity.EntityFramework;
using OwinSample.WebApp.Models;

namespace OwinSample.WebApp.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();
    }
}