using UnityEngine;

public class Chest : MonoBehaviour, IIteractable
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public Sprite openSprite; // The sprite to show when the chest is open
    public Sprite closeSprite;
    public bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone? openSprite : closeSprite; // Set the initial sprite based on isDone
    }

    public void triggerAction()
    {
        Debug.Log("open!");
        if(!isDone) // Only open the chest if it's not already done
        {
          
           
            openChest(); // Call the method to handle opening the chest (e.g., give rewards, play sound, etc.)
        }
    }

    private void openChest()
    {
        spriteRenderer.sprite = openSprite; // Change the sprite to the open chest (you can also add animations or sound effects here)
        isDone = true; // Mark the chest as opened
        this.gameObject.tag="Untagged"; // Change the tag to prevent further interactions (optional, depending on your game logic)
    }
}
