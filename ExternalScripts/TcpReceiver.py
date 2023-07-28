import socket
from confluent_kafka import Producer

bootstrap_servers = 'localhost:29092'
topic = 'meutopico'

def send_message_to_kafka(message):
    producer = Producer({'bootstrap.servers': bootstrap_servers})
    producer.produce(topic, message.encode('utf-8'))
    producer.flush()

# Endereço IP e porta do servidor
server_ip = '127.0.0.1'
server_port = 1234

# Crie um socket TCP/IP
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((server_ip, server_port))
server_socket.listen(1)

print('Aguardando conexão...')

# Aceite a conexão do cliente
client_socket, client_address = server_socket.accept()
print('Conexão estabelecida:', client_address)

while True:
    # Receba a posição X enviada pelo cliente Unity
    position_x = client_socket.recv(1024).decode()

    # Faça algo com a posição X recebida
    print('Posição X recebida:', position_x)

    # Envie a posição X para o Kafka
    send_message_to_kafka(position_x)

# Feche o socket do cliente e do servidor
client_socket.close()
server_socket.close()
