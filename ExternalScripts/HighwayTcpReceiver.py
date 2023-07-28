from confluent_kafka import Producer
import json
import socket

bootstrap_servers = 'localhost:29092'
topic = 'meutopico'


def send_message_to_kafka(message):
    producer = Producer({'bootstrap.servers': bootstrap_servers})
    producer.produce(topic, message.encode('utf-8'))
    producer.flush()


# Endereço IP e porta do servidor Unity
server_ip = '127.0.0.1'
server_port = 6065

# Cria o socket TCP
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as server_socket:
    # Vincula o socket ao IP e porta do servidor
    server_socket.bind((server_ip, server_port))

    # Ouve por conexões entrantes
    server_socket.listen()

    print("Servidor TCP esperando conexões...")

    while True:
        # Aceita uma nova conexão
        client_socket, client_addr = server_socket.accept()
        print(f"Conexão estabelecida com {client_addr}")

        while True:
            # Recebe os dados enviados pela Unity
            data = client_socket.recv(1024)

            # Se não houver dados, a conexão foi fechada pela Unity
            if not data:
                print("Conexão encerrada.")
                break

            # Decodifica os dados recebidos em formato JSON
            jsonData = data.decode('utf-8')
            jsonData = jsonData.strip()

            # Exibe a mensagem recebida (status da rodovia) em tempo real
            print("Dados recebidos:", jsonData)

            # Faz o parsing do JSON para um objeto Python
            try:
                parsedData = json.loads(jsonData)
                highwayID = parsedData["highwayID"]
                status = parsedData["status"]
                print(f"Status da rodovia {highwayID}: {status}")

                # Envia os dados para o Kafka
                send_message_to_kafka(jsonData)
            except json.JSONDecodeError:
                print("Erro ao fazer parsing do JSON.")

        # Fecha a conexão com o cliente
        client_socket.close()
