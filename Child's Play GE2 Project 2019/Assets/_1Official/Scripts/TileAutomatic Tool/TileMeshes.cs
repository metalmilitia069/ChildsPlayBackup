using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tile Meshes_SO",menuName = "Tiles")]
public class TileMeshes : ScriptableObject
{
    [SerializeField] private int _tileSize = 5;
    [SerializeField] private Mesh[] _meshes;

    public Mesh[] Meshes { get => _meshes; }
    public int TileSize { get => _tileSize; }
}
