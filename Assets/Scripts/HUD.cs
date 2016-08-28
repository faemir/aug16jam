using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    public Transform parent;
    public float lerpPosSpeed = 1.0f;
    public float lerpRotSpeed = 1.0f;

    public RectTransform healthBar;
    public RectTransform energyBar;

    // lerp HUD to a parent position to create a lag effect when the camera moves
    // do this in FixedUpdate because player movement is controlled on FixedUpdate
	void FixedUpdate() {
        if (!parent) return;
        transform.position = parent.position;
        transform.rotation = Quaternion.Lerp(
            transform.rotation, parent.rotation, Time.fixedDeltaTime * lerpRotSpeed);
    }

    public void OnChangeHealth(int currentHealth) {
        healthBar.sizeDelta = new Vector2(healthBar.sizeDelta.x, currentHealth);
    }

    public void OnChangeEnergy(int currentEnergy) {
        energyBar.sizeDelta = new Vector2(energyBar.sizeDelta.x, currentEnergy);
    }
}
