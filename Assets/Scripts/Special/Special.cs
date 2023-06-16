using System;
using UnityEngine;
using UnityEngine.UI;

public class Special : MonoBehaviour
{
    public static bool IsActive { get; private set; }
    public GameObject PlayerExplosionKiller;
    public GameObject PlayerFireball;
    public AudioClip SpecialAudio;
    public static float LifeTime { get; private set; }
    public SpecialCardsInterfaceController _specialCardsController;
    public enum Specials
    {
        UnlimitedShots,
        Boom,
        FastShots
    }
    public Specials LastSpecial;
    public static GameObject LastSpecialObject;
    private GameInterfaceController _gameInterfaceController;
    private float _playerFireballGeneratorDefaultTimeToGenerate;
    private float _playerFireballControllerDefaultSpeed;
    private Vector2 _direction;
    private Behavior _specialBehavior;

    // Start is called before the first frame update
    void Start()
    {
        _gameInterfaceController = GameObject.FindWithTag(Tags.Interface).GetComponent<GameInterfaceController>();
        _specialBehavior = GetComponent<Behavior>();

        if (transform.position.x < 0)
            _direction = Vector2.right;
        else
            _direction = Vector2.left;

        _playerFireballGeneratorDefaultTimeToGenerate = PlayerFireballGenerator.TimeToGenerate;
        _playerFireballControllerDefaultSpeed = PlayerFireball.GetComponent<Behavior>().Speed;

        _specialCardsController = GameObject.FindWithTag(Tags.CardsInterface)
            .GetComponent<SpecialCardsInterfaceController>();

        Destroy(gameObject, 45f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _specialBehavior.MoveTo(_direction);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PlayerExplosion))
        {
            LastSpecialObject = gameObject;
            gameObject.SetActive(false);
            AudioController.AudioSource.PlayOneShot(SpecialAudio);
            _specialCardsController.Call();
        }
    }

    void OnDestroy()
    {
        PlayerFireballGenerator.TimeToGenerate = _playerFireballGeneratorDefaultTimeToGenerate;
        PlayerFireball.transform.GetComponent<Behavior>().Speed = _playerFireballControllerDefaultSpeed;
        _gameInterfaceController.Odd = 1;
        _gameInterfaceController.OddText.text = string.Empty;
    }
}
