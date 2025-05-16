using UnityEngine;

public class stop : MonoBehaviour
{
    private GameObject targetUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetUI = GameObject.Find("Canvas");

        targetUI.SetActive(false);
    }
}
