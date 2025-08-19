using System;
using UnityEngine;
using TMPro;

public class TimeScene : MonoBehaviour
{
    private float timeToGo = 70f;
    private PlayerHealth playerHealth;
    public TextMeshProUGUI timeText; // Reference to the TextMeshPro UI component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Obsolete("Use Awake() for initialization instead of Start().")]
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToGo > 0)
        {
            timeToGo -= Time.deltaTime;
        }

        // Update the UI Text with the remaining time
        timeText.text = "Time: " + Mathf.CeilToInt(timeToGo).ToString();

        if (timeToGo <= 0)
        {
            playerHealth.TakeDamage(playerHealth.timeUpDamage);
        }
    }
}
