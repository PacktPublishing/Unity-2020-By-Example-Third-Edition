using UnityEngine;

public class TriggerAnimator : MonoBehaviour 
{
	//Animator to trigger
	public Animator TriggerAnim = null;

	//Name of boolean to set
	public string AnimBoolean = string.Empty;

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(!other.CompareTag("Player"))return;

		TriggerAnim.SetBool(AnimBoolean, true);
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if(!other.CompareTag("Player"))return;

		TriggerAnim.SetBool(AnimBoolean, false);
	}
}