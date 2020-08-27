using UnityEngine;

public class Spawner : MonoBehaviour
{
	public float MaxRadius = 1f;
	public float Interval = 5f;
	public GameObject ObjToSpawn = null;
	private Transform Origin = null;

	void Awake()
	{
		Origin = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}

	void Start () 
	{
		InvokeRepeating("Spawn", 0f, Interval);
	}

	void Spawn () 
	{
		if(Origin == null)return;

		Vector3 SpawnPos = Origin.position + Random.onUnitSphere * MaxRadius;
		SpawnPos = new Vector3(SpawnPos.x, 0f, SpawnPos.z);
		Instantiate(ObjToSpawn, SpawnPos, Quaternion.identity);
	}
}