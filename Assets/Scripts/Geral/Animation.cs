using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    public void Explode(bool b, Vector2 localToExplode)
    {
        if (localToExplode != null)
        {
            transform.position = (Vector3) localToExplode;
        }
        _animator.SetBool("Explosion", b);
    }
}
