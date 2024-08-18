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

   public bool RotatePieceLeft()
   {
      return TryRotateActivePiece(true);
   }
   public bool RotatePieceright()
   {
      return TryRotateActivePiece(false);
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
      //TODO: Random system needs work

      TetrisShape nextShape = UnityEngine.Random.Range(0, 2) == 1 ? TetrisShape.O : TetrisShape.T;

      ActivePiece = new TetrisPiece(nextShape, new Vector2Int(TetrisBoard.BoardSize.x / 2, 18));
      foreach ((Vector2Int pos, Square s) in ActivePiece.squares)
      {
         OnSquareAdded.Invoke(pos, s);
      }
   }

   private bool TryMoveActivePiece(Vector2Int direction)
   {
      TetrisPiece newPiece = ActivePiece.GetPieceMoved(direction);
      if (CheckLocationIsValid(newPiece))
      {
         //piece moved sucuessfully
         UpdateActivePiece(newPiece);
         return true;
      }
      return false;
   }


   private bool TryRotateActivePiece(bool left)
   {
      TetrisPiece rotatedPiece = ActivePiece.GetPieceRotated(left);
      if (CheckLocationIsValid(rotatedPiece))
      {
         UpdateActivePiece(rotatedPiece);
         return true;
      }
      else
      {
         //Wall-kick left
         TetrisPiece kickedPiece = rotatedPiece.GetPieceMoved(Vector2Int.left);
         if (CheckLocationIsValid(kickedPiece))
         {
            UpdateActivePiece(kickedPiece);
            return true;
         }
         //Wall-kick right
         kickedPiece = rotatedPiece.GetPieceMoved(Vector2Int.right);
         if (CheckLocationIsValid(kickedPiece))
         {
            UpdateActivePiece(kickedPiece);
            return true;
         }
      }
      return false;
   }

   private bool CheckLocationIsValid(TetrisPiece pieceToCheck)
   {
      foreach ((Vector2Int pos, _) in pieceToCheck.squares)
      {
         if (pos.y < 0 || pos.x < 0 || pos.x >= TetrisBoard.BoardSize.x || !board.SpaceIsFree(pos))
         {
            return false;
         }
      }
      return true;
   }

   private void UpdateActivePiece(TetrisPiece newPiece)
   {
      for (int i = 0; i < 4; ++i)
      {
         OnSquareMoved.Invoke(ActivePiece.squares[i].Item2, newPiece.squares[i].Item1);
      }
      ActivePiece = newPiece;
   }

}
