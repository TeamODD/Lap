using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public LegacyDialogueManager dialogueManager; // ✅ 반드시 연결

    void Start()
    {
        fadeImage.raycastTarget = true;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.raycastTarget = false;
        dialogueManager.enabled = true; // ⏳ 페이드 인 후 대화 시작
    }
}
