namespace MusicServiceGraphQL.Models;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Duration { get; set; } // Duration in seconds
    public string Genre { get; set; } = string.Empty;
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
} 