using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using MusicServiceGraphQL.Data;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Song> GetSongs(MusicDbContext context)
    {
        return context.Songs;
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Playlist> GetPlaylists(MusicDbContext context)
    {
        return context.Playlists;
    }


    public async Task<Song?> GetSongById(MusicDbContext context, int id)
    {
        return await context.Songs.FindAsync(id);
    }

    public async Task<Playlist?> GetPlaylistById( MusicDbContext context, int id)
    {
        return await context.Playlists.FindAsync(id);
    }
} 