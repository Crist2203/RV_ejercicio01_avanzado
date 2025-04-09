using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
using System.Runtime.InteropServices;
public class GameControllerVasitos : MonoBehaviour {

	public TextMeshProUGUI Acertado;
    public TextMeshProUGUI NoAcertado;
    public TextMeshProUGUI infoText;
	public GameObject ball;
	public Player player;
	public Cup[] cups;

	private int acertado;
	private int fallado;
	private int gano;

	private float resetTimer = 3f;

	// Use this for initialization
	void Start () {
		infoText.text = "Escoge el vaso que contiene la pelota!";

		StartCoroutine (ShuffleRoutine());
		Acertado.text = "acertado: "+PlayerPrefs.GetInt("acertado", 0).ToString();
        NoAcertado.text = "fallado" +PlayerPrefs.GetInt("fallado", 0).ToString();
    }
	
	// Update is called once per frame
	void Update () {
		if (player.picked) {
			if (player.won) {
				infoText.text = "Ganaste!";
				gano = 1;
			} else {
				infoText.text = "Perdiste :( Intentalo de nuevo!";
				gano = 0;
            }

			resetTimer -= Time.deltaTime;
			if (resetTimer <= 0f) {
				if (gano > 0)
				{
					PlayerPrefs.SetInt("acertado", PlayerPrefs.GetInt("acertado", 0) + 1);
				}
				else {
                    PlayerPrefs.SetInt("fallado", PlayerPrefs.GetInt("fallado", 0) + 1);
                }
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	private IEnumerator ShuffleRoutine () {
		yield return new WaitForSeconds (1f);

		foreach (Cup cup in cups) {
			cup.MoveUp ();
		}

		yield return new WaitForSeconds (0.5f);

		Cup targetCup = cups[Random.Range(0, cups.Length)];
		targetCup.ball = ball;
		ball.transform.position = new Vector3 (
			targetCup.transform.position.x,
			ball.transform.position.y,
			targetCup.transform.position.z
		);

		yield return new WaitForSeconds (1.0f);

		foreach (Cup cup in cups) {
			cup.MoveDown ();
		}

		yield return new WaitForSeconds (1.0f);

		for (int i = 0; i < 5; i++) {
			Cup cup1 = cups[Random.Range(0, cups.Length)];
			Cup cup2 = cup1;

			while (cup2 == cup1) {
				cup2 = cups[Random.Range(0, cups.Length)];
			}

			Vector3 cup1Position = cup1.targetPosition;

			cup1.targetPosition = cup2.targetPosition;
			cup2.targetPosition = cup1Position;

			yield return new WaitForSeconds (0.75f);
		}

		player.canPick = true;
	}
}
