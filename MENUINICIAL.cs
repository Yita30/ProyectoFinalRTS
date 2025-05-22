using UnityEngine;
using UnityEngine.SceneManagement;

public class MENUINICIAL : MonoBehaviour
{
    // M�todo para cargar la escena del juego
    public void CargarJuego()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia el nombre por el de tu escena
    }
    public void salirmenu()
    {
        SceneManager.LoadScene("menuinicial"); // Cambia el nombre por el de tu escena
    }
    // M�todo para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    void Update()
    {
        // Permite salir con la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Salir();
        }
    }
}
