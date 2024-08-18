using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSquareMoved : UnityEvent<Square, Vector2Int> {}
public class OnSquareAdded : UnityEvent<Vector2Int, Square> {}
public class OnSquareRemoved : UnityEvent<Square> {}

public class TetrisGame
{
   public TetrisPiece ActivePiece;
   public TetrisBoard board;
   public bool GameOver;

   public float tps = 2;

   public OnSquareAdded OnSquareAdded;
   public OnSquareMoved OnSquareMoved;
   public OnSquareRemoved OnSquareRemoved;

   public TetrisGame()
   {
      board = new TetrisBoard();

      OnSquareAdded = new OnSquareAdded();
      OnSquareMoved = new OnSquareMoved();
      OnSquareRemoved = new OnSquareRemoved();

      board.SquareMoved.AddListener((a, b) => OnSquareMoved.Invoke(a, b));
      board.SquareRemoved.AddListener(s => OnSquareRemoved.Invoke(s));
   }

   public bool MovePieceLeft()
   {
      return TryMoveActivePiece(Vector2Int.left);
   }

   public bool MovePieceRight()
   {
      return TryMoveActivePiece(Vector2Int.right);
   }
   public bool MovePieceDown()
   {
      return TryMoveActivePiece(Vector2Int.down);
   }

   public IEnumerator GameLoop()
   {
      //Add initial piece, this needs to be changed to allow no active piece on a given board
      CreateNewPiece();

      while (!GameOver)
      {
         if (MovePieceDown())
         {
            //TODO: intentionally empty in case I need this later
         }
         else
         {
            //The piece is at the bottom
            foreach ((Vector2Int pos, Square s) in ActivePiece.squares)
            {
               board.FillSquare(pos, s);
            }

            CreateNewPiece();
         }
         board.CheckForTetris();
         yield return new WaitForSeconds(1f / tps);
      }
   }

   private void CreateNewPiece()
   {
      ActivePiece = new TetrisPiece(TetrisShape.O, new Vector2Int(TetrisBoard.BoardSize.x / 2, 20));
      foreach ((Vector2Int pos, Square s) in ActivePiece.squares)
      {
         OnSquareAdded.Invoke(pos, s);
      }
   }

   private bool TryMoveActivePiece(Vector2Int direction)
   {
      TetrisPiece newPiece = ActivePiece.GetPieceMoved(direction);
      foreach((Vector2Int pos, _) in newPiece.squares)
      {
         if(pos.y < 0 || pos.x < 0 || pos.x >= TetrisBoard.BoardSize.x || !board.SpaceIsFree(pos))
         {
            return false;
         }
      }

      //piece moved sucuessfully
      for (int i = 0; i < 4; ++i)
      {
         OnSquareMoved.Invoke(ActivePiece.squares[i].Item2, newPiece.squares[i].Item1);
      }
      ActivePiece = newPiece;
      return true;
   }
}
