using UnityEngine;
using UnityEngine.EventSystems;

public class InitialButtonSelector : MonoBehaviour
{
    public GameObject firstSelectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
