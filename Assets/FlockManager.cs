using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {

	//Public variables
	[Header ("School Settings")]
	public GameObject fishPrefab;
	public int fishCount;
	public Vector3 swimLimits;
	[Range(0f, 1f)]	public float newTargetChance;
	[Range(0f, 1f)]	public float ruleApplicationFrequency;
	public GameObject[] school { get; private set; }
	public Vector3 target {  get; private set; }

	[Header ("Fish Settings")]
	[Range(0f, 5f)]		public float minSpeed;
	[Range(0f, 5f)]		public float maxSpeed;
	[Range(1f, 10f)]	public float rotationSpeed;
	[Range(0f, 5f)]		public float neighbourDistance;
	[Range(1f, 5f)]		public float avoidanceDistance;
	[Range(0f, 5f)]		public float changeSpeedChance;



	void Start () {
		GenerateFlock();
	}

	void Update () {
		if(Random.Range(0f, 1f) < 0.01f) {
			target = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
														Random.Range(-swimLimits.y, swimLimits.y),
														Random.Range(-swimLimits.z, swimLimits.z));
		}
	}

	void GenerateFlock () {
		school = new GameObject[fishCount];

		for (int i = 0; i < fishCount; i++) {
			Vector3 position = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
															Random.Range(-swimLimits.y, swimLimits.y),
															Random.Range(-swimLimits.z, swimLimits.z));
			school[i] = Instantiate(fishPrefab, position, Quaternion.identity, transform);

		}		
	}
}
