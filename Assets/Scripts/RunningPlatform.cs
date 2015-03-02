using UnityEngine;
using System.Collections;

public class RunningPlatform : MonoBehaviour {

	public PlayerController player;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		player.KinectMove (Vector3.forward);
	}
}
