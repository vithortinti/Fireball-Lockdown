using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public AudioClip HitAudio;
    public GameObject Explosion;
    private Vector2 _direction;
    private Behavior _meteorBehavior;
    private Animation _meteorAnimation;
    private bool _isMoving = true;
    private BuildingsController _buildingsController;
    private GameInterfaceController _interfaceController;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
        _buildingsController = GameObject.FindWithTag(Tags.Buildings).GetComponent<BuildingsController>();
        _interfaceController = GameObject.FindWithTag(Tags.Interface).GetComponent<GameInterfaceController>();
        _meteorBehavior = GetComponent<Behavior>();
        _meteorAnimation = GetComponent<Animation>();
        var targetTransform = GetTargets();
        _direction = _meteorBehavior.GetDirection(targetTransform.position);
        _meteorBehavior.RotateTo(_direction);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (_isMoving)
            _meteorBehavior.MoveTo(_direction);
        else
            _meteorBehavior.Stop();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PlayerBuilding))
        {
            DestroyAndDoExplosionAnimation();
            _buildingsController.DestroyBuilding();
            if (_buildingsController.IsAllBuildingsDestroyed())
            {
                _interfaceController.ShowGameOverInterface(true, true);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag(Tags.Ground))
        {
            DestroyAndDoExplosionAnimation();
        }
        else if (collision.CompareTag(Tags.PlayerAirCraft))
        {
            DestroyAndDoExplosionAnimation();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag(Tags.PlayerExplosion))
        {
            var positionToExplode = transform.GetChild(0);
            Instantiate(Explosion, positionToExplode.position, positionToExplode.rotation);
            Destroy(gameObject);
            AudioController.AudioSource.PlayOneShot(HitAudio);
        }
    }

    Transform GetTargets()
    {
        var playerTargets = GameObject.FindGameObjectsWithTag(Tags.PlayerAirCraft);
        var playerBuildingTargets = GameObject.FindGameObjectsWithTag(Tags.PlayerBuilding);
        var playerGOs = playerTargets.Concat(playerBuildingTargets).ToArray();
        var targetObjectIndex = Random.Range(0, playerGOs.Length);
        return playerGOs[targetObjectIndex].transform;
    }

    void DestroyAndDoExplosionAnimation()
    {
        _isMoving = false;
        var positionToExplode = transform.GetChild(0).position;
        AudioController.AudioSource.PlayOneShot(HitAudio);
        _meteorAnimation.Explode(true, positionToExplode);
    }
}
