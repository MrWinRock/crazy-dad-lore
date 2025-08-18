using System.Collections.Generic;
using UnityEngine;

public class SpawnBarrel : MonoBehaviour
{
    [SerializeField] private List<GameObject> barrelParents = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (GameObject parent in barrelParents)
        {
            if (parent != null)
                parent.SetActive(true); // ✅ เปิดทั้งชุด Barrel + TargetRed + BarrelExposive
        }
    }
}