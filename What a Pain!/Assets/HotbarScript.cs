using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HotbarScript : MonoBehaviour
{
    public RawImage slot;
    public Texture2D transparent;
    public Texture2D selected; //change the selected slot to this texture
    public Texture2D slotTexture;
    public Texture2D canThrowTexture;
    public Texture2D willThrowTexture;
    public Texture2D placeholder;
    public int numberOfSquares = 10;
    public float squareWidth = 10;

    //public Texture2D[] iconsPrefab = new Texture2D[10];
    public RawImage[] slots = new RawImage[10];
    public RawImage[] slotIcons = new RawImage[10]; // Change to List

    PlayerScript playerScript;
    TipsScript tips;

    void Start()
    {
        tips = transform.Find("Tip").GetComponent<TipsScript>();
        tips.gameObject.SetActive(false);
        // Create and position each square in the hotbar
        for (int i = 0; i < numberOfSquares; i++)
        {
            slots[i] = Instantiate(slot, transform.Find("Slot"));
            slotIcons[i] = Instantiate(new GameObject("Icon " + i).AddComponent<RawImage>(), transform.Find("Icons"));
        }
        FormatSquares(slots);
        FormatSquares(slotIcons); // Convert list to array for FormatSquares method
    }

    private void Update()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        if (playerScript.itemList.Count > 0)
        {
            //flush
            for (int i = 0; i < slots.Length; i++)
            {
                if (i > playerScript.itemList.Count - 1)
                {
                    slots[i].texture = slotTexture;
                    slotIcons[i].texture = transparent;
                }
            }
            //selection of slot, and updating the icon
            for (int i = 0; i < playerScript.itemList.Count; i++)
            {
                //Apply item texture
                if (playerScript.itemList[i].GetComponent<ItemInfo>())
                {
                    slotIcons[i].texture = playerScript.itemList[i].GetComponent<ItemInfo>().itemTexture;
                }
                else slotIcons[i].texture = placeholder;

                //change the selected slot texture
                if (i == playerScript.currentItemIndex)
                {
                    slots[i].texture = selected;
                }
                else slots[i].texture = slotTexture;
            }

        }
        else if (playerScript.itemList.Count == 0)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].texture = slotTexture;
                slotIcons[i].texture = transparent;
            }
        }

        Coroutine switchslot = null;
        //if items are able to be thrown, highlight it
        if (playerScript.matchIndex != -1)
        {
            tips.gameObject.SetActive(true);
            switchslot = StartCoroutine(SwitchSlot());
        }
        else
        {
            if(switchslot != null)
                StopCoroutine(switchslot);
            tips.gameObject.SetActive(false);
        }
    }

    IEnumerator SwitchSlot()
    {
        while (true)
        {
            slots[playerScript.matchIndex].texture = canThrowTexture;
            if (playerScript.matchIndex == playerScript.currentItemIndex)
            {
                slots[playerScript.matchIndex].texture = willThrowTexture;
            }
            yield return new WaitForSecondsRealtime(tips.duration);
            slots[playerScript.matchIndex].texture = slotTexture;
            if (playerScript.matchIndex == playerScript.currentItemIndex)
            {
                slots[playerScript.matchIndex].texture = selected;
            }
            yield return new WaitForSecondsRealtime(tips.duration);
        }
    }
    //Spawns a square based on its index and returns each square
    void FormatSquares(RawImage[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            RawImage square = list[i];

            // Set the position and size of the square
            float xPos = i * 11 * squareWidth - 500;
            float yPos = -Screen.height / 2 + squareWidth * 15; // yPos relative to screen height
            square.rectTransform.anchoredPosition = new Vector2(xPos, yPos);
            square.rectTransform.sizeDelta = new Vector2(squareWidth, squareWidth);

            // Set the anchors to Min(0,0) and Max(1,1)
            square.rectTransform.anchorMin = Vector2.zero;
            square.rectTransform.anchorMax = Vector2.one;
        }
    }

    //Spawns a square based on its index and returns each square
    void FormatSquares(List<RawImage> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            RawImage square = list[i];

            // Set the position and size of the square
            float xPos = i * 11 * squareWidth - 500;
            float yPos = -Screen.height / 2 + squareWidth * 15; // yPos relative to screen height
            square.rectTransform.anchoredPosition = new Vector2(xPos, yPos);
            square.rectTransform.sizeDelta = new Vector2(squareWidth, squareWidth);

            // Set the anchors to Min(0,0) and Max(1,1)
            square.rectTransform.anchorMin = Vector2.zero;
            square.rectTransform.anchorMax = Vector2.one;
        }
    }
}
