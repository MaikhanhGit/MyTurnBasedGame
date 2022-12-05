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
    [SerializeField] ParticleSystem _killedFX;
    [SerializeField] Animator _happyAnimation;
    [SerializeField] float _happyAnimationDuration = 2;
    [SerializeField] float _destroyDelay = 2f;

    public int _team;
    public int _currentX;   
    public int _currentY;
    public GamePieceType _type;

    private Vector3 _desiredPosition;
    private Vector3 _desiredScale = new Vector3 (0.7f, 0.05f, 0.7f);
    private bool _audioPlayed = false;
    private bool _somethingKilled = false;
    private ParticleSystem _killedVisual;
    private Animator _animation;

    private void Awake()
    {
        _animation = _happyAnimation.GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, Time.deltaTime * 10);
        //transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * 10);        
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

            _somethingKilled = KillOne(p1, p2, team);
            if (_somethingKilled == true)
            {
                _killCount++;
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
        // up & down Repeated kill count FIX
        if ((_currentY + 1) < tileCountY && (_currentY - 1) >= 0)
        {
            GamePiece p1 = board[_currentX, _currentY + 1];
            GamePiece p2 = board[_currentX, _currentY - 1];

            // top 2 down 1
            if ((_currentY + 2) < tileCountY)
            {                
                GamePiece p3 = board[_currentX, _currentY + 2];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }
            // top 1 down 2
            if ((_currentY - 2) >= 0)
            {
                GamePiece p3 = board[_currentX, _currentY - 2];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }
            // top 2 down 2
            if ((_currentY + 2) < tileCountY && (_currentY - 2) >= 0)
            {
                GamePiece p3 = board[_currentX, _currentY + 2];
                GamePiece p4 = board[_currentX, _currentY - 2];

                _somethingKilled = KillNone(p1, p2, p3, p4, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 2 double counts
                    _killCount -= 2;
                }
            }
        }
        // sides Repeated kill count FIX
        if ((_currentX + 1) < tileCountX && (_currentX - 1) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY];
            GamePiece p2 = board[_currentX - 1, _currentY];
            // right 2 left 1
            if ((_currentX + 2) < tileCountX)
            {                
                GamePiece p3 = board[_currentX + 2, _currentY];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }
            // right 1 left 2
            if ((_currentX - 2) >= 0)
            {
                GamePiece p3 = board[_currentX - 2, _currentY];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }
            // right 2 left 2
            if ((_currentX + 2) < tileCountX && (_currentX - 2) >= 0)
            {
                GamePiece p3 = board[_currentX + 2, _currentY];
                GamePiece p4 = board[_currentX - 2, _currentY];

                _somethingKilled = KillNone(p1, p2, p3, p4, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 2 double counts
                    _killCount -= 2;
                }
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

        // right diagonal Repeated kill count FIX
        if ((_currentX + 1) < tileCountX && (_currentX - 1) >= 0 &&
            (_currentY + 1) < tileCountY && (_currentY - 1) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY + 1];
            GamePiece p2 = board[_currentX - 1, _currentY - 1];

            // top 2 down 1
            if ((_currentX + 2) < tileCountX && (_currentY + 2) < tileCountY)
            {                
                GamePiece p3 = board[_currentX + 2, _currentY + 2];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }
            // top 1 down 2
            if ((_currentX - 2) >= 0 && (_currentY - 2) >= 0)
            {               
                GamePiece p3 = board[_currentX - 2, _currentY - 2];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }

            // top 2 down 2
            if ((_currentX + 2) < tileCountX && (_currentY + 2) < tileCountY &&
                (_currentX - 2) >= 0 && (_currentY - 2) >= 0)
            {                
                GamePiece p3 = board[_currentX + 2, _currentY + 2];
                GamePiece p4 = board[_currentX + 2, _currentY + 2];

                _somethingKilled = KillNone(p1, p2, p3, p4, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 2 double counts
                    _killCount -= 2;
                }
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

        // left diagonal Repeated kill count FIX
        if ((_currentX + 1) < tileCountX && (_currentX - 1) >= 0 &&
            (_currentY + 1) < tileCountY && (_currentY - 1) >= 0)
        {
            GamePiece p1 = board[_currentX + 1, _currentY - 1];
            GamePiece p2 = board[_currentX - 1, _currentY + 1];

            // top 2 down 1
            if ((_currentX - 2) >= 0 && (_currentY + 2) < tileCountY)
            {
                GamePiece p3 = board[_currentX - 2, _currentY + 2];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }
            // top 1 down 2
            if ((_currentX + 2) < tileCountX && (_currentY - 2) >= 0)
            {
                GamePiece p3 = board[_currentX + 2, _currentY - 2];

                _somethingKilled = KillOnlyOne(p1, p2, p3, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 1 double count
                    _killCount -= 1;
                }
            }

            // top 2 down 2
            if ((_currentX + 2) < tileCountX && (_currentY + 2) < tileCountY &&
                (_currentX - 2) >= 0 && (_currentY - 2) >= 0)
            {
                GamePiece p3 = board[_currentX - 2, _currentY + 2];
                GamePiece p4 = board[_currentX + 2, _currentY - 2];

                _somethingKilled = KillNone(p1, p2, p3, p4, team);
                if (_somethingKilled == true)
                {
                    // kill 2 minus 2 double counts
                    _killCount -= 2;
                }
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
            StartHappyAnimation();
            PlayFeedback(p1);

            StartCoroutine(StartDestroyOne(p1));
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
            StartHappyAnimation();
            PlayFeedback(p1);
            PlayFeedback(p2);

            StartCoroutine(StartDestroyTwo(p1, p2));
            _somethingKilled = true;
            return _somethingKilled;

        }
        _somethingKilled = false;
        return _somethingKilled;
    }

    private bool KillOnlyOne(GamePiece p1, GamePiece p2, GamePiece p3, int team)
    {
        if (p1 & p2 & p3 != null &&
            p1._team != team &&
            p2._team != team &&
            p3._team == _team)
        {
            StartHappyAnimation();
            EliminateTwo(p1, p2);
            _somethingKilled = true;
            return _somethingKilled;
        }
        _somethingKilled = false;
        return _somethingKilled;
    }

    private bool KillNone(GamePiece p1, GamePiece p2, GamePiece p3, GamePiece p4, int team)
    {
        if (p1 & p2 & p3 & p4 != null &&
            p1._team != team &&
            p2._team != team &&
            p3._team == _team &&
            p4._team == _team)
        {
            StartHappyAnimation();
            EliminateTwo(p1, p2);
            _somethingKilled = true;
            return _somethingKilled;
        }
        _somethingKilled = false;
        return _somethingKilled;
    }

    private void EliminateTwo(GamePiece p1, GamePiece p2)
    {
        AudioHelper.PlayClip2D(_killedSFX, 1);
        PlayFeedback(p1);
        PlayFeedback(p2);

        StartCoroutine(StartDestroyTwo(p1, p2));
    }

    private void PlayMoveSFX()
    {
        if (_moveSFX != null)
        {
            AudioHelper.PlayClip2D(_moveSFX, 2f);
        }
    }  

    private void PlayFeedback (GamePiece p)
    {        
        _killedVisual = Instantiate(_killedFX, p.transform.position, Quaternion.identity);
        _killedVisual.Play();        
    }

    private void StartHappyAnimation()
    {
        _animation.SetBool("isHappy", true);
        StartCoroutine(StopHappyAnimation(_happyAnimationDuration));
    }

    private IEnumerator StopHappyAnimation(float duration)
    {        
        yield return new WaitForSeconds(duration);
        _animation.SetBool("isHappy", false);
    }

    private IEnumerator StartDestroyOne(GamePiece p)
    {
        p._animation.SetBool("isDead", true);        
        yield return new WaitForSeconds(_destroyDelay);        
        Destroy(p.gameObject);
        p = null;
    }

    private  IEnumerator StartDestroyTwo(GamePiece p1, GamePiece p2)
    {
        p1._animation.SetBool("isDead", true);
        p2._animation.SetBool("isDead", true);
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(p1.gameObject);
        p1 = null;
        Destroy(p2.gameObject);
        p2 = null;
    }
    
}

