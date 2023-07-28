using UnityEngine;
using Confluent.Kafka;
using System;

public class KafkaConsumer : MonoBehaviour
{
    public string bootstrapServers = "localhost:29092";
    public string topic = "meutopico";
    public string groupId = "grupo_unity";

    private bool isConsuming = true;
    private IConsumer<Ignore, string> consumer;

    private void Start()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);

        // Inicia uma thread para consumir as mensagens do Kafka
        new System.Threading.Thread(() =>
        {
            while (isConsuming)
            {
                try
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));
                    if (consumeResult != null)
                    {
                        // Chama um método para processar a mensagem recebida
                        ProcessMessage(consumeResult.Message.Value);
                    }
                }
                catch (ConsumeException e)
                {
                    Debug.LogError($"Erro ao consumir a mensagem do Kafka: {e.Error.Reason}");
                }
            }
        }).Start();
    }

    private void ProcessMessage(string message)
    {
        // Aqui você pode processar a mensagem recebida do Kafka.
        // Por exemplo, converter o JSON em um objeto ou realizar alguma ação na Unity.
        Debug.Log($"Mensagem recebida do Kafka: {message}");
    }

    private void OnDestroy()
    {
        isConsuming = false;
        consumer?.Close();
    }
}
