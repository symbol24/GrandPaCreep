using UnityEngine;
using System.Collections;

public class Game_Manager : MonoBehaviour {
	public int totalScoreValue;
	public GUIText textForScore;
	public Camera mainCamera;
	public AudioClip[] grandPaClips;
	public AudioClip[] doorClips;
	public float DelayToNextGrandPaSound;
	private float timeToNExtGrandPaSound;
	private int grandPaSoundId = 0;


	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		timeToNExtGrandPaSound = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeToNExtGrandPaSound) {
			timeToNExtGrandPaSound = Time.time + DelayToNextGrandPaSound;
			AudioSource.PlayClipAtPoint(grandPaClips[grandPaSoundId], mainCamera.transform.position);
			grandPaSoundId++;
			if(grandPaSoundId >= grandPaClips.Length){
				grandPaSoundId = 0;
			}
		}
	}

	public void updateScore(int scoreValue){
		totalScoreValue += scoreValue;
		textForScore.text = totalScoreValue.ToString();
	}
}
