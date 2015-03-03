using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

	public enum State {
		DEFAULT,
		PAUSED,
		PLAYING,
		RESUME
	}

	public State state = State.DEFAULT;
	public Text stateInfo;

	private byte countDown;

	// Use this for initialization
	void Start () {
		this.ResetCountDown();
		switch (state) {
		case State.RESUME:
			StartCoroutine("ResumeCountDown");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	/* Static Functionc */
	public void ResumeGame() {
		this.state = State.RESUME;

		StartCoroutine("ResumeCountDown");

		this.ResetCountDown ();
		this.state = State.PLAYING;
		Time.timeScale = 1.0f;

	}

	public void PauseGame() {
		this.state = State.PAUSED;
		Time.timeScale = 0.0f;
	}


	IEnumerator ResumeCountDown() {
		for (int i = countDown; i >= 0; i--) {
			stateInfo.text = (countDown > 0) ? "" + this.countDown : "";
			this.countDown--;
			yield return new WaitForSeconds(1f);
		}
	}

	/* Private Functions */
	private void ResetCountDown() {
		this.countDown = 3;
	}
}
