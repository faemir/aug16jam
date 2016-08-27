using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    public int health {
        set {
            OnChangeHealth(value);
        }
    }
    public int energy {
        set {
            OnChangeEnergy(value);
        }
    }

    public Transform parent;
    public float lerpPosSpeed = 1.0f;
    public float lerpRotSpeed = 1.0f;

    private RectTransform healthBar;
    private RectTransform energyBar;

    void Start() {
        healthBar = transform.FindChild("Healthbar Foreground").GetComponent<RectTransform>();
        energyBar = transform.FindChild("Boostbar Foreground").GetComponent<RectTransform>();
    }

	void FixedUpdate() {

        transform.position = Vector3.Lerp(transform.position, parent.position, Time.deltaTime * lerpPosSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, Time.deltaTime * lerpRotSpeed);
    }

    void OnChangeHealth(int currentHealth) {
        healthBar.sizeDelta = new Vector2(healthBar.sizeDelta.x, currentHealth);
    }

    void OnChangeEnergy(int currentEnergy) {
        energyBar.sizeDelta = new Vector2(energyBar.sizeDelta.x, currentEnergy);
    }
}
