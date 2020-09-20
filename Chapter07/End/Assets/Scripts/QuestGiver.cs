using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestGiver : MonoBehaviour
{
	//Human readable quest name
	public string QuestName = string.Empty;
	//Reference to UI Text Box
	public Text Captions = null;
	//List of strings to say
	public string[] CaptionText;

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(!other.CompareTag("Player"))return;

		Quest.QUESTSTATUS Status = QuestManager.GetQuestStatus(QuestName);
		Captions.text = CaptionText[(int) Status];
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		Quest.QUESTSTATUS Status = QuestManager.GetQuestStatus(QuestName);
		if (Status == Quest.QUESTSTATUS.UNASSIGNED)
		{
			QuestManager.SetQuestStatus(QuestName, Quest.QUESTSTATUS.ASSIGNED);
		}
		else if(Status == Quest.QUESTSTATUS.COMPLETE)
        {
			SceneManager.LoadScene(5);
        }
	}
}