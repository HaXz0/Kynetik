using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("Loading Settings")]
    // This is set to your specific scene name
    public string sceneToLoad = "Arena_Test01"; 
    public float minLoadTime = 2.0f;
    public float promptFadeSpeed = 4.0f; // Faster fade for the text

    [Header("UI References")]
    public Slider progressBar;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI tipText;
    public TextMeshProUGUI pressKeyText; 
    
    private float promptAlpha = 0f;      
    private bool canLoadNow = false;

    public string[] gameTips;

    void Start()
    {
        // Reset UI
        if (pressKeyText != null) { Color c = pressKeyText.color; c.a = 0f; pressKeyText.color = c; }
        if (progressBar != null) progressBar.value = 0;
        
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        // Random Tip
        if (gameTips.Length > 0 && tipText != null)
            tipText.text = gameTips[Random.Range(0, gameTips.Length)];

        // Start loading Arena_Test01
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float timer = 0;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;
            
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float timerProgress = Mathf.Clamp01(timer / minLoadTime);
            float finalProgress = Mathf.Min(progress, timerProgress);

            if (progressBar != null) progressBar.value = finalProgress;
            if (percentageText != null) percentageText.text = (finalProgress * 100f).ToString("F0") + "%";

            // If we are at 100%
            if (finalProgress >= 1.0f)
            {
                percentageText.text = "100%";

                // Fade in the "Press Any Key" text
                if (pressKeyText != null && promptAlpha < 1.0f)
                {
                    promptAlpha += Time.deltaTime * promptFadeSpeed;
                    Color textColor = pressKeyText.color;
                    textColor.a = Mathf.Clamp01(promptAlpha);
                    pressKeyText.color = textColor;
                }

                canLoadNow = true;
            }

            // The moment any key is pressed after reaching 100%
            if (canLoadNow && Input.anyKeyDown)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}