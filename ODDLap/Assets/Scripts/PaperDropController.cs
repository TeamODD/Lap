using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaperDropController : MonoBehaviour
{
    public RectTransform[] pages;        // 종이 이미지들
    public float shrinkDuration = 1f;    // 축소에 걸리는 시간
    public float fastMultiplier = 3f;    // 스페이스바로 빠르게 축소
 
    private int currentPage = 0;
    private bool isShrinking = false;
    private bool isSkipping = false;

    void Start()
    {
        // 모든 페이지 비활성화 및 초기화
        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
            page.anchoredPosition = Vector2.zero;
            page.localScale = Vector3.one;
        }

        ShowNextPage();
    }

    void Update()
    {
        // Shrinking 도중 스페이스바 누르면 빠르게 줄어듦
        if (isShrinking && Input.GetKeyDown(KeyCode.Space))
        {
            isSkipping = true;
        }
        // Shrinking 끝난 후 스페이스바 누르면 다음 페이지 등장
        else if (!isShrinking && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextPage();
        }
    }

    void ShowNextPage()
    {
        if (currentPage >= pages.Length) return;

        RectTransform page = pages[currentPage];
        page.localScale = Vector3.one;
        page.anchoredPosition = Vector2.zero;
        page.gameObject.SetActive(true);

        StartCoroutine(ShrinkPage(page));
        currentPage++;
    }

    IEnumerator ShrinkPage(RectTransform page)
    {
        isShrinking = true;
        isSkipping = false;

        float time = 0f;

        while (time < 1f)
        {
            float speed = isSkipping ? fastMultiplier : 1f;
            time += Time.deltaTime / shrinkDuration * speed;

            page.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);

            yield return null;
        }

        page.localScale = Vector3.zero;
        isShrinking = false;
    }
}
