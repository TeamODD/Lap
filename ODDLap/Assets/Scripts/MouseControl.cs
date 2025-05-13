using UnityEngine;

public class MouseControl : MonoBehaviour
{
    void Start()
    {
        // 마우스 커서 숨기기
        Cursor.visible = false;

        // 마우스 커서를 게임 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }
}