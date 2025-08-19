using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DeathCount;

public class PlayerHealth : MonoBehaviour
{
    public WheelController wheelController;
    private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public GameObject fireEffect;
    public GameObject expoldeEffect;
    public AudioSource carExplode;
    public AudioSource carFire;
    public int takePoliceDamage = 20;
    public int takeRockDamage = 35;
    public int takeTrainDamage = 100;
    public int timeUpDamage = 100;
    public int bombDamage = 50;
    public List<GameObject> portals = new List<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        fireEffect.SetActive(false);
        expoldeEffect.SetActive(false);
        wheelController = GetComponent<WheelController>();
    }

    // ฟังก์ชันรับดาเมจ
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
        {
            return; // ถ้าสุขภาพเป็น 0 หรือ น้อยกว่า 0 จะไม่ทำอะไร
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            CarExplode();
            Invoke(nameof(Die), 2f);
            foreach (GameObject p in portals)
            {
                Destroy(p);
            }
        }
    }

    [Obsolete("Die() is obsolete. Use a custom respawn or game over handler instead of reloading the scene directly.")]
    public void Die()
    {
        if (DeathCounter.instance != null)
        {
            DeathCounter.instance.deathCount++;
            int count = DeathCounter.instance.deathCount;

            if (count == 3)
                FindObjectOfType<DeathReaction>()?.death3Times?.Play();
            else if (count == 5)
                FindObjectOfType<DeathReaction>()?.death5Times?.Play();
            else if (count == 8)
                FindObjectOfType<DeathReaction>()?.death7Times?.Play();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Police"))
        {
            TakeDamage(takePoliceDamage);
        }

        if (other.gameObject.CompareTag("Rock"))
        {
            TakeDamage(takeRockDamage);
        }

        if (other.gameObject.CompareTag("Train"))
        {
            TakeDamage(takeTrainDamage);
        }
    }

    private void CarExplode()
    {
        GetComponent<WheelController>().enabled = false;
        fireEffect.SetActive(true);
        expoldeEffect.SetActive(true);
        carExplode.Play();
        carFire.Play();
    }
}