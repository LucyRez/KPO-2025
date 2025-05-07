using HotChocolate.Types;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.GraphQL.Types;

public class SongInputType : InputObjectType<Song>
{
    protected override void Configure(IInputObjectTypeDescriptor<Song> descriptor)
    {
        descriptor.Description("Input type for creating or updating a song");
        
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
    }
} 