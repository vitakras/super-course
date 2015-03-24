using UnityEngine;
using System.Collections.Generic;


public class PlatformManager : MonoBehaviour {

	//public PlayerController playerScript;   // the scipt Player
	public float positionOffset;  			// Positions offset of the next Terrain
	public float recycleOffset;   			// Positions when to move the Current Terrain player is on
	//public Vector3 startPosition; 			// Initial Position of the Terrain
	public GameObject player;
	public Transform[] prefabs;   			// Various Terrains for the Level

	private Vector3 nextPosition;
	private Queue<Transform> platformQueue;
	private float distance_traveled; 

	// Use this for initialization
	void Start () {
		this.platformQueue = new Queue<Transform> (prefabs.Length);
		//this.nextPosition = this.startPosition;
		Debug.Log("called");
		//Creates all objects and puts them in the Queue;
		foreach (Transform prefab in this.prefabs) {
			this.nextPosition = prefab.position;
			Debug.Log(this.nextPosition);
			this.platformQueue.Enqueue(prefab);
			//this.AddPlatform(o);
		}
		this.nextPosition.z += this.positionOffset;
		Debug.Log(this.nextPosition);
	}
	
	// Update is called once per frame
	void Update () {
		this.distance_traveled = player.transform.position.z; 

		if (this.platformQueue.Peek ().localPosition.z + this.recycleOffset < this.distance_traveled) {
			Debug.Log(this.platformQueue.Peek ().localPosition.z + this.recycleOffset);
			this.Recycle();
		}
	}

	// Reuses platforms that the player walked Passed
	private void Recycle() {
		Transform o = platformQueue.Dequeue ();
		this.AddPlatform (o);
	}

	// Adds platforms to the Manager
	private void AddPlatform(Transform o) {
		o.localPosition = this.nextPosition;
		this.nextPosition.z += this.positionOffset;
		this.platformQueue.Enqueue(o);
	}
}
