using UnityEngine;
using System.Collections;


public class RoadBlockCreator : MonoBehaviour {

	public float offset;             // Offset of where to place the first block
	public float distance;           // Distance between blocks
	public uint num_of_road_blocks;  // Max number of blocks to place
	public GameObject road_block;    // Game object to place

	private static RoadBlockCreator _instance;

	/*************************************** Public Functions *****************************************/
	public static RoadBlockCreator Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<RoadBlockCreator>();
			}
			
			return _instance;
		}
	}
	
	public void SetDistance(float distance) {
		this.distance = distance;
	}

	public void SetNumOfBlocks(uint num) {
		this.num_of_road_blocks = num;
	}

	/***************************************** Trigger Handler *****************************************/
	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Player")) {
			Vector3 newPos = this.transform.position;
			newPos.z += this.offset + this.distance;

			for(int i = 0; i < this.num_of_road_blocks; i++) {
				Instantiate(this.road_block, newPos, road_block.transform.rotation);
				newPos.z += this.distance;
			}

			this.gameObject.transform.position = newPos;
		}
	}
}
