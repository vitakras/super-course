using UnityEngine;
using System.Collections;

public class MarkerPath : MonoBehaviour {

	public GameObject marker_holder;

	private int index;
	public Transform [] markers;
	private bool nextMarker = true;

	// Use this for initialization
	void Start () {
		markers = new Transform[marker_holder.transform.childCount];

		for(int i =0; i < marker_holder.transform.childCount; i++) {
			markers[i] = marker_holder.transform.GetChild(i);
		}
		//System.Array.Reverse(markers);
		this.index = 0;
	}

	/* Returns the Directions of the Next marker or Zero vector is at last marker */
	public Vector3 NextMarkerDirection(Vector3 playerPosition) {
		if(index >= this.markers.Length) {
			return Vector3.zero;
		}

		Vector3 heading = markers[index].position - playerPosition;
		//heading.y = 0;

		if ((((int)heading.x) == 0) && (((int)heading.z) == 0)) {
			index++;
			nextMarker = true;
			if(index >= this.markers.Length) {
				return Vector3.zero;
			} else {
				heading = markers[index].position - playerPosition;
			}
		}

		float distance = heading.magnitude;

		return heading / distance;
	}

	public Vector3 NextPosition() {
		if(index < this.markers.Length) {
			return this.markers[index].position;
		}

		return Vector3.zero;
	}

	public bool NextMarker() {
		if (nextMarker) {
			nextMarker = false;
			return true;
		}

		return false;
	}
}
