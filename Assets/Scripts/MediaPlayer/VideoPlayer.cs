using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayer : MediaPlayer
{
  
    public VideoPlayer(Transform parent)
    {
        _mediaUrl = MediaDownloadManager.Instance.videoAddress;
        _parent = parent;
    }

    public override void Play()
    {
        Debug.Log("Video Play");
        // Implement video download and play logic
        DownloadVideo(_mediaUrl,_parent);
    }


    public override bool IsMediaLoaded()
    {
        return _isLoaded;
    }

    private void DownloadVideo(string url, Transform trasform)
    {
        //Debug.Log("Video Download");
        // Implement video download logic
        MediaDownloadManager.Instance.DownloadAndPlayMedia(this,url,true, trasform);
    }

    protected override void OnMediaLoaded(bool isLoaded,string address)
    {
       // Debug.Log("OnMediaLoaded");
        _isLoaded = isLoaded;
        Addressables.LoadAssetAsync<VideoClip>(address).Completed += OnVideoInstantiated;
    }

    private void OnVideoInstantiated(AsyncOperationHandle<VideoClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            //Debug.Log("OnVideoInstantiated");

            VideoClip videoClip = handle.Result;

            GameObject gameObject = new GameObject("Video Player");
            gameObject.transform.SetParent(_parent);
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<RawImage>().texture = MediaDownloadManager.Instance.videoTexture;

            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;


            UnityEngine.Video.VideoPlayer videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.clip = videoClip;
            videoPlayer.targetTexture = MediaDownloadManager.Instance.videoTexture as RenderTexture;
            videoPlayer.Play();
            MediaDownloadManager.Instance.OnCompleteMediadPlay((float)videoClip.length, gameObject);

        }
    }
}