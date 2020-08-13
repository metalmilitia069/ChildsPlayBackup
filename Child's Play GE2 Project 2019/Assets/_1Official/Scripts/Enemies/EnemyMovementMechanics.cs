using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// The enemy will move trought a list of nodes.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovementMechanics : MonoBehaviour
{
    //Game Object Components
    private NavMeshAgent _navMeshAgent;

    //Node & Position
    private Node _currentDestination;
    protected Vector3 _nextDestination;

    //Other
    private float _initialMovementSpeed;
    public float InitialMovementSpeed { get => _initialMovementSpeed; }
    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; set => _navMeshAgent = value; }
    public Node CurrentDestination { get => _currentDestination; }

    private float _inititalStoppingDistance;
    [SerializeField] private float _attackAvoidanceRadious = 0.09375f;
    [SerializeField] private float _attackStopingDistance = 5f;
    [SerializeField] private float _agentAvoidanceRadious = 0.15f;

    /// <summary>
    /// Called immediately after the object is created
    /// </summary>
    void Awake()
    {
        _currentDestination = SpawnManager.GetInstance().SpawnPoint;
        EnemyAgentNaveMeshSetup();
        SetNode(_currentDestination);
    }

    /// <summary>
    /// Basic setup for navmesh agent
    /// </summary>
    public void EnemyAgentNaveMeshSetup()
    {  
        if (_navMeshAgent == null)
        {
            this._navMeshAgent = GetComponent<NavMeshAgent>();
            this._navMeshAgent.radius = _agentAvoidanceRadious; //THIS CONTROLS THE AGENT AVOIDANCE RADIUS.

            _initialMovementSpeed = _navMeshAgent.speed;
            _inititalStoppingDistance = _navMeshAgent.stoppingDistance;
        }
        
        this._navMeshAgent.enabled = true;
        this._navMeshAgent.isStopped = false;
    }

    /// <summary>
    /// Finds the next node in the path
    /// </summary>
    public void GetNextNode(Node currentlyEnteredNode)
    {
        //Don't do anything if the currentNode is not the same as the enteredNode.
        if (_currentDestination != currentlyEnteredNode)
        {
            return;
        }
        if (_currentDestination == null)
        {
            return;
        }

        Node nextNode = _currentDestination.GetNextNode();
        if (nextNode == null)
        {
            if (_navMeshAgent.enabled)
            {
                FinalNodeReached();
            }
            return;
        }

        Debug.Assert(nextNode != _currentDestination);
        SetNode(nextNode);
        MoveToNode();
    }

    /// <summary>
    /// Sets the node to navigate to
    /// </summary>
    /// <param name="node">The node that the enemy will navigate to</param>
    public void SetNode(Node node)
    {
        _currentDestination = node;
    }

    /// <summary>
    /// Make the enemy move towards the currentDestinationNode
    /// </summary>
    public void MoveToNode()
    {
        NavigateTo(_currentDestination.transform.position);
    }

    /// <summary>
    /// Make the enemy move towards any transform passed on to it.
    /// </summary>
    public void NavigateTo(Vector3 nextPoint)
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.SetDestination(nextPoint);
        }
    }

    /// <summary>
    /// Enemy succesfully scapes with your food.
    /// </summary>
    internal void FinalNodeReached()
    { 
        _navMeshAgent.isStopped = true;
        BroadcastMessage("LeaveWithFood");
    }

    /// <summary>
    /// Movement behaviour when encountering a wall.
    /// </summary>
    public void StopAndGo()
    {
        if (_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = false;     
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }
    }

    /// <summary>
    /// Set stoping distance of the nav agent
    /// </summary>
    /// <param name="distance">new distance</param>
    private void SetStoppingDistance(float distance)
    {
        _navMeshAgent.stoppingDistance = distance;
    }

    /// <summary>
    /// Set avoidance radius of the nav agent
    /// </summary>
    /// <param name="radius">new radius</param>
    private void SetAvoidanceRadius(float radius)
    {
        _navMeshAgent.radius = radius;
    }

    /// <summary>
    /// Set the nav agent to attack mode
    /// </summary>
    /// <param name="target">Target to move to</param>
    public void AttackStance(Vector3 target)
    {
        this.NavigateTo(target);
        this.SetStoppingDistance(_attackStopingDistance);
        this.SetAvoidanceRadius(_attackAvoidanceRadious);
    }

    /// <summary>
    /// Set the nav agent to moving mode
    /// </summary>
    public void MoveOnStance()
    {
        this.MoveToNode();
        this.SetStoppingDistance(_inititalStoppingDistance);
        this.SetAvoidanceRadius(_agentAvoidanceRadious);
    }

}


