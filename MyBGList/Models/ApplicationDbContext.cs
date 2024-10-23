using Microsoft.EntityFrameworkCore;

namespace MyBGList.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<BoardGame> BoardGames => Set<BoardGame>();
    public DbSet<Domain> Domains => Set<Domain>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BoardGame2Domain> BoardGames2Domains => Set<BoardGame2Domain>();
    public DbSet<BoardGame2Mechanic> BoardGames2Mechanics => Set<BoardGame2Mechanic>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BoardGame>()
            .HasOne(e => e.Publisher)
            .WithMany(e => e.BoardGames)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BoardGame2Domain>()
            .HasKey(bg => new { bg.BoardGameId, bg.DomainId });

        modelBuilder.Entity<BoardGame2Domain>()
            .HasOne(e => e.BoardGame)
            .WithMany(e => e.BoardGames_Domains)
            .HasForeignKey(e => e.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BoardGame2Domain>()
            .HasOne(e => e.Domain)
            .WithMany(e => e.BoardGames2Domains)
            .HasForeignKey(e => e.DomainId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BoardGame2Mechanic>()
            .HasKey(bg => new { bg.BoardGameId, bg.MechanicId });

        modelBuilder.Entity<BoardGame2Mechanic>()
            .HasOne(e => e.BoardGame)
            .WithMany(e => e.BoardGames_Mechanics)
            .HasForeignKey(e => e.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGame2Mechanic>()
            .HasOne(e => e.Mechanic)
            .WithMany(e => e.BoardGames2Mechanics)
            .HasForeignKey(e => e.MechanicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BoardGame2Category>()
            .HasKey(bg => new { bg.BoardGameId, bg.CategoryId });

        modelBuilder.Entity<BoardGame2Category>()
            .HasOne(e => e.BoardGame)
            .WithMany(e => e.BoardGames_Categories)
            .HasForeignKey(e => e.BoardGameId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardGame2Category>()
            .HasOne(e => e.Category)
            .WithMany(e => e.BoardGames2Categories)
            .HasForeignKey(e => e.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


    }
}