using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject firstSelectedButton; // ← 설정창에서 기본 선택될 버튼
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
        Debug.Log("게임은 현재 일시정지 상태입니다.");
        settingsPanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        // 기존 선택 해제 후 다시 지정 (안정성을 위해 두 단계)
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
