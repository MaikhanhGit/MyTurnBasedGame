using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPiece : GamePiece
{
    public override List<Vector2Int> GetAvailableMoves(GamePiece[,] board, int tileCountX, int tileCountY)
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

    public override List<Vector2Int> CheckForKill(GamePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        Debug.Log("currentX: " + _currentX);
        Debug.Log("currentY: " + _currentY);
        /// Up        
        if ((_currentY + 2) < tileCountY)
        {
            if ((board[_currentX, _currentY + 1]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX, _currentY + 2]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX, _currentY + 1]);
                r.Add(new Vector2Int(_currentX, _currentY + 1));
                Debug.Log("Killed 1: ");
            }
        }

        // down
        if ((_currentY - 2) >= 0)
        {
            if ((board[_currentX, _currentY - 1]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX, _currentY - 2]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX, _currentY - 1]);
                r.Add(new Vector2Int(_currentX, _currentY - 1));
                Debug.Log("Killed 2: ");
            }
        }

        // right
        if ((_currentX + 2) < tileCountX)
        {
            if ((board[_currentX + 1, _currentY]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX + 2, _currentY]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX + 1, _currentY]);
                r.Add(new Vector2Int(_currentX + 1, _currentY));
                Debug.Log("Killed 3: ");
            }
        }

        // left
        if ((_currentX - 2) >= 0)
        {
            if ((board[_currentX - 1, _currentY]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX - 2, _currentY]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX - 1, _currentY]);
                r.Add(new Vector2Int(_currentX - 1, _currentY));
                Debug.Log("Killed 4: ");
            }
        }

        // top right
        if ((_currentX + 2) < tileCountX && (_currentY + 2) < tileCountY)
        {
            if ((board[_currentX + 1, _currentY + 1]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX + 2, _currentY + 2]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX + 1, _currentY + 1]);
                r.Add(new Vector2Int(_currentX + 1, _currentY + 1));
                Debug.Log("Killed 8: ");
            }
        }

        // bottom right
        if ((_currentX + 2) < tileCountX && (_currentY - 2) >= 0)
        {
            if ((board[_currentX + 1, _currentY - 1]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX + 2, _currentY - 2]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX + 1, _currentY - 1]);
                r.Add(new Vector2Int(_currentX + 1, _currentY - 1));
                Debug.Log("Killed 5: ");
            }
        }

        // top left
        if ((_currentX - 2) >= 0 && (_currentY + 2) < tileCountY)
        {
            if ((board[_currentX - 1, _currentY + 1]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX - 2, _currentY + 2]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX - 1, _currentY + 1]);
                r.Add(new Vector2Int(_currentX - 1, _currentY + 1));
                Debug.Log("Killed 6: ");
            }
        }

        // bottom left
        if ((_currentX - 2) >= 0 && (_currentY - 2) >= 0)
        {
            if ((board[_currentX - 1, _currentY - 1]?._team != board[_currentX, _currentY]._team) &&
                (board[_currentX - 2, _currentY - 2]?._team == board[_currentX, _currentY]._team))
            {
                Destroy(board[_currentX - 1, _currentY - 1]);
                r.Add(new Vector2Int(_currentX - 1, _currentY - 1));
                Debug.Log("Killed 7");                
            }
        }

        return r;
    }
}
