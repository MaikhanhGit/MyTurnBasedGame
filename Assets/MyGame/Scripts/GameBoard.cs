using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private Material _tileMaterial;
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private float _tileSize = 1.0f;
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField] private Vector3 _boardCenter = Vector3.zero;
    [SerializeField] private float _draggOffset = 1;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private Material[] _teamMaterial;


    // LOGIC
    private GamePiece[,] _gamePieces;
    private GamePiece _currentlyDragging;
    private List<Vector2Int> _availableMoves = new List<Vector2Int>();
    private List<GamePiece> deadPlayer = new List<GamePiece>();
    private List<GamePiece> deadAI = new List<GamePiece>();
    private const float TILE_COUNT_X = 5;
    private const float TILE_COUNT_Y = 5;
    private GameObject[,] _tiles;
    private Camera _currentCamera;
    private Vector2Int _currentHover;
    private Vector3 _bounds;

    private void Awake()
    {
        GenerateAllTiles(_tileSize, TILE_COUNT_X, TILE_COUNT_Y);
        SpawnAllPieces();
        PositionAllPieces();
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
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover")))
        {
            // get the indexes of the tiles I've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            // if we're hovering a tile after not hovering any tiles
            if (_currentHover == -Vector2Int.one)
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

            // if we press down on the mouse
            if (Input.GetMouseButtonDown(0))
            {
                if(_gamePieces[hitPosition.x, hitPosition.y] != null)
                {
                    // is it our turn?
                    if (true)
                    {
                        _currentlyDragging = _gamePieces[hitPosition.x, hitPosition.y];

                        // get a list of available positions, highlight
                        _availableMoves = _currentlyDragging.GetAvailableMoves(ref _gamePieces, 
                            (int)TILE_COUNT_X, (int)TILE_COUNT_Y);
                        HighLightTiles();
                    }
                }
            }
            // if we releasing the nouse button
            if (_currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(_currentlyDragging._currentX, _currentlyDragging._currentY);

                bool validMove = MoveTo(_currentlyDragging, hitPosition.x, hitPosition.y);
                if(validMove == false)
                {
                    _currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    _currentlyDragging = null;
                }
                else
                {
                    _currentlyDragging = null;
                }
            }
        }
        else
        {
            if (_currentHover != -Vector2Int.one)
            {
                _tiles[_currentHover.x, _currentHover.y].layer = LayerMask.NameToLayer("Tile");
                _tiles[_currentHover.x, _currentHover.y].GetComponent<MeshRenderer>().material = _tileMaterial;
                _currentHover = -Vector2Int.one;
            }

            if(_currentlyDragging == true && Input.GetMouseButtonUp(0))
            {
                _currentlyDragging.SetPosition(GetTileCenter(_currentlyDragging._currentX, _currentlyDragging._currentY));
                _currentlyDragging = null;
            }
        }        

        // if currently dragging a piece, levitate
        if(_currentlyDragging == true)
        {
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * _yOffset);
            float distance = 0.0f;
            if(horizontalPlane.Raycast(ray, out distance))
            {
                _currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * _draggOffset);
            }
        }
    }   

    // Generate the game board
    void GenerateAllTiles(float tileSize, float tileCountX, float tileCountY)
    {
        _yOffset += transform.position.y;
        _bounds = new Vector3((tileCountX / 2)*tileSize, 0, (tileCountX / 2) * tileSize) + _boardCenter;        
        _tiles = new GameObject[(int)tileCountX, (int)tileCountX];
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
        vertices[0] = new Vector3(x * tileSize, _yOffset, y * tileSize) - _bounds;
        vertices[1] = new Vector3(x * tileSize, _yOffset, (y + 1) * tileSize) - _bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, _yOffset, y * tileSize) - _bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, _yOffset, (y + 1) * tileSize) - _bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    // Spawning game pieces
    private void SpawnAllPieces()
    {
        _gamePieces = new GamePiece[(int)TILE_COUNT_X, (int)TILE_COUNT_Y];

        int playerTeam = 0;
        int AITeam = 1;

        // player team
        _gamePieces[0, 1] = SpawnSinglePiece(GamePieceType.PlayerPiece, playerTeam);
        _gamePieces[4, 1] = SpawnSinglePiece(GamePieceType.PlayerPiece, playerTeam);
        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            _gamePieces[i, 0] = SpawnSinglePiece(GamePieceType.PlayerPiece, playerTeam);
        }
        
        
        // AI team
        _gamePieces[0, 3] = SpawnSinglePiece(GamePieceType.AIPiece, AITeam);
        _gamePieces[4, 3] = SpawnSinglePiece(GamePieceType.AIPiece, AITeam);
        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            _gamePieces[i, 4] = SpawnSinglePiece(GamePieceType.AIPiece, AITeam);
        }
    }

    private GamePiece SpawnSinglePiece(GamePieceType type, int team)
    {
        GamePiece piece = Instantiate(_prefabs[(int)type - 1], transform).GetComponent<GamePiece>();

        piece._type = type;
        piece._team = team;
        piece.GetComponent<MeshRenderer>().material = _teamMaterial[team];

        return piece;
    }

    // Positioning game pieces
    private void PositionAllPieces()
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if(_gamePieces[x, y] != null)
                {
                    PositionSinglePiece(x, y, true);
                }                
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        _gamePieces[x, y]._currentX = x;
        _gamePieces[x, y]._currentY = y;
        _gamePieces[x, y].SetPosition(GetTileCenter(x, y), force);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * _tileSize, _yOffset, y * _tileSize) - _bounds + new Vector3(_tileSize/2, 0, _tileSize/2);
    }

    // Highlight Tiles
    private void HighLightTiles()
    {
        for (int i = 0; i < _availableMoves.Count; i++)
        {
            _tiles[_availableMoves[i].x, _availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
        }
    }
    private void RemoveHighLightTiles()
    {
        for (int i = 0; i < _availableMoves.Count; i++)
        {
            _tiles[_availableMoves[i].x, _availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");
            _availableMoves.Clear();
        }
    }

    // Operations
    private bool MoveTo(GamePiece currentPiece, int x, int y)
    {
        Vector2Int previousPosition = new Vector2Int(currentPiece._currentX, currentPiece._currentY);

        // if there's another piece on the target position?
        if (_gamePieces[x, y] != null)
        {
            GamePiece otherGP = _gamePieces[x, y];

            // determine how to eliminate game pieces here
            return false;
        }

        

        _gamePieces[x, y] = currentPiece;
        _gamePieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);

        return true;
    }
    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (_tiles[x, y] == hitInfo)
                    return new Vector2Int(x, y);

        return -Vector2Int.one; // Invalid
    }

    

}
