using UnityEngine;

public class MouseControl : MonoBehaviour
{
    void Start()
    {
        // ���콺 Ŀ�� �����
        Cursor.visible = false;

        // ���콺 Ŀ���� ���� ȭ�� �߾ӿ� ����
        Cursor.lockState = CursorLockMode.Locked;
    }
}