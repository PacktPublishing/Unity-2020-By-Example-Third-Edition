using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
	//Quest completed status
	public enum QUESTSTATUS { UNASSIGNED = 0, ASSIGNED = 1, COMPLETE = 2 };
	public QUESTSTATUS Status = QUESTSTATUS.UNASSIGNED;
	public string QuestName = string.Empty;
}
