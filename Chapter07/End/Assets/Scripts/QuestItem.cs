using UnityEngine;

public class QuestItem : MonoBehaviour 
{
	public string QuestName;
	private AudioSource ThisAudio = null;
	private SpriteRenderer ThisRenderer = null;
	private Collider2D ThisCollider = null;

	void Awake()
	{
		ThisAudio = GetComponent<AudioSource>();
		ThisRenderer = GetComponent<SpriteRenderer>();
		ThisCollider = GetComponent<Collider2D>();
	}

	void Start () 
	{
		//Hide object
		gameObject.SetActive(false);

		//Show object if quest is assigned
		if (QuestManager.GetQuestStatus(QuestName) == Quest.QUESTSTATUS.ASSIGNED)
		{
			gameObject.SetActive(true);
		}
	}

	//If item is visible and collected
	void OnTriggerEnter2D(Collider2D other) 
	{
		if(!other.CompareTag("Player"))return;

		if(!gameObject.activeSelf)return;
		
		//We are collected. Now complete quest
		QuestManager.SetQuestStatus(QuestName, Quest.QUESTSTATUS.COMPLETE);

		ThisRenderer.enabled=ThisCollider.enabled=false;

		if(ThisAudio!=null)ThisAudio.Play();
	}
}
