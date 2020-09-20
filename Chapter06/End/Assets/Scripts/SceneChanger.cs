using UnityEngine;
using UnityEngine.SceneManagement;

//Scene changer based on fades
public class SceneChanger : MonoBehaviour
{
	//Scene to change
	public int SceneDestination = 0;

	//Fade out on start
	public bool FadeOnStart = true;

	//Target position for player when they arrive at destination
	public Vector3 TargetPosition = Vector3.zero;

	//Last position to set player
	public static Vector3 LastTarget = Vector3.zero;

	//Trigger Name
	private int FadeInTrigger = Animator.StringToHash("FadeIn");
	private int FadeOutTrigger = Animator.StringToHash("FadeOut");

	//Fade Animator
	private Animator FadeAnimator = null;

    void Awake()
    {
		FadeAnimator = GetComponent<Animator>();
	}

    void Start()
	{
		if (FadeOnStart && FadeAnimator != null)
		{
			FadeAnimator.SetTrigger(FadeInTrigger);
		}
	}

	public void SceneChange()
	{
		SceneChanger.LastTarget = TargetPosition;
		SceneManager.LoadScene(SceneDestination);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//If not player entered, then exit
		if(!other.gameObject.CompareTag("Player")) return;

		//Lock player controls
		PlayerControl.PlayerInstance.CanControl = false;

		FadeAnimator.SetTrigger(FadeOutTrigger);
	}
}