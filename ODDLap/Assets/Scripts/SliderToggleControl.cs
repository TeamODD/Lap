using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SliderToggleControl : MonoBehaviour
{
    public UnityEngine.UI.Button toggleButton;  // 슬라이더 조작 버튼
    public Slider targetSlider;

    private bool isSliderActive = false;
    private bool justActivated = false; // ← 키 입력 무시용 플래그

    void Start()
    {
        toggleButton.onClick.AddListener(ActivateSliderControl);
        SetSliderInteractable(false);
    }

    void Update()
    {
        if (justActivated)
        {
            // 슬라이더 활성화 직후 한 프레임은 키 입력 무시
            justActivated = false;
            return;
        }

        if (isSliderActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            Debug.Log("슬라이더 종료 → 버튼 포커스");
            SetSliderInteractable(false);
            EventSystem.current.SetSelectedGameObject(toggleButton.gameObject);
        }
    }

    void ActivateSliderControl()
    {
        Debug.Log("슬라이더 활성화 시도");
        SetSliderInteractable(true);
        justActivated = true; // ← 바로 다음 프레임은 키 입력 무시
        StartCoroutine(FocusSliderNextFrame());
    }

    IEnumerator FocusSliderNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(targetSlider.gameObject);
        Debug.Log("슬라이더에 포커스 설정 완료");
    }

    void SetSliderInteractable(bool interactable)
    {
        targetSlider.interactable = interactable;
        isSliderActive = interactable;
        Debug.Log($"슬라이더 Interactable: {interactable}");
    }
}
