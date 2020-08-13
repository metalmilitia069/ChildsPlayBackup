using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMechanics), typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{

    private EnemyMechanics _enemyMechanics;
    private Enemy _enemy;

    private NavMeshAgent _navMeshAgent;

    private Transform targetNavPoint;
    private int _navPointIndex = 0;


    

    

    // Start is called before the first frame update
    void Start()
    {
        StartParameters();        
    }

    // Update is called once per frame
    void Update()
    {
        _enemyMechanics.MoveToPoint(targetNavPoint.position);

        if (Vector3.Distance(this.transform.position, targetNavPoint.position) <= 0.3f)//
        {
            SwitchNavPoint();            
        }
    }

    void StartParameters()
    {
        this._enemyMechanics = GetComponent<EnemyMechanics>();
        this._enemy = GetComponent<Enemy>();
        targetNavPoint = NavigationPoints.navPointsArray[_navPointIndex];
        this._navMeshAgent = _enemy.GetComponent<NavMeshAgent>();
    }

    private void SwitchNavPoint()
    {
        if(_navPointIndex >= NavigationPoints.navPointsArray.Length - 1)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _navPointIndex++;
           targetNavPoint = NavigationPoints.navPointsArray[_navPointIndex];            
        }        
    }
}
