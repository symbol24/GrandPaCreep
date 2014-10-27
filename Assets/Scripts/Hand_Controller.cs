using UnityEngine;
using System.Collections;

public class Hand_Controller : MonoBehaviour {
	public KeyCode downKey;
	public KeyCode grabKey;
	private float mouvementX;
	private float mouvementZ;
	private float mouvementY;
	public float movementSpeed;
	public float yMovementSpeed;
	public float ySpeedMultiplier;
	public float limiterXPositive;
	public float limiterXNegative;
	public float limiterYPositive;
	public float limiterYNegative;
	public float limiterZPositive;
	public float limiterZNegative;
	private bool isGrabbing;
	public Candy_Controller[] objectsGrabbed;
	public Candy_Controller[] objectsTargetted;
	private int amountOfGrabbedObjects = 0;
	private int amountOfTargets = 0;
	private bool isHandShaking = false;
	private float timeToNextRelease;
	public float delayForNextRelease;
	public float shakingLimiter;
	public KeyCode[] lettersToPressKeycodes;
	private KeyCode letter1KeyCode;
	private KeyCode letter2KeyCode;
	public GUIText letter1;
	public GUIText letter2;
	private bool isPressingToPreventShake = false;
	public float timeToNextShake;
	public float delayForNextShake;
	public float timeToNextKeyChange;
	public float delayForNextKeyChange;
	private int lastLetterChanged;

	// Use this for initialization
	void Start () {
		limiterYPositive = transform.position.y;
		timeToNextRelease = Time.time;
		timeToNextShake = Time.time;
		timeToNextKeyChange = Time.time;
		SetKeyToPress (2);
		SetKeyToPress (1);
	}
	
	// Update is called once per frame
	void Update () {
		mouvementX = Input.GetAxis ("Mouse X") * movementSpeed * Time.deltaTime;
		mouvementZ = Input.GetAxis("Mouse Y") * movementSpeed * Time.deltaTime;
	
		if(Input.GetKey(downKey)){
			mouvementY = -(yMovementSpeed * Time.deltaTime);
		}else if(transform.position.y < limiterYPositive){
			mouvementY = +(yMovementSpeed * Time.deltaTime * ySpeedMultiplier);
		}

		Vector3 mouvement = new Vector3(mouvementX, mouvementY, mouvementZ);

		transform.Translate (mouvement, Space.World);

		float clampedLimitX = Mathf.Clamp(transform.position.x, limiterXNegative, limiterXPositive);
		float clampedLimitY = Mathf.Clamp(transform.position.y, limiterYNegative, limiterYPositive);
		float clampedLimitZ = Mathf.Clamp(transform.position.z, limiterZNegative, limiterZPositive);
		transform.position = new Vector3 (clampedLimitX, clampedLimitY, clampedLimitZ);

		if(Input.GetKey(grabKey) && !isGrabbing){
			GrabObjects();
			isGrabbing = true;
			timeToNextRelease = Time.time + delayForNextRelease;
		}else if(Input.GetKeyUp(grabKey) && isGrabbing){
			ReleaseObjects();
			isGrabbing = false;
		}

		if(Time.time > timeToNextKeyChange){
			timeToNextKeyChange = Time.time + delayForNextKeyChange;
			SetKeyToPress(lastLetterChanged);
		}

		if(Input.GetKey(letter1KeyCode) && Input.GetKey(letter2KeyCode)){
			isPressingToPreventShake = true;
		}else{
			isPressingToPreventShake = false;
		}

		if(Time.time > timeToNextShake){
			timeToNextShake = Time.time + delayForNextShake;
			if(!isPressingToPreventShake){
				ShakeHand();
				isHandShaking = true;
			}else{
				isHandShaking = false;
			}
		}

		if(Time.time > timeToNextRelease){
			timeToNextRelease = Time.time + delayForNextRelease;
			ReleaseOneObject(isHandShaking);
		}
	}

	private void GrabObjects(){
		for(int i = 0; i < amountOfTargets; i++){
			objectsGrabbed[i] = objectsTargetted[i];
			objectsGrabbed[i].rigidbody.isKinematic = true;
			objectsGrabbed[i].transform.parent = transform;
			amountOfGrabbedObjects++;
		}
	}

	private void ReleaseObjects(){
		for(int i = 0; i < amountOfGrabbedObjects; i++){
			objectsGrabbed[i].rigidbody.isKinematic = false;
			objectsGrabbed[i].transform.parent = null;
		}
		amountOfGrabbedObjects = 0;
	}

	public void ReleaseOneObject(bool isShaking){
		if(amountOfGrabbedObjects > 0 && isShaking){
			amountOfGrabbedObjects--;
			objectsGrabbed[amountOfGrabbedObjects].rigidbody.isKinematic = false;
			objectsGrabbed[amountOfGrabbedObjects].transform.parent = null;
			if(amountOfGrabbedObjects == 0){
				isGrabbing = false;
			}
		}
	}

	void OnTriggerEnter(Collider coll){
		if(coll.gameObject.GetComponent<Candy_Controller>() != null){
			Candy_Controller candyTarget = coll.gameObject.GetComponent<Candy_Controller>();
			addTarget(candyTarget);
		}
	}

	void OnTriggerExit(Collider coll){
		if(coll.gameObject.GetComponent<Candy_Controller>() != null){
			if(amountOfTargets > 0)	amountOfTargets--;
		}
	}

	private void addTarget(Candy_Controller targetCandy){
		if(amountOfTargets < objectsTargetted.Length){
			objectsTargetted[amountOfTargets] = targetCandy;
			amountOfTargets++;
		}
	}

	private void ShakeHand(){
		float shakeX = Random.Range (-shakingLimiter, shakingLimiter);
		float shakeZ = Random.Range (-shakingLimiter, shakingLimiter);
		Vector3 shakeMouvement = new Vector3 (shakeX, 0, shakeZ);
		transform.Translate (shakeMouvement, Space.World);

		float clampedLimitX = Mathf.Clamp(transform.position.x, limiterXNegative, limiterXPositive);
		float clampedLimitY = Mathf.Clamp(transform.position.y, limiterYNegative, limiterYPositive);
		float clampedLimitZ = Mathf.Clamp(transform.position.z, limiterZNegative, limiterZPositive);
		transform.position = new Vector3 (clampedLimitX, clampedLimitY, clampedLimitZ);	
	}

	private void SetKeyToPress(int letterIDtoChange){
		int keyNumber = Random.Range (0, lettersToPressKeycodes.Length-1);
		KeyCode keyToChangeWith = lettersToPressKeycodes [keyNumber];
		while(keyToChangeWith == letter1KeyCode || keyToChangeWith == letter2KeyCode){
			keyNumber = Random.Range (0, lettersToPressKeycodes.Length-1);
			keyToChangeWith = lettersToPressKeycodes [keyNumber];
		}
		if(letterIDtoChange == 2){
			letter1.text = keyToChangeWith.ToString();
			letter1KeyCode = keyToChangeWith;
			lastLetterChanged = 1;
		}else{
			letter2.text = keyToChangeWith.ToString();
			letter2KeyCode = keyToChangeWith;
			lastLetterChanged = 2;
		}
	}
}
