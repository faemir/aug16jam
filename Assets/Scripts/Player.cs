using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float userSpeed = 5.0f;
	Vector3 MousePos;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// camera mouse rotation
		float rotX = Input.GetAxis ("Mouse X");
		float rotY = Mathf.Clamp(Input.GetAxis ("Mouse Y"), -90, 90);
		transform.localEulerAngles = new Vector3 (-rotY, rotX, 0.0f);
		//keyboard movement
		Vector3 vecMove = new Vector3 (Input.GetAxis ("Horizontal") * userSpeed, 0, Input.GetAxis ("Vertical") * userSpeed);
		transform.Translate(vecMove * Time.deltaTime);
	}
}
