// Parte de los using o imports
using System.Collections; // Manejo de listas y colecciones
using System.Collections.Generic; // Manejo de listas y colecciones
using UnityEngine; // para el acceso basico de unity
using UnityEngine.SceneManagement; // Para cambiar la escena
using UnityEngine.UI; //manipular los elementos de interfaz como text
using TMPro;

public class GameController_nave : MonoBehaviour // hay que definir la clase, hereda de MonoBehaviour, permite que se use como componente
{
    #region Referencias a Objetos en el juego // sirve para la organizaciond el codigo
    public Camera gameCamera; // Referencia a la camara del juego, que usara para disparar 
    public GameObject bulletPrefab; // el prefab de la bala que sera una instancia al disparar
    public GameObject enemyPrefab; // prefab del enemigo que sera una instancia en el juego
    [SerializeField] TextMeshProUGUI puntaje; // Texto en pantalla que muestra el puntaje
    #endregion

    #region Configuración de temporizadores y puntaje
    public float enemySpawningCooldown = 1f; // Tiempo entre la aparicion de enemigos
    public float enemySpawningDistance = 7f; // Distancia a la aparecen los enemigos desde la camara
    public float shootingCooldown = 0.5f; // Tiempo que pasa entre cada disparo

    private float enemySpawningTimer = 0; // temporizador para spawnear enemigos
    private float shootingTimer = 0; // Temporizador que ira descontando para controlar cuando se puede disparar
    private int npuntaje = 0; // para almacenar el puntaje del jugador
    #endregion

    private void OnTriggerEnter(Collider other) // Se ejecuta automaticamente cuando objeto entra en el collider con "Is Trigger" activado
    {
        if (other.tag == "Enemy") { // Verifica si el objeto qie colisiono esta asignado con la etiqueta "Enemy"
            // Guardamos el puntaje en PlayerPrefs
            PlayerPrefs.SetInt("Puntaje", npuntaje);
            PlayerPrefs.Save(); // Guarda los cambios hechos en PlayerPrefs.
            SceneManager.LoadScene("Menu_Game_Over"); //Cargamos la escena del Game Over
        }
    }

    // Update ejecuta una vez por frame
    void Update() 
    {
        //Restamos tiempo a los temporizadores
        shootingTimer -= Time.deltaTime;
        enemySpawningTimer -= Time.deltaTime;
        if (enemySpawningTimer <= 0f) { //Si el temporizador para la aparicion de enemigos llega a 0 
            enemySpawningTimer = enemySpawningCooldown; // Reinicia el temporizador del spawn
        
            GameObject enemyObject = Instantiate(enemyPrefab); //Creamos un nuevo enemigo
            float randomAngle = Random.Range(0, Mathf.PI * 2); //Generamos un numero aleatorio
            enemyObject.transform.position = new Vector3( //Posicionamos al Enemigo en una posición aleatoria en el eje x,y
                gameCamera.transform.position.x + Mathf.Cos(randomAngle) * enemySpawningDistance,
                0,
                gameCamera.transform.position.y + Mathf.Sin(randomAngle) * enemySpawningDistance
                );
            //seleccionamos el enemigo creado y configuramos su dirección para que apunte hacia donde esta el player (Camera)
            Enemy_nave enemy = enemyObject.GetComponent<Enemy_nave>(); //Obtiene el componente Enemy del objeteo recien creado
            enemy.direction = (gameCamera.transform.position - enemy.transform.position).normalized; // da al enemigo una direccion apuntando hacia la camara (player).
            enemy.transform.LookAt(Vector3.zero); // El enemigo apunta hacia el origen del mundo (tambien se puede ajustar si el jugador no esta en 0,0,0)
        }
        RaycastHit hit; //Crea una variable para guardar la informacion del objeto golpeado 

        if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hit)){ // Lanza un rayo desde la camara hacia adelante para detectar colisiones
            if (hit.transform.tag == "Enemy" && shootingTimer <= 0f) // Si el raycast golpea a un objeto con el tag "Enemy" y el temporizador de disparo está en cero
            {
                shootingTimer = shootingCooldown;  // Reinicia el temporizador de disparo
                GameObject bulletObject = Instantiate(bulletPrefab);
                bulletObject.transform.position = gameCamera.transform.position; // Instancia una bala en la posición de la cámara

                Bullet_nave bullet = bulletObject.GetComponent<Bullet_nave>();
                bullet.direction = gameCamera.transform.forward; // Configura la dirección de la bala hacia adelante

                npuntaje += 100;                
                puntaje.text = "Puntaje: " + npuntaje; // Aumenta el puntaje y actualiza el texto del puntaje en la interfaz de usuario
            }
        }
    }
}
