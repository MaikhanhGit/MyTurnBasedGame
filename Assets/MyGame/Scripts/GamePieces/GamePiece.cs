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
    private Vector3 _desiredScale = new Vector3 (0.7f, 0.05f, 0.7f);

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * 10);

    }

    public List<Vector2Int> GetAvailableMoves(ref GamePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        r.Add(new Vector2Int(1, 2));
        r.Add(new Vector2Int(3, 2));
        r.Add(new Vector2Int(1, 3));
        r.Add(new Vector2Int(3, 3));

        return r;
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
