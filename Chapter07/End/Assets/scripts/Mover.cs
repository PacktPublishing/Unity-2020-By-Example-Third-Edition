using UnityEngine;

public class Mover : MonoBehaviour 
{
	public float Speed = 10f;

	void Update () 
	{
		//Update object position
		transform.position += transform.forward * Speed * Time.deltaTime;
	}
}