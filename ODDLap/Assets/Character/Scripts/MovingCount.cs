using System.Collections;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class MovingCount : MonoBehaviour
{
    private RestartScene restartScene; // 씬 재시작
    private Character character; // 씬 재시작

    public Text text;
    [SerializeField] private int totalCount = 30;
    private int remainCount;

    private AudioSource audioSource;
    public AudioClip die;

    public bool clear = false;

    void Awake()
    {
        clear = false;
        restartScene = FindAnyObjectByType<RestartScene>();
        character = FindAnyObjectByType<Character>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        initCount();
    }

    public void initCount()
    {
        remainCount = totalCount;
        SetText();
    }
    public void MoveCounting()
    {
        remainCount--;
        SetText();
    }   
    public void isClear()
    {
        clear = true;
    }

    public void SetText()
    {
        if (remainCount == 0)
        {
            Debug.Log("남은 이동횟수 = 0");
            //클리어가 아니라면 리스타트
            if (clear == false)
            {
                Die();
            }
            else
            {
                //클리어시 다음씬 클리어 노래
            }
        }
        text.text = remainCount.ToString();
    }

    public void Die()
    {
        //색상반전
        StartCoroutine(ResetInvert());
    }
    private IEnumerator ResetInvert()
    {
        audioSource.PlayOneShot(die);
        yield return new WaitForSeconds(2f); // 1초 동안 색상 반전
        //반전 종료
        character.inputQueue.Clear();
        initCount();
        restartScene.Restart();
    }
}
