using System;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject barrel;
    public GameObject explosionEffect;
    private float setActiveTime;
    private bool isExploding;

    [SerializeField] private float range;

    public AudioSource bombSound;
    private PlayerHealth playerHealth;

    private Collider[] overlapResults = new Collider[10]; // Buffer for non-allocating overlap

    [Obsolete("Awake is obsolete. Use Start() for initialization instead.")]
    void Awake()
    {
        barrel.SetActive(true);
        explosionEffect.SetActive(false);
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void Explode()
    {
        
        bombSound.Play();
        
        explosionEffect.transform.SetParent(null);
        
        explosionEffect.SetActive(true);
        
        Invoke(nameof(DisableBarrel), 0.3f);
        
        Destroy(explosionEffect, 3f);
        
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, range, overlapResults);
        for (int i = 0; i < hitCount; i++)
        {
            Collider col = overlapResults[i];
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerHealth>().TakeDamage(playerHealth.bombDamage);
            }
        }
    }

    void DisableBarrel()
    {
        barrel.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
