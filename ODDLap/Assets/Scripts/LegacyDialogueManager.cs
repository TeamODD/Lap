using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TMPFlexibleDialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea]
        public string text;
        public bool isLeftSpeaker; // 체크박스
    }

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;

    public float typingSpeed = 0.05f;

    private int currentIndex = 0;
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
        if (currentIndex >= dialogueLines.Count)
        {
            leftText.text = "";
            rightText.text = "";
            return;
        }

        var line = dialogueLines[currentIndex];
        SetSpeakerVisibility(line.isLeftSpeaker);
        StartCoroutine(TypeLine(line.text, line.isLeftSpeaker));

        currentIndex++;
    }

    IEnumerator TypeLine(string text, bool isLeft)
    {
        isTyping = true;

        if (isLeft)
        {
            leftText.text = "";
            rightText.text = "";
        }
        else
        {
            rightText.text = "";
            leftText.text = "";
        }

        for (int i = 0; i < text.Length; i++)
        {
            if (skipTyping)
            {
                if (isLeft) leftText.text = text;
                else rightText.text = text;
                break;
            }

            if (isLeft)
                leftText.text += text[i];
            else
                rightText.text += text[i];

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
