using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void OnGUI()
    {
        Vector3 worldPosition = transform.position + Vector3.up * 2f; // ajustar altura
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        if (screenPosition.z > 0) // Si está frente a la cámara
        {
            float width = 60;
            float height = 8;
            float x = screenPosition.x - width / 2;
            float y = Screen.height - screenPosition.y - height; // invertir eje Y

            // Fondo
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(x, y, width, height), Texture2D.whiteTexture);

            // Relleno
            GUI.color = Color.green;
            float healthPercent = currentHealth / maxHealth;
            GUI.DrawTexture(new Rect(x, y, width * healthPercent, height), Texture2D.whiteTexture);

            GUI.color = Color.white; // restaurar color
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        if (CompareTag("PlayerBase"))
        {
            GameManager.Instance.EndGame(playerLost: true);
        }
        else if (CompareTag("EnemyBase"))
        {
            GameManager.Instance.EndGame(playerLost: false);
        }

        Destroy(gameObject);
    }
}
