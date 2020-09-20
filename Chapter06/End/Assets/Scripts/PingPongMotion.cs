using UnityEngine;
using System.Collections;

public class PingPongMotion : MonoBehaviour 
{
	//Original position
	private Vector3 OrigPos = Vector3.zero;

	//Axes to move on
	public Vector3 MoveAxes = Vector2.zero;

	//Speed
	public float Distance = 3f;

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