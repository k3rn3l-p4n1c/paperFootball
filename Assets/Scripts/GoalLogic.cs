using UnityEngine;
using System.Collections;

public class GoalLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter (Collision other) 
	{
		Debug.Log ("Enter");
	}
}