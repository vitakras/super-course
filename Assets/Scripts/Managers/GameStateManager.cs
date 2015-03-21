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

	public Text stateInfo;

	// Static singleton instance
	private static GameStateManager _instance;
	private State state = State.DEFAULT;
	private byte countDown = 3;

	public static GameStateManager Instance {
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameStateManager>();
				//Tell unity not to destroy this object when loading a new scene!
				//DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}

	/* Static Functionc */
	public void ResumeGame() {
		this.ResetCountDown ();
		this.state = State.RESUME;
		StartCoroutine("ResumeCountDown");
	}

	public void PauseGame() {
		this.state = State.PAUSED;
	}

	public State GameState() {
		return this.state;
	}


	IEnumerator ResumeCountDown() {
		for (int i = countDown; i >= 0; i--) {
			stateInfo.text = (countDown > 0) ? "" + this.countDown : "";
			this.countDown--;
			yield return new WaitForSeconds(1f);
		}

		this.ResetCountDown ();
		this.state = State.PLAYING;
	}

	/* Private Functions */
	private void ResetCountDown() {
		this.countDown = 3;
	}

}