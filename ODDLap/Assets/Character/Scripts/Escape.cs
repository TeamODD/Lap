using UnityEngine;

public class Escape : MonoBehaviour
{
    private bool key = false;

    public void GetKey()
    {
        key = true;
    }

    public bool HasKey()
    {
        if (key) return true;
        else return false;
    }
}
