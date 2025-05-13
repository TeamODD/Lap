using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LegacyDialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public bool isLeftSpeaker;     // true: 왼쪽 캐릭터, false: 오른쪽 캐릭터
        [TextArea]
        public string lineText;
    }

    public Text leftText;
    public Text rightText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;

    public DialogueLine[] dialogueLines;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        ShowNextLine();
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
                ShowNextLine();
            }
        }
    }

    void ShowNextLine()
    {
        if (index >= dialogueLines.Length)
        {
            leftText.text = "";
            rightText.text = "";
            return;
        }

        bool isLeft = dialogueLines[index].isLeftSpeaker;
        string line = dialogueLines[index].lineText;

        SetSpeakerVisibility(isLeft);
        StartCoroutine(TypeLine(line, isLeft));

        index++;
    }

    IEnumerator TypeLine(string line, bool isLeft)
    {
        isTyping = true;

        // Clear both
        leftText.text = "";
        rightText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            if (skipTyping)
            {
                if (isLeft) leftText.text = line;
                else rightText.text = line;
                break;
            }

            if (isLeft)
                leftText.text += line[i];
            else
                rightText.text += line[i];

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skipTyping = false;
    }

    void SetSpeakerVisibility(bool isLeftSpeaking)
    {
        SetAlpha(leftCharacterImage, isLeftSpeaking ? 1f : 0.5f);
        SetAlpha(rightCharacterImage, isLeftSpeaking ? 0.5f : 1f);

        leftText.gameObject.SetActive(isLeftSpeaking);
        rightText.gameObject.SetActive(!isLeftSpeaking);
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
