using UnityEngine;

using System.Collections;

[RequireComponent (typeof (PlayerMotor))]
[RequireComponent(typeof(AudioSource))]
public class PlayerTriggerHandler : MonoBehaviour {

	// Other Private Functions
	private PlayerMotor motor;
	private PlayerKinectController controller;
	private AudioSource sound;
	private Rigidbody rigidbody;

	private bool isPlayerJumped = true;

	// Use this for initialization
	void Start () {
		this.motor = GetComponent<PlayerMotor>();
		this.controller = GetComponent<PlayerKinectController>();
		this.sound = GetComponent<AudioSource>();
		this.rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isPlayerJumped && motor.IsGrounded()) {
			this.isPlayerJumped = false;
			this.controller.playerCanMove = true;
		} else if (!this.isPlayerJumped) {
			if(controller.PlayerJumped()) {
				this.isPlayerJumped = true;
			}
		}
			

	}

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("RoadBlock")) {
			this.isPlayerJumped = false;
			//this.sound.Play();
			this.controller.StopPlayer();
		}
	}

	void OnCollisionEnter (Collision collision) {
		// Checks if Player Collided with the floor
		if(collision.gameObject.transform.position.y < this.transform.position.y) {
			if(collision.gameObject.tag.Equals("RoadBlock")) {
				this.isPlayerJumped = false;
				this.sound.Play();
				this.controller.StopPlayer();
				Destroy(collision.collider);
			}
		}
	}
}
