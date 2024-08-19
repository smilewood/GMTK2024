using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(RectTransform))]
public class TetrisGameView : MonoBehaviour
{
   public GameObject SquarePrefab;
   public Dictionary<int, RectTransform> squares;
   private RectTransform myTransform;
   private TetrisGame game;
   private Vector2 scaleFactor;

   private void Awake()
   {
      squares = new Dictionary<int, RectTransform>();
      myTransform = GetComponent<RectTransform>();
      scaleFactor = new Vector2(myTransform.rect.width / TetrisBoard.BoardSize.x, myTransform.rect.height / TetrisBoard.BoardSize.y);

      game = new TetrisGame();

      game.OnSquareAdded.AddListener(AddSquare);
      game.OnSquareMoved.AddListener((s, pos) => MoveSquare(s.ID, pos));
      game.OnSquareRemoved.AddListener(s => ClearSquare(s.ID));
   }

   public int BoardWeight => game.BoardWeight;

   public bool MovePieceLeft()
   {
      return game.MovePieceLeft();
   }
   public bool MovePieceRight()
   {
      return game.MovePieceRight();
   }
   public bool MovePieceDown()
   {
      return game.MovePieceDown();
   }
   public bool RotatePieceLeft()
   {
      return game.RotatePieceLeft();
   }
   public bool RotatePieceright()
   {
      return game.RotatePieceright();
   }
   public TetrisPiece RemoveActivePiece()
   {
      return game.RemoveActivePiece();
   }
   public bool TryAddActivePiece(TetrisPiece newPiece)
   {
      return game.TryAddActivePiece(newPiece);
   }
   public void RemoveAllBoxes()
   {
      game.RemoveAllBoxes();
   }
   public void StartGame()
   {
      StartCoroutine(game.GameLoop());
   }

   public void AddSquare(Vector2Int gridPosition, Square addedSquare)
   {
      GameObject newSquare = Instantiate(SquarePrefab, this.transform);
      RectTransform rc = newSquare.GetComponent<RectTransform>();
      rc.anchoredPosition = gridPosition * scaleFactor;
      rc.sizeDelta = scaleFactor;
      rc.pivot = Vector2.zero;
      newSquare.GetComponent<Image>().color = Square.GetPieceColor(addedSquare.color);
      squares.Add(addedSquare.ID, rc);
   }

   public void MoveSquare(int id, Vector2Int newGridPos)
   {
      squares[id].anchoredPosition = newGridPos * scaleFactor;
   }

   public void ClearSquare(int id)
   {
      Destroy(squares[id].gameObject);
      squares.Remove(id);
   }

}
