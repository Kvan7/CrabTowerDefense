using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textMesh; //Reference to damage text
    private Color textColor; //Change the alpha to fade out
    int health = 100;

    void Start()
    {
        UpdateScoreUI();
    }

    public void ScoreTakeDamage(int amount)
    {
        if (health - amount < 0)
        {
            health = 0;
        }
        else
        {
            health -= amount;
        }
        
        UpdateScoreUI();
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
}
