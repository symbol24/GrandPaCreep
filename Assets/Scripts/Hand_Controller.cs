using UnityEngine;
using System.Collections;

public class Hand_Controller : MonoBehaviour {
	public KeyCode downKey;
	public KeyCode grabKey;
	private float movementX;
	private float movementZ;
	private float yHeightStart;
	public float movementSpeed;
	public float yMovementSpeed;
	public float ySpeedMultiplier;
	private float yHeightUsed;
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

	// Use this for initialization
	void Start () {
		yHeightStart = transform.position.y;
		yHeightUsed = yHeightStart;
	}
	
	// Update is called once per frame
	void Update () {
		movementX = Input.GetAxis ("Mouse X") * movementSpeed * Time.deltaTime;
		movementZ = Input.GetAxis("Mouse Y") * movementSpeed * Time.deltaTime;
	
		if(Input.GetKey(downKey)){
			yHeightUsed = -(yMovementSpeed * Time.deltaTime);
		}else if(yHeightUsed < yHeightStart){
			yHeightUsed = +(yMovementSpeed * Time.deltaTime * ySpeedMultiplier);
		}

		Vector3 mouvement = new Vector3(movementX, yHeightUsed, movementZ);

		transform.Translate (mouvement, Space.World);

		float newY = transform.position.y;
		float clampedLimitX = Mathf.Clamp(transform.position.x, limiterXNegative, limiterXPositive);
		float clampedLimitY = Mathf.Clamp(transform.position.y, limiterYNegative, yHeightStart);
		float clampedLimitZ = Mathf.Clamp(transform.position.z, limiterZNegative, limiterZPositive);
		transform.position = new Vector3 (clampedLimitX, clampedLimitY, clampedLimitZ);

		if(Input.GetKey(grabKey) && !isGrabbing){
			GrabObjects();
			isGrabbing = true;
		}else if(Input.GetKeyUp(grabKey) && isGrabbing){
			ReleaseObjects();
			isGrabbing = false;
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
