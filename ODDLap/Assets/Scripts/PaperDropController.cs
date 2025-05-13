using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaperDropController : MonoBehaviour
{
    public RectTransform[] pages;        // ���� �̹�����
    public float shrinkDuration = 1f;    // ��ҿ� �ɸ��� �ð�
    public float fastMultiplier = 3f;    // �����̽��ٷ� ������ ���
 
    private int currentPage = 0;
    private bool isShrinking = false;
    private bool isSkipping = false;

    void Start()
    {
        // ��� ������ ��Ȱ��ȭ �� �ʱ�ȭ
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
        // Shrinking ���� �����̽��� ������ ������ �پ��
        if (isShrinking && Input.GetKeyDown(KeyCode.Space))
        {
            isSkipping = true;
        }
        // Shrinking ���� �� �����̽��� ������ ���� ������ ����
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
