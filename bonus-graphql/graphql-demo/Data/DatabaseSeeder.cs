using MusicServiceGraphQL.Models;

namespace MusicServiceGraphQL.Data;

public static class DatabaseSeeder
{
    public static void SeedData(MusicDbContext context)
    {
        if (!context.Songs.Any() && !context.Playlists.Any())
        {
            // Create some songs
            var songs = new List<Song>
            {
                new Song
                {
                    Title = "Bohemian Rhapsody",
                    Artist = "Queen",
                    Album = "A Night at the Opera",
                    Year = 1975,
                    Duration = 354,
                    Genre = "Rock"
                },
                new Song
                {
                    Title = "Stairway to Heaven",
                    Artist = "Led Zeppelin",
                    Album = "Led Zeppelin IV",
                    Year = 1971,
                    Duration = 482,
                    Genre = "Rock"
                },
                new Song
                {
                    Title = "Billie Jean",
                    Artist = "Michael Jackson",
                    Album = "Thriller",
                    Year = 1982,
                    Duration = 294,
                    Genre = "Pop"
                },
                new Song
                {
                    Title = "Sweet Child O' Mine",
                    Artist = "Guns N' Roses",
                    Album = "Appetite for Destruction",
                    Year = 1987,
                    Duration = 356,
                    Genre = "Rock"
                },
                new Song
                {
                    Title = "Smells Like Teen Spirit",
                    Artist = "Nirvana",
                    Album = "Nevermind",
                    Year = 1991,
                    Duration = 301,
                    Genre = "Grunge"
                }
            };

            // Create some playlists
            var playlists = new List<Playlist>
            {
                new Playlist
                {
                    Name = "Classic Rock",
                    Description = "The best classic rock songs"
                },
                new Playlist
                {
                    Name = "80s Hits",
                    Description = "Popular songs from the 80s"
                },
                new Playlist
                {
                    Name = "90s Alternative",
                    Description = "Alternative rock from the 90s"
                }
            };

            // Add songs to playlists
            playlists[0].Songs.Add(songs[0]); // Bohemian Rhapsody to Classic Rock
            playlists[0].Songs.Add(songs[1]); // Stairway to Heaven to Classic Rock
            playlists[0].Songs.Add(songs[3]); // Sweet Child O' Mine to Classic Rock

            playlists[1].Songs.Add(songs[2]); // Billie Jean to 80s Hits
            playlists[1].Songs.Add(songs[3]); // Sweet Child O' Mine to 80s Hits

            playlists[2].Songs.Add(songs[4]); // Smells Like Teen Spirit to 90s Alternative

            // Add to context and save
            context.Songs.AddRange(songs);
            context.Playlists.AddRange(playlists);
            context.SaveChanges();
        }
    }
} 