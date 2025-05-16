using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject firstSelectedButton; // �� ����â���� �⺻ ���õ� ��ư
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Debug.Log("������ ���� �Ͻ����� �����Դϴ�.");
        settingsPanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        // ���� ���� ���� �� �ٽ� ���� (�������� ���� �� �ܰ�)
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
}
