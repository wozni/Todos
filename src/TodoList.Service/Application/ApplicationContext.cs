using Microsoft.EntityFrameworkCore;
using TodoList.Service.Model;

namespace TodoList.Service.Application;

public class ApplicationContext : DbContext
{
    public DbSet<Notebook> Notebooks { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notebook>(notebook =>
        {
            notebook.ToTable("Notebooks");
            notebook.HasKey(x => x.Id);
            notebook.Property(x => x.Title)
                .IsRequired();
            notebook.Property(x => x.Description)
                .IsRequired();
            notebook.HasMany(x => x.Notes)
                .WithOne(x => x.Owner)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Note>(note =>
        {
            note.ToTable("Notes");
            note.HasKey(x => x.Id);
            note.Property(x => x.Title)
                .IsRequired();
            note.Property(x => x.Content)
                .IsRequired();
        });
    }
}
