using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (PlayerMotor))]
public class PlayerKinectController : MonoBehaviour, KinectGestures.GestureListenerInterface {

	public float move_speed =  10f; // Speed the player moves at
	public Text DebugInfo; // GUI Text to display the gesture messages.

	public bool debug;
	public bool playerCanMove = true;

	// Handles game State
	public GameStateManager gameState;
	public MarkerPath path;

	// Private Kinect Variables
	private KinectManager manager;
	private KinectGestures.Gestures last_complete_gesture = KinectGestures.Gestures.None;
	private float gesture_progress = -1;
	private float gestureStartTime; //time gesture started;
	private float gestureTime; //time gestrue took;
	private uint num_steps = 0;
	private uint num_jumps = 0;
	private bool isPlayerJump = false;

	// Other Private Functions
	private PlayerMotor motor = new PlayerMotor();
	public float speed;

	// Use this for initialization
	void Start () {
		this.motor = GetComponent<PlayerMotor>();
		this.speed = move_speed;

		//Initializes Kinect
		if (this.manager == null) {
			this.manager = KinectManager.Instance;
		}

		GameStateManager.Instance.PauseGame();
		GameStateManager.Instance.ResumeGame();
	}
	
	// Update is called once per frame
	void Update () {
		// Pauses the Player from using Game
		if (GameStateManager.Instance.GameState() != GameStateManager.State.PLAYING) {
			this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			this.motor.velocity = Vector3.zero;
			return;
		} else if (GameStateManager.Instance.GameState() == GameStateManager.State.PLAYING) {
			this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		}

		//Debug.Log("Jumps" + num_jumps);
		if(motor.IsGrounded()) {
			this.speed = this.move_speed;
		}

		if (this.playerCanMove) {
			if (!this.debug) {
				if (last_complete_gesture == KinectGestures.Gestures.Walk) {
					//Debug.Log("Walking");
					float speed = (this.gestureTime == 0.0f) ? 0 : this.speed / this.gestureTime; // Speed of the Player
				//	Vector3 direction = (path.NextMarkerDirection(transform.position)); // Direction of the Player
				//	direction.y = 0f;


					//this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation,
					                                            //    Quaternion.LookRotation(direction), 5*Time.deltaTime);
					// Moves the Player 
					this.motor.velocity = Vector3.forward * speed;
				} 
			} else {
//				Vector3 direction = (path.NextMarkerDirection(transform.position)); // Direction of the Player
				//direction.y = 0f;

				//this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation,
				                                        //              Quaternion.LookRotation(direction), 5*Time.deltaTime);

				// Moves the Player 
				this.motor.velocity = Vector3.forward * this.speed;//direction * this.speed;
			} 
		} else {
			this.motor.velocity = Vector3.zero;
		}

		if (Input.GetButton("Jump")) {
			this.motor.Jump();
			this.isPlayerJump = true;
		}
	}
	/****************************************PUBLIC FUNCTIONCS ********************************************/

	public bool PlayerJumped() {
		if(this.isPlayerJump) {
			this.isPlayerJump = false;
			return true;
		}

		return false;
	}

	public void StopPlayer() {
		this.playerCanMove = false;
		this.motor.velocity = Vector3.zero;
	}

	public uint GetSteps() {
		return this.num_steps;
	}

	public uint GetJumps() {
		return this.num_jumps;
	}

	/****************************************KINECT INTERFACE FUNCTIONS **********************************/
	public void UserDetected(long userId, int userIndex) {
		// the gestures are allowed for the primary user only
		if (!this.manager || (userId != manager.GetPrimaryUserID ())) {
			return;
		}
		
		// detect these user specific gestures
		//this.manager.DetectGesture (userId, KinectGestures.Gestures.Jump);
		this.manager.DetectGesture (userId, KinectGestures.Gestures.Walk);
		this.manager.DetectGesture (userId, KinectGestures.Gestures.Wave);
		
		if (this.DebugInfo) {
			this.DebugInfo.text = "User Visible";
		}
		
	}
	
	public void UserLost(long userId, int userIndex)
	{
		if (!manager || (userId != manager.GetPrimaryUserID ())) {
			return;
		}

		//Pauses the game If the Player is not Infront of Camera
		if(GameStateManager.Instance.GameState() == GameStateManager.State.PLAYING) {
			PauseMenuHandler.Instance.Pause();

			//this.gameState.PauseGame();
		}
	}
	
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
		// the gestures are allowed for the primary user only
		if (!this.manager || (userId != this.manager.GetPrimaryUserID ())) {
			return;
		}
		
		if ((gesture == KinectGestures.Gestures.Walk) && // check when gesture walk started;
		    (progress == 0.2f) && (gesture_progress != progress)) {
			this.gestureStartTime = Time.time;

		} else if ((gesture == KinectGestures.Gestures.Walk) && 
		           (progress == 0.5f) && (gesture_progress != progress)) {

			this.num_steps++;
		}  else if ((gesture == KinectGestures.Gestures.Jump) &&
		            (progress == 0.5f) && (gesture_progress != progress)) {
			Debug.Log("player Jump began");
			this.gestureStartTime = Time.time;
			this.speed = 0;
			this.motor.Jump();
		} 

		if(gesture == KinectGestures.Gestures.Stop) {
			Debug.Log("Stop started");
		}

		// Sets the progress of the current gesture
		this.gesture_progress = progress;
	}
	
	public bool GestureCompleted (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint, Vector3 screenPos)
	{	
		// the gestures are allowed for the primary user only
		if (!manager || (userId != manager.GetPrimaryUserID ())) {
			return false;
		}
		
		this.gestureTime = Time.time - this.gestureStartTime;
		this.last_complete_gesture = gesture;
		
		switch (gesture) {
		case KinectGestures.Gestures.Jump:;
			this.isPlayerJump = true;
			this.num_jumps++;
			Debug.Log("Jumps" + num_jumps);
			break;
		case KinectGestures.Gestures.Walk:
			this.num_steps++;
			break;
		case KinectGestures.Gestures.Wave:
			if(GameStateManager.Instance.GameState() == GameStateManager.State.PLAYING) {
				PauseMenuHandler.Instance.Pause();
			}
			break;
		}
		
		this.ResetGestureProgress ();

		// Debug Info
		if (this.DebugInfo) {
			this.DebugInfo.text = "Last Complete Gesture: " + gesture;
		}

		return true;
	}
	
	public bool GestureCancelled (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint)
	{
		if (!manager || (userId != manager.GetPrimaryUserID ())) {
			return false;
		}

		this.gestureTime = 0f;
		
		ResetGestureProgress ();
		return true;
	}

	/***************************************************** PRIVATE FUNCTIONS ****************************************/
	
	private void ResetGestureProgress() {
		this.gesture_progress = -1;
	}
}
