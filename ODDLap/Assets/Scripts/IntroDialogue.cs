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
                // 현재 타이핑 중이면 → 전체 문장 즉시 출력
                skipTyping = true;
            }
            else
            {
                // 다음 대사로 이동
                index++;
                if (index < introLines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    EndIntro(); // 인트로 종료 후 다음 이벤트 처리
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
        Debug.Log("인트로 끝!");
        // 예: SceneManager.LoadScene("NextScene");
    }
}
