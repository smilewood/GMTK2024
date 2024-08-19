using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
   private PieceBag pieceBag;
   public TetrisGame()
   {
      board = new TetrisBoard();
      pieceBag = new PieceBag();
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

      TetrisShape nextShape = pieceBag.NextShape;

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
      if(ActivePiece.shape == TetrisShape.O)
      {
         return true;
      }

      TetrisPiece rotatedPiece = ActivePiece.GetPieceRotated(left);
      int mult = ActivePiece.shape == TetrisShape.I ? 2 : 1;
      Vector2Int[] initialStateOffsets = offsetTable[ActivePiece.rotation * mult];
      Vector2Int[] newStateOffsets = offsetTable[rotatedPiece.rotation * mult];

      for (int i = 0; i < 5; ++i) 
      {
         Vector2Int translation = newStateOffsets[i] - initialStateOffsets[i];
         TetrisPiece kickedPiece = rotatedPiece.GetPieceMoved(translation);

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


   private static readonly Vector2Int[][] offsetTable =
   {
     /*0*/    new Vector2Int[]{Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero},
     /*R*/    new Vector2Int[]{Vector2Int.zero, Vector2Int.right, new Vector2Int(1,-1), new Vector2Int(0, 2), new Vector2Int(1,2)},
     /*2*/    new Vector2Int[]{Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero},
     /*L*/    new Vector2Int[]{Vector2Int.zero, Vector2Int.left, new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1,2)},
   
     /*0*/    new Vector2Int[]{Vector2Int.zero, Vector2Int.left, new Vector2Int(2, 0), Vector2Int.left, new Vector2Int(2, 0)},
     /*R*/    new Vector2Int[]{Vector2Int.left, Vector2Int.zero, Vector2Int.zero, Vector2Int.right, new Vector2Int(0,-2)},
     /*2*/    new Vector2Int[]{ new Vector2Int(1,1), new Vector2Int(1, 1), new Vector2Int(-1, 1), Vector2Int.right, new Vector2Int(-2,0)},
     /*L*/    new Vector2Int[]{Vector2Int.right, Vector2Int.right, Vector2Int.right, Vector2Int.left, new Vector2Int(0,-2)}
   };

   private class PieceBag
   {
      private Queue<TetrisShape> bag;

      public PieceBag()
      {
         bag = new Queue<TetrisShape>();
      }

      public TetrisShape NextShape
      {
         get
         {
            if (!bag.Any())
            {
               FillBag();
            }
            return bag.Dequeue();
         }
      }


      private void FillBag()
      {
         TetrisShape[] pieces = (TetrisShape[])Enum.GetValues(typeof(TetrisShape));
         for(int i = 0; i < pieces.Length - 2; ++i)
         {
            int j = UnityEngine.Random.Range(i, pieces.Length);
            (pieces[i], pieces[j]) = (pieces[j], pieces[i]);
         }
         foreach(TetrisShape s in pieces)
         {
            bag.Enqueue(s);
         }
      }
   }

}
