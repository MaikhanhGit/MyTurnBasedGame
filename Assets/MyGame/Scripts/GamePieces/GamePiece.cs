using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GamePieceType
{
    None = 0,
    PlayerPiece = 1,
    AIPiece = 2
}
public class GamePiece : MonoBehaviour
{
    public int _team;
    public int _currentX;
    public int _currentY;
    public GamePieceType _type;

    private Vector3 _desiredPosition;
    private Vector3 _desiredScale;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * 10);

    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        _desiredPosition = position;
        if(force == true)
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
}
