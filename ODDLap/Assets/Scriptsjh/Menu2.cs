using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2 : MonoBehaviour
{
    public Character targetScript; // ������Ű���� ��ũ��Ʈ
    public GameObject AdviceUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            targetScript.enabled = false;
            Time.timeScale = 0;
            AdviceUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            targetScript.enabled = true;
            Time.timeScale = 1;
            AdviceUI.SetActive(false);
        }
    }
}
