using Microsoft.EntityFrameworkCore;

public class Muresan_Alexandru_Lab2Context(DbContextOptions<Muresan_Alexandru_Lab2Context> options) : DbContext(options)
{
    public DbSet<Muresan_Alexandru_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Muresan_Alexandru_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Muresan_Alexandru_Lab2.Models.Author> Author { get; set; } = default!;
    public DbSet<Muresan_Alexandru_Lab2.Models.Category> Category { get; set; } = default!;
}
