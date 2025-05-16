using System.Collections;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public AudioClip clr;

    public bool clear = false;
    public string sceneToLoad;
    public Character targetScript;
    public GameObject DeathPanel;
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
                StartCoroutine(ClearCoroutine());
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
        targetScript.inputQueue.Clear();
        targetScript.enabled = false;
        audioSource.PlayOneShot(die);
        DeathPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f); // 1�� ���� ���� ����
                                             //���� ����
        targetScript.enabled = true;
        DeathPanel.gameObject.SetActive(false);
        character.inputQueue.Clear();
        initCount();
        restartScene.Restart();
    }
    private IEnumerator ClearCoroutine()
    {
        targetScript.inputQueue.Clear();
        targetScript.enabled = false;
        audioSource.PlayOneShot(clr);
        yield return new WaitForSeconds(2f); // 1�� ���� ���� ����b
        targetScript.enabled = true;
        SceneManager.LoadScene(sceneToLoad);
        //���� ����
    }
}
