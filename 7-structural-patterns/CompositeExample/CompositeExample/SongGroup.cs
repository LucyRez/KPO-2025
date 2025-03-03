using System;

namespace CompositeExample;

public class SongGroup: ISongComponent
    {
        public string GroupName { get; }
        public string GroupDescription { get; }
        private List<ISongComponent> songComponents = new List<ISongComponent>();

        public SongGroup(string groupName, string groupDescription)
        {
            GroupName = groupName;
            GroupDescription = groupDescription;
        }

        public void Add(ISongComponent newSongComponent)
        {
            songComponents.Add(newSongComponent);
        }

        public void Remove(ISongComponent newSongComponent)
        {
            songComponents.Remove(newSongComponent);
        }

        public ISongComponent GetComponent(int componentIndex)
        {
            return songComponents[componentIndex];
        }

        public void DisplaySongInfo()
        {
            // TODO: Display info about all songs and song components inside songComponents list
        }

    public void Play()
    {
        Console.WriteLine(GroupName);
       foreach (var item in songComponents)
       {
            item.Play();
       }
    }
}