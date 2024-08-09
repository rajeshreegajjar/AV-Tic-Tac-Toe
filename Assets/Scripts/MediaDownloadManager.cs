using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;
using System;

public class MediaDownloadManager : MonoBehaviour
{
    public static MediaDownloadManager Instance;
    public GameObject progressBarPrefab;
    private ProgressBar progressBarInstance;

    public string videoAddress, audioAddress;

    public Texture videoTexture;
    public Texture audioTexture;

    public static Action onMediaDownload;
    public static Action onMediaPlayComplete;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void DownloadAndPlayMedia(IMediaPlayer mediaPlayer,string address,bool isVideo, Transform parent)
    {
        onMediaDownload.Invoke();
        StartCoroutine(DownloadCoroutine(mediaPlayer , address,isVideo,parent));
    }

    private IEnumerator DownloadCoroutine(IMediaPlayer mediaPlayer, string address,bool isVideo,Transform parent)
    {
        GameObject progressBarObject = Instantiate(progressBarPrefab, parent);
        ProgressBar progressBar = progressBarObject.GetComponent<ProgressBar>();
        yield return null;

        AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(address);
        while (!handle.IsDone)
        {
            progressBar.SetProgress(handle.PercentComplete);
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(handle.Status);
            mediaPlayer.OnMediaLoaded(true, address);
        }
        else
        {
            Debug.LogError("Download failed.");
        }

        Destroy(progressBarObject);
    }

    public void OnCompleteMediadPlay(float duration,GameObject mediaPlayer)
    {
        StartCoroutine(CompleteMediaPlay(duration, mediaPlayer));
    }

    IEnumerator CompleteMediaPlay(float time,GameObject mediaPlayer)
    {
        Debug.Log("CompleteMediaPlay " + time);
        var waitForClipRemainingTime = new WaitForSeconds(time);
        yield return waitForClipRemainingTime;
        mediaPlayer.SetActive(false);
        // Trigger event here!
        onMediaPlayComplete.Invoke();

    }
}

