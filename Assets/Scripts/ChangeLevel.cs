using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("triggered");
		if(other.tag.Equals("Player")) {
			Application.LoadLevel(1);

		}
	}
}
