using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The child of the queen.
/// </summary>
[System.Serializable]
public class AntChild
{
    [SerializeField] private GameObject _antGO;
    [SerializeField] private float _spawnWeight;
    public GameObject AntGO { get => _antGO; }
    public float SpawnWeight { get => _spawnWeight; }
}

public class EnemyQueen : Enemy
{
    [Header("Spawning options")]
    [SerializeField, Range(1, 5)] private float secBetweenEggLaying = 2.0f;
    [SerializeField, Range(5, 15)] private float eggHatchTime = 10.0f;
    [Header("Set weight in descending order, highest to lowest.")]
    [SerializeField] private AntChild[] _antChildren;
    [SerializeField] private GameObject _eggGO;
    private Vector3 offset;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    protected override void Start()
    {
        base.Start();
        offset = Vector3.up * eMMCode.NavMeshAgent.baseOffset;
        InvokeRepeating("SpawningChildren", secBetweenEggLaying, secBetweenEggLaying);
    }

    /// <summary>
    /// Method that is invoked repeating when the Queen spawns.
    /// It will repeats it self base on set variable secBetweenEggLaying
    /// It will randomly choose an ant to spawn based on the setup.
    /// </summary>
    public void SpawningChildren()
    {
        float randomNumber = Random.Range(0.0f, 100.0f);
        int spawnIndex = -1;

        float weightValue = 0;
        for (int i = 0; i < _antChildren.Length; i++)
        {
            weightValue += _antChildren[i].SpawnWeight;
            if (randomNumber <= weightValue)
            {
                spawnIndex = i;
                break;
            }
        }

        if (spawnIndex >= 0)
        {
            StartCoroutine(SpawnAnt(spawnIndex));
        }
        
    }

    /// <summary>
    /// The coroutine called when the queen lays an egg. 
    /// After a certain time it will hatch.
    /// If the queen is killed before the egg hatch the egg will just disapear.
    /// </summary>
    /// <param name="spawnIndex">The index of the enemy to spawn</param>
    /// <returns></returns>
    private IEnumerator SpawnAnt(int spawnIndex)
    {
        const float DISTANCE_CAST = 1.5f;
        float leftDistance = DISTANCE_CAST;
        float rightDistance = DISTANCE_CAST;
        if (Physics.Raycast(this.transform.position + (this.transform.up) - offset, -this.transform.right, out RaycastHit hit, DISTANCE_CAST))
        {
            Debug.DrawLine(this.transform.position + (this.transform.up) - offset, hit.point, Color.blue, 3.0f);
            leftDistance = hit.distance - 0.5f;
        }
        else if (Physics.Raycast(this.transform.position + (this.transform.up) - offset, this.transform.right, out hit, DISTANCE_CAST))
        {
            Debug.DrawLine(this.transform.position + (this.transform.up) - offset, hit.point, Color.blue, 3.0f);
            rightDistance = hit.distance - 0.5f;
        }
        float randomPosition = Random.Range(-leftDistance, rightDistance);

        Vector3 spawnPosition = this.transform.position - offset + (this.transform.right * randomPosition) - this.transform.forward;
        Quaternion spawnRotation = this.transform.rotation;

        GameObject egg = Instantiate(_eggGO, spawnPosition , spawnRotation, LevelManager.GetInstance().CurrentLevelGO.transform);
        egg.GetComponent<Animator>().SetFloat("Speed", Random.Range(1 - 0.2f, 1 + 0.2f));
        egg.transform.Rotate(Vector3.up * Random.Range(-180.0f, 180.0f));
        float randomHatching = Random.Range(0, eggHatchTime) + eggHatchTime;
        Destroy(egg, randomHatching);

        yield return new WaitForSeconds(randomHatching);
        if (_isDying)
        {
            yield break;
        }

        GameObject ant = Instantiate(_antChildren[spawnIndex].AntGO, spawnPosition, spawnRotation, null);
        ant.transform.Rotate(Vector3.up * Random.Range(-180.0f, 180.0f));
        ant.GetComponentInChildren<Enemy>().EMMCode.SetNode(this.eMMCode.CurrentDestination);
        ant.GetComponentInChildren<EnemyMovementMechanics>().MoveToNode();

    }

    /// <summary>
    /// Called when enemy dies.
    /// </summary>
    protected override void Die()
    {
        base.Die();

        Destroy(this.gameObject, 15);
        CancelInvoke("SpawningChildren");
    }
}
