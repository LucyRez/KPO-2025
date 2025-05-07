using HotChocolate.Types;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.GraphQL.Types;

public class PlaylistType : ObjectType<Playlist>
{
    protected override void Configure(IObjectTypeDescriptor<Playlist> descriptor)
    {
        descriptor.Description("Represents a playlist in the music service");
        
        descriptor
            .Field(p => p.Id)
            .Description("The unique identifier of the playlist");
            
        descriptor
            .Field(p => p.Name)
            .Description("The name of the playlist");
            
        descriptor
            .Field(p => p.Description)
            .Description("The description of the playlist");
            
        descriptor
            .Field(p => p.Songs)
            .Description("The songs included in the playlist");
    }
} 
