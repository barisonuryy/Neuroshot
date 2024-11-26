using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMov : MonoBehaviour
{
     public Transform target; // Hedef transform
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgent bileşenini al
        agent = GetComponent<NavMeshAgent>();

        // Hedef kontrolü
        if (target != null)
        {
            // Hedef pozisyonunu ayarla
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogWarning("Target is not assigned!");
        }
    }

    void Update()
    {
        // Hedef sürekli hareket ediyorsa, her karede hedefi güncelle
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        if (target != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                // Hedef geçerli bir NavMesh alanında
                agent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("Target is not on the NavMesh!");
            }
        }
    }
}
