using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    //[SerializeField] public Transform movePositionTransform = null;
    public Vector3 movePosition;
    public bool move = false;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (move)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = movePosition;
        }
        else
            navMeshAgent.isStopped = true;
            
    }
}
