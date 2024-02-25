using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using PokeApiNet;
using webmvc.Models;

namespace webmvc.Infrastructure
{
    public class MyDbContext: IdentityDbContext<User, IdentityRole<int>, int, IdentityUserClaim<int>,
        IdentityUserRole<int>, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            :base(options)
        {

        }
        public DbSet<Pokemon> Pokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(m => m.UserId);
            modelBuilder.Entity<IdentityUserRole<int>>().HasKey(m => m.UserId);
            modelBuilder.Entity<IdentityUserToken<int>>().HasKey(m => m.UserId);
        }
    }
}
