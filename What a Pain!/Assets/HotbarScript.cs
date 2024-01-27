using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public RawImage emptySlot;
    public RawImage filledSlot;
    public int numberOfSquares = 10;
    public float squareWidth = 10;

    public RawImage[] itemIcons = new RawImage[10];
    public RawImage[] slots = new RawImage[10];

    void Start()
    {
        CreateHotbar();
    }

    void CreateHotbar()
    {
        // Create and position each square in the hotbar
        for (int i = 0; i < numberOfSquares; i++)
        {
            slots[i] = SpawnSquare(i, emptySlot);
        }

        //Destroy(slots[6]);
    }

    //Spawns a square based on its index and returns each square
    RawImage SpawnSquare(int index, RawImage square)
    {
        RawImage spawnedSquare = Instantiate(square, transform.Find("Slot"));

        // Set the position and size of the square
        float xPos = index * 11 * squareWidth - 500;
        float yPos = -Screen.height / 2 + squareWidth * 15; // yPos relative to screen height
        spawnedSquare.rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        spawnedSquare.rectTransform.sizeDelta = new Vector2(squareWidth, squareWidth);

        return spawnedSquare;
    }

    //When item is picked up, cast the item icon
    public void FillSquare(int index, string itemName)
    {
        SpawnSquare(index, itemIcons[index]);
    }
}
