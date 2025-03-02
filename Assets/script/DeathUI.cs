using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public GameObject deathUI;

    private void Start()
    {
        // Asegurarse de que la UI de muerte esté desactivada al inicio
        deathUI.SetActive(false);
    }

    public void Retry()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" por el nombre de tu escena de menú
    }

    public void ShowDeathUI()
    {
        deathUI.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    public void HideDeathUI()
    {
        deathUI.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }
}


