using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
	//Maximum time to complete level (in seconds)
	public float MaxTime = 60f;
	
	//Countdown
	[SerializeField]
	private float CountDown = 0;
	
	void Start () 
	{
		CountDown = MaxTime;
	}

	void Update () 
	{
		//Reduce time
		CountDown -= Time.deltaTime;

		//Restart level if time runs out
		if(CountDown <= 0)
		{
			//Reset coin count
			Coin.CoinCount=0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}