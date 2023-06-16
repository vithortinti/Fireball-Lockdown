using UnityEngine;

public class SpecialGenerator : Generator
{
    public bool IsAbleToGenerate;
    public float RegenerateAfter;
    private float TimeCounter;

    void Update()
    {
        if (IsAbleToGenerate)
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter > RegenerateAfter)
            {
                bool spawned = Generate();
                if (spawned)
                {
                    TimeCounter = 0;
                    IsAbleToGenerate = false;
                }
            }
        }
        else
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter > RegenerateAfter)
            {
                IsAbleToGenerate = true;
                TimeCounter = 0;
            }
        }
    }

    public static void DecreaseRegenerationTime(float value)
    {
        GameObject[] specialsGenerators = GameObject.FindGameObjectsWithTag(Tags.SpecialGenerator);
        foreach(GameObject special in specialsGenerators)
        {
            special.GetComponent<SpecialGenerator>().RegenerateAfter -= value;
        }
    }
}
