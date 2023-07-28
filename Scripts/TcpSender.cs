using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Text;

public class TcpSender : MonoBehaviour
{
    private TcpClient client;
    private StreamWriter writer;
    public float posX;

    private void Start()
    {
        // Endereço IP e porta do servidor Python
        string serverIP = "127.0.0.1";
        int serverPort = 1234;

        // Conecte-se ao servidor Python
        client = new TcpClient(serverIP, serverPort);
        writer = new StreamWriter(client.GetStream());
    }

    private void Update()
    {
        // Envie a posição X do GameObject para o servidor Python
        posX = transform.position.x;
        writer.WriteLine(posX);
        writer.Flush();
    }

    private void OnDestroy()
    {
        // Feche a conexão com o servidor Python antes de encerrar o jogo
        writer.Close();
        client.Close();
    }
}
