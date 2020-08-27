using UnityEngine;

public class AmmoSpawner : MonoBehaviour 
{
	public GameObject AmmoPrefab = null;

	//Vector for time range
	public Vector2 TimeDelayRange = Vector2.zero;

	//Lifetime for ammo spawned
	public float AmmoLifeTime = 2f;
	public float AmmoSpeed = 4f;
	public float AmmoDamage = 100f;


	void Start()
	{
		FireAmmo();
	}

	public void FireAmmo()
	{
		GameObject Obj = Instantiate(AmmoPrefab, transform.position, transform.rotation) as GameObject;
		Ammo AmmoComp = Obj.GetComponent<Ammo>();
		Mover MoveComp = Obj.GetComponent<Mover>();
		AmmoComp.LifeTime = AmmoLifeTime;
		AmmoComp.Damage = AmmoDamage;
		MoveComp.Speed = AmmoSpeed;

		//Wait until next random interval
		Invoke("FireAmmo", Random.Range(TimeDelayRange.x, TimeDelayRange.y));
	}
}