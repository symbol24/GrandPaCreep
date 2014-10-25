using UnityEngine;
using System.Collections;

public class Receiving_Controller : MonoBehaviour {
	public Game_Manager gmeMgr;
	public string bagOwner;
	public int minimumAmountOfCandy;
	public int limitAmountofCandy;
	public int currentAmountOfGoodCandy;
	public int currentAmountOfBadCandy;
	public int totalAmountOfCandy;
	public GUIText emotionDisplay;
	public enum emotionalStates{
		neutral,content,happy,sad,angry
	}
	public emotionalStates currentEmotion = emotionalStates.neutral;
	public string[] emotions;


	// Use this for initialization
	void Start () {
		gmeMgr = FindObjectOfType(typeof(Game_Manager)) as Game_Manager;
		emotionDisplay.text = emotions [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider coll){
		if(coll.gameObject.GetComponent<Candy_Controller>() != null){
			int scoreValue = coll.gameObject.GetComponent<Candy_Controller>().scoreValue;
			Candy_Controller.allCandyTypes thisCandyType = coll.gameObject.GetComponent<Candy_Controller>().thisCandyType;
			gmeMgr.updateScore(scoreValue);
			if(thisCandyType == Candy_Controller.allCandyTypes.good){
				currentAmountOfGoodCandy++;
			}else{
				currentAmountOfBadCandy++;
			}
			totalAmountOfCandy = currentAmountOfGoodCandy + currentAmountOfBadCandy;
		}
	}

	public void changeEmotionalState(Candy_Controller.allCandyTypes passedCandyType){
		if(currentAmountOfGoodCandy > currentAmountOfBadCandy && totalAmountOfCandy < minimumAmountOfCandy){
			currentEmotion = emotionalStates.content;
		}else if(currentAmountOfGoodCandy > currentAmountOfBadCandy && totalAmountOfCandy > minimumAmountOfCandy){
			currentEmotion = emotionalStates.happy;
		}else if(currentAmountOfGoodCandy < currentAmountOfBadCandy && totalAmountOfCandy < minimumAmountOfCandy){
			currentEmotion = emotionalStates.sad;
		}else if(currentAmountOfGoodCandy < currentAmountOfBadCandy && totalAmountOfCandy > minimumAmountOfCandy){
			currentEmotion = emotionalStates.angry;
		}
	}
}
