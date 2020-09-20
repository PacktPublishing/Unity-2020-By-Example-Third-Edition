using UnityEngine;

public class PingPongMotion : MonoBehaviour 
{
	//Axes to move on
	public Vector3 MoveAxes = Vector2.zero;

	//Speed
	public float Distance = 3f;

	//Original position
	private Vector3 OrigPos = Vector3.zero;

	void Start()
	{
		//Copy original position
		OrigPos = transform.position;
	}

	void Update()
	{
		//Update platform position with ping pong
		transform.position = OrigPos + MoveAxes * Mathf.PingPong(Time.time, Distance);
	}
}