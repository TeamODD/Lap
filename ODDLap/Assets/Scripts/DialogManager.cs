using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogLine
    {
        public string speaker; // "Left" or "Right"
        [TextArea(2, 5)] public string sentence;
        public GameObject[] objectsToEnable;
        public GameObject[] objectsToDisable;
    }

    [Header("UI Elements")]
    public TextMeshProUGUI dialogText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;

    [Header("Dialog Settings")]
    public List<DialogLine> dialogLines;
    public float typingSpeed = 0.05f;

    [Header("Fade Out Settings")]
    public Image fadeOutImage;
    public float fadeDuration = 1.5f;
    public string nextSceneName;

    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        StartCoroutine(PlayDialog());
    }

    IEnumerator PlayDialog()
    {
        while (currentLine < dialogLines.Count)
        {
            DialogLine line = dialogLines[currentLine];

            // 캐릭터 투명도 조절
            SetCharacterAlpha(line.speaker == "Left");

            // 오브젝트 On/Off
            HandleObjectToggle(line);

            // 텍스트 출력
            yield return StartCoroutine(TypeSentence(line.sentence));

            // Space 키 대기
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            currentLine++;
        }

        // 페이드 아웃 후 씬 전환
        yield return StartCoroutine(FadeOutAndLoadScene());
    }

    void SetCharacterAlpha(bool isLeftSpeaking)
    {
        Color opaque = new Color(1, 1, 1, 1f);
        Color transparent = new Color(1, 1, 1, 0.3f);

        leftCharacterImage.color = isLeftSpeaking ? opaque : transparent;
        rightCharacterImage.color = isLeftSpeaking ? transparent : opaque;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void HandleObjectToggle(DialogLine line)
    {
        foreach (var obj in line.objectsToEnable) if (obj != null) obj.SetActive(true);
        foreach (var obj in line.objectsToDisable) if (obj != null) obj.SetActive(false);
    }

    IEnumerator FadeOutAndLoadScene()
    {
        fadeOutImage.gameObject.SetActive(true);
        Color color = fadeOutImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeOutImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
