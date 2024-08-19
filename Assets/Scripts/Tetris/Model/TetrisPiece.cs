using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum TetrisShape
{
   O,
   S,
   Z,
   T,
   L,
   J,
   I
}


public class TetrisPiece
{
   public (Vector2Int, Square)[] squares;
   public TetrisShape shape;
   public int rotation
   {
      get; private set;
   }
   private TetrisPiece(TetrisShape shape)
   {
      this.shape = shape;
      squares = new (Vector2Int, Square)[4];
   }
   public TetrisPiece(TetrisShape shape, Vector2Int position) : this(shape)
   {
      InitializeShape(position);
   }

   private void InitializeShape(Vector2Int position)
   {
      Vector2Int[] offsets;
      PieceColor color;
      switch (this.shape)
      {
         case TetrisShape.O:
         {
            offsets = rotationO;
            color = PieceColor.Yellow;
            break;
         }
         case TetrisShape.S:
         {
            offsets = rotationsS[0];
            color = PieceColor.Green;
            break;
         }
         case TetrisShape.Z:
         {
            offsets = rotationsZ[0];
            color = PieceColor.Red;
            break;
         }
         case TetrisShape.T:
         {
            offsets = rotationsT[0];
            color = PieceColor.Purple;
            break;
         }
         case TetrisShape.L:
         {
            offsets = rotationsL[0];
            color = PieceColor.Orange;
            break;
         }
         case TetrisShape.J:
         {
            offsets = rotationsJ[0];
            color = PieceColor.DarkBlue;
            break;
         }
         case TetrisShape.I:
         {
            offsets = rotationsI[0];
            color = PieceColor.LightBlue;
            break;
         }
         default:
         {
            throw new ArgumentException("How did we get an unknown shape?");
         }
      }

      rotation = 0;
      squares[0] = (position + offsets[0], new Square(color));
      squares[1] = (position + offsets[1], new Square(color));
      squares[2] = (position + offsets[2], new Square(color));
      squares[3] = (position + offsets[3], new Square(color));
   }

   public void RotatePiece()
   {
      
   }

   public TetrisPiece GetPieceMoved(Vector2Int direction)
   {
      TetrisPiece newPiece = new TetrisPiece(this.shape);
      newPiece.rotation = this.rotation;
      newPiece.squares[0] = (this.squares[0].Item1 + direction, this.squares[0].Item2);
      newPiece.squares[1] = (this.squares[1].Item1 + direction, this.squares[1].Item2);
      newPiece.squares[2] = (this.squares[2].Item1 + direction, this.squares[2].Item2);
      newPiece.squares[3] = (this.squares[3].Item1 + direction, this.squares[3].Item2);
      return newPiece;
   }

   public TetrisPiece GetPieceRotated(bool left)
   {
      int rotationIndex = (this.rotation + (left ? 1 : -1)) % 4;

      Vector2Int[] offsets;
      switch (shape)
      {
         case TetrisShape.O:
         {
            offsets = rotationO;
            break;
         }
         case TetrisShape.S:
         {
            offsets = rotationsS[rotationIndex];
            break;
         }
         case TetrisShape.Z:
         {
            offsets = rotationsZ[rotationIndex];
            break;
         }
         case TetrisShape.T:
         {
            offsets = rotationsT[rotationIndex];
            break;
         }
         case TetrisShape.L:
         {
            offsets = rotationsL[rotationIndex];
            break;
         }
         case TetrisShape.J:
         {
            offsets = rotationsJ[rotationIndex];
            break;
         }
         case TetrisShape.I:
         {
            offsets = rotationsI[rotationIndex];
            break;
         }
         default:
         {
            throw new ArgumentException("How did we get an unknown shape?");
         }
      }

      TetrisPiece newPiece = new TetrisPiece(this.shape)
      {
         rotation = rotationIndex
      };

      Vector2Int origin = this.squares[0].Item1;
      newPiece.squares[0] = (origin + offsets[0], this.squares[0].Item2);
      newPiece.squares[1] = (origin + offsets[1], this.squares[1].Item2);
      newPiece.squares[2] = (origin + offsets[2], this.squares[2].Item2);
      newPiece.squares[3] = (origin + offsets[3], this.squares[3].Item2);
      return newPiece;
   }

   #region rotations
   private static readonly Vector2Int[] rotationNone = new Vector2Int[] { Vector2Int.zero, Vector2Int.zero, Vector2Int.zero, Vector2Int.zero };

   private static readonly Vector2Int[] rotationO = new Vector2Int[] { Vector2Int.zero, Vector2Int.left, Vector2Int.down, new Vector2Int(-1, -1) };

   private static readonly Vector2Int[][] rotationsT = { 
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.up, Vector2Int.right },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.up, Vector2Int.right },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.left, Vector2Int.right },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.left, Vector2Int.up }
   };

   private static readonly Vector2Int[][] rotationsL = {
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.right, new Vector2Int(1, 1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.down, new Vector2Int(1, -1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.right, new Vector2Int(-1, -1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.up, new Vector2Int(-1, 1) }
   };

   private static readonly Vector2Int[][] rotationsJ = {
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.right, new Vector2Int(-1, 1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.down, new Vector2Int(1, 1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.right, new Vector2Int(1, -1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.up, new Vector2Int(-1, -1) }
   };

   private static readonly Vector2Int[][] rotationsS = {
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.left, new Vector2Int(1,1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.right, new Vector2Int(1,-1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.right, new Vector2Int(-1,-1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.left, new Vector2Int(-1,1) }
   };

   private static readonly Vector2Int[][] rotationsZ = {
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.right, new Vector2Int(-1,1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.right, new Vector2Int(1,1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.down, Vector2Int.left, new Vector2Int(1,-1) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.left, new Vector2Int(-1,-1) }
   };

   private static readonly Vector2Int[][] rotationsI =
   {
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.right, new Vector2Int(2,0) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.down, new Vector2Int(0,-2) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.left, Vector2Int.right, new Vector2Int(-2,0) },
      new Vector2Int[]{Vector2Int.zero,Vector2Int.up, Vector2Int.down, new Vector2Int(0,2) }
   };

   #endregion

}
