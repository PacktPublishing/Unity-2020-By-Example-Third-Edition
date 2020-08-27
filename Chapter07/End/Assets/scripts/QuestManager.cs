using UnityEngine;

[System.Serializable]
public class Quest
{
	//Quest completed status
	public enum QUESTSTATUS {UNASSIGNED=0,ASSIGNED=1,COMPLETE=2};
	public QUESTSTATUS Status = QUESTSTATUS.UNASSIGNED;
	public string QuestName = string.Empty;
}

public class QuestManager : MonoBehaviour
{
	public Quest[] Quests;

	private static QuestManager ThisInstance = null;

	void Awake()
	{
		if (ThisInstance == null)
		{
			DontDestroyOnLoad(this);
			ThisInstance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	public static Quest.QUESTSTATUS GetQuestStatus(string QuestName)
	{
		foreach(Quest Q in ThisInstance.Quests)
		{
			if (Q.QuestName.Equals(QuestName))
			{
				return Q.Status;
			}
		}

		return Quest.QUESTSTATUS.UNASSIGNED;
	}

	public static void SetQuestStatus(string QuestName, Quest.QUESTSTATUS NewStatus)
	{
		foreach(Quest Q in ThisInstance.Quests)
		{
			if(Q.QuestName.Equals(QuestName))
			{
				Q.Status = NewStatus;
				return;
			}
		}
	}

	public static void Reset()
	{
		if(ThisInstance==null)return;

		foreach (Quest Q in ThisInstance.Quests)
		{
			Q.Status = Quest.QUESTSTATUS.UNASSIGNED;
		}
		
	}
}