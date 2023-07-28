using System.Collections.Generic;
using UnityEngine;

public class HighwayManager : MonoBehaviour
{
    public GameObject highwaysObj;
    public List<Highway> highways = new List<Highway>();


    private void Start()
    {
        AddHighwaysInChildren();
    }

    public bool IsHighwayClear(Highway highway)
    {
        foreach (CarController car in highway.cars)
        {
            if (car.isAccidentEnabled)
            {
                return false;
            }
        }

        return true;
    }

    public void RegisterHighway(Highway highway)
    {
        highways.Add(highway);
    }

    public void UnregisterHighway(Highway highway)
    {
        highways.Remove(highway);
    }

    private void AddHighwaysInChildren()
    {
        if (highwaysObj == null)
        {
            Debug.LogWarning("O objeto highwaysObj não foi atribuído ou é nulo.");
            return;
        }

        // Obtém todos os componentes do tipo Highway presentes nos filhos do objeto highwaysObj
        Highway[] highwaysInChildren = highwaysObj.GetComponentsInChildren<Highway>();

        // Adiciona as rodovias encontradas à lista de rodovias
        foreach (Highway highway in highwaysInChildren)
        {
            highways.Add(highway);

            // Busca e atribui os objetos startPoint e endPoint para cada rodovia
            Transform startPoint = highway.transform.Find("startPoint");
            Transform endPoint = highway.transform.Find("endPoint");

            if (startPoint != null)
            {
                highway.startPoint = startPoint;
            }
            else
            {
                Debug.LogWarning("Objeto 'startPoint' não encontrado em uma das rodovias.");
            }

            if (endPoint != null)
            {
                highway.endPoint = endPoint;
            }
            else
            {
                Debug.LogWarning("Objeto 'endPoint' não encontrado em uma das rodovias.");
            }
        }
    }
}
