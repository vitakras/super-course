using UnityEngine;
using System.Collections;

public class Flashing : MonoBehaviour {

	public Light RedLight;
	public Light BlueLight;
	public int Number = 1;
	// Use this for initialization
	void Start ()
	{
		Number = 1;
		BlueLight.GetComponent<Light>().intensity = 0;
		RedLight.GetComponent<Light>().intensity = 0;
	}
	// Update is called once per frame
	void Update ()
	{
		if (Number == 1)
		{
			BlueLight.GetComponent<Light>().intensity = 0;
			RedLight.GetComponent<Light>().intensity = 1.5f;
			StartCoroutine(waitforred());
		}
		if (Number == 2)
		{
			RedLight.GetComponent<Light>().intensity = 0;
			BlueLight.GetComponent<Light>().intensity = 1.5f;
			StartCoroutine(waitforblue());
		}
	}
	IEnumerator waitforred()
	{
		yield return new WaitForSeconds (0.2f);
		Number = 2;
	}
	IEnumerator waitforblue()
	{
		yield return new WaitForSeconds (0.2f);
		Number = 1;
	}
}
