using UnityEngine;
using System.Collections;

public class IntroAudio : MonoBehaviour {

	private AudioSource sound;

	// Use this for initialization
	void Start () {
		this.sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
			this.sound.Play();
	}
}
