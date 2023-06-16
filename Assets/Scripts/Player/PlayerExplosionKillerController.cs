using UnityEngine;

public class PlayerExplosionKillerController : MonoBehaviour
{
    public AudioClip[] AudioExplosion;
    private GameInterfaceController _score;
    private GameInterfaceController _interfaceController;

    void Awake()
    {
        _interfaceController = GameObject.FindWithTag(Tags.Interface)
            .GetComponent<GameInterfaceController>();
        _score = GameObject.FindWithTag(Tags.Interface)
            .GetComponent<GameInterfaceController>();
    }

    void Start()
    {
        if (AudioExplosion.Length != 0)
        {
            var randomAudio = Random.Range(0, AudioExplosion.Length);
            AudioController.AudioSource.PlayOneShot(AudioExplosion[randomAudio]);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Meteor))
        {
            if (_interfaceController.ActualScore == 10000)
            {
                _interfaceController.ShowTemporaryMessage("MAIS DIFÍCIL!", 5f, soundEffect: true);
                Generator.DecreaseGenerationTime(Tags.MeteorGenerator, 1f, 1.5f, 3.5f);
                SpecialGenerator.DecreaseRegenerationTime(15f);
            }
            else if (_interfaceController.ActualScore == 15000)
            {
                _interfaceController.ShowTemporaryMessage("WOOOW!", 5f, soundEffect: true);
                Generator.DecreaseGenerationTime(Tags.MeteorGenerator, 0.5f, 1f, 3f);
                SpecialGenerator.DecreaseRegenerationTime(4f);
            }
            else if (_interfaceController.ActualScore == 20000)
            {
                _interfaceController.ShowTemporaryMessage("MUITO MAIS DIFÍCIL!", 5f, soundEffect: true);
                Generator.DecreaseGenerationTime(Tags.MeteorGenerator, 0.5f, 1f, 1.5f);
                SpecialGenerator.DecreaseRegenerationTime(1f);
            }
            else
            {
                Generator.DecreaseGenerationTime(Tags.MeteorGenerator, 0.025f, 2.5f, 4f);
            }
            _score.UpdateScore(25);
        }
    }
}
