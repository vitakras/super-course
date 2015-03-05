using UnityEngine;
using System.Collections;

public class MarkerPath : MonoBehaviour {

	public GameObject[] markers;

	private int index;

	// Use this for initialization
	void Start () {
		this.index = 0;
	}

	/* Returns the Directions of the Next marker or Zero vector is at last marker */
	public Vector3 NextMarkerDirection(Vector3 playerPosition) {
		if(index >= this.markers.Length) {
			return Vector3.zero;
		}

		Vector3 heading = markers[index].transform.position - playerPosition;
		//heading.y = 0;

		if ((((int)heading.x) == 0) && (((int)heading.z) == 0)) {
			index++;
			if(index >= this.markers.Length) {
				return Vector3.zero;
			} else {
				heading = markers[index].transform.position - playerPosition;
			}
		}

		float distance = heading.magnitude;

		return heading / distance;
	}
}
