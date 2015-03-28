using UnityEngine;
using System.Collections;

public class MenuHander : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadDemo () {
		Application.LoadLevel("GameDemo");
	}

	public void ExitGame() {
		Application.Quit();
	}
}
