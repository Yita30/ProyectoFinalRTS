using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void EndGame(bool playerLost)
    {
        if (playerLost)
            Debug.Log("¡Has perdido la partida!");
        else
            Debug.Log("¡Has ganado!");

        // Puedes añadir aquí cargar escena, mostrar UI, etc
        SceneManager.LoadScene("menuinicial");
    }
}
