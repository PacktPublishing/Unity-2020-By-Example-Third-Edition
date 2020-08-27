using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Weapon : MonoBehaviour 
{
    private ParticleSystem PS;

	void Awake () 
    {
        PS = GetComponent<ParticleSystem>();
	}
	
	void Update () 
    {
        if(Input.GetButtonDown("Fire1") || OVRInput.GetDown(OVRInput.Button.One))
        {
            PS.Play();
        }
        else if (Input.GetButtonUp("Fire1") || OVRInput.GetUp(OVRInput.Button.One))
        {
            PS.Stop();
        }
	}
}
