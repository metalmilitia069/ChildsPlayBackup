using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectTileType { Bullet, Missile, Laser };

[CreateAssetMenu(fileName = "New Tower", menuName = "Actos/Towers")]
public class Tower_SO : ScriptableObject
{
    [Header ("Current Tower Stats")]
    public ProjectTileType towerType = ProjectTileType.Bullet;

    [System.Serializable]
    public class TowerLevels
    {   
        public string name;
        public float range;
        public float bulletPerSecond;
        public GameObject projectilePrefab;
    }

    [Header("Tower Levels")]
    public TowerLevels[] TowerLevelsArray;
    
}
