using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody ThisBody = null;

	public bool MouseLook = true;
	public string HorzAxis = "Horizontal";
	public string VertAxis = "Vertical";
	public string FireAxis = "Fire1";

	public float MaxSpeed = 5f;
	public float ReloadDelay = 0.3f;
	public bool CanFire = true;

	public Transform[] TurretTransforms;

	void Awake ()
	{
		ThisBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
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

		//Check fire control
		if(Input.GetButtonDown(FireAxis) && CanFire)
		{
			foreach (Transform T in TurretTransforms)
			{
				AmmoManager.SpawnAmmo(T.position, T.rotation);
			}

			CanFire = false;
			Invoke ("EnableFire", ReloadDelay);
		}
	}

	void EnableFire()
	{
		CanFire = true;
	}

	public void Die()
	{
		GameController.GameOver();
		Destroy(gameObject);
	}
}
