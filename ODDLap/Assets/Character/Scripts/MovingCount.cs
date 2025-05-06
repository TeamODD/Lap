using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class MovingCount : MonoBehaviour
{
    public Text text;
    [SerializeField] private int totalCount = 30;
    private int remainCount;

    private void Start()
    {
        initCount();
    }

    public void initCount()
    {
        remainCount = totalCount;
        SetText();
    }
    public void MoveCounting()
    {
        remainCount--;
        SetText();
    }   

    public void SetText()
    {
        if (remainCount == 0)
        {
            //die
            Debug.Log("이동횟수 부족");
        }
        text.text = remainCount.ToString();
    }
}
