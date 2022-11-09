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
    [SerializeField] AudioClip _moveSFX;
    [SerializeField] AudioClip _killedSFX;

    public int _team;
    public int _currentX;   
    public int _currentY;
    public GamePieceType _type;

    private Vector3 _desiredPosition;
    private Vector3 _desiredScale = new Vector3 (0.7f, 0.05f, 0.7f);
    private bool _audioPlayed = false;
    private bool _somethingKilled = false;

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
        PlayMoveSFX();
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
                AudioHelper.PlayClip2D(_killedSFX, 1);
                _killCount++;
                Destroy(p1.gameObject);               
                p1 = null;                
            }
        }

        // down
        if ((_currentY - 2) >= 0)
        {
            GamePiece p1 = board[_currentX, _currentY - 1];
            GamePiece p2 = board[_currentX, _currentY - 2];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;               
            }
        }        

        // right
        if ((_currentX + 2) < tileCountX)
        {
            GamePiece p1 = board[_currentX + 1, _currentY];
            GamePiece p2 = board[_currentX + 2, _currentY];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;                
            }
        }

        // left
        if ((_currentX - 2) >= 0)
        {
            GamePiece p1 = board[_currentX - 1, _currentY];
            GamePiece p2 = board[_currentX - 2, _currentY];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;                
            }
        }

        // top right
        if ((_currentX + 2) < tileCountX && (_currentY + 2) < tileCountY)
        {
            GamePiece p1 = board[_currentX + 1, _currentY + 1];
            GamePiece p2 = board[_currentX + 2, _currentY + 2];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;               
            }
        }

        // bottom right
        if ((_currentX + 2) < tileCountX && (_currentY - 2) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY - 1];
            GamePiece p2 = board[_currentX + 2, _currentY - 2];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;
            }
        }

        // top left
        if ((_currentX - 2) >= 0 && (_currentY + 2) < tileCountY)
        {
            GamePiece p1 = board[_currentX - 1, _currentY + 1];
            GamePiece p2 = board[_currentX - 2, _currentY + 2];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;
            }
        }

        // bottom left
        if ((_currentX - 2) >= 0 && (_currentY - 2) >= 0)
        {
            GamePiece p1 = board[_currentX - 1, _currentY - 1];
            GamePiece p2 = board[_currentX - 2, _currentY - 2];

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;
            }            
        }

        // up & down
        if ((_currentY + 1) < tileCountY && (_currentY - 1) >= 0)
        {
            GamePiece p1 = board[_currentX, _currentY + 1];
            GamePiece p2 = board[_currentX, _currentY - 1];

            _somethingKilled = KillTwo(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount += 2;
            }
        }

        // sides
        if ((_currentX + 1) < tileCountX && (_currentX - 1) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY];
            GamePiece p2 = board[_currentX - 1, _currentY];

            _somethingKilled = KillTwo(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount += 2;
            }
        }

        // right diagonal
        if ((_currentX + 1) < tileCountX && (_currentX - 1) >= 0 &&
            (_currentY + 1) < tileCountY && (_currentY - 1) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY + 1];
            GamePiece p2 = board[_currentX - 1, _currentY - 1];

            _somethingKilled = KillTwo(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount += 2;
            }
        }
        // left diagonal
        if ((_currentX + 1) < tileCountX && (_currentX - 1) >= 0 &&
            (_currentY + 1) < tileCountY && (_currentY - 1) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY - 1];
            GamePiece p2 = board[_currentX - 1, _currentY + 1];

            _somethingKilled = KillTwo(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount += 2;
            }
        }
        
        return _killCount;
    }

    private bool KillOne(GamePiece p1, GamePiece p2, int team)
    {
        if (p1 != null && p1._team != team &&
                p2 != null && p2._team == team)
        {
            AudioHelper.PlayClip2D(_killedSFX, 1);           
            Destroy(p1.gameObject);
            p1 = null;
            _somethingKilled = true;
            return _somethingKilled;
        }
        _somethingKilled = false;
        return _somethingKilled;
    }

    private bool KillTwo(GamePiece p1, GamePiece p2, int team)
    {
        if (p1 != null && p1._team != team &&
                p2 != null && p2._team != team)
        {
            AudioHelper.PlayClip2D(_killedSFX, 1);
            Destroy(p1.gameObject);
            p1 = null;
            Destroy(p2.gameObject);
            p2 = null;
            _somethingKilled = true;
            return _somethingKilled;
        }
        _somethingKilled = false;
        return _somethingKilled;
    }

    private void PlayMoveSFX()
    {
        if(_moveSFX != null)
        {
            AudioHelper.PlayClip2D(_moveSFX, 1);            
        }
    }
}

