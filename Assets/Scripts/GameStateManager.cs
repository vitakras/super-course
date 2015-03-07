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

	public static State gameState;
	public GameObject player;
	public Text stateInfo;

	private byte countDown;
	private State state = State.DEFAULT;
	private SpeechManager speechManager;


	// Use this for initialization
	void Start () {
		this.state = State.RESUME;
		// get the speech manager instance
		if(this.speechManager == null)
		{
			this.speechManager = SpeechManager.Instance;
		}

		this.ResetCountDown();
		switch (state) {
		case State.RESUME:
			StartCoroutine("ResumeCountDown");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("p")) {
			if(state == State.PLAYING) {
				PauseGame();
			} else {
				ResumeGame();
			}
		}
	}

	void FixedUpdate ()
	{
		// get the speech manager instance
		if(speechManager == null)
		{
			speechManager = SpeechManager.Instance;
		}
		
		if(speechManager != null && speechManager.IsSapiInitialized())
		{
			if(speechManager.IsPhraseRecognized())
			{
				string sPhraseTag = speechManager.GetPhraseTagRecognized();
				
				switch(sPhraseTag)
				{
				case "PAUSE":
					if (this.state == State.PLAYING) {
						PauseGame();
					}
					break;
				case "RESUME":
					if (this.state == State.PAUSED) {
						ResumeGame();
					}
					break;
				}
				speechManager.ClearPhraseRecognized();
			}
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
		stateInfo.text = "Paused";
		DisablePlayerScripts();
		setGameState();
	}


	IEnumerator ResumeCountDown() {
		for (int i = countDown; i >= 0; i--) {
			stateInfo.text = (countDown > 0) ? "" + this.countDown : "";
			this.countDown--;
			yield return new WaitForSeconds(1f);
		}

		this.ResetCountDown ();
		this.state = State.PLAYING;
		EnablePlayerScripts();
		setGameState();
	}

	/* Private Functions */
	private void ResetCountDown() {
		this.countDown = 3;
	}

	private void setGameState() {
		GameStateManager.gameState = this.state;
	}

	private void DisablePlayerScripts() {
		MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour script in scripts)
		{
			script.enabled = false;
		}
	}
	private void EnablePlayerScripts() {
		MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour script in scripts)
		{
			script.enabled = true;
		}
	}

}
