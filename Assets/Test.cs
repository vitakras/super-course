using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


	private CharacterMotor motor;

	// Use this for initialization
	void Start () {
		motor = GetComponent <CharacterMotor> ();
		//motor.inputJump = true;
	}
	
	// Update is called once per frame
	void Update () {
		motor.inputJump = Input.GetButton("Jump");
	}
}
