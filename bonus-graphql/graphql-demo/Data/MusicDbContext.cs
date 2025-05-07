using Microsoft.EntityFrameworkCore;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options)
    {
    }

    public DbSet<Song> Songs { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>()
            .HasMany(s => s.Playlists)
            .WithMany(p => p.Songs)
            .UsingEntity(j => j.ToTable("PlaylistSongs"));
    }
} 