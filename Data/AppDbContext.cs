using DnD_CharacterCreation.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DnD_CharacterCreation.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Class> Class { get; set; }      
        public DbSet<Race> Race { get; set; }
        public DbSet<SubRace> SubRace { get; set; }
        public DbSet<Feat> Feat { get; set; }
        public DbSet<Spell> Spell { get; set; }
        public DbSet<Background> Background { get; set; }
        public DbSet<CharacterClass> CharacterClass { get; set; }
        public DbSet<CharacterFeat> CharacterFeat { get; set; }      
        public DbSet<CharacterSpell> CharacterSpell { get; set; }
        public DbSet<CharacterAbility> CharacterAbility { get; set; }
        public DbSet<CharacterSkill> CharacterSkill { get; set; }
  

        public AppDbContext(DbContextOptions options) : base(options)
        { 
        
        }

       /* protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                      .WithMany()
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired();

                entity.HasOne(ur => ur.Role)
                      .WithMany()
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired();
            });
        }*/
    }
}
