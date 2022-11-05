using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPiece : GamePiece
{
    public override List<Vector2Int> GetAvailableMoves(ref GamePiece[,] board, int tileCountX, int tileCountY)
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

        // back
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
}
