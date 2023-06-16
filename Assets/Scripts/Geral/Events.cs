using UnityEngine;

public class Events : MonoBehaviour
{
    internal void DestroyTheObjectAtTheEndOfTheAnimation()
    {
        Destroy(transform.gameObject);
    }

    internal void RemoveCollison()
    {
        Destroy(transform.GetComponent<Collider2D>());
    }
}
