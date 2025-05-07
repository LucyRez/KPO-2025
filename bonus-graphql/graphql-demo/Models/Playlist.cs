namespace MusicServiceGraphQL.Models;

public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Song> Songs { get; set; } = new List<Song>();
} 