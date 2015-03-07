using UnityEngine;
using System.Collections;

public class LoadScoreScene : MonoBehaviour {

	public PlayerController player;

	static public uint Steps;
	static public uint Jumps;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Player")) {
			LoadScoreScene.Steps = player.GetSteps();
			LoadScoreScene.Jumps = player.GetJumps();

			Application.LoadLevel("ScoreScene");
		}
	}
}
