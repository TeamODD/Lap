using UnityEngine;
using System.Collections;

public class boxHitsElect : MonoBehaviour
{
    private Animator animForBox1;
    private Animator animForBox2;
    private Animator animForBox3;

    private void Awake()
    {
        animForBox1 = GameObject.Find("Box1_1").GetComponent<Animator>();
        animForBox2 = GameObject.Find("Box1_1 (1)").GetComponent<Animator>();
        animForBox3 = GameObject.Find("Box1_2").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("box"))
        {
            StartCoroutine(boxBreak(other));
        }
    }
    private IEnumerator boxBreak(Collider2D other)
    {
        if(other.gameObject.name == "Box1_1")
        {
            animForBox1.SetTrigger("Break");
        }
        else if (other.gameObject.name == "Box1_1 (1)")
        {
            animForBox2.SetTrigger("Break");
        }
        else if (other.gameObject.name == "Box1_2")
        {
            animForBox3.SetTrigger("Break");
        }
        yield return new WaitForSeconds(0.45f);
        other.gameObject.SetActive(false);
    }
}