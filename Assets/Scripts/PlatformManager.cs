using UnityEngine;
using System.Collections.Generic;


public class PlatformManager : MonoBehaviour {

	public PlayerController playerScript;   // the scipt Player
	public float positionOffset;  			// Positions offset of the next Terrain
	public float recycleOffset;   			// Positions when to move the Current Terrain player is on
	public Vector3 startPosition; 			// Initial Position of the Terrain
	public Transform[] prefabs;   			// Various Terrains for the Level

	private Vector3 nextPosition;
	private Queue<Transform> platformQueue;	

	// Use this for initialization
	void Start () {
		this.platformQueue = new Queue<Transform> (prefabs.Length);
		this.nextPosition = this.startPosition;

		//Creates all objects and puts them in the Queue;
		foreach (Transform prefab in this.prefabs) {
			Transform o = (Transform) Instantiate(prefab);
			this.AddPlatform(o);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.platformQueue.Peek ().localPosition.z + this.recycleOffset < this.playerScript.GetDistanceTraveled()) {
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
