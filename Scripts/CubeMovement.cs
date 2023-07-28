using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento
    public float distance = 100f; // Distância a percorrer

    private Vector3 startPosition; // Posição inicial
    private float currentDistance; // Distância percorrida até agora

    private void Start()
    {
        startPosition = transform.position; // Define a posição inicial
    }

    private void Update()
    {
        // Verifica se a distância percorrida é menor do que a distância total
        if (currentDistance < distance)
        {
            // Move o cubo na direção positiva do eixo X
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            currentDistance = Vector3.Distance(startPosition, transform.position); // Atualiza a distância percorrida
        }
        else
        {
            // Retorna o cubo para a posição inicial
            transform.position = startPosition;
            currentDistance = 0f;
        }
    }
}

