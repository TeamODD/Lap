using UnityEngine;
using UnityEngine.UI;

public class UIAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip sliderTickSound;

    private float lastSliderValue = -1f; // �����̴� �ʱⰪ üũ��
    public float sliderThreshold = 0.05f; // �� �� �̻� ��ȭ���� ���� �Ҹ� ���
    public Slider bgmSlider;

    void Start()
    {
        // �ʱⰪ�� ���� �������� ������
        if (SoundManager.Instance != null && SoundManager.Instance.musicAudioSource != null)
        {
            bgmSlider.value = SoundManager.Instance.musicAudioSource.volume;
        }

        // �����̴� ���� �� ���� ����
        bgmSlider.onValueChanged.AddListener(SetVolume);
    }
    public void SetVolume(float value)
    {
        if (SoundManager.Instance != null && SoundManager.Instance.musicAudioSource != null)
        {
            SoundManager.Instance.musicAudioSource.volume = value;
        }
    }
    // ��ư�� ������ �Լ�
    public void PlayButtonClickSound()
    {
        if (audioSource && buttonClickSound)
            audioSource.PlayOneShot(buttonClickSound);
    }


    // �����̴��� ������ �Լ�
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
