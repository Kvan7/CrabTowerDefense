using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class HealthManager : NetworkBehaviour
{
    [SerializeField] private TMP_Text textMesh; //Reference to damage text
    private Color textColor; //Change the alpha to fade out

    [SyncVar(hook = nameof(OnCurrentHealthChanged))]
    public int health = 100;

    void Start()
    {
        UpdateScoreUI();
    }

    public void ScoreTakeDamage(int amount)
    {
        int oldHealth = health;

        if (health - amount < 0)
        {
            health = 0;
        }
        else
        {
            health -= amount;
        }
        
        OnCurrentHealthChanged(oldHealth, health);
    }

    void UpdateColor()
    {
        if(health > 50)
        {
            float normalizedHealth = (health - 50) / 50f;
            textColor = Color.Lerp(Color.green, Color.yellow, 1 - normalizedHealth);
        }
        else
        {
            float normalizedHealth = health / 50f;
            textColor = Color.Lerp(Color.yellow, Color.red, 1 - normalizedHealth);
        }
    }

    void UpdateScoreUI()
    {
        UpdateColor();
        textMesh.color = textColor;
        textMesh.text = "Health: " + health.ToString();
    }

    private void OnCurrentHealthChanged(int oldHealth, int newHealth)
    {
        if (isServer)
        {
            RpcUpdateHealth(newHealth);
        }
    }

    // Called on clients to update their currentWaveIndex
    [ClientRpc]
    private void RpcUpdateHealth(int newHealth)
    {
        health = newHealth;
        UpdateScoreUI();
    }
}
