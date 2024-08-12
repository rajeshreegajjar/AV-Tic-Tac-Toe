using UnityEngine;

public interface IMediaPlayer
{
    void Play();
    bool IsMediaLoaded();
    void OnMediaLoaded(string Address,ProgressBar progressBar);

    public delegate void MediaPlayerPlaying();
    public static event MediaPlayerPlaying onMediaPlayerPlaying;
}

public abstract class MediaPlayer : IMediaPlayer
{
    protected string _mediaUrl;
    protected bool _isLoaded;
    protected Transform _parent;
    public abstract void Play();
    protected abstract void OnMediaLoaded(string Address,ProgressBar progressBar);
    public abstract bool IsMediaLoaded();

  

    void IMediaPlayer.OnMediaLoaded(string Address,ProgressBar progressBar)
    {
        OnMediaLoaded( Address, progressBar);
    }
}
