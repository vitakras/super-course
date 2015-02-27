using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, KinectGestures.GestureListenerInterface {

	// GUI Text to display the gesture messages.
	public GUIText GestureInfo;


	private float distanceTraveled;
	private KinectManager manager;


	private bool playerKicking;

	private bool leftKneeUp;
	private bool rightKneeUp;

	// Use this for initialization
	void Start () {
		this.distanceTraveled = 0.0f;

		// get Kinect Instance
		this.manager = KinectManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		this.distanceTraveled = transform.localPosition.z;
		if (leftKneeUp || rightKneeUp) {
			this.Move (Vector3.forward, 5f);
			leftKneeUp = false; rightKneeUp = false;
		}
	}

	public void Move(Vector3 direction, float speed) {
		this.gameObject.transform.Translate(direction * speed * Time.deltaTime);
	}

	public float GetDistanceTraveled() {
		return this.distanceTraveled;
	}

	public void UserDetected(long userId, int userIndex)
	{
		// the gestures are allowed for the primary user only
		if(!this.manager || (userId != manager.GetPrimaryUserID()))
			return;
		
		// detect these user specific gestures
		manager.DetectGesture(userId, KinectGestures.Gestures.RightKneeUp);
		manager.DetectGesture(userId, KinectGestures.Gestures.LeftKneeUp);
		
		if(GestureInfo != null)
		{
			GestureInfo.guiText.text = "Test Kicking";
		}
	}
	
	public void UserLost(long userId, int userIndex)
	{
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;
		
		if(GestureInfo != null)
		{
			GestureInfo.guiText.text = string.Empty;
		}
	}
	
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
		if (gesture == KinectGestures.Gestures.LeftKneeUp)
			this.leftKneeUp = true;
		else if (gesture == KinectGestures.Gestures.RightKneeUp)
			this.rightKneeUp = true; 
	}
	
	public bool GestureCompleted (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint, Vector3 screenPos)
	{
		// the gestures are allowed for the primary user only
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return false;
		
		string sGestureText = gesture + " detected";
		
		if (gesture == KinectGestures.Gestures.LeftKneeUp)
			this.leftKneeUp = true;
		else if (gesture == KinectGestures.Gestures.RightKneeUp)
			this.rightKneeUp = true;
		//else if(gesture == KinectGestures.Gestures.RightKneeUp)
			//sGestureText = gesture + "RightKneeUp";
		
			if(GestureInfo != null)
		{
			GestureInfo.guiText.text = sGestureText;
		}
		
		return true;
	}
	
	public bool GestureCancelled (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint)
	{

		if(GestureInfo != null)
		{
			GestureInfo.guiText.text = string.Empty;
		}

		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return false;
		
		// don't do anything here, just reset the gesture state
		return true;
	}
}
