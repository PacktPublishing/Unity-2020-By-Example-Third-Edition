using UnityEngine;

public class ScoreOnDestroy : MonoBehaviour
{
	public int ScoreValue = 50;

	void OnDestroy()
	{
		GameController.Score += ScoreValue;
	}
}