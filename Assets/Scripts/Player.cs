using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private float distanceTraveled;

	// Use this for initialization
	void Start () {
		this.distanceTraveled = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		this.distanceTraveled = transform.localPosition.z;
	}

	public void Move(Vector3 direction, float speed) {
		this.gameObject.transform.Translate(direction * speed * Time.deltaTime);
	}

	public float GetDistanceTraveled() {
		return this.distanceTraveled;
	}
}
