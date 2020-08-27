using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	//Game score
	public static int Score;

	//Prefix
	public string ScorePrefix = string.Empty;

	//Score text object
	public Text ScoreText = null;

	//Game over text
	public Text GameOverText = null;

	public static GameController ThisInstance = null;

	void Awake()
	{
		ThisInstance = this;
	}

	void Update()
	{
		//Update score text
		if (ScoreText != null)
		{
			ScoreText.text = ScorePrefix + Score.ToString();
		}
	}

	public static void GameOver()
	{
		if (ThisInstance.GameOverText != null)
		{
			ThisInstance.GameOverText.gameObject.SetActive(true);
		}
	}
}
