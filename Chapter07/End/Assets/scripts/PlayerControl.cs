using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour
{
	public enum FACEDIRECTION {FACELEFT = -1, FACERIGHT = 1};
	public FACEDIRECTION Facing = FACEDIRECTION.FACERIGHT;
	public LayerMask GroundLayer;
	public CircleCollider2D FeetCollider = null;
	public bool isGrounded = false;
	public string HorzAxis = "Horizontal";
	public string JumpButton = "Jump";
	public float MaxSpeed = 50f;
	public float JumpPower = 600;
	public float JumpTimeOut = 1f;
	public bool CanControl = true;
	public static PlayerControl PlayerInstance = null;
	public GameObject DeathParticles = null;

	public static float Health
	{
		get
		{
			return _Health;
		}

		set
		{
			_Health = value;

			//If we are dead, then end game
			if(_Health <= 0)
			{
				Die();
			}
		}
	}

	[SerializeField]
	private static float _Health = 100f;

	private Rigidbody2D ThisBody = null;
	private bool CanJump = true;
	private Animator ThisAnimator = null;
	private int MotionVal = Animator.StringToHash("Motion");


	void Awake ()
	{
		//Get transform and rigid body
		ThisBody = GetComponent<Rigidbody2D>();

		//Get Animator
		ThisAnimator = GetComponent<Animator>();

		//Set static instance
		PlayerInstance = this;
	}

	void Start()
	{
		//Level begins. Set starting position
		transform.position = SceneChanger.LastTarget;
	}

	//Returns bool - is player on ground?
	private bool GetGrounded()
	{
		//Check ground
		Vector2 CircleCenter = new Vector2(transform.position.x, transform.position.y) + FeetCollider.offset;
		Collider2D[] HitColliders = Physics2D.OverlapCircleAll(CircleCenter, FeetCollider.radius, GroundLayer);
		if(HitColliders.Length > 0) return true;
		return false;
	}

	//Flips character direction
	private void FlipDirection()
	{
		Facing = (FACEDIRECTION) ((int)Facing * -1f);
		Vector3 LocalScale = transform.localScale;
		LocalScale.x *= -1f;
		transform.localScale = LocalScale;
	}

	//Engage jump
	private void Jump()
	{
		//If we are grounded, then jump
		if(!isGrounded || !CanJump)return;

		//Jump
		ThisBody.AddForce(Vector2.up * JumpPower);
		CanJump = false;
		Invoke ("ActivateJump", JumpTimeOut);
	}

	//Activates can jump variable after jump timeout
	//Prevents double-jumps
	private void ActivateJump()
	{
		CanJump = true;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		//If we cannot control character, then exit
		if(!CanControl || Health <= 0f)
		{
			//Update motion Animation
			ThisAnimator.SetFloat(MotionVal, 0f, 0.1f, Time.deltaTime);
			return;
		}

		//Update grounded status
		isGrounded = GetGrounded();
		float Horz = CrossPlatformInputManager.GetAxis(HorzAxis);
		ThisBody.AddForce(Vector2.right * Horz * MaxSpeed);

		if(CrossPlatformInputManager.GetButton(JumpButton))
			Jump();
		
		//Clamp velocity
		ThisBody.velocity = new Vector2(Mathf.Clamp(ThisBody.velocity.x, -MaxSpeed, MaxSpeed), 
		                                Mathf.Clamp(ThisBody.velocity.y, -Mathf.Infinity, JumpPower));
	
		//Flip direction if required
		if((Horz < 0f && Facing != FACEDIRECTION.FACELEFT) || (Horz > 0f && Facing != FACEDIRECTION.FACERIGHT))
			FlipDirection();

		//Update motion Animation
		ThisAnimator.SetFloat(MotionVal, Mathf.Abs(Horz), 0.1f, Time.deltaTime);
	}

	void OnDestroy()
	{
		PlayerInstance = null;
	}

	//Function to kill player
	static void Die()
	{
		//Spawn particle system for death
		if (PlayerControl.PlayerInstance.DeathParticles != null)
		{
			Instantiate(PlayerControl.PlayerInstance.DeathParticles, PlayerControl.PlayerInstance.transform.position, PlayerControl.PlayerInstance.transform.rotation);
		}

		Destroy(PlayerControl.PlayerInstance.gameObject);
	}

	//Resets player back to defaults
	public static void Reset()
	{
		Health = 100f;
		//Set to default position
		SceneChanger.LastTarget = new Vector3(1.55f,-1.63f,0f);
	}
}