using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;

public class HighwayManager : MonoBehaviour
{
    public GameObject highwaysObj;
    public List<Highway> highways = new List<Highway>();

    public string serverIP = "127.0.0.1"; // Endereço IP do servidor Python
    public int serverPort = 6060; // Porta do servidor Python
    public int timeToSendMsg = 2000; //ms



    private void Start()
    {
        AssignHighwayIDs();
        AddHighwaysInChildren();

        // Inicia o envio das mensagens
        InvokeRepeating("SendStatusToPython", 0f, (timeToSendMsg / 1000)); // Intervalo de 0.1 segundos (100 ms)
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

    // Função para atribuir IDs únicos a cada rodovia na lista
    private void AssignHighwayIDs()
    {
        for (int i = 0; i < highways.Count; i++)
        {
            highways[i].highwayID = i + 1;
        }
    }

    // Função para enviar os status das rodovias para o servidor Python
    private void SendStatusToPython()
    {
        foreach (Highway highway in highways)
        {
            bool hasAccident = highway.HasAccident();
            var data = new Dictionary<string, object>
            {
                { "highwayID", highway.highwayID },
                { "status", (hasAccident ? "accident" : "clear") }
            };
            string jsonData = JsonConvert.SerializeObject(data);
            SendTCPMessage(jsonData);
            Debug.Log("HighwayTcpSend Msg: " + jsonData);
        }
    }
    // Função para enviar uma mensagem TCP para o servidor Python
    private void SendTCPMessage(string message)
    {
        try
        {
            TcpClient client = new TcpClient(serverIP, serverPort);
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            client.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log("Error sending TCP message: " + e.Message);
        }
    }

}
