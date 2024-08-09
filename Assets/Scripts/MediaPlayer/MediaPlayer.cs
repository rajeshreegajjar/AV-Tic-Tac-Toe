using UnityEngine;

public interface IMediaPlayer
{
    void Play();
    bool IsMediaLoaded();
    void OnMediaLoaded(bool isLoaded,string Address);

    public delegate void MediaPlayerPlaying();
    public static event MediaPlayerPlaying onMediaPlayerPlaying;
}

public abstract class MediaPlayer : IMediaPlayer
{
    protected string _mediaUrl;
    protected bool _isLoaded;
    protected Transform _parent;
    public abstract void Play();
    protected abstract void OnMediaLoaded(bool isLoaded,string Address);
    public abstract bool IsMediaLoaded();

  

    void IMediaPlayer.OnMediaLoaded(bool isLoaded,string Address)
    {
        OnMediaLoaded(isLoaded, Address);
    }
}
