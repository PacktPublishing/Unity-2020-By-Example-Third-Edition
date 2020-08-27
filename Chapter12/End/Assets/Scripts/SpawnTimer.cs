using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    public string SpawnPoolTag = "EnemyPool";
    public float SpawnInterval = 5f;

    private ObjectPool Pool = null;

    void Awake()
    {
        Pool = GameObject.FindWithTag(SpawnPoolTag).GetComponent<ObjectPool>();
    }

    void Start()
    {
        InvokeRepeating("Spawn", SpawnInterval, SpawnInterval);
    }

    public void Spawn()
    {
        Pool.Spawn(null, transform.position, transform.rotation, Vector3.one);
    }
}
