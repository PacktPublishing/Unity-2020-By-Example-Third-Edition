using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerReset : MonoBehaviour 
{
	public float ResetTime = 5f;

	void Start()
	{
		Invoke ("Reset", ResetTime);
	}

	void Reset()
	{
		PlayerControl.Reset();
        SceneManager.LoadScene(1);
	}
}
