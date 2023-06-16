using UnityEngine;

public class AntiAircraftController : MonoBehaviour
{
    private float MaxMissileAngle = 90;
    private float MinMissileAngle = -90;
    public Transform Cannon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStatus.IsPaused) // Caso o jogo esteja pausado, irá travar a movimentação dos canhões
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            LookAtPoint(mousePosition, MinMissileAngle, MaxMissileAngle);
        }
    }

    public void LookAtPoint(Vector3 position, float minRotation = 180.0f, float maxRotation = -180f)
    {
        Vector2 direction = position - Cannon.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float maxAngle = Mathf.Clamp(-angle, minRotation, maxRotation);
        Quaternion rotation = Quaternion.Euler(0f, 0f, maxAngle);
        Cannon.rotation = rotation;
    }
}
