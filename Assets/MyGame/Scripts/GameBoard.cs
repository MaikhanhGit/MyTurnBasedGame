using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class GameBoard : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private Material _tileMaterial;
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private float _tileSize = 1.0f;
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField] private Vector3 _boardCenter = Vector3.zero;
    [SerializeField] private float _draggOffset = 1;
    [SerializeField] TextMeshProUGUI _playerRemainingCountText;
    [SerializeField] TextMeshProUGUI _AIRemainingCountText;
    [SerializeField] ParticlePool _particlePool = null;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] _prefabs;
   // [SerializeField] private Material[] _teamMaterial;
    [SerializeField] AudioClip _playerMoveSFX;
    [SerializeField] PlayerTurnGameState _playerTurnGameState;
    [SerializeField] AITurnGameState _AITurnGameState;
    [SerializeField] SetupGameState _setupGameState;
    [SerializeField] CheckGameEndGameState _checkGameEndState;

    
    // LOGIC
    private GamePiece[,] _gamePieces;
    private GamePiece _currentlyDragging;
    private List<Vector2Int> _availableMoves = new List<Vector2Int>();    
    private const float TILE_COUNT_X = 5;
    private const float TILE_COUNT_Y = 5;
    private GameObject[,] _tiles;
    private Camera _currentCamera;
    private Vector2Int _currentHover;
    private Vector3 _bounds;
    private bool _playersTurn = false;
    private bool _AIsTurn = false;    
    private bool _currentPlayersTurn = false;
    private bool _won = false;
    private PlayerTurnGameState PlayerTurnGameState;
    private AITurnGameState AITurnGameState;
    private SetupGameState SetupGameState;
    private CheckGameEndGameState CheckGameEndState;    
    private GamePiece _AIcurrentPiece;
    int _playerTeam = 0;
    int _AITeam = 1;
    private int _playerPieceCount = 7;
    private int _AIPieceCount = 7;
    
    public int PlayerPieceCount => _playerPieceCount;
    public int AIPieceCount => _AIPieceCount;
    public bool CurrentPlayersTurn => _currentPlayersTurn;
    public bool Won => _won;

    private void Awake()
    {       
        PlayerTurnGameState = _playerTurnGameState.GetComponent<PlayerTurnGameState>();
        AITurnGameState = _AITurnGameState.GetComponent<AITurnGameState>();
        SetupGameState = _setupGameState.GetComponent<SetupGameState>();
        CheckGameEndState = _checkGameEndState.GetComponent<CheckGameEndGameState>();        
    }
    private void Update()
    {
        if (_playersTurn == true)
        {
            if (_playerPieceCount <= 0 || _AIPieceCount <= 0 || (_playerPieceCount == 1 && _AIPieceCount == 1))
            {
                PlayerTurnGameState.Exit();
            }

            if (_currentCamera == null)
            {
                _currentCamera = Camera.main;
                return;
            }

            RaycastHit info;
            Ray ray = _currentCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover", "Highlight")))
            {
                // get the indexes of the tiles I've hit
                Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

                // if we're hovering a tile after not hovering any tiles
                if (_currentHover == -Vector2Int.one)
                {
                    _currentHover = hitPosition;
                    SetHoverColor(hitPosition);
                }

                // if we were already hovering a tile, change the previous one
                if (_currentHover != hitPosition)
                {
                    if (ContainsValidMove(ref _availableMoves, _currentHover))
                    {
                        HighLightTiles();
                    }
                    else
                    {
                        SetTileColor();
                    }
                    _currentHover = hitPosition;
                    SetHoverColor(hitPosition);
                }

                // if we press down on the mouse
                if (Input.GetMouseButtonDown(0))
                {
                    if (_gamePieces[hitPosition.x, hitPosition.y] != null &&
                        _gamePieces[hitPosition.x, hitPosition.y]._team == _playerTeam)
                    {
                        _currentlyDragging = _gamePieces[hitPosition.x, hitPosition.y];

                        // get a list of available positions, highlight
                        _availableMoves = _currentlyDragging.GetAvailableMoves(_gamePieces,
                            (int)TILE_COUNT_X, (int)TILE_COUNT_Y);
                        HighLightTiles();
                    }
                }
                // if we releasing the mouse button
                if (_currentlyDragging != null && Input.GetMouseButtonUp(0))
                {
                    Vector2Int previousPosition = new Vector2Int(_currentlyDragging._currentX, _currentlyDragging._currentY);

                    bool validMove = MoveTo(_currentlyDragging, hitPosition.x, hitPosition.y);

                    if (validMove == false)
                    {
                        _currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    }
                    else
                    {
                        RemoveHighLightTiles();
                        // check for kill
                        int killCount = _gamePieces[hitPosition.x, hitPosition.y].CheckForKill(_gamePieces,
                            (int)TILE_COUNT_X, (int)TILE_COUNT_Y, _playerTeam);

                        _AIPieceCount -= killCount;
                        if(_AIPieceCount <= 3)
                        {
                            _AIRemainingCountText.color = Color.red;
                        }
                        
                        _AIRemainingCountText.text = "Opponent's Remaining Pieces: " + _AIPieceCount.ToString();
                        _currentPlayersTurn = true;
                        _playersTurn = false;
                        // end state
                        PlayerTurnGameState.Exit();
                        

                    }                    
                    _currentlyDragging = null;
                    RemoveHighLightTiles();
                }
            }

            else
            {
                if (_currentHover != -Vector2Int.one)
                {
                    if (ContainsValidMove(ref _availableMoves, _currentHover))
                    {
                        HighLightTiles();
                    }
                    else
                    {
                        SetTileColor();
                    }

                    _currentHover = -Vector2Int.one;
                }

                if (_currentlyDragging && Input.GetMouseButtonUp(0))
                {
                    _currentlyDragging.SetPosition(GetTileCenter(_currentlyDragging._currentX, _currentlyDragging._currentY));
                    _currentlyDragging = null;

                    RemoveHighLightTiles();
                }
            }
            
            // if currently dragging a piece, levitate
            if (_currentlyDragging == true)
            {
                Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * _yOffset);
                float distance = 0.0f;
                if (horizontalPlane.Raycast(ray, out distance))
                {
                    _currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * _draggOffset);
                }
            }            
        }

            // if AI's turn
        if (_AIsTurn == true)
        {
            if (_playerPieceCount <= 0 || _AIPieceCount <= 0 || (_playerPieceCount == 1 && _AIPieceCount == 1))
            {
                AITurnGameState.Exit();
            }

            List<Vector2Int> availableMoves = new List<Vector2Int>();
            // pick a random AI piece
            for (int x = 0; x < TILE_COUNT_X; x++)
            {
                for (int y = 0; y < TILE_COUNT_Y; y++)
                {                    
                    if (_gamePieces[x, y] != null && _gamePieces[x, y]._team == _AITeam)
                    {
                        // check if there's available moves for this piece                        
                        availableMoves = _gamePieces[x, y].GetAvailableMoves(_gamePieces,
                        (int)TILE_COUNT_X, (int)TILE_COUNT_Y);
                        if (availableMoves != null && availableMoves.Count > 0)
                        {
                            _AIcurrentPiece = _gamePieces[x, y];
                            break;
                        }
                    }
                }
                if (availableMoves != null && availableMoves.Count > 0)
                {
                    MoveAIpiece(_AIcurrentPiece, availableMoves);
                    _AIsTurn = false;
                    _currentPlayersTurn = false;
                    break;
                }
            }
            _AIsTurn = false;
            if(availableMoves == null)
            {
                _won = true;
            }
            _currentPlayersTurn = false;
            // Game End with Win           
            AITurnGameState.Exit();            

        }            
    }    

    // AI movement
    private void MoveAIpiece(GamePiece piece, List<Vector2Int> AMoves)
    {
        Vector2Int previousPosition = new Vector2Int(piece._currentX, piece._currentY);

        int killCount;

        //find a valid spot                       
        int random = Random.Range(0, AMoves.Count);        
        Vector2Int randomPosition = new Vector2Int(AMoves[random].x, AMoves[random].y);        
        int x = randomPosition.x;
        int y = randomPosition.y;
        // move to valid spot
        _gamePieces[x, y] = piece;
        _gamePieces[previousPosition.x, previousPosition.y] = null;
        PositionSinglePiece(x, y);
        AMoves.Clear();
        // check for kill
        killCount = _gamePieces[x, y].CheckForKill(_gamePieces, (int)TILE_COUNT_X, (int)TILE_COUNT_Y, _AITeam);
        
        _playerPieceCount -= killCount;

        if(_playerPieceCount <= 3)
        {
            _playerRemainingCountText.color = Color.red;
        }
        
        _playerRemainingCountText.text = "Player's Remaining Pieces: " + _playerPieceCount.ToString();
        _currentPlayersTurn = false;
        // change state            
        //AITurnGameState.Exit();
    }
    

    // build gameboard
    public void BuildGameBoard()
    {
        GenerateAllTiles(_tileSize, TILE_COUNT_X, TILE_COUNT_Y);
        SpawnAllPieces();
        PositionAllPieces();
        SetupGameState.Exit();
    }

    // player's turn
    public void ActivatePlayersTurn()
    {        
        _AIsTurn = false;
        _playersTurn = true;
        _AITurnGameState.Exit();
    }

    public void DeactivatePlayersTurn()
    {        
        _playersTurn = false;
        _AIsTurn = true;
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

        // player team
        _gamePieces[0, 1] = SpawnSinglePiece(GamePieceType.PlayerPiece, _playerTeam);
        _gamePieces[4, 1] = SpawnSinglePiece(GamePieceType.PlayerPiece, _playerTeam);
        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            _gamePieces[i, 0] = SpawnSinglePiece(GamePieceType.PlayerPiece, _playerTeam);
        }
        
        
        // AI team
        _gamePieces[0, 3] = SpawnSinglePiece(GamePieceType.AIPiece, _AITeam);
        _gamePieces[4, 3] = SpawnSinglePiece(GamePieceType.AIPiece, _AITeam);
        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            _gamePieces[i, 4] = SpawnSinglePiece(GamePieceType.AIPiece, _AITeam);
        }
    }

    private GamePiece SpawnSinglePiece(GamePieceType type, int team)
    {
        GamePiece piece = Instantiate(_prefabs[(int)type], transform).GetComponent<GamePiece>();

        piece._type = type;
        piece._team = team;
        //piece.GetComponent<MeshRenderer>().material = _teamMaterial[team];

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
        if(_playersTurn == true)
        {
            AudioHelper.PlayClip2D(_playerMoveSFX, 1.5f);
        }
        
        _gamePieces[x, y]._currentX = x;
        _gamePieces[x, y]._currentY = y;
        _gamePieces[x, y].SetPosition(GetTileCenter(x, y), force);

        Vector3 newPosition = GetTileCenter(x, y);
        // get a particle from pool and turn it on
        Particle newParticle = _particlePool.ActivateFromPool();
        // give it the pool so it can return itself whenever it needs
        newParticle.AssignPool(_particlePool);
        // move it to the position we want and enable
        newParticle.transform.position = newPosition;
        newParticle.gameObject.SetActive(true);
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
            _tiles[_availableMoves[i].x, _availableMoves[i].y].GetComponent<MeshRenderer>().material = _highlightMaterial;
        }
    }
    private void RemoveHighLightTiles()
    {
        for (int i = 0; i < _availableMoves.Count; i++)
        {
            _tiles[_availableMoves[i].x, _availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");
            _tiles[_availableMoves[i].x, _availableMoves[i].y].GetComponent<MeshRenderer>().material = _tileMaterial;            
        }
        _availableMoves.Clear();
    }

    // Operations
    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if(moves[i].x == pos.x && moves[i].y == pos.y)
            {
                return true;
            }
        }
        return false;
    }
    private bool MoveTo(GamePiece currentPiece, int x, int y)
    {        
        if(ContainsValidMove(ref _availableMoves, new Vector2(x, y)) == false)
        {
            return false;
        }
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

    private void SetTileColor()
    {
        _tiles[_currentHover.x, _currentHover.y].layer = LayerMask.NameToLayer("Tile");
        _tiles[_currentHover.x, _currentHover.y].GetComponent<MeshRenderer>().material = _tileMaterial;
    }

    private void SetHoverColor(Vector2Int hitPosition)
    {
        _tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
        _tiles[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = _hoverMaterial;
    }     
   
    public int CheckKill()
    {
        int killCount = 0;

        return killCount;
    }

    // flashing text
    private void FlashingText()
    {

    }
   
}
