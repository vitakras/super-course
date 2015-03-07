using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
		text.text = "Steps X" + LoadScoreScene.Steps + "\n";
		text.text += "Jumps X" + LoadScoreScene.Jumps;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
