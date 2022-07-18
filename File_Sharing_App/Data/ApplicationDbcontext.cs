using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace File_Sharing_App.Data
{
    public class ApplicationDbcontext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbcontext(DbContextOptions options):base(options)
        {
                
        }
        public DbSet<Uploads> Uploads { get; set; }  
        public DbSet<Contact> Contact { get; set; }  
    }
}
