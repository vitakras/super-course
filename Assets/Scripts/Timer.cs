using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float Seconds = 59;
	public float Minutes = 0;
	public GameObject prefab;
	int count = 0;
	Vector3 playerPos;
	public GameObject player;
	Vector3 prefabPos;
	Quaternion rotation;
	//public PlayerController player;

	// Update is called once per frame
	void Update () {
		if(Seconds <= 0)
		{
			Seconds = 59;
			if(Minutes >= 1)
			{
				Minutes--;
			}
			else
			{
				Minutes = 0;
				Seconds = 0;
				// This makes the guiText show the time as X:XX. ToString.("f0") formats it so there is no decimal place.
			}
		}
		else
		{
			Seconds -= Time.deltaTime;
		}

		if (Seconds == 0 && Minutes == 0 && count == 0) {
			//player = GameObject.FindGameObjectsWithTag("Player");
			playerPos = player.transform.position;
			prefabPos = playerPos;
			prefabPos.x = prefabPos.x + 10;
			prefabPos.z = prefabPos.z + 2;
			rotation = Quaternion.identity;
			rotation.y = 90;
			Instantiate(prefab,prefabPos, Quaternion.identity);
			count++;
		}
	}
}
