using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AudioPlayer : MediaPlayer
{

    public AudioPlayer(Transform parent)
    {
        _mediaUrl = MediaDownloadManager.Instance.audioAddress;
        _parent = parent;
    }



    public override void Play()
    {
        //Debug.Log("Audio Play");
        // Implement audio download and play logic
        DownloadAudio(_mediaUrl, _parent);
    }

    public override bool IsMediaLoaded()
    {
        return _isLoaded;
    }

    private void DownloadAudio(string url, Transform trasform)
    {
        //Debug.Log("DownloadAudio");
        // Implement video download logic
        MediaDownloadManager.Instance.DownloadAndPlayMedia(this, url, false, trasform);
    }

    protected override void OnMediaLoaded(bool isLoaded, string address)
    {
        //Debug.Log("OnMediaLoaded");
        _isLoaded = isLoaded;
        Addressables.LoadAssetAsync<AudioClip>(address).Completed += OnAudioLoaded;
    }

    private void OnAudioLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {

            //Debug.Log("OnAudioLoaded");

            AudioClip audioClip = handle.Result;

            GameObject gameObject = new GameObject("Audio Player");
            gameObject.transform.SetParent(_parent);
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            gameObject.AddComponent<RawImage>().texture = MediaDownloadManager.Instance.audioTexture;

            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
            MediaDownloadManager.Instance.OnCompleteMediadPlay(audioSource.clip.length, gameObject);
        }
    }
}