using UnityEngine;

public class PlayerFireballController : MonoBehaviour
{
    private Vector2 _mouseClick;
    private Vector2 _direction;
    private Behavior _fireballBehavior;
    public GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        _fireballBehavior = GetComponent<Behavior>();
        _mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _direction = _fireballBehavior.GetDirection(_mouseClick);
        _fireballBehavior.RotateTo(_direction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(_mouseClick, transform.position) <= 0.1f)
        {
            _fireballBehavior.InstantiateNewGOAndDestroyActual(Explosion);
        }
        else
        {
            _fireballBehavior.MoveTo(_direction);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Ground))
        {
            Destroy(transform.gameObject);
        }
    }
}
