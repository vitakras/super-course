using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour {
	enum Life {
		DEFAULT,
		READY,
		DEAD
	};

	public float alive_timer = 5.0f; // Alive for 5 seconds
	public float fall_sleed = 9.8f; //
	public GameObject apple;
	public Material [] material = new Material[2];

	private float half_time; //used to swap material at half of its life
	private Life life;

	// Use this for initialization
	void Start () {
		this.half_time = alive_timer / 2;
		life = Life.DEFAULT;
	}
	
	// Update is called once per frame
	void Update () {
		if (life != Life.DEAD) {
			alive_timer -= Time.deltaTime;

			if (alive_timer < 0) {
				life = Life.DEAD;
				apple.renderer.material = material [1];
			} else if ((life == Life.DEFAULT) && alive_timer < half_time) {
				life = Life.READY;
				apple.renderer.material = material [0];
			}
		} else {
			this.gameObject.transform.Translate(Vector3.down * Time.deltaTime * this.fall_sleed);
		}
	}
}
