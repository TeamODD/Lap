using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LegacyDialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea]
        public string text;
        public bool isLeftSpeaker;

        public GameObject[] offObjects;

        public GameObject leftImageObject;
        public GameObject rightImageObject;
    }

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    public GameObject defaultLeftImage;
    public GameObject defaultRightImage;

    public Image fadeImage;
    public float fadeDuration = 1f;

    public float typingSpeed = 0.05f;
    public string nextSceneName = "NextScene";

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool waitingForSceneChange = false;

    void Start()
    {
        StartCoroutine(FadeInThenStartDialogue());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else if (waitingForSceneChange)
            {
                StartCoroutine(FadeAndLoadScene());
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    IEnumerator FadeInThenStartDialogue()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1);
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            fadeImage.raycastTarget = false;
        }

        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentIndex >= dialogueLines.Count)
        {
            waitingForSceneChange = true;
            return;
        }

        var line = dialogueLines[currentIndex];

        // ✅ offObjects만 처리
        if (line.offObjects != null)
        {
            foreach (var obj in line.offObjects)
                if (obj != null) obj.SetActive(false);
        }

        if (defaultLeftImage != null) defaultLeftImage.SetActive(false);
        if (line.leftImageObject != null) line.leftImageObject.SetActive(true);

        if (defaultRightImage != null) defaultRightImage.SetActive(false);
        if (line.rightImageObject != null) line.rightImageObject.SetActive(true);

        SetSpeakerVisibility(line.isLeftSpeaker);
        StartCoroutine(TypeLine(line.text, line.isLeftSpeaker));

        currentIndex++;
    }

    IEnumerator TypeLine(string text, bool isLeft)
    {
        isTyping = true;
        skipTyping = false;

        leftText.text = "";
        rightText.text = "";

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
    }

    void SetAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void SetSpeakerVisibility(bool isLeftSpeaking)
    {
        leftText.gameObject.SetActive(isLeftSpeaking);
        rightText.gameObject.SetActive(!isLeftSpeaking);
    }


    IEnumerator FadeAndLoadScene()
    {
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = true;

            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
