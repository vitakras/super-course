using UnityEngine;
using System.Collections;

public class PauseMenuHandler : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject gameMenu;

	private static PauseMenuHandler _instance;

	public static PauseMenuHandler Instance {
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PauseMenuHandler>();
				//Tell unity not to destroy this object when loading a new scene!
				//DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("escape")) {
			Pause();
		}
	}

	public void Quit() {
		Application.LoadLevel("MainMenu");
	}

	public void Resume() {
		pauseMenu.SetActive(false);
		gameMenu.SetActive(true);
		GameStateManager.Instance.ResumeGame();
	}

	public void Pause() {
		GameStateManager.Instance.PauseGame();
		pauseMenu.SetActive(true);
		gameMenu.SetActive(false);
	}
}
