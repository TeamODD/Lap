using UnityEngine;
using UnityEngine.TextCore.Text;

public class NextImage : MonoBehaviour
{
    public GameObject NextIMG;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextIMG.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
    }


}
