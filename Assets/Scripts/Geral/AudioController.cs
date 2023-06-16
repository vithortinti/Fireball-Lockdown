using UnityEngine;

public class AudioController : MonoBehaviour
{
    [HideInInspector]
    public static AudioSource AudioSource;

    // Start is called before the first frame update
    void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }
}
