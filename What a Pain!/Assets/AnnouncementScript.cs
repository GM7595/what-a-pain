using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnnouncementScript: MonoBehaviour
{
    public float fadeInDuration = 1.0f;  // Time taken for the image to fade in
    public float displayDuration = 2.0f;  // Time the image stays visible
    public float fadeOutDuration = 1.0f;  // Time taken for the image to fade out

    TMP_Text textComp;
    private void Start()
    {
        textComp = GetComponentInChildren<TMP_Text>();
        textComp.text = null;

        Color startColor = textComp.color;
        startColor.a = 0f;
        textComp.color = startColor;
    }

    public void Announce(string pun)
    {
        StopAllCoroutines();
        textComp.text = pun;
        // Start the fade-in coroutine
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {

        float elapsedTime = 0f;

        // Gradually increase the alpha value to make the image visible
        while (elapsedTime < fadeInDuration)
        {
            Color currentColor = textComp.color;
            currentColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            textComp.color = currentColor;

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
            Color currentColor = textComp.color;
            currentColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            textComp.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textComp.text = null;
    }
}
