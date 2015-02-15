using UnityEngine;
using System.Collections;

public class KillBellow : MonoBehaviour {

	public float die_at = -10f;

	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < this.die_at) {
			Destroy(this.gameObject);
		}
	}
}
