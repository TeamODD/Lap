using UnityEngine;

public class Btncontroll : MonoBehaviour
{
    public GameObject PreUI;
    public GameObject NextUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void nextUI()
    {
        PreUI.SetActive(false);
        NextUI.SetActive(true);



    }
}
