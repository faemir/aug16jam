using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float userSpeed = 100.0f;
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

		if (readKbd() != new Vector3 (0, 0, 0)) {
			transform.Translate(readKbd() * Time.deltaTime);
			transform.Translate(readKbd() * Time.deltaTime);
		}

	}

	Vector3 readKbd() {
		Vector3 keyboardInput = new Vector3();
		if (Input.GetKey (KeyCode.W)) {
			keyboardInput.z += 1;
		}
		if (Input.GetKey (KeyCode.A)) {
			keyboardInput.x += -1;
		}
		if (Input.GetKey (KeyCode.S)) {
			keyboardInput.z += -1;
		}
		if (Input.GetKey (KeyCode.D)) {
			keyboardInput.x += 1;
		}
		return keyboardInput;
	}
}
