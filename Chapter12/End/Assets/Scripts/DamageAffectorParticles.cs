using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageAffectorParticles : MonoBehaviour
{
    public string TagDamage = "Enemy";
    public float DamageAmount = 2f;

    private Health ThisHealth = null;

    void Awake()
    {
        ThisHealth = GetComponent<Health>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(TagDamage))
        {
            ThisHealth.HealthPoints -= DamageAmount;
        }
    }
}
