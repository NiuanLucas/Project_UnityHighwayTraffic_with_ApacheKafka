using System.Collections.Generic;
using UnityEngine;

public class Highway : MonoBehaviour
{
    public List<CarController> cars = new List<CarController>();

    public Color highwayColorWhenFree = new Color(150f / 255f, 1f, 70f / 255f, 1f); // Verde claro
    public Color highwayColorWhenAccident = new Color(1f, 144f / 255f, 132f / 255f, 1f); // Vermelho claro
    private Material highwayMaterial;
    private Color originalColor;

    public Transform startPoint; // Referência para o ponto de início da rodovia
    public Transform endPoint; // Referência para o ponto de fim da rodovia

    private void Start()
    {
        highwayMaterial = GetComponent<Renderer>().material;
        originalColor = highwayMaterial.color;
    }

    public void Update()
    {
        UpdateHighwayColor();
    }

    public void SpawnCar()
    {
        // Implemente a lógica de spawn de carros na rodovia aqui
        // Por exemplo, instanciar um novo objeto de carro na posição inicial da rodovia
    }

    public bool HasAccident()
    {
        // Verifica se a rodovia tem algum carro acidentado
        foreach (CarController car in cars)
        {
            if (car.isAccidentEnabled)
            {
                return true;
            }
        }

        return false;
    }

    public void RegisterCar(CarController car)
    {
        cars.Add(car);
    }

    public void UnregisterCar(CarController car)
    {
        cars.Remove(car);
    }

    public void UpdateHighwayColor()
    {
        if (HasAccident())
        {
            highwayMaterial.color = highwayColorWhenAccident;
        }
        else
        {
            highwayMaterial.color = highwayColorWhenFree;
        }
    }

    public void ResetHighwayColor()
    {
        highwayMaterial.color = originalColor;
    }
    public int GetCarCount()
    {
        return cars.Count;
    }
}
