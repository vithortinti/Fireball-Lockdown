using UnityEngine;

public class MeteorGenerator : Generator
{
    private static GameObject[] _meteorGenerators;

    void Start()
    {
        _meteorGenerators = GameObject.FindGameObjectsWithTag(Tags.MeteorGenerator);
    }

    void Update()
    {
        Generate();
    }
}
