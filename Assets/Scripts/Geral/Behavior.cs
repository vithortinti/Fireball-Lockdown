using UnityEngine;

public class Behavior : MonoBehaviour
{
    public float Speed;
    public bool RemovePhysics = true;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = RemovePhysics; // retirando a física
    }

    public void MoveTo(Vector2 direction)
    {
        _rigidbody.MovePosition(_rigidbody.position + direction.normalized * Speed * Time.deltaTime);
    }

    public void Stop()
    {
        _rigidbody.position = transform.position;
    }

    public void RotateTo(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void InstantiateNewGOAndDestroyActual(GameObject newTransform)
    {
        Instantiate(newTransform, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }

    public Vector2 GetDirection(Vector2 direction)
    {
        return direction - ((Vector2) transform.position);
    }
}
