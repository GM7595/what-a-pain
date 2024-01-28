using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsScript : MonoBehaviour
{
    public float duration = 0.3f;
    float timer;
    float nextTimeStamp;
    Color transparent;
    TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        transparent = text.color;
        nextTimeStamp = duration;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextTimeStamp)
        {
            transparent.a = (transparent.a == 0) ? 1 : 0;
            text.color = transparent;
            nextTimeStamp += duration;
        }
    }
}
