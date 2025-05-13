using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("Dialogue")]
    public Text dialogueText;               // 대사 텍스트
    [TextArea] public string[] dialogueLines; // 대사 내용 배열
    public float typingSpeed = 0.05f;       // 타이핑 속도

    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    [Header("Objects to Activate")]
    public GameObject[] objectsToActivate; // 여러 개의 오브젝트를 배열로 설정
    private int objectIndex = 0;           // 활성화할 오브젝트의 인덱스

    [Header("Scene to Load")]
    public string sceneToLoad;             // 전환할 씬의 이름

    void Start()
    {
        dialogueText.text = "";
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        // 스페이스바를 눌러서 대사 넘어가고 오브젝트 활성화
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 타이핑이 끝나지 않았다면 스페이스바로 강제 종료
                skipTyping = true;
            }
            else
            {
                // 대사 끝나면 해당 오브젝트 활성화
                StartCoroutine(ActivateNextObject());
            }
        }
    }

    // 대사 한 줄씩 타이핑하는 함수
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

    // 대사가 끝난 후 스페이스바를 눌러서 오브젝트를 활성화
    IEnumerator ActivateNextObject()
    {
        // 현재 대사가 끝난 후 해당 오브젝트를 활성화
        if (dialogueIndex < objectsToActivate.Length)
        {
            GameObject obj = objectsToActivate[dialogueIndex];
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log(obj.name + " 오브젝트가 활성화되었습니다!");
            }
        }

        // 대사 끝난 후, 스페이스바를 눌러서 다음 대사로 넘어감
        dialogueIndex++;
        if (dialogueIndex < dialogueLines.Length)
        {
            dialogueText.text = ""; // 텍스트를 비우고, 다음 대사로
            StartCoroutine(TypeLine());
        }
        else
        {
            // 모든 대사가 끝난 후 씬 전환
            Debug.Log("모든 대사가 끝났습니다.");
            // 씬 전환
            SceneManager.LoadScene(sceneToLoad);
        }

        yield return null;
    }
}
