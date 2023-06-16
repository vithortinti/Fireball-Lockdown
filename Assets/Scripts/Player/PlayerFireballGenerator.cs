using UnityEngine;
using UnityEngine.UI;

public class PlayerFireballGenerator : MonoBehaviour
{
    public Slider Slider;
    [HideInInspector]
    public bool IsAbleToGenerate { get; private set; }
    public static float TimeToGenerate = 0.75f;
    [HideInInspector]
    public float TimeCounter = float.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        Slider.maxValue = TimeToGenerate;
    }

    // Update is called once per frame
    void Update()
    {
        LoadFireball();
    }

    public void Shoot(GameObject gameObject)
    {
        if (!GameStatus.IsPaused)
        {
            Instantiate(gameObject, transform.position, transform.rotation);
            TimeCounter = 0f;
        }
    }

    private void LoadFireball()
    {
        TimeCounter += Time.deltaTime;

        if (TimeCounter > TimeToGenerate)
        {
            IsAbleToGenerate = true;
        }
        else
        {
            IsAbleToGenerate = false;
        }

        LoadSlider();
    }

    private void LoadSlider()
    {
        if (TimeCounter < TimeToGenerate && !IsAbleToGenerate)
        {
            Slider.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            Slider.value = TimeCounter;
        }
        else
        {
            Slider.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    void OnDestroy()
    {
        try
        {
            Destroy(Slider.gameObject);
        }
        catch { }
    }
}
