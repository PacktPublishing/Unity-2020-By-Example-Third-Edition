using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTimer : MonoBehaviour 
{
	//Scene to load after time
	public int SceneID = 0;
	public float TimeDelay = 5f;

	void Start () 
	{
		Invoke("LoadScene", TimeDelay);
	}

	void LoadScene () 
	{
		SceneManager.LoadScene(SceneID);
	}
}
