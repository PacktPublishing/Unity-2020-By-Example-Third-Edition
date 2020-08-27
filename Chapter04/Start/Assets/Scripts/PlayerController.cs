using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool MouseLook = true;
	public string HorzAxis = "Horizontal";
	public string VertAxis = "Vertical";
	public string FireAxis = "Fire1";
	public float MaxSpeed = 5f;

	private Rigidbody ThisBody = null;

	void Awake ()
	{
		ThisBody = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		//Update movement
		float Horz = Input.GetAxis(HorzAxis);
		float Vert = Input.GetAxis(VertAxis);
		Vector3 MoveDirection = new Vector3(Horz, 0.0f, Vert);
		ThisBody.AddForce(MoveDirection.normalized * MaxSpeed);
		
		//Clamp speed
		ThisBody.velocity = new Vector3(Mathf.Clamp(ThisBody.velocity.x, -MaxSpeed, MaxSpeed),
		                                Mathf.Clamp(ThisBody.velocity.y, -MaxSpeed, MaxSpeed),
		                                Mathf.Clamp(ThisBody.velocity.z, -MaxSpeed, MaxSpeed));
		
		//Should look with mouse?
		if(MouseLook)
		{
			//Update rotation - turn to face mouse pointer
			Vector3 MousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
			MousePosWorld = new Vector3(MousePosWorld.x, 0.0f, MousePosWorld.z);
			
			//Get direction to cursor
			Vector3 LookDirection = MousePosWorld - transform.position;
			
			//FixedUpdate rotation
			transform.localRotation = Quaternion.LookRotation(LookDirection.normalized,Vector3.up);
		}
	}
}