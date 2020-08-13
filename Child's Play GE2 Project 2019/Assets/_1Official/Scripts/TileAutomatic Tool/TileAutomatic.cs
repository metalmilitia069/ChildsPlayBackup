#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum TileMesh
{
    Side_0,                 // index : 0-
    Side_1,                 // index : 1-
    Sides_2_Corner,         // index : 2-
    Sides_2_OppositeSide,   // index : 3-
    Sides_3,                // index : 4-
    Sides_4,                // index : 5-
    Corner_1,               // index : 6-
    Corners_2_SameSide,     // index : 7-
    Corners_2_OppositeSide, // index : 8-
    Corners_3,              // index : 9-
    Corners_4,              // index : 10-
    Side_1_Corner_1,        // index : 11-
    Sides_2_Corner_1_Corner,// index : 12-
    Side_1_Corners_2        // index : 13-
}

[InitializeOnLoad]
public class TileAutomatic : EditorWindow
{
    private static TileMeshes _tiles = null;

    static TileAutomatic()
    {
        string find = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Tile Meshes_SO")[0]);
        _tiles = (TileMeshes)AssetDatabase.LoadAssetAtPath(find, typeof(TileMeshes));
    }

    private static List<ItemTile> _tileList;
    private static ItemTile _myScript;
    private static MeshFilter _meshFilter;
    private static Editor _setupGUI;
    private static Vector2 _scrollBarPos;

    private static void PopulateTileList()
    {
        _tileList = new List<ItemTile>();
        foreach (var tile in GameObject.FindObjectsOfType<ItemTile>())
        {
            _tileList.Add(tile);
        }
    }

    [MenuItem("TileAutomatic/Refresh Tiles", false, 0)]
    private static void RefreshTiles()
    {
        PopulateTileList();
        foreach (ItemTile tileObject in _tileList)
        {
            if (!tileObject.CompareTag("TilePath"))
            {
                UpdateObject(tileObject);
            }
        }
    }

    [MenuItem("TileAutomatic/Reset/Reset Tower", false, 1)]
    public static void ResetTowerTiles()
    {
        PopulateTileList();
        foreach (ItemTile tileObject in _tileList)
        {
            if (!tileObject.CompareTag("TilePath") && !tileObject.CompareTag("TileBush"))
            {
                _myScript = tileObject;
                _myScript.GetComponent<MeshFilter>().mesh = _tiles.Meshes[0];
                ResetTile();
            }
        }
    }

    [MenuItem("TileAutomatic/Reset/Reset Bush", false, 2)]
    public static void ResetBushTiles()
    {
        PopulateTileList();
        foreach (ItemTile tileObject in _tileList)
        {
            if (!tileObject.CompareTag("TilePath") && !tileObject.CompareTag("TileTower"))
            {
                _myScript = tileObject;
                _myScript.GetComponent<MeshFilter>().mesh = _tiles.Meshes[0];
                ResetTile();
            }
        }
    }

    [MenuItem("TileAutomatic/Reset/Reset All", false, 3)]
    public static void ResetAllTiles()
    {
        PopulateTileList();
        foreach (ItemTile tileObject in _tileList)
        {
            if (!tileObject.CompareTag("TilePath"))
            {
                _myScript = tileObject;
                _myScript.GetComponent<MeshFilter>().mesh = _tiles.Meshes[0];
                ResetTile();
            }
        }
    }

    [MenuItem("TileAutomatic/SnapTiles", false, 4)]
    private static void TileFloorTransform()
    {

        if (EditorUtility.DisplayDialog("Snap Tiles",
            $"Do you want to snap the tile to the grid?", "Yes", "Cancel"))
        {
            foreach (var tile in GameObject.FindObjectsOfType<ItemTile>())
            {
                Vector3 pos = tile.transform.position;
                pos.x = Mathf.Round(pos.x / 5) * 5;
                pos.z = Mathf.Round(pos.z / 5) * 5;
                tile.transform.position = new Vector3(pos.x, pos.y, pos.z);


            }
        }
    }

    [MenuItem("TileAutomatic/Setup/Tiles Meshes")]
    public static void TilesMeshes()
    {
        GetWindow(typeof(TileAutomatic));
        _setupGUI = Editor.CreateEditor(EditorUtility.InstanceIDToObject(_tiles.GetInstanceID()));
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("CLOSE", GUILayout.MaxWidth(100), GUILayout.Height(25)))
        {
            this.Close();
            return;
        }
        GUILayout.EndHorizontal();
        _scrollBarPos = GUILayout.BeginScrollView(_scrollBarPos, false, true, GUILayout.ExpandHeight(true));
        _setupGUI.OnInspectorGUI();

