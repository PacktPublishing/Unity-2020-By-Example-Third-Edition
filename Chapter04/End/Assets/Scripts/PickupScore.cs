using UnityEngine;
using UnityEngine.UI;

public class PickupScore : MonoBehaviour 
{
	public int ScorePoints = 100;
	private Text BonusText = null;
	public float MessageDelay = 2f;
	public string MessageTag = string.Empty;

	void Awake()
	{
		GameObject BonusObject = GameObject.FindGameObjectWithTag(MessageTag);
		BonusText = BonusObject.GetComponent<Text>();
	}

	void PowerupCollect () 
	{
		GameController.Score += ScorePoints;

		//Show score text
		BonusText.enabled=true;

		Invoke("HideText", MessageDelay);
	}

	public void HideText()
	{
		BonusText.enabled=false;
	}
}