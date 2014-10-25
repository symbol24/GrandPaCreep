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
	public bool isGrabbing;
	public Candy_Controller[] objectsGrabbed;
	public Candy_Controller[] objectsTargetted;
	private int amountOfGrabbedObjects = 0;
	private int amountOfTargets = 0;
	public bool isHandShaking = false;
	public float timeToNextRelease;
	public float delayForNextRelease;

	// Use this for initialization
	void Start () {
		limiterYPositive = transform.position.y;
		timeToNextRelease = Time.time;
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

		if(Time.time > timeToNextRelease){
			ReleaseOneObject();
			timeToNextRelease = Time.time + delayForNextRelease;
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

	public void ReleaseOneObject(){
		if(amountOfGrabbedObjects > 0){
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
}
