using System.Collections;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class MovingCount : MonoBehaviour
{
    private RestartScene restartScene; // �� �����
    private Character character; // �� �����

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
            Debug.Log("���� �̵�Ƚ�� = 0");
            //Ŭ��� �ƴ϶�� ����ŸƮ
            if (clear == false)
            {
                Die();
            }
            else
            {
                //Ŭ����� ������ Ŭ���� �뷡
            }
        }
        text.text = remainCount.ToString();
    }

    public void Die()
    {
        //�������
        StartCoroutine(ResetInvert());
    }
    private IEnumerator ResetInvert()
    {
        audioSource.PlayOneShot(die);
        yield return new WaitForSeconds(2f); // 1�� ���� ���� ����
        //���� ����
        character.inputQueue.Clear();
        initCount();
        restartScene.Restart();
    }
}
