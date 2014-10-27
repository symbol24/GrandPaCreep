using UnityEngine;
using System.Collections;

public class Game_Manager : MonoBehaviour {
	public int totalScoreValue;
	public GUIText textForScore;
	public Camera mainCamera;
	public AudioClip[] grandPaClips;
	public AudioClip[] doorClips;
	public float DelayToNextGrandPaSound;
	public float timeToNExtGrandPaSound;
	private int grandPaSoundId = 0;
	private AudioSource soundSource;


	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		soundSource = mainCamera.GetComponent<AudioSource> ();
		timeToNExtGrandPaSound = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeToNExtGrandPaSound) {
			timeToNExtGrandPaSound = Time.time + DelayToNextGrandPaSound;
			soundSource.audio.clip = grandPaClips[grandPaSoundId];
			soundSource.audio.Play();
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
