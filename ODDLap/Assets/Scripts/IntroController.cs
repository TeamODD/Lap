using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("Dialogue")]
    public Text dialogueText;               // ��� �ؽ�Ʈ
    [TextArea] public string[] dialogueLines; // ��� ���� �迭
    public float typingSpeed = 0.05f;       // Ÿ���� �ӵ�

    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    [Header("Objects to Activate")]
    public GameObject[] objectsToActivate; // ���� ���� ������Ʈ�� �迭�� ����
    private int objectIndex = 0;           // Ȱ��ȭ�� ������Ʈ�� �ε���

    [Header("Scene to Load")]
    public string sceneToLoad;             // ��ȯ�� ���� �̸�

    void Start()
    {
        dialogueText.text = "";
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        // �����̽��ٸ� ������ ��� �Ѿ�� ������Ʈ Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Ÿ������ ������ �ʾҴٸ� �����̽��ٷ� ���� ����
                skipTyping = true;
            }
            else
            {
                // ��� ������ �ش� ������Ʈ Ȱ��ȭ
                StartCoroutine(ActivateNextObject());
            }
        }
    }

    // ��� �� �پ� Ÿ�����ϴ� �Լ�
    IEnumerator TypeLine()
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        string currentLine = dialogueLines[dialogueIndex];
        for (int i = 0; i < currentLine.Length; i++)
        {
            if (skipTyping)
            {
                dialogueText.text = currentLine;
                break;
            }

            dialogueText.text += currentLine[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // ��簡 ���� �� �����̽��ٸ� ������ ������Ʈ�� Ȱ��ȭ
    IEnumerator ActivateNextObject()
    {
        // ���� ��簡 ���� �� �ش� ������Ʈ�� Ȱ��ȭ
        if (dialogueIndex < objectsToActivate.Length)
        {
            GameObject obj = objectsToActivate[dialogueIndex];
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log(obj.name + " ������Ʈ�� Ȱ��ȭ�Ǿ����ϴ�!");
            }
        }

        // ��� ���� ��, �����̽��ٸ� ������ ���� ���� �Ѿ
        dialogueIndex++;
        if (dialogueIndex < dialogueLines.Length)
        {
            dialogueText.text = ""; // �ؽ�Ʈ�� ����, ���� ����
            StartCoroutine(TypeLine());
        }
        else
        {
            // ��� ��簡 ���� �� �� ��ȯ
            Debug.Log("��� ��簡 �������ϴ�.");
            // �� ��ȯ
            SceneManager.LoadScene(sceneToLoad);
        }

        yield return null;
    }
}
