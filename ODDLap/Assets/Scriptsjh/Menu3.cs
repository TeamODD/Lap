using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu3 : MonoBehaviour
{
    public GameObject AdviceUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("BackGround3");



        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale = 0;
            AdviceUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            AdviceUI.SetActive(false);
        }
    }
}
