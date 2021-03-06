﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
public class PlayerMotor : MonoBehaviour {

	public Vector3 velocity;
	public float jumpForce;

	//Private Variables
	public Rigidbody rigidbody;
	public bool isGrounded = false;
	private int ground_ID = -1;

	// Use this for initialization
	void Start () {
		this.rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {}

	// FixedUpdate should be used instead of Update when dealing with Rigidbody.
	void FixedUpdate() {
		if (this.isGrounded && (this.velocity != Vector3.zero)) {
			this.rigidbody.velocity = this.velocity;
		}
	}

	/***********************************COLLISION HANDLERS **************************************/
	void OnCollisionEnter(Collision collision) {
		RaycastHit hit;
		// Checks if Player Collided with the floor
		if(collision.gameObject.tag == "Terrain") {
			this.isGrounded = true;
		} else if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
			if (hit.transform.GetInstanceID() == collision.transform.GetInstanceID()) {
			this.isGrounded = true;
			this.ground_ID = collision.transform.GetInstanceID();
			}
		} 
	}
	
	void OnCollisionExit(Collision collision) {
		//Checks if Player is no Longer Colliding with the floor
		if(collision.gameObject.tag == "Terrain") {
			this.isGrounded = false;
		} else if (this.ground_ID == collision.transform.GetInstanceID()) {
			this.ground_ID = -1;
			this.isGrounded = false;
		} 
	}

	/***************************************PUBLIC FUNCTIONC ***************************************/

	// Makes the Player Jump
	public void Jump() {
		if (this.isGrounded) {
			this.isGrounded = false;
			Vector3 jumpForce = (Vector3.up) * this.jumpForce;
			jumpForce.z = this.jumpForce / 4;
			this.rigidbody.AddForce(jumpForce);
		}
	}

	public bool IsGrounded() {
		return this.isGrounded;
	}
}
