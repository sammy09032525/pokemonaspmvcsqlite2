using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokeApiNet;
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
            modelBuilder.Entity<NamedApiResource<Ability>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<Generation>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<Item>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<Move>>().ToTable(nameof(NamedApiResource<Move>), t => t.ExcludeFromMigrations()).HasNoKey();
            modelBuilder.Entity<NamedApiResource<MoveLearnMethod>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<PokemonForm>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<PokemonSpecies>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<Stat>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<PokeApiNet.Type>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<PokeApiNet.Version>>().HasNoKey();
            modelBuilder.Entity<NamedApiResource<VersionGroup>>().HasNoKey();
            modelBuilder.Entity<PokemonHeldItem>().ToTable(nameof(PokemonHeldItem), t => t.ExcludeFromMigrations()).HasNoKey();
            modelBuilder.Entity<PokemonHeldItemVersion>().HasNoKey();
            modelBuilder.Entity<PokemonMove>().ToTable(nameof(PokemonMove), t => t.ExcludeFromMigrations()).HasNoKey();
            modelBuilder.Entity<PokemonMoveVersion>().HasNoKey();
            modelBuilder.Entity<PokemonPastTypes>().ToTable(nameof(PokemonPastTypes), t => t.ExcludeFromMigrations()).HasNoKey();
            modelBuilder.Entity<PokemonStat>().HasNoKey();
            modelBuilder.Entity<PokemonType>().HasNoKey();
            modelBuilder.Entity<VersionGameIndex>().HasNoKey();

            modelBuilder.Entity<PokemonPastTypes>().Ignore(c => c.Generation);
            modelBuilder.Entity<PokemonHeldItem>().ToTable(nameof(PokemonHeldItem), t => t.ExcludeFromMigrations()).Ignore(c => c.Item).Ignore(c=> c.VersionDetails);
//            modelBuilder.Entity<PokemonMove>().Ignore(c => c.Move);
            modelBuilder.Entity<PokemonMoveVersion>().Ignore(c => c.MoveLearnMethod);
            modelBuilder.Entity<PokemonMove>().Ignore(c => c.Move);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.Forms);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.Species);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.Sprites);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.HeldItems);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.Moves);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.Stats);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.GameIndicies);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.PastTypes);
            modelBuilder.Entity<Pokemon>().Ignore(c => c.Types);
            modelBuilder.Entity<PokemonStat>().Ignore(c => c.Stat);
            modelBuilder.Entity<PokemonType>().Ignore(c => c.Type);
            modelBuilder.Entity<PokemonHeldItemVersion>().Ignore(c => c.Version);
            modelBuilder.Entity<VersionGameIndex>().Ignore(c => c.Version);
            modelBuilder.Entity<PokemonMoveVersion>().Ignore(c => c.VersionGroup);

            modelBuilder.Entity<PokemonHeldItem>().ToTable(nameof(PokemonHeldItem), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(m => m.UserId);
            modelBuilder.Entity<IdentityUserRole<int>>().HasKey(m => m.UserId);
            modelBuilder.Entity<IdentityUserToken<int>>().HasKey(m => m.UserId);
        }
    }
}
