using UnityEngine;

public class TimedDestroy : MonoBehaviour 
{
	public float DestroyTime = 2f;

	void Start ()
	{
		Destroy(gameObject, DestroyTime);
	}
}