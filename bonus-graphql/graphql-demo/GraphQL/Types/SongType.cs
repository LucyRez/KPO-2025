using HotChocolate.Types;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.GraphQL.Types;

public class SongType : ObjectType<Song>
{
    protected override void Configure(IObjectTypeDescriptor<Song> descriptor)
    {
        descriptor.Description("Represents a song in the music service");
        
        descriptor
            .Field(s => s.Id)
            .Description("The unique identifier of the song");
            
        descriptor
            .Field(s => s.Title)
            .Description("The title of the song");
            
        descriptor
            .Field(s => s.Artist)
            .Description("The artist who performed the song");
            
        descriptor
            .Field(s => s.Album)
            .Description("The album the song belongs to");
            
        descriptor
            .Field(s => s.Year)
            .Description("The year the song was released");
            
        descriptor
            .Field(s => s.Duration)
            .Description("The duration of the song in seconds");
            
        descriptor
            .Field(s => s.Genre)
            .Description("The genre of the song");
            
        descriptor
            .Field(s => s.Playlists)
            .Description("The playlists that include this song");
    }
} 