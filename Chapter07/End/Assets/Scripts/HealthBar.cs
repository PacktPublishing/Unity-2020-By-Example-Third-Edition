using UnityEngine;

public class HealthBar : MonoBehaviour
{
	//Reference to this transform component
	private RectTransform ThisTransform = null;

	//Catch up speed
	public float MaxSpeed = 10f;

	void Awake()
	{
		//Get transform component
		ThisTransform = GetComponent<RectTransform>();
	}

	void Start()
	{
		//Set Start Health
		if (PlayerControl.PlayerInstance != null)
		{
			ThisTransform.sizeDelta = new Vector2(Mathf.Clamp(PlayerControl.Health, 0, 100), ThisTransform.sizeDelta.y);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		//Update health property
		float HealthUpdate = 0f;

		if (PlayerControl.PlayerInstance != null)
		{
			HealthUpdate = Mathf.MoveTowards(ThisTransform.rect.width, PlayerControl.Health, MaxSpeed);
		}

		ThisTransform.sizeDelta = new Vector2(Mathf.Clamp(HealthUpdate,0,100),ThisTransform.sizeDelta.y);
	}
}
