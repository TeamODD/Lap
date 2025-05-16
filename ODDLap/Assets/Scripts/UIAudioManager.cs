using UnityEngine;
using UnityEngine.UI;

public class UIAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip sliderTickSound;

    private float lastSliderValue = -1f; // �����̴� �ʱⰪ üũ��
    public float sliderThreshold = 0.05f; // �� �� �̻� ��ȭ���� ���� �Ҹ� ���

    // ��ư�� ������ �Լ�
    public void PlayButtonClickSound()
    {
        if (audioSource && buttonClickSound)
            audioSource.PlayOneShot(buttonClickSound);
    }


    // �����̴��� ������ �Լ�
    public void OnSliderChanged(float value)
    {
        if (Mathf.Abs(value - lastSliderValue) >= sliderThreshold)
        {
            lastSliderValue = value;

            if (audioSource && sliderTickSound)
                audioSource.PlayOneShot(sliderTickSound);
        }
    }

}
