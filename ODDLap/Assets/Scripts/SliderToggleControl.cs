using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SliderToggleControl : MonoBehaviour
{
    public UnityEngine.UI.Button toggleButton;  // �����̴� ���� ��ư
    public Slider targetSlider;

    private bool isSliderActive = false;
    private bool justActivated = false; // �� Ű �Է� ���ÿ� �÷���

    void Start()
    {
        toggleButton.onClick.AddListener(ActivateSliderControl);
        SetSliderInteractable(false);
    }

    void Update()
    {
        if (justActivated)
        {
            // �����̴� Ȱ��ȭ ���� �� �������� Ű �Է� ����
            justActivated = false;
            return;
        }

        if (isSliderActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            Debug.Log("�����̴� ���� �� ��ư ��Ŀ��");
            SetSliderInteractable(false);
            EventSystem.current.SetSelectedGameObject(toggleButton.gameObject);
        }
    }

    void ActivateSliderControl()
    {
        Debug.Log("�����̴� Ȱ��ȭ �õ�");
        SetSliderInteractable(true);
        justActivated = true; // �� �ٷ� ���� �������� Ű �Է� ����
        StartCoroutine(FocusSliderNextFrame());
    }

    IEnumerator FocusSliderNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(targetSlider.gameObject);
        Debug.Log("�����̴��� ��Ŀ�� ���� �Ϸ�");
    }

    void SetSliderInteractable(bool interactable)
    {
        targetSlider.interactable = interactable;
        isSliderActive = interactable;
        Debug.Log($"�����̴� Interactable: {interactable}");
    }
}
