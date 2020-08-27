using UnityEngine;
using System.Collections.Generic;

public class AmmoManager : MonoBehaviour
{
	//Reference to ammo prefab
	public GameObject AmmoPrefab = null;

	//Ammo pool count
	public int PoolSize = 100;

	public Queue<Transform> AmmoQueue = new Queue<Transform>();

	//Array of ammo objects to generate
	private GameObject[] AmmoArray;

	public static AmmoManager AmmoManagerSingleton = null;

	void Awake ()
	{
		if(AmmoManagerSingleton != null)
		{
			Destroy(GetComponent<AmmoManager>());
			return;
		}

		AmmoManagerSingleton = this;
		AmmoArray = new GameObject[PoolSize];

		for(int i = 0; i < PoolSize; ++i)
		{
			AmmoArray[i] = Instantiate(AmmoPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			Transform ObjTransform = AmmoArray[i].transform;
			AmmoQueue.Enqueue(ObjTransform);
			AmmoArray[i].SetActive(false);
		}
	}

	public static Transform SpawnAmmo(Vector3 Position, Quaternion Rotation)
	{
		//Get ammo
		Transform SpawnedAmmo = AmmoManagerSingleton.AmmoQueue.Dequeue();

		SpawnedAmmo.gameObject.SetActive(true);
		SpawnedAmmo.position = Position;
		SpawnedAmmo.localRotation = Rotation;

		//Add to queue end
		AmmoManagerSingleton.AmmoQueue.Enqueue(SpawnedAmmo);

		//Return ammo
		return SpawnedAmmo;
	}
}