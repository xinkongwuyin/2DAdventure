using UnityEngine;

public class SavePoint : MonoBehaviour, IIteractable
{
    [Header("广播")]
    public VoidEventsSQ SaveGameEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;

    public Sprite activeSprite; // The sprite to show when the save point is active 
    public Sprite inactiveSprite; // The sprite to show when the save point is inactive

    public bool isDone;

    public GameObject lightObj;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? activeSprite : inactiveSprite;
        lightObj.SetActive(isDone); // Show light if not done, hide if done
    }



    public void triggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = activeSprite;
            lightObj.SetActive(true); // Show light when activated
                                       // You can also add additional logic here, such as saving the game state or playing a sound effect

            //保存数据
            SaveGameEvent.RaiseEvent(); // Raise the event to trigger the save/load logic in your game manager or other relevant scripts
            this.gameObject.tag = "Untagged";
        }
    }

    
}
