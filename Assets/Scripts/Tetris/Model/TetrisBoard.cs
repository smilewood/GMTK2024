using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

public class TetrisBoard
{
   public static Vector2Int BoardSize = new(10, 20);

   public OnSquareMoved SquareMoved;
   public OnSquareRemoved SquareRemoved;
   private int filledSquares = 0;
   private Square[,] gameBoard;

   public TetrisBoard()
   {
      SquareMoved = new OnSquareMoved();
      SquareRemoved = new OnSquareRemoved();
      gameBoard = new Square[BoardSize.x, BoardSize.y];
   }

   public int SquareCount => filledSquares;

   public bool SpaceIsFree(Vector2Int position)
   {
      return SpaceIsFree(position.x, position.y);
   }
   private bool SpaceIsFree(int x, int y)
   {
      return gameBoard[x, y] is null;
   }

   public void FillSquare(Vector2Int pos, Square s)
   {
      gameBoard[pos.x, pos.y] = s;
      ++filledSquares;
   }

   public int CheckForTetris()
   {
      int clearedLines = 0;
      for(int i = 0; i < BoardSize.y; ++i)
      {
         //Check row for spaces
         bool rowComplete = true;
         for(int j = 0; j < BoardSize.x; ++j)
         {
            if(SpaceIsFree(j, i))
            {
               rowComplete = false;
               break;
            }
         }
         //No spaces found
         if (rowComplete)
         {
            ++clearedLines;
            //Remove the row
            for (int j = 0; j < BoardSize.x; ++j)
            {
               SquareRemoved.Invoke(gameBoard[j, i]);
               gameBoard[j, i] = null;
               --filledSquares;
            }
            //Move everything down
            for (int y = i; y < BoardSize.y; ++y)
            {
               for (int x = 0; x < BoardSize.x; ++x)
               {
                  gameBoard[x, y] = y < BoardSize.y - 1 ? gameBoard[x, y + 1] : null;
                  if (gameBoard[x, y] is Square s)
                  {
                     SquareMoved.Invoke(s, new Vector2Int(x, y));
                  }
               }
            }
            --i;
         }
      }
      return clearedLines;
   }

   public void ClearBoard()
   {
      for(int i = 0; i < BoardSize.x; ++i)
      {
         for(int j = 0; j < BoardSize.y; ++j)
         {
            if (gameBoard[i,j] is Square s)
            {
               SquareRemoved.Invoke(s);
               gameBoard[i, j] = null;
               --filledSquares;
            }
         }
      }
      Debug.Assert(filledSquares == 0, "Everything should have been cleared");
   }
}
