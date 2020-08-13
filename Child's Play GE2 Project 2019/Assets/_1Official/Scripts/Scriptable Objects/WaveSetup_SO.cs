using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave Setup")]
public class WaveSetup_SO : ScriptableObject
{
    [System.Serializable]
    public class Wave
    {
        [SerializeField] private GameObject _enemy;
        [SerializeField] private int _count;

        public GameObject Enemy { get => _enemy; set => _enemy = value; }
        public int Count { get => _count; set => _count = value; }
    }

    [Header("Waves Setup")]
    [SerializeField] private Wave[] _subWaves;
    [Header("Time between enemies")]
    [SerializeField] private float _rate;

    public Wave[] SubWaves { get => _subWaves; set => _subWaves = value; }
    public float Rate { get => _rate; set => _rate = value; }
}
