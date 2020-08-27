
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static int CoinCount = 0;

	void Awake () 
	{
		//Object created, increment coin count
		++Coin.CoinCount;
	}

	void OnTriggerEnter(Collider Col)
	{
		//If player collected coin, then destroy object
		if (Col.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		--Coin.CoinCount;

        //Check remaining coins
        if (Coin.CoinCount <= 0)
        {
            //Game is won. Collected all coins
            //Destroy Timer and launch fireworks
            GameObject Timer = GameObject.Find("LevelTimer");
            Destroy(Timer);

            GameObject[] FireworkSystems = GameObject.FindGameObjectsWithTag("Fireworks");

			if (FireworkSystems.Length <= 0) { return; }

            foreach (GameObject GO in FireworkSystems)
            {
                GO.GetComponent<ParticleSystem>().Play();
            }
        }
	}
}