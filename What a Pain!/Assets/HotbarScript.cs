using UnityEngine;
using UnityEngine.UI;
using System;

public class HotbarScript : MonoBehaviour
{
    public RawImage slot;
    public Texture2D transparent; 
    public int numberOfSquares = 10;
    public float squareWidth = 10;

    //public Texture2D[] iconsPrefab = new Texture2D[10];
    public RawImage[] slots = new RawImage[10];
    public RawImage[] slotIcons = new RawImage[10];

    PlayerScript playerScript;
    void Start()
    {
        // Create and position each square in the hotbar
        for (int i = 0; i < numberOfSquares; i++)
        {
            slots[i] = Instantiate(slot, transform.Find("Slot"));
            slotIcons[i] = Instantiate(new GameObject("RawImageObject " + i).AddComponent<RawImage>(), transform.Find("Icons"));
            slotIcons[i].texture = transparent;
        }
        FormatSquares(slots);
        FormatSquares(slotIcons);

        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        for (int i = 0; i < playerScript.itemList.Length; i++)
            if (playerScript.itemList[i] != null)
                slotIcons[i].texture = playerScript.itemList[i].GetComponent<ItemInfo>().itemTexture;
    }

    //Spawns a square based on its index and returns each square
    void FormatSquares(RawImage[] list)
    {
        for(int i = 0; i < list.Length; i++)
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
