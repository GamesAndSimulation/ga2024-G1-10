using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    void Start()
    {
        // Get the NavMeshAgent component attached to the enemy
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Check if the player reference is set
        if (player == null)
        {
            Debug.LogError("Player reference is not set in the inspector");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Set the destination of the NavMeshAgent to the player's position
            navMeshAgent.SetDestination(player.position);
        }
    }
}
