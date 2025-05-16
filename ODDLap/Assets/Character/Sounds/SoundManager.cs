using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // static 변수
    public AudioSource musicAudioSource;

    void Awake()
    {
        // 인스턴스가 이미 존재하면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // 인스턴스 설정
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (musicAudioSource == null)
        {
            // AudioSource가 없으면 생성
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.loop = true;
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}