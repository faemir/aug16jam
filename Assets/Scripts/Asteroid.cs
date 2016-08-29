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
		xSpin = Random.Range (0.0f, 0.2f);
		ySpin = Random.Range (0.0f, 0.2f);
		zSpin = Random.Range (0.0f, 0.2f);
		transform.Rotate (xSpin, ySpin, zSpin);
	}
}
