using HotChocolate.Types;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.GraphQL.Types;

public class PlaylistInputType : InputObjectType<Playlist>
{
    protected override void Configure(IInputObjectTypeDescriptor<Playlist> descriptor)
    {
        descriptor.Description("Input type for creating or updating a playlist");
        
        descriptor
            .Field(p => p.Name)
            .Description("The name of the playlist");
            
        descriptor
            .Field(p => p.Description)
            .Description("The description of the playlist");
    }
} 