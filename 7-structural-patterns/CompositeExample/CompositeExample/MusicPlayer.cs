using System;

namespace CompositeExample;

public class MusicPlayer
{
    private readonly ISongComponent songComponent;

    public MusicPlayer(ISongComponent songComponent) {
        this.songComponent = songComponent;
    }

    public void Play() {
        songComponent.Play();
    }
}