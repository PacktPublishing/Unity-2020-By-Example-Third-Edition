using UnityEngine;

public class CollideDestroy : MonoBehaviour
{
	//When hit objects with associated tag, then destroy
	public string TagCompare = string.Empty;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag(TagCompare)) return;

		Destroy(gameObject);
	}
}