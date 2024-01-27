using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectScript : MonoBehaviour
{
    public RawImage rawImage;
    public float fadeInDuration = 1.0f;  // Time taken for the image to fade in
    public float displayDuration = 2.0f;  // Time the image stays visible
    public float fadeOutDuration = 1.0f;  // Time taken for the image to fade out

    TMP_Text textComp;
    private void Start()
    {
        textComp = GetComponentInChildren<TMP_Text>();
        ResetText();

        // Initialize the image to be transparent at the start
        rawImage = GetComponent<RawImage>();
        Color startColor = rawImage.color;
        startColor.a = 0f;
        rawImage.color = startColor;
    }

    public void ShowEffect()
    {
        // Start the fade-in coroutine
        StartCoroutine(FadeIn());
    }

    public void ShowEffect(string itemName)
    {
        textComp.text = "Picked Up: " + itemName;
        // Start the fade-in coroutine
        StartCoroutine(FadeIn(itemName));

        Invoke("ResetText", fadeInDuration + displayDuration + fadeOutDuration);
    }

    void ResetText()
    {
        textComp.text = null;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        // Gradually increase the alpha value to make the image visible
        while (elapsedTime < fadeInDuration)
        {
            Color currentColor = rawImage.color;
            currentColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            rawImage.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for the display duration
        yield return new WaitForSecondsRealtime(displayDuration);

        // Start the fade-out coroutine
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn(string itemName)
    {

        float elapsedTime = 0f;

        // Gradually increase the alpha value to make the image visible
        while (elapsedTime < fadeInDuration)
        {
            Color currentColor = rawImage.color;
            currentColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            rawImage.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for the display duration
        yield return new WaitForSecondsRealtime(displayDuration);

        // Start the fade-out coroutine
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        // Gradually decrease the alpha value to make the image invisible
        while (elapsedTime < fadeOutDuration)
        {
            Color currentColor = rawImage.color;
            currentColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            rawImage.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Optionally, you can destroy or deactivate the image object after fading out
        Destroy(gameObject);
    }
}
