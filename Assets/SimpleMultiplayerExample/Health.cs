using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public int initialHealth = 100;

    // Sync health
    // When server tells client that currentHealth has changed, OnChangeHealth is executed
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth;
    
    public RectTransform healthBar;

    void Awake() {
        currentHealth = initialHealth;
    }

    [ClientRpc] // server tells client to respawn
    void RpcRespawn() {
        if (isLocalPlayer) { // player gameobject has localauthority on client
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }

    public void TakeDamage(int amount) {
        if (!isServer) return;  // server has authority over currentHealth
        currentHealth -= amount;
        if (currentHealth <= 0) {
            currentHealth = initialHealth;
            RpcRespawn();
        }
    }

    void OnChangeHealth(int currentHealth) {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }


}
