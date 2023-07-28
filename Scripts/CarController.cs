using UnityEngine;

public class CarController : MonoBehaviour
{
    public int carID;
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 4f;
    public float minDistance = 2f;
    public float accidentDuration = 5f;
    public Material carMaterial;
    public Color carColorWhenAccident = Color.red;
    public Color carColorWhenNormal = Color.green;
    public float accidentTimer { get; private set; }
    public bool isAccidentEnabled = false;
    public Highway currentHighway { get; private set; }

    private Rigidbody rb;
    private Vector3 frontFaceDirection;

    private void Start()
    {
        accidentDuration = Random.Range(30, 61);
        rb = GetComponent<Rigidbody>();
        frontFaceDirection = transform.forward; // Armazena a direção da face frontal do carro
        carMaterial = transform.GetChild(0).GetComponent<Renderer>().materials[0];
        ResetAccidentTimer();
    }

    private void Update()
    {
        if (isAccidentEnabled)
        {
            // O carro está em um acidente, permanece parado até o tempo de duração do acidente terminar
            rb.velocity = Vector3.zero;
            carMaterial.color = carColorWhenAccident; // Define a cor do material do carro como vermelho

            accidentTimer -= Time.deltaTime;
            if (accidentTimer <= 0f)
            {
                // O tempo de duração do acidente acabou, volta a se mover normalmente e restaura a cor original
                ResetAccidentTimer();
                carMaterial.color = carColorWhenNormal; // Define a cor do material do carro como verde
            }
        }
        else
        {
            RaycastHit hit;
            Vector3 raycastDirection = frontFaceDirection;

            if (Physics.Raycast(transform.position, raycastDirection, out hit, minDistance))
            {
                // Reduz a velocidade gradualmente se houver um carro à frente
                float distance = hit.distance;
                float targetSpeed = 0f; // Define a velocidade alvo como zero para evitar empurrar o carro da frente
                float currentSpeed = rb.velocity.magnitude;
                float newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
                rb.velocity = raycastDirection * newSpeed;

                Debug.DrawLine(transform.position, hit.point, Color.red); // Linha de debug mostrando a direção do raio e ponto de colisão
            }
            else
            {
                // Acelera até a velocidade máxima se não houver carros à frente
                float currentSpeed = rb.velocity.magnitude;
                float newSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
                rb.velocity = raycastDirection * newSpeed;

                Debug.DrawRay(transform.position, raycastDirection * minDistance, Color.green); // Linha de debug mostrando a direção do raio
            }

            // Verifica se o carro chegou ao ponto de fim (endpoint) da rodovia
            if (Vector3.Distance(transform.position, currentHighway.endPoint.position) <= 5f)
            {
                // Remove o carro da rodovia e o deleta
                currentHighway.UnregisterCar(this);
                Destroy(gameObject);
            }
        }
    }

    private void ResetAccidentTimer()
    {
        accidentTimer = accidentDuration;
        isAccidentEnabled = false;
    }

    public void SetHighway(Highway highway)
    {
        currentHighway = highway;
    }

    public void RegisterToHighway()
    {
        if (currentHighway != null)
        {
            currentHighway.RegisterCar(this);
        }
    }

    public void UnregisterFromHighway()
    {
        if (currentHighway != null)
        {
            currentHighway.UnregisterCar(this);
        }
    }
}