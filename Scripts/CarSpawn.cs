using UnityEngine;

public class CarSpawn : MonoBehaviour
{
    public GameObject carPrefab;
    public float spawnInterval = 3f;
    public int maxCarsPerHighway = 5; // Limite máximo de carros por rodovia

    private float spawnTimer;
    private HighwayManager highwayManager;

    private void Start()
    {
        maxCarsPerHighway = 5;
        spawnInterval = 3f;
        highwayManager = FindObjectOfType<HighwayManager>();
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (spawnTimer <= 0f)
        {
            SpawnCar();
            spawnTimer = spawnInterval;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void SpawnCar()
    {
        foreach (Highway highway in highwayManager.highways)
        {
            //Debug.Log("Highway (" + highway.HasAccident() + ") " + highway.gameObject.name + " | GetCarCount: " + highway.GetCarCount() + " < " + maxCarsPerHighway + " | bool: " + (highway.GetCarCount() < maxCarsPerHighway));
            if ((highway.GetCarCount() < maxCarsPerHighway) && (!highway.HasAccident()))
            {
                // Obtem o ID do carro localmente
                int carID = GetNextCarID();

                GameObject carObject = Instantiate(carPrefab, highway.startPoint.position, Quaternion.identity);
                carObject.GetComponent<CarController>().carID = carID;
                carObject.name = "Car_ID_" + carID;

                CarController carController = carObject.GetComponent<CarController>();
                carController.SetHighway(highway);
                highway.RegisterCar(carController);

                return;
            }
        }

        Debug.Log("Não foi possível spawnar o carro. Todas as rodovias estão ocupadas.");
    }

    private int nextCarID = 0; // Variável local para armazenar o próximo ID do carro

    private int GetNextCarID()
    {
        nextCarID++;
        return nextCarID;
    }
}
