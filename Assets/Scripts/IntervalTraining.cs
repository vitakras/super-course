using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[System.Serializable]
public class Interval {
	public uint min_heart_rate;
	public uint max_heart_rate;
	public float time;          //seconds
}

public class IntervalTraining : MonoBehaviour {
	
	public Interval[] intervals;
	public Text text;
	public bool end_game_on_empty = false;

	private Queue<Interval> interval_queue;
	private Interval current;
	private float total_time; //total time;
	private RoadBlockCreator roadBlock;

	//Interval variables;
	//private float distance = 5;
	private uint num_of_jumps = 1;
	//private bool added_jump;

	// Constants

	//private const float min_distance = 10;
	//private const float increase_distance_by = 5;
	//private const float max_distance = 30;
	private const uint max_jumps = 20;
	private const uint min_jumps = 0;

	// Use this for initialization
	void Start () {
		//this.added_jump = false;
		this.total_time = 0;
		this.interval_queue = new Queue<Interval>();
		this.roadBlock = RoadBlockCreator.Instance;

		//Initialize to 
		this.num_of_jumps = this.roadBlock.num_of_road_blocks;

		//Adds intervals into Queue;
		for(int i = 0; i < this.intervals.Length; i++) {
			this.total_time += this.intervals[i].time;
			this.interval_queue.Enqueue(this.intervals[i]);
		}

		this.current = this.interval_queue.Dequeue();

//		Debug.Log(new Time(total_time));
		if(text != null) {
			float minutes = total_time / 60;
			float seconds = total_time % 60;
			text.text = string.Format ("{0:00}:{1:00}", minutes, seconds); 
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GameStateManager.Instance.GameState() != GameStateManager.State.PLAYING) {
			return;
		}

		this.total_time -= Time.deltaTime;
		this.current.time -= Time.deltaTime;

		// Checks if Game ended or to Remove item from the Queue
		if(this.current.time == 0) {
			if((this.interval_queue.Count == 0) && this.end_game_on_empty) {
				Application.LoadLevel("MainMenu");
			} else {
				this.current = this.interval_queue.Dequeue();
			}
		}

		if (this.roadBlock.HitCollider()) {
			// Increases or decreases dificulty based on the heartrate
			if (HRManager.AverageHeartRate == 0) {
				return;
			}

			if (HRManager.AverageHeartRate < this.current.min_heart_rate) {
				this.AddDifficulty();
				this.UpdateRoadBlockCreator();
			} else if (HRManager.AverageHeartRate > this.current.max_heart_rate) {
				this.SubtractDifficulty();
				this.UpdateRoadBlockCreator();
			}
		}


		// Displays text only if it exists
		if(text != null) {
			//Debug.Log(total_time);
			float minutes = (int)(total_time / 60);
			float seconds = total_time % 60;
			text.text = string.Format ("{0:00}:{1:00}", minutes, seconds); 
		}
	}

	private void AddDifficulty() {
		/*
		if (added_jump) {
			this.distance -= IntervalTraining.increase_distance_by;

			// Prevent from going over max distance
			if (this.distance < IntervalTraining.min_distance) {
				this.distance = IntervalTraining.min_distance;
			}

			this.added_jump = false;
		} else { */
			this.num_of_jumps++;

			// Prevent from going voer max jumps
			if (this.num_of_jumps > IntervalTraining.max_jumps) {
				this.num_of_jumps = IntervalTraining.max_jumps;
			}

			//this.added_jump = true;
		//}
	}

	private void SubtractDifficulty() {
		//if (added_jump) {
			this.num_of_jumps--;
			
			// Prevent from going voer max jumps
			if (this.num_of_jumps < IntervalTraining.min_jumps) {
				this.num_of_jumps = IntervalTraining.min_jumps;
			}
			
			//this.added_jump = false;
	/*	} else { 
			this.distance += IntervalTraining.increase_distance_by;
			
			// Prevent from going over max distance
			if (this.distance > IntervalTraining.max_distance) {
				this.distance = IntervalTraining.max_distance;
			}
			
			this.added_jump = true;
		}*/
	}

	private void UpdateRoadBlockCreator() {
		//this.roadBlock.distance = this.distance;
		this.roadBlock.num_of_road_blocks = this.num_of_jumps;
	}
}
