using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroDialogue : MonoBehaviour
{
    public Text dialogueText;
    [TextArea] public string[] introLines;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // ���� Ÿ���� ���̸� �� ��ü ���� ��� ���
                skipTyping = true;
            }
            else
            {
                // ���� ���� �̵�
                index++;
                if (index < introLines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    EndIntro(); // ��Ʈ�� ���� �� ���� �̺�Ʈ ó��
                }
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        string currentLine = introLines[index];

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
        skipTyping = false;
    }

    void EndIntro()
    {
        Debug.Log("��Ʈ�� ��!");
        // ��: SceneManager.LoadScene("NextScene");
    }
}
