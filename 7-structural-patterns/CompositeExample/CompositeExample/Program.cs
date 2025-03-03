using CompositeExample;

class Program
    {
        static void Main(string[] args)
        {
            // TODO: Create SongComponents instead of SongGroups
            SongGroup industrialMusic = new SongGroup("Industrial", "is a style of experimental music that draws on transgressive and provocative themes");

            SongGroup heavyMetalMusic = new SongGroup("\nHeavy Metal", "is a genre of rock that developed in the late 1960s, largely in the UK and in the US");

            SongGroup dubstepMusic = new SongGroup("\nDubstep", "is a genre of electronic dance music that originated in South London, England");

            ISongComponent everySong = new SongGroup("Song List", "Every Song Available");
            everySong.Add(heavyMetalMusic);

            heavyMetalMusic.Add(new Song("War Pigs", "Black Sabbath", 1970));
            heavyMetalMusic.Add(new Song("Ace of Spades", "Motorhead", 1980));

            MusicPlayer crazyLarry = new MusicPlayer(everySong);
            crazyLarry.Play();
        }
    }
