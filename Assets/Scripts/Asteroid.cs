using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float xSpin;
	public float ySpin;
	public float zSpin;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		xSpin = Random.Range (10, 50);
		ySpin = Random.Range (10, 50);
		zSpin = Random.Range (10, 50);
		transform.Rotate (xSpin, ySpin, zSpin);
	}
}
