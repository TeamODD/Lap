using UnityEngine;

public class disablessss : MonoBehaviour
{
    public GameObject targetUI;
    // Start is called once
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetUI.SetActive(true);
            transform.root.gameObject.SetActive(false);
        }
    }
}
