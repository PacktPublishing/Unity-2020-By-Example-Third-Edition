using UnityEngine;

public class Health : MonoBehaviour
{
	public GameObject DeathParticlesPrefab = null;
	public bool ShouldDestroyOnDeath = true;

	public float HealthPoints
	{
		get
		{
			return _HealthPoints;
		}

		set
		{
			_HealthPoints = value;

			if(_HealthPoints <= 0)
			{
				SendMessage("Die", SendMessageOptions.DontRequireReceiver);

				if (DeathParticlesPrefab != null)
				{
					Instantiate(DeathParticlesPrefab, transform.position, transform.rotation);
				}

				if (ShouldDestroyOnDeath)
				{
					Destroy(gameObject);
				}
			}
		}
	}

	[SerializeField]
	private float _HealthPoints = 100f;
}
