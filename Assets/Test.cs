using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public MarkerPath path;

	//private CharacterMotor motor;


	// Use this for initialization
	void Start () {
		///motor = GetComponent <CharacterMotor> ();
		//motor.inputJump = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = (path.NextMarkerDirection(transform.position));
		transform.Translate(direction * 5.0f * Time.deltaTime);
		//motor.inputMoveDirection = transform.rotation * direction;
		//motor.inputJump = Input.GetButton("Jump");
	}
}
