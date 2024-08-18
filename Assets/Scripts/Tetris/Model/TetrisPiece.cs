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
      //TODO: Diferent shapes

      squares[0] = (position, new Square(PieceColor.Yellow));
      squares[1] = (position + Vector2Int.down, new Square(PieceColor.Yellow));
      squares[2] = (position + Vector2Int.left, new Square(PieceColor.Yellow));
      squares[3] = (position + Vector2Int.down + Vector2Int.left, new Square(PieceColor.Yellow));
   }

   public void RotatePiece()
   {
   
   }

   public TetrisPiece GetPieceMoved(Vector2Int direction)
   {
      TetrisPiece newPiece = new TetrisPiece(this.shape);
      newPiece.squares[0] = (this.squares[0].Item1 + direction, this.squares[0].Item2);
      newPiece.squares[1] = (this.squares[1].Item1 + direction, this.squares[1].Item2);
      newPiece.squares[2] = (this.squares[2].Item1 + direction, this.squares[2].Item2);
      newPiece.squares[3] = (this.squares[3].Item1 + direction, this.squares[3].Item2);
      return newPiece;
   }
}
