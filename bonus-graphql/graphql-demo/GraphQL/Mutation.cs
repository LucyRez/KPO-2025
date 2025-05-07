using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using MusicServiceGraphQL.Data;
using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.GraphQL;


public class Mutation
{
    public async Task<Song> AddSong(MusicDbContext context, Song song)
    {
        context.Songs.Add(song);
        await context.SaveChangesAsync();
        return song;
    }

    public async Task<Playlist> AddPlaylist(MusicDbContext context, Playlist playlist)
    {
        context.Playlists.Add(playlist);
        await context.SaveChangesAsync();
        return playlist;
    }

    public async Task<Song?> UpdateSong( MusicDbContext context, int id, Song song)
    {
        var existingSong = await context.Songs.FindAsync(id);
        if (existingSong == null)
            return null;

        existingSong.Title = song.Title;
        existingSong.Artist = song.Artist;
        existingSong.Album = song.Album;
        existingSong.Year = song.Year;
        existingSong.Duration = song.Duration;
        existingSong.Genre = song.Genre;

        await context.SaveChangesAsync();
        return existingSong;
    }

    public async Task<Playlist?> UpdatePlaylist(MusicDbContext context, int id, Playlist playlist)
    {
        var existingPlaylist = await context.Playlists.FindAsync(id);
        if (existingPlaylist == null)
            return null;

        existingPlaylist.Name = playlist.Name;
        existingPlaylist.Description = playlist.Description;

        await context.SaveChangesAsync();
        return existingPlaylist;
    }

    public async Task<bool> DeleteSong(MusicDbContext context, int id)
    {
        var song = await context.Songs.FindAsync(id);
        if (song == null)
            return false;

        context.Songs.Remove(song);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePlaylist( MusicDbContext context, int id)
    {
        var playlist = await context.Playlists.FindAsync(id);
        if (playlist == null)
            return false;

        context.Playlists.Remove(playlist);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Playlist?> AddSongToPlaylist(MusicDbContext context, int playlistId, int songId)
    {
        var playlist = await context.Playlists
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == playlistId);
            
        var song = await context.Songs.FindAsync(songId);
        
        if (playlist == null || song == null)
            return null;

        playlist.Songs.Add(song);
        await context.SaveChangesAsync();
        return playlist;
    }

    public async Task<Playlist?> RemoveSongFromPlaylist( MusicDbContext context, int playlistId, int songId)
    {
        var playlist = await context.Playlists
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == playlistId);
            
        var song = await context.Songs.FindAsync(songId);
        
        if (playlist == null || song == null)
            return null;

        playlist.Songs.Remove(song);
        await context.SaveChangesAsync();
        return playlist;
    }
} 