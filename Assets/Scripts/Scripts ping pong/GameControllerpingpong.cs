using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerpingpong : MonoBehaviour
{
    public Player player;    // Referencia al jugador
    public Ball ball;        // Referencia a la pelota
    public TextMesh scoreText; // Texto de puntuación en la pantalla
    public GameObject gameOverUI; // Referencia al Canvas de Game Over

    private bool isGameOver = false; // Variable para saber si el juego ha terminado

    void Start()
    {
        gameOverUI.SetActive(false); // Ocultamos el menú de Game Over al inicio
    }

    void Update()
    {
        if (!isGameOver) // Solo verificamos si el juego aún no ha terminado
        {
            bool gameOverCondition = ball.transform.position.z < player.transform.position.z;

            if (!gameOverCondition)
            {
                // Si el juego sigue en marcha, actualizamos la puntuación
                scoreText.text = "Score: " + ball.score;
            }
            else
            {
                // Si el juego ha terminado
                isGameOver = true;  // Marcamos el juego como terminado
                gameOverUI.SetActive(true); // Mostramos la UI de Game Over
            }
        }
    }

    // Método para reiniciar el juego
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Método para ir al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" por el nombre de tu escena del menú
    }
}

