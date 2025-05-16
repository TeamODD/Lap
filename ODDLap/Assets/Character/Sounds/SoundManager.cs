using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // static ����
    public AudioSource musicAudioSource;

    void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϸ� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // �ν��Ͻ� ����
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı� ����
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (musicAudioSource == null)
        {
            // AudioSource�� ������ ����
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.loop = true;
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}