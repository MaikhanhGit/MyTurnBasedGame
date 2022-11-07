using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GamePieceType
{    
    PlayerPiece = 0,
    AIPiece = 1
}
public class GamePiece : MonoBehaviour
{
    public int _team;
    public int _currentX;   
    public int _currentY;
    public GamePieceType _type;

    private Vector3 _desiredPosition;
    private Vector3 _desiredScale = new Vector3 (0.7f, 0.05f, 0.7f);    

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * 10);        
    }

    public virtual List<Vector2Int> GetAvailableMoves(GamePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        // Up        
        if ((_currentY + 1) < tileCountY)
        {
            if (board[_currentX, _currentY + 1] == null)
            {
                r.Add(new Vector2Int(_currentX, _currentY + 1));
            }
        }

        // down
        if ((_currentY - 1) >= 0)
        {
            if (board[_currentX, _currentY - 1] == null)
            {
                r.Add(new Vector2Int(_currentX, _currentY - 1));
            }
        }

        // right
        if ((_currentX + 1) < tileCountX)
        {
            if (board[_currentX + 1, _currentY] == null)
            {
                r.Add(new Vector2Int(_currentX + 1, _currentY));
            }
        }

        // left
        if ((_currentX - 1) >= 0)
        {
            if (board[_currentX - 1, _currentY] == null)
            {
                r.Add(new Vector2Int(_currentX - 1, _currentY));
            }
        }

        // top right
        if ((_currentX + 1) < tileCountX && (_currentY + 1) < tileCountY)
        {
            if ((board[_currentX + 1, _currentY + 1] == null))
            {
                r.Add(new Vector2Int(_currentX + 1, _currentY + 1));
            }
        }

        // bottom right
        if ((_currentX + 1) < tileCountX && (_currentY - 1) >= 0)
        {
            if ((board[_currentX + 1, _currentY - 1] == null))
            {
                r.Add(new Vector2Int(_currentX + 1, _currentY - 1));
            }
        }

        // top left
        if ((_currentX - 1) >= 0 && (_currentY + 1) < tileCountY)
        {
            if ((board[_currentX - 1, _currentY + 1] == null))
            {
                r.Add(new Vector2Int(_currentX - 1, _currentY + 1));
            }
        }

        // bottom left
        if ((_currentX - 1) >= 0 && (_currentY - 1) >= 0)
        {
            if ((board[_currentX - 1, _currentY - 1] == null))
            {
                r.Add(new Vector2Int(_currentX - 1, _currentY - 1));
            }
        }
        
        return r;
    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        _desiredPosition = position;                 

        if (force == true)
        {            
            transform.position = _desiredPosition;
        }
    }

    public virtual void SetScale(Vector3 scale, bool force = false)
    {
        _desiredScale = scale;
        if (force == true)
        {
            transform.localScale = _desiredScale;
        }
    }

    public virtual int CheckForKill(GamePiece[,] board, int tileCountX, int tileCountY, int team)
    {
        int _killCount = 0;        
        
        /// Up        
        if ((_currentY + 2) < tileCountY)
        {
            GamePiece p1 = board[_currentX, _currentY + 1];
            GamePiece p2 = board[_currentX, _currentY + 2];

            if (p1 != null && p1._team != team &&
                (p2 != null && p2._team == team))
            {
                _killCount++;
                Destroy(p1.gameObject);

                Debug.Log(p1);
                p1 = null;

                //r.Add(new Vector2Int(_currentX, _currentY + 1));
                Debug.Log("Killed 1: ");
            }
        }

        // down
        if ((_currentY - 2) >= 0)
        {
            GamePiece p1 = board[_currentX, _currentY - 1];
            GamePiece p2 = board[_currentX, _currentY - 2];

            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                p1 = null;
                //r.Add(new Vector2Int(_currentX, _currentY - 1));
                Debug.Log("Killed 2: ");
            }
        }

        // right
        if ((_currentX + 2) < tileCountX)
        {
            GamePiece p1 = board[_currentX + 1, _currentY];
            GamePiece p2 = board[_currentX + 2, _currentY];

            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                board[_currentX + 1, _currentY] = null;
                //r.Add(new Vector2Int(_currentX + 1, _currentY));
                Debug.Log("Killed 3: ");
            }
        }

        // left
        if ((_currentX - 2) >= 0)
        {
            GamePiece p1 = board[_currentX - 1, _currentY];
            GamePiece p2 = board[_currentX - 2, _currentY];

            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                p1 = null;
                //r.Add(new Vector2Int(_currentX - 1, _currentY));
                Debug.Log("Killed 4: ");
            }
        }

        // top right
        if ((_currentX + 2) < tileCountX && (_currentY + 2) < tileCountY)
        {
            GamePiece p1 = board[_currentX + 1, _currentY + 1];
            GamePiece p2 = board[_currentX + 2, _currentY + 2];

            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                p1 = null;
                //r.Add(new Vector2Int(_currentX + 1, _currentY + 1));
                Debug.Log("Killed 8: ");
            }
        }

        // bottom right
        if ((_currentX + 2) < tileCountX && (_currentY - 2) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY - 1];
            GamePiece p2 = board[_currentX + 2, _currentY - 2];
            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                p1 = null;
                //r.Add(new Vector2Int(_currentX + 1, _currentY - 1));
                Debug.Log("Killed 5: ");
            }
        }

        // top left
        if ((_currentX - 2) >= 0 && (_currentY + 2) < tileCountY)
        {
            GamePiece p1 = board[_currentX - 1, _currentY + 1];
            GamePiece p2 = board[_currentX - 2, _currentY + 2];

            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                p1 = null;
                //r.Add(new Vector2Int(_currentX - 1, _currentY + 1));
                Debug.Log("Killed 6: ");
            }
        }

        // bottom left
        if ((_currentX - 2) >= 0 && (_currentY - 2) >= 0)
        {
            GamePiece p1 = board[_currentX - 1, _currentY - 1];
            GamePiece p2 = board[_currentX - 2, _currentY - 2];

            if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
            {
                _killCount++;
                Destroy(p1.gameObject);
                p1 = null;
                //r.Add(new Vector2Int(_currentX - 1, _currentY - 1));
                Debug.Log("Killed 7");
            }
        }

        return _killCount;
    }
}

