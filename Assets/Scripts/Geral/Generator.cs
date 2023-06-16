using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject ToGenerate;
    public float MinToGenerate;
    public float MaxToGenerate;
    private float TimeCounter;
    private float TimeToGenerate;

    void Awake()
    {
        TimeToGenerate = GetRandomTime();
    }

    private float GetRandomTime()
    {
        return Random.Range(MinToGenerate, MaxToGenerate);
    }

    protected bool Generate()
    {
        TimeCounter += Time.deltaTime;

        if (TimeCounter > TimeToGenerate)
        {
            Instantiate(ToGenerate, transform.position, transform.rotation);
            TimeCounter = 0;
            TimeToGenerate = GetRandomTime();
            return true;
        }
        return false;
    }

    public static void DecreaseGenerationTime(string tag, float decreaseTime, float min = 1f, float max = 5f)
    {
        GameObject[] _generators = GameObject.FindGameObjectsWithTag(tag);
        foreach (var g in _generators)
        {
            var generator = g.GetComponent<Generator>();
            generator.DecreaseTime(decreaseTime, min, max);
        }
    }

    private void DecreaseTime(float decreaseTime, float min = 1f, float max = 5f)
    {
        if (min <= MinToGenerate)
            MinToGenerate -= decreaseTime;
        if (max <= MaxToGenerate)
            MaxToGenerate -= decreaseTime;
    }
}
