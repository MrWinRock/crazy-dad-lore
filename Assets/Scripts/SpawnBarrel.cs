using System.Collections.Generic;
using UnityEngine;

public class SpawnBarrel : MonoBehaviour
{
    public List<GameObject> barrel = new List<GameObject>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (barrel == null || barrel.Count == 0)
            {
                Debug.LogError("SpawnBarrel: The barrel list is not assigned or is empty. Please assign barrel GameObjects in the Inspector.");
                return;
            }
            foreach (GameObject b in barrel)
            {
                if (b != null)
                {
                    b.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("SpawnBarrel: One of the barrel GameObjects in the list is null.");
                }
            }
        }
    }
}
