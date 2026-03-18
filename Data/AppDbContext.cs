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
        public DbSet<Subclass> SubClass { get; set; }
        public DbSet<Feat> Feat { get; set; }
        public DbSet<Spell> Spell { get; set; }
        public DbSet<Background> Background { get; set; }
        public DbSet<CharacterSubClass> CharacterSubClass { get; set; }
        public DbSet<CharacterClass> CharacterClass { get; set; }
        public DbSet<CharacterFeat> CharacterFeat { get; set; }      
        public DbSet<CharacterSpell> CharacterSpell { get; set; }
        public DbSet<CharacterAbility> CharacterAbility { get; set; }
        public DbSet<CharacterSkill> CharacterSkill { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

   /*         modelBuilder.Entity<CharacterSkill>()
        .HasOne<Character>()
        .WithMany()
        .HasForeignKey(s => s.CharacterId)
        .OnDelete(DeleteBehavior.Cascade);*/
            modelBuilder.Entity<CharacterSubClass>(entity =>
            {
                entity.HasOne(cs => cs.Character)
                    .WithMany(c => c.CharacterSubClasses) 
                    .HasForeignKey(cs => cs.CharacterId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(cs => cs.Class)
                      .WithMany()
                      .HasForeignKey(cs => cs.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cs => cs.SubClass)
                      .WithMany()
                      .HasForeignKey(cs => cs.SubclassId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cs => cs.CharacterClass)
                    .WithMany(cc => cc.CharacterSubClasses)
                    .HasForeignKey(cs => cs.CharacterClassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
