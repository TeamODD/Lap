using UnityEngine;
using UnityEngine.UI;

public class UIAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip sliderTickSound;

    private float lastSliderValue = -1f; // 슬라이더 초기값 체크용
    public float sliderThreshold = 0.05f; // 이 값 이상 변화했을 때만 소리 재생
    public Slider bgmSlider;

    void Start()
    {
        // 초기값을 현재 볼륨에서 가져옴
        if (SoundManager.Instance != null && SoundManager.Instance.musicAudioSource != null)
        {
            bgmSlider.value = SoundManager.Instance.musicAudioSource.volume;
        }

        // 슬라이더 변경 시 볼륨 변경
        bgmSlider.onValueChanged.AddListener(SetVolume);
    }
    public void SetVolume(float value)
    {
        if (SoundManager.Instance != null && SoundManager.Instance.musicAudioSource != null)
        {
            SoundManager.Instance.musicAudioSource.volume = value;
        }
    }
    // 버튼에 연결할 함수
    public void PlayButtonClickSound()
    {
        if (audioSource && buttonClickSound)
            audioSource.PlayOneShot(buttonClickSound);
    }


    // 슬라이더에 연결할 함수
    public void OnSliderChanged(float value)
    {
        if (audioSource)
        {
            audioSource.volume = value;
        }

        if (Mathf.Abs(value - lastSliderValue) >= sliderThreshold)
        {
            lastSliderValue = value;

            if (audioSource && sliderTickSound)
                audioSource.PlayOneShot(sliderTickSound);
        }
    }

}
