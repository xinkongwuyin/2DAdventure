using UnityEngine;
using UnityEngine.EventSystems;
public class Menu : MonoBehaviour
{
    public GameObject newGameButton;
    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton);
    
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
