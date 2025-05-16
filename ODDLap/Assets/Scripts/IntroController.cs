using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("Dialogue")]
    public TextMeshProUGUI dialogueText;         // ✅ Text → TextMeshProUGUI
    [TextArea] public string[] dialogueLines;
    public float typingSpeed = 0.05f;

    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    [Header("Objects to Activate")]
    public GameObject[] objectsToActivate;
    private int objectIndex = 0;

    [Header("Scene to Load")]
    public string sceneToLoad;

    void Start()
    {
        dialogueText.text = "";
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                StartCoroutine(ActivateNextObject());
            }
        }
    }

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

    IEnumerator ActivateNextObject()
    {
        if (dialogueIndex < objectsToActivate.Length)
        {
            GameObject obj = objectsToActivate[dialogueIndex];
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log(obj.name + " 오브젝트가 활성화되었습니다!");
            }
        }

        dialogueIndex++;
        if (dialogueIndex < dialogueLines.Length)
        {
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("모든 대사가 끝났습니다.");
            SceneManager.LoadScene(sceneToLoad);
        }

        yield return null;
    }
}
