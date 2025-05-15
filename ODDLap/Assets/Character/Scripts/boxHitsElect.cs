using UnityEngine;

public class boxHitsElect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("box"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
