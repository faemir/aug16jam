using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform anchor;
    public float lerpSpeed = 1.0f;

    void FixedUpdate() {
        if (anchor) {
            transform.position = Vector3.Lerp(
                transform.position, 
                anchor.position, 
                Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                anchor.rotation,
                Time.deltaTime * lerpSpeed);
        }
    }
}
