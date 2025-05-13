using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("Intro");
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnSettingButton()
    {
        SceneManager.LoadScene("SettingScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
