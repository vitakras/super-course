using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerMotor))]
[RequireComponent(typeof(AudioSource))]
public class PlayerTriggerHandler : MonoBehaviour {

	// Other Private Functions
	private PlayerMotor motor;
	private PlayerKinectController controller;
	private AudioSource sound;

	private bool isPlayerJumped = true;
	private float player_speed;

	// Use this for initialization
	void Start () {
		this.motor = GetComponent<PlayerMotor>();
		this.controller = GetComponent<PlayerKinectController>();
		this.sound = GetComponent<AudioSource>();

		this.player_speed = this.controller.move_speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isPlayerJumped) {
			this.controller.move_speed = this.player_speed;
		} else if (!this.isPlayerJumped) {
			if(controller.PlayerJumped()) {
				this.isPlayerJumped = true;
			}
		}

	}

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("RoadBlock")) {
			this.sound.Play();
			this.controller.move_speed = 0f;
			this.isPlayerJumped = false;

		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.tag.Equals("RoadBlock")) {


		}
	}
}
