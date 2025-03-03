using System;

namespace CompositeExample;

public class Song: ISongComponent
    {
        public string SongName { get; }
        public string BandName { get; }
        public int ReleaseYear { get; }

        public Song(string songName, string bandName, int releaseYear)
        {
            SongName = songName;
            BandName = bandName;
            ReleaseYear = releaseYear;
        }

        public void DisplaySongInfo()
        {
            Console.WriteLine($"{SongName} was recorded by {BandName} in {ReleaseYear}");
        }

    public void Play()
    {
        DisplaySongInfo();
    }

}