        GUILayout.EndScrollView();
    }

    private static void UpdateObject(ItemTile myScriptInc)
    {
        _myScript = myScriptInc;
        _meshFilter = _myScript.GetComponent<MeshFilter>();

        //0 Sides, i[0]
        /* XXX
         * XTX
         * XXX
         */
        ResetTile();

        if (IsNorth() && IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[0];
        }

        //1 Side, i[1]
        /* OXO
         * TTT
         * TTT
         */
        else if (IsWest() && IsEast() && IsSouth() && IsSouthEast() && IsSouthWest() && !IsNorth())
        {
            _meshFilter.mesh = _tiles.Meshes[1];
            MeshRot90();
            MeshFlipZ();
        }

        //1 Side, i[1]
        /* OTT
         * XTT
         * OTT
         */
        else if (IsNorth() && IsSouth() && IsEast() && IsSouthEast() && !IsWest() && IsNorthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[1];
        }

        //1 Side, i[1]
        /* TTT
         * TTT
         * OXO
         */
        else if (IsNorth() && IsEast() && IsWest() && IsNorthWest() && IsNorthEast() && !IsSouth())
        {
            _meshFilter.mesh = _tiles.Meshes[1];
            MeshRot90();
            MeshFlipX();
        }

        //1 Side, i[1]
        /* TTO
         * TTX
         * TTO
         */
        else if (IsNorth() && IsSouth() && IsWest() && IsNorthWest() && IsSouthWest() && !IsEast())
        {
            _meshFilter.mesh = _tiles.Meshes[1];
            MeshFlipX();
        }

        //2 Side Corner, i[2]
        /* OXO
         * XTT
         * OTT
         */
        else if (IsEast() && IsSouth() && IsSouthEast() && !IsNorth() && !IsWest())
        {
            _meshFilter.mesh = _tiles.Meshes[2];
            MeshFlipZ();
        }

        //2 Side Corner, i[2]
        /* OTT
         * XTT
         * OXO
         */
        else if (IsEast() && IsNorth() && IsNorthEast() && !IsSouth() && !IsWest())
        {

            _meshFilter.mesh = _tiles.Meshes[2];
        }

        //2 Side Corner, i[2]
        /* TTO
         * TTX
         * OXO
         */
        else if (IsWest() && IsNorth() && IsNorthWest() && !IsSouth() && !IsEast())
        {
            _meshFilter.mesh = _tiles.Meshes[2];
            MeshFlipX();
        }

        //2 Side Corner, i[2]
        /* OXO
         * TTX
         * TTO
         */
        else if (IsWest() && IsSouth() && IsSouthWest() && !IsNorth() && !IsEast())
        {
            _meshFilter.mesh = _tiles.Meshes[2];
            MeshFlipX();
            MeshFlipZ();
        }

        //2 Sides opposite, i[3]
        /* OXO
         * TTT
         * OXO
         */
        else if (IsWest() && IsEast() && !IsNorth() && !IsSouth())
        {
            _meshFilter.mesh = _tiles.Meshes[3];
        }

        //2 Sides opposite, i[3]
        /* OTO
         * XTX
         * OTO
         */
        else if (IsNorth() && IsSouth() && !IsEast() && !IsWest())
        {
            _meshFilter.mesh = _tiles.Meshes[3];
            MeshRot90();
        }

        //3 Sides, i[4]
        /* OTO
         * XTX
         * OXO
         */
        else if (IsNorth() && !IsSouth() && !IsEast() && !IsWest())
        {
            _meshFilter.mesh = _tiles.Meshes[4];
            MeshRot90();
            MeshFlipX();
        }

        //3 Sides, i[4]
        /* OXO
         * XTX
         * OTO
         */
        else if (IsSouth() && !IsNorth() && !IsEast() && !IsWest())
        {
            _meshFilter.mesh = _tiles.Meshes[4];
            MeshRot90();
        }

        //3 Sides, i[4]
        /* OXO
         * XTT
         * OXO
         */
        else if (IsEast() && !IsNorth() && !IsSouth() && !IsWest())
        {
            _meshFilter.mesh = _tiles.Meshes[4];
        }

        //3 Sides, i[4]
        /* OXO
         * TTX
         * OXO
         */
        else if (IsWest() && !IsNorth() && !IsSouth() && !IsEast())
        {

            _meshFilter.mesh = _tiles.Meshes[4];
            MeshFlipX();
        }

        //4 Sides, i[5]
        /* OOO
         * OTO
         * OOO
         */
        else if (!IsNorth() && !IsEast() && !IsSouth() && !IsWest())
        {
            _meshFilter.mesh = _tiles.Meshes[5];
        }

        //1 Corner, i[6]
        /* TTT
         * TTT
         * TTX
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[6];
            MeshFlipZ();
        }

        //1 Corner, i[6]
        /* TTX
         * TTT
         * TTT
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[6];
        }

        //1 Corner, i[6]
        /* XTT
         * TTT
         * TTT
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[6];
            MeshFlipX();
        }

        //1 Corner, i[6]
        /* TTT
         * TTT
         * XTT
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[6];
            MeshFlipX();
            MeshFlipZ();
        }

        //2 Corners SameSide, i[7]
        /* TTT
         * TTT
         * XTX
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[7];
            MeshFlipZ();
        }

        //2 Corners SameSide, i[7]
        /* TTX
         * TTT
         * TTX
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[7];
            MeshRot90();
        }

        //2 Corners SameSide, i[7]
        /* XTX
         * TTT
         * TTT
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[7];
        }

        //2 Corners SameSide, i[7]
        /* XTT
         * TTT
         * XTT
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[7];
            MeshRot90();
            MeshFlipZ();
        }

        //2 Corners OppositeSide, i[8]
        /* TTX
         * TTT
         * XTT
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[8];
        }

        //2 Corners OppositeSide, i[8]
        /* XTT
         * TTT
         * TTX
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[8];
            MeshRot90();
        }

        //3 Corners, i[9]
        /* XTX
         * TTT
         * TTX
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[9];
            MeshRot90();
        }

        //3 Corners, i[9]
        /* TTX
         * TTT
         * XTX
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[9];
            MeshFlipZ();
            MeshFlipX();
        }

        //3 Corners, i[9]
        /* XTT
         * TTT
         * XTX
         */
        else if (IsNorth() && IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[9];
            MeshFlipZ();
        }

        //4 Corners, i[10]
        /* XTX
         * TTT
         * XTX
         */
        else if (IsNorth() && !IsNorthEast() && IsEast() && !IsSouthEast() && IsSouth() && !IsSouthWest() && IsWest() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[10];
        }

        //1 Side 1 corner
        /* OXO
         * TTT
         * TTX
         */
        else if (IsWest() && IsEast() && IsSouth() && IsSouthWest() && !IsNorth() && !IsSouthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshFlipZ();
        }

        //1 Side 1 corner
        /* OTX
         * XTT
         * OTT
         */
        else if (IsNorth() && IsSouth() && IsEast() && IsSouthEast() && !IsWest() && !IsNorthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshRot90();
            MeshFlipX();
        }

        //1 Side 1 corner
        /* XTT
         * TTT
         * OXO
         */
        else if (IsWest() && IsEast() && IsNorth() && IsNorthEast() && !IsSouth() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshFlipX();
        }

        //1 Side 1 corner
        /* TTO
         * TTX
         * XTO
         */
        else if (IsNorth() && IsSouth() && IsWest() && IsNorthWest() && !IsEast() && !IsSouthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshRot90();
            MeshFlipZ();
        }

        //1 Side 1 corner
        /* OXO
         * TTT
         * XTT
         */
        else if (IsWest() && IsEast() && IsSouth() && IsSouthEast() && !IsNorth() && !IsSouthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshFlipZ();
            MeshFlipX();
        }

        //1 Side 1 corner
        /* XTO
         * TTX
         * TTO
         */
        else if (IsNorth() && IsSouth() && IsWest() && IsSouthWest() && !IsEast() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshRot90();
            MeshFlipZ();
            MeshFlipX();
        }

        //1 Side 1 corner
        /* TTX
         * TTT
         * OXO
         */
        else if (IsWest() && IsEast() && IsNorth() && IsNorthWest() && !IsSouth() && !IsNorthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
        }

        //1 Side 1 corner
        /* OTT
         * XTT
         * OTX
         */
        else if (IsNorth() && IsSouth() && IsEast() && IsNorthEast() && !IsWest() && !IsSouthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[11];
            MeshRot90();
        }
        //2 Side 1 Corner, i[12]
        /* OXO
         * XTT
         * OTX
         */
        else if (IsEast() && IsSouth() && !IsNorth() && !IsWest() && !IsSouthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[12];
            MeshFlipZ();
        }

        //2 Side 1 Corner, i[12]
        /* OTX
         * XTT
         * OXO
         */
        else if (IsEast() && IsNorth() && !IsSouth() && !IsWest() && !IsNorthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[12];
        }

        //2 Side 1 Corner, i[12]
        /* XTO
         * TTX
         * OXO
         */
        else if (IsWest() && IsNorth() && !IsSouth() && !IsEast() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[12];
            MeshFlipX();
        }

        //2 Side 1 Corner, i[12]
        /* OXO
         * TTX
         * XTO
         */
        else if (IsWest() && IsSouth() && !IsNorth() && !IsEast() && !IsSouthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[12];
            MeshFlipX();
            MeshFlipZ();
        }


        //1 Side 2 corner, i[13]
        /* OXO
         * TTT
         * XTX
         */
        else if (IsWest() && IsEast() && IsSouth() && !IsNorth() && !IsSouthEast() && !IsSouthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[13];
            MeshFlipZ();
        }

        //1 Side 2 corner, i[13]
        /* XTX
         * TTT
         * OXO
         */
        else if (IsWest() && IsEast() && IsNorth() && !IsSouth() && !IsNorthEast() && !IsNorthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[13];
        }

        //1 Side 2 corner, i[13]
        /* OTX
         * XTT
         * OTX
         */
        else if (IsNorth() && IsSouth() && IsEast() && !IsWest() && !IsNorthEast() && !IsSouthEast())
        {
            _meshFilter.mesh = _tiles.Meshes[13];
            MeshRot90();
        }

        //1 Side 2 corner, i[13]
        /* XTO
         * TTX
         * XTO
         */
        else if (IsNorth() && IsSouth() && IsWest() && !IsEast() && !IsNorthWest() && !IsSouthWest())
        {
            _meshFilter.mesh = _tiles.Meshes[13];
            MeshRot90();
            MeshFlipZ();
        }

        _myScript = null;
        _meshFilter = null;
    }

    private static ItemTile IsWest()
    {
        return _tileList.Find(x => x.transform.position.x == _myScript.transform.position.x - _tiles.TileSize &&
                                                          x.transform.position.z == _myScript.transform.position.z &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsEast()
    {
        return _tileList.Find(x => x.transform.position.x == _myScript.transform.position.x + _tiles.TileSize &&
                                                          x.transform.position.z == _myScript.transform.position.z &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsNorth()
    {
        return _tileList.Find(x => x.transform.position.z == _myScript.transform.position.z + _tiles.TileSize &&
                                                          x.transform.position.x == _myScript.transform.position.x &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsSouth()
    {
        return _tileList.Find(x => x.transform.position.z == _myScript.transform.position.z - _tiles.TileSize &&
                                                          x.transform.position.x == _myScript.transform.position.x &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsNorthEast()
    {
        return _tileList.Find(x => x.transform.position.z == _myScript.transform.position.z + _tiles.TileSize &&
                                                          x.transform.position.x == _myScript.transform.position.x + _tiles.TileSize &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsNorthWest()
    {
        return _tileList.Find(x => x.transform.position.z == _myScript.transform.position.z + _tiles.TileSize &&
                                                          x.transform.position.x == _myScript.transform.position.x - _tiles.TileSize &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsSouthEast()
    {
        return _tileList.Find(x => x.transform.position.z == _myScript.transform.position.z - _tiles.TileSize &&
                                                          x.transform.position.x == _myScript.transform.position.x + _tiles.TileSize &&
                                                          x.CompareTag(_myScript.tag));
    }

    private static ItemTile IsSouthWest()
    {
        return _tileList.Find(x => x.transform.position.z == _myScript.transform.position.z - _tiles.TileSize &&
                                                          x.transform.position.x == _myScript.transform.position.x - _tiles.TileSize &&
                                                          x.CompareTag(_myScript.tag));
    }


    private static void ResetTile()
    {
        MeshResetRot90();
        MeshResetX();
        MeshResetZ();
    }

    private static void MeshRot90()
    {
        _myScript.transform.rotation = Quaternion.Euler(0, 90, 0);
        _myScript.Rot90Degree = true;
    }

    private static void MeshResetRot90()
    {
        _myScript.transform.rotation = Quaternion.Euler(0, 0, 0);
        _myScript.Rot90Degree = false;
    }

    private static void MeshFlipZ()
    {
        _myScript.transform.localScale = new Vector3(_myScript.transform.localScale.x, 1, -1);
    }
    private static void MeshResetZ()
    {
        _myScript.transform.localScale = new Vector3(_myScript.transform.localScale.x, 1, 1);
    }

    private static void MeshFlipX()
    {
        _myScript.transform.localScale = new Vector3(-1, 1, _myScript.transform.localScale.z);
    }

    private static void MeshResetX()
    {
        _myScript.transform.localScale = new Vector3(1, 1, _myScript.transform.localScale.z);
    }

    private static void UpdatePathTiles(ItemTile myScriptInc)
    {

    }
} 
#endif