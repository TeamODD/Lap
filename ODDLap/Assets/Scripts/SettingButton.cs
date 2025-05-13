using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public void OnMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnResumeButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}
