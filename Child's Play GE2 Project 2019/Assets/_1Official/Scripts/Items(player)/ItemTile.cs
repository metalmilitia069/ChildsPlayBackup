using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if (UNITY_EDITOR)
[CustomEditor(typeof(ItemTile))]
[CanEditMultipleObjects]
public class ObjectBuilderEditor : Editor
{
    private ItemTile _myScript;
    private List<ItemTile> _myScripts;

    /// <summary>
    /// Called when the object is initialized, but only if the object is active.
    /// Then called every time the object becomes active
    /// </summary>
    private void OnEnable()
    {
        _myScript = (ItemTile)target;

        _myScripts = new List<ItemTile>();
        foreach (var item in targets)
        {
            _myScripts.Add((ItemTile)item);
        }
    }

    /// <summary>
    /// What shows in the inspector window for the object.
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UpdateObject();
    }

    /// <summary>
    /// called when the object is select and the inspector shown.
    /// </summary>
    public void UpdateObject()
    {      
        if (_myScripts.Count > 1)
        {
            MeshRot90Multiple();
        }
        else
        {
           MeshRot90();     
        }
    }

    /// <summary>
    /// Check if the selected tiles needs to be rotated 90 degree on Y.
    /// </summary>
    private void MeshRot90Multiple()
    {
        foreach (var tiles in _myScripts)
        {
            if (tiles.Rot90Degree)
            {
                tiles.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                tiles.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    /// <summary>
    /// Check if the selected tile needs to be rotated 90 degree on Y.
    /// </summary>
    private void MeshRot90()
    {
        if (_myScript.Rot90Degree)
        {
            _myScript.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            _myScript.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
#endif

/// <summary>
/// Type the tile is
/// </summary>
public enum TileType
{
    Tower,      // Player can place a tower on the tile
    Barrier,    // Player can place a barrier on the tile
    Unavailable // Player cannot place anything on it
}

public class ItemTile : MonoBehaviour
{
    [SerializeField] private TileType _tileType;
    [SerializeField] private GameObject _currentItem;
    public GameObject CurrentItem { get => _currentItem; set => _currentItem = value; }
    public TileType TileType { get => _tileType; }

#if (UNITY_EDITOR)
    [Header("Tile Options")]
    [SerializeField] private bool _rot90Degree;
    [SerializeField] private Mesh _arrowMesh;
    public bool Rot90Degree { get => _rot90Degree; set => _rot90Degree = value; }
#endif
    [SerializeField] private GameObject _barrierHintGO;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    private void Start()
    {
        if (_tileType == TileType.Barrier)
        {
            if (_barrierHintGO != null)
            {
                Instantiate(_barrierHintGO, this.transform.position + (Vector3.up * 3.0f), this.transform.rotation, this.transform);
            }
        }
    }
    /// <summary>
    /// Called every frame when the cursor is above the tile.
    /// </summary>
    private void OnMouseOver()
    {
        // Check if the pointer is over a UI element, if it is exit method.
        if (IsPointerOverUI())
        {
            return;
        }

        if (Input.GetButton("Select"))
        {
            if (_tileType != TileType.Unavailable)
            {
                GameManager.GetInstance().TileSelection(this);
                Shop.GetInstance().MoveToClick();
            }
        }
    }

    /// <summary>
    /// Called once when the cursor exit the tile.
    /// </summary>
    private void OnMouseExit()
    {
        GameManager.GetInstance().TileSelectionCursor.SetActive(false);
        //clickCounter = 0;
    }
    
    /// <summary>
    /// Called once when the cursor enter the tile.
    /// </summary>
    private void OnMouseEnter()
    {
        // Check if the pointer is over a UI element, if it is exit method.
        if (IsPointerOverUI())
        {
            return;
        }

        if (_tileType != TileType.Unavailable)
        {
            GameManager.GetInstance().ShowCursorOnTile(GameManager.GetInstance().TileSelectionCursor, this); 
        }
    }

#if(UNITY_EDITOR)
    /// <summary>
    /// Draw a gizmo in the scene view of unity.
    /// </summary>
    void OnDrawGizmos()
    {
        if (_tileType == TileType.Barrier)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawMesh(_arrowMesh, transform.position + (Vector3.up * 3.0f), transform.rotation); 
        }
    }
#endif

    /// <summary>
    /// Check if the cursor is above a UI object.
    /// </summary>
    /// <returns>True if the cursor is above a UI object.</returns>
    private bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
