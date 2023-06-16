using UnityEngine;

public class BuildingsController : MonoBehaviour
{
    private GameObject[] _buildings;
    private int _buildingsCount;

    void Start()
    {
        _buildings = GameObject.FindGameObjectsWithTag(Tags.PlayerBuilding);
        _buildingsCount = _buildings.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBuilding()
    {
        _buildingsCount--;
    }

    public bool IsAllBuildingsDestroyed()
    {
        return _buildingsCount <= 0;
    }
}
