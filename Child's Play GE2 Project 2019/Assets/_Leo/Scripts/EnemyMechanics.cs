using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMechanics : MonoBehaviour
{

    private NavMeshAgent _navMeshAgent;

    //private Transform targetNavPoint;

    // Start is called before the first frame update
    void Start()
    {
        PlayerNavMeshSetUp();
        //targetNavPoint = NavigationPoints.navPointsArray[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerNavMeshSetUp()
    {
        this._navMeshAgent = GetComponent<NavMeshAgent>();

        this._navMeshAgent.speed = 3;
        this._navMeshAgent.angularSpeed = 1200;
        this._navMeshAgent.acceleration = 20;
    }

    public void MoveToPoint(Vector3 pointToMove)
    {
        this._navMeshAgent.SetDestination(pointToMove);
    }
}
