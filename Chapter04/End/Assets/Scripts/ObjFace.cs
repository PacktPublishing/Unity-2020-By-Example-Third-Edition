using UnityEngine;

public class ObjFace : MonoBehaviour
{
	public Transform ObjToFollow = null;
	public bool FollowPlayer = false;

	void Awake () 
	{
		//Should face player?
		if(!FollowPlayer)return;

		//Get player transform
		GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player");
		if (PlayerObj != null)
		{
			ObjToFollow = PlayerObj.GetComponent<Transform>();
		}
	}

	void Update ()
	{
		//Follow destination object
		if (ObjToFollow == null)
		{
			return;
		}

		//Get direction to follow object
		Vector3 DirToObject = ObjToFollow.position - transform.position;

		if (DirToObject != Vector3.zero)
		{
			transform.localRotation = Quaternion.LookRotation(DirToObject.normalized, Vector3.up);
		}
	}
}