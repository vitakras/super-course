using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class RoadBlock : MonoBehaviour {

	public float offset = 10;  //Offset When to Destroy the player

	//Private Variables
	private AudioSource sound;
	private GameObject player;

	// Use this for initialization
	void Start () {
		this.player = null;
		this.sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.player && this.player.transform.position.z > (this.transform.position.z + this.offset)) {
			Destroy(this.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag.Equals("Player")) {
			//Checks if a clip is attached;
			if (this.sound.clip) {
				this.sound.Play();
			}

			this.player = collision.gameObject;
			this.player.GetComponent<PlayerKinectController>().StopPlayer();
			Destroy(this.GetComponent<Collider>());
		}
	}
}
