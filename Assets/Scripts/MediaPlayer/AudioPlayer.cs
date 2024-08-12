using System.Collections;
using System.Threading.Tasks;
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

    protected override async void OnMediaLoaded( string address,ProgressBar progressBar)
    {
        //Debug.Log("OnMediaLoaded");
        AudioClip clip = await LoadAudioClipAsync(address, progressBar);

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
        audioSource.clip = clip;
        audioSource.Play();
        _isLoaded = true;
        MediaDownloadManager.Instance.OnCompleteMediadPlay(audioSource.clip.length, gameObject);
    }

    // Task function to load an AudioClip asynchronously using Addressables
    public async Task<AudioClip> LoadAudioClipAsync(string address,ProgressBar progressBar)
    {
        // Start the asynchronous load operation
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(address);

        // Continuously update the progress bar while loading
        while (!handle.IsDone)
        {
            if (progressBar != null)
            {
                progressBar.fillSlider.value = handle.PercentComplete;  // Update the progress bar
            }

            await Task.Yield();  // Yield control back to the main thread
        }

        // Check if the load operation was successful
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;  // Return the loaded AudioClip
        }
        else
        {
            Debug.LogError($"Failed to load AudioClip from address: {address}");
            return null;  // Return null if loading failed
        }
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