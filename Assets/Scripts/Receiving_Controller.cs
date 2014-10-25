using UnityEngine;
using System.Collections;

public class Receiving_Controller : MonoBehaviour {
	public Game_Manager gmeMgr;
	public string bagOwner;
	public int thisReceiverScore;
	public int contentScore;
	public int HappyScore;
	public int sadScore;
	public int angryScore;
	public GUIText emotionDisplay;
	public enum emotionalStates{
		neutral,content,happy,sad,angry
	}
	public emotionalStates currentEmotion = emotionalStates.neutral;
	public string[] emotions;
	public AudioClip[] kidClips;
	public Camera mainCamera;
	private AudioSource soundSource;


	// Use this for initialization
	void Start () {
		gmeMgr = FindObjectOfType(typeof(Game_Manager)) as Game_Manager;
		soundSource = mainCamera.GetComponent<AudioSource> ();
		emotionDisplay.text = emotions [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll){
		if(coll.gameObject.GetComponent<Candy_Controller>() != null){
			int scoreValue = coll.gameObject.GetComponent<Candy_Controller>().scoreValue;
			gmeMgr.updateScore(scoreValue);
			thisReceiverScore += scoreValue;
			changeEmotionalState(thisReceiverScore);
		}
	}

	public void changeEmotionalState(int scoreCheck){
		int soundID = 2;
		if(scoreCheck >= contentScore && scoreCheck < HappyScore){
			currentEmotion = emotionalStates.content;
			emotionDisplay.text = emotions [1];
			soundID = 1;
		}else if(scoreCheck >= HappyScore){
			currentEmotion = emotionalStates.happy;
			soundID = 1;
			emotionDisplay.text = emotions [2];
		}else if(scoreCheck < sadScore && scoreCheck >= angryScore){
			currentEmotion = emotionalStates.sad;
			soundID = 0;
			emotionDisplay.text = emotions [3];
		}else if(scoreCheck <= angryScore){
			currentEmotion = emotionalStates.angry;
			emotionDisplay.text = emotions [4];
			soundID = 0;
		}else{
			currentEmotion = emotionalStates.neutral;
			emotionDisplay.text = emotions [0];
		}
		if(soundID != 2){
			soundSource.audio.clip = kidClips[soundID];
			soundSource.audio.Play();
		}
	}
}
