using UnityEngine;
using System.Collections;

public class Game_Manager : MonoBehaviour {
	public int totalScoreValue;
	public GUIText textForScore;


	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateScore(int scoreValue){
		totalScoreValue += scoreValue;
		textForScore.text = totalScoreValue.ToString();
	}
}
