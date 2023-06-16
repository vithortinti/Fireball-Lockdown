using System;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Fireball;
    public float LoadingGeneratorsTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootTheMostClosedAircraft();
        }
    }

    void ShootTheMostClosedAircraft()
    {
        var generationPoints = GameObject.FindGameObjectsWithTag(Tags.FireballGenerator);
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float minDistance = float.MaxValue;
        try
        {
            var avaiablegenerationPoints = generationPoints.Where(
                x => x.GetComponent<PlayerFireballGenerator>().IsAbleToGenerate
                );

            PlayerFireballGenerator generationPoint = null;
            foreach (var local in avaiablegenerationPoints)
            {
                float distance = Vector2.Distance(mousePosition, local.transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    generationPoint = local.GetComponent<PlayerFireballGenerator>();
                }
            }

            generationPoint.Shoot(Fireball);
        }
        catch (Exception e)
        {
            Debug.Log("Nenhum AirCraft está disponível para gerar uma bola de fogo\n" + e.Message);
        }
    }
}
