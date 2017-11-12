using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flock : MonoBehaviour {

	FlockManager myFlock;
	Animator anim;

	float speed = 0;
	float forwardSight = 2f;
	float sideSight = 0.25f;
	
	bool isTurning = false;
	
		
	void Start () {
		myFlock = GetComponentInParent<FlockManager>();
		if (myFlock != null) {
			SetRandomSpeed();
			anim = GetComponent<Animator>();
			anim.SetFloat("cycleOffset", Random.Range(0f, 1f));
			anim.SetFloat("speedMultiplier", speed);
		}
	}
	

	void Update () {
		Bounds bound = new Bounds(myFlock.transform.position, myFlock.swimLimits * 2);

		RaycastHit hitInfo = new RaycastHit();
		Vector3 direction = myFlock.transform.position - transform.position; ;

		if (!bound.Contains(transform.position)) {
			isTurning = true;
		}
		else if (Physics.Raycast(transform.position, transform.forward, out hitInfo, forwardSight)) {
			isTurning = true;
			direction = Vector3.Reflect(transform.forward, hitInfo.normal);
			//Debug.DrawRay(transform.position, transform.forward * forwardSight, Color.red);
		}
		else if (Physics.Raycast(transform.position, transform.right, out hitInfo, sideSight)) {
			isTurning = true;
			direction = Vector3.Reflect(transform.right, hitInfo.normal);
			//Debug.DrawRay(transform.position, transform.right * sideSight, Color.green);
		}
		else if (Physics.Raycast(transform.position, -transform.right, out hitInfo, sideSight)) {
			isTurning = true;
			direction = Vector3.Reflect(-transform.right, hitInfo.normal);
			//Debug.DrawRay(transform.position, -transform.right * sideSight, Color.green);
		}
		else if (Physics.Raycast(transform.position, transform.up, out hitInfo, sideSight)) {
			isTurning = true;
			direction = Vector3.Reflect(transform.up, hitInfo.normal);
			//Debug.DrawRay(transform.position, transform.up * sideSight, Color.green);
		}
		else if (Physics.Raycast(transform.position, -transform.up, out hitInfo, sideSight)) {
			isTurning = true;
			direction = Vector3.Reflect(-transform.up, hitInfo.normal);
			//Debug.DrawRay(transform.position, -transform.up * sideSight, Color.green);
		}
		else {
			isTurning = false;
		}

		if (isTurning) {	
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myFlock.rotationSpeed * Time.deltaTime);
		}
		else {
			if (Random.Range(0f, 1f) < myFlock.changeSpeedChance) {
				SetRandomSpeed();
			}

			if (Random.Range(0f, 1f) < myFlock.ruleApplicationFrequency) {
				ApplyFlockRules();
			}
		}

		transform.Translate(0, 0, speed * Time.deltaTime);
	}

	void SetRandomSpeed () {
		speed = Random.Range(myFlock.minSpeed, myFlock.maxSpeed);
	}

	void ApplyFlockRules () {
		GameObject[] fishGOs = myFlock.school;

		Vector3 aveCenter = Vector3.zero;
		Vector3 aveAvoid = Vector3.zero;
		float aveSpeed = 0f;
		float neighbourDist;
		int groupSize = 0;

		foreach (GameObject fish in fishGOs) {
			if (fish != this) {
				neighbourDist = Vector3.Distance(fish.transform.position, transform.position);
				if (neighbourDist <= myFlock.neighbourDistance) {
					aveCenter += fish.transform.position;
					groupSize++;

					if (neighbourDist < myFlock.avoidanceDistance) {
						aveAvoid += transform.position - fish.transform.position;
					}

					Flock anotherFish = fish.GetComponent<Flock>();
					aveSpeed += anotherFish.speed;
				}
			}
		}

		if (groupSize > 0) {
			aveCenter = (aveCenter / groupSize) + (myFlock.target - transform.position);
			speed = aveSpeed / groupSize;

			Vector3 lookDirection = (aveCenter + aveAvoid) - transform.position;

			if (lookDirection != Vector3.zero) {
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), myFlock.rotationSpeed * Time.deltaTime);
			}
		}
	}
}
