using UnityEngine;

public class Ammo : MonoBehaviour
{	
	//Damage inflicted on Player
	public float Damage = 100f;

	//Lifetime for ammo
	public float LifeTime = 1f;

	void Start()
	{
		Destroy(gameObject, LifeTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//If not player then exit
		if(!other.CompareTag("Player"))return;
		
		//Inflict damage
		PlayerControl.Health -= Damage;
	}

	public void Die()
	{
		Destroy(gameObject);
	}
}
