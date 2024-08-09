using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider fillSlider;
    private float targetProgress = 0f;
    private float currentProgress = 0f;
    public float fillSpeed = 0.5f;

    void Update()
    {
        if (currentProgress < targetProgress)
        {
            currentProgress += fillSpeed * Time.deltaTime;
            fillSlider.value = currentProgress;
        }
    }

    public void SetProgress(float progress)
    {
        targetProgress = progress;
    }
}
