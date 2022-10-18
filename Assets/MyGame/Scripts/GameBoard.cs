using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] Material _tileMaterial;
    [SerializeField] Material _hoverMaterial;

    // LOGIC
    private const int TILE_COUNT_X = 5;
    private const int TILE_COUNT_Y = 5;
    private GameObject[,] _tiles;
    private Camera _currentCamera;
    private Vector2Int _currentHover;

    private void Awake()
    {
        GenerateAllTiles(1, TILE_COUNT_X, TILE_COUNT_Y);
    }

    private void Update()
    {
        if(_currentCamera == null)
        {
            _currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = _currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile")))
        {
            // get the indexes of the tiles I've hit
            Vector2Int hitPosition = LookupFileIndex(info.transform.gameObject);

            // if we're hoving a tile after not hovering any tiles
            if(_currentHover == -Vector2Int.one)
            {
                _currentHover = hitPosition;
                _tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                _tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = _hoverMaterial;
            }

            // if we were already hovering a tile, change the previous one
            if (_currentHover != hitPosition)
            {
                _tiles[_currentHover.x, _currentHover.y].layer = LayerMask.NameToLayer("Tile");
                _tiles[_currentHover.x, _currentHover.y].GetComponent<MeshRenderer>().material = _tileMaterial;
                _currentHover = hitPosition;
                _tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                _tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = _hoverMaterial;
            }
            else
            {
                if(_currentHover != -Vector2Int.one)
                {
                    _tiles[_currentHover.x, _currentHover.y].layer = LayerMask.NameToLayer("Tile");
                    _tiles[_currentHover.x, _currentHover.y].GetComponent<MeshRenderer>().material = _tileMaterial;
                    _currentHover = -Vector2Int.one;
                }
            }
        }
    }

    // Generate the game board
    void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        _tiles = new GameObject[tileCountX, tileCountX];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                _tiles[x, y] = GenerateSingleTile(tileSize, x, y);

    }
        
    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = _tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, 0, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y + 1) * tileSize);
        vertices[2] = new Vector3((x + 1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, 0, (y + 1) * tileSize);

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    // Operations
    private Vector2Int LookupFileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (_tiles[x, y] == hitInfo)
                    return new Vector2Int(x, y);

        return -Vector2Int.one; // Invalid

           

        
    }
}
