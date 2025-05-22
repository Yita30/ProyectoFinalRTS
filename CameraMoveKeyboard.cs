using UnityEngine;

public class CameraMoveKeyboard : MonoBehaviour
{
    public float speed = 2f;
    public float rotationSpeed = 50f; // Velocidad de rotación

    public Transform corner1;
    public Transform corner2;

    private Bounds bounds;

    void Start()
    {
        // Calcular el centro y el tamaño del área de movimiento
        Vector3 center = Vector3.Lerp(corner1.position, corner2.position, 0.5f);
        Vector3 size = new Vector3(
            Mathf.Abs(corner1.position.x - corner2.position.x),
            Mathf.Abs(corner1.position.y - corner2.position.y),
            Mathf.Abs(corner1.position.z - corner2.position.z)
        );

        this.bounds = new Bounds(center, size);
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // Movimiento basado en la dirección de la cámara
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;  // Adelante
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;  // Atrás
        if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;    // Izquierda
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;    // Derecha

        moveDirection.y = 0; // Evita que la cámara se mueva verticalmente

        // Si hay movimiento, actualizar la posición de la cámara respetando los límites
        if (moveDirection != Vector3.zero)
        {
            Vector3 newPosition = transform.position + moveDirection.normalized * speed * Time.deltaTime;
            transform.position = bounds.ClosestPoint(newPosition);
        }

        // Rotar la cámara con la tecla Q
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, +rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        // Mostrar los límites del área en la vista de escena
        if (corner1 != null && corner2 != null)
        {
            Vector3 center = Vector3.Lerp(corner1.position, corner2.position, 0.5f);
            Vector3 size = corner1.position - corner2.position;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
