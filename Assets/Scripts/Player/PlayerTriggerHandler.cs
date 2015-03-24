using UnityEngine;

using System.Collections;

[RequireComponent (typeof (PlayerMotor))]
public class PlayerTriggerHandler : MonoBehaviour {

	// Other Private Functions
	private PlayerMotor motor;
	private PlayerKinectController controller;
	private Rigidbody rigidbody;
	private KinectManager kinect;

	private bool isPlayerJumped = true;

	// Use this for initialization
	void Start () {
		this.motor = GetComponent<PlayerMotor>();
		this.controller = GetComponent<PlayerKinectController>();
		this.rigidbody = GetComponent<Rigidbody>();
		this.kinect = KinectManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isPlayerJumped && motor.IsGrounded()) {
			this.isPlayerJumped = false;
			this.controller.playerCanMove = true;
		} else if (!this.isPlayerJumped) {
			if(controller.PlayerJumped()) {
				this.isPlayerJumped = true;
				kinect.DeleteGesture(kinect.GetPrimaryUserID(), KinectGestures.Gestures.Jump);
			}
		}
			

	}
	

	void OnCollisionEnter (Collision collision) {
		// Checks if Player Collided with the floor
		if(collision.gameObject.transform.position.y < this.transform.position.y) {
			if(collision.gameObject.tag.Equals("RoadBlock")) {
				this.isPlayerJumped = false;
				kinect.DetectGesture (kinect.GetPrimaryUserID(), KinectGestures.Gestures.Jump);
				//Destroy(collision.collider);
			}
		}
	}
}
