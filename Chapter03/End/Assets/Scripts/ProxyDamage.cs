using UnityEngine;

public class ProxyDamage : MonoBehaviour
{
	//Damage per second
	public float DamageRate = 10f;

	void OnTriggerStay(Collider Col)
	{
		Health H = Col.gameObject.GetComponent<Health>();

		if (H == null)
		{
			return;
		}

		H.HealthPoints -= DamageRate * Time.deltaTime;
	}
}