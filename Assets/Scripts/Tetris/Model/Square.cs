using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PieceColor
{
   LightBlue,
   DarkBlue,
   Orange,
   Yellow,
   Green,
   Purple,
   Red
}

public class Square
{
   public PieceColor color;
   public readonly int ID;
   private static int nextID = 0;

   // Start is called before the first frame update
   public Square(PieceColor color)
   {
      this.ID = nextID++;
      this.color = color;
   }

   public static Color GetPieceColor(PieceColor color)
   {
      return color switch
      {
         PieceColor.LightBlue => Color.cyan,
         PieceColor.DarkBlue => Color.blue,
         PieceColor.Orange => new Color(255, 127, 0),
         PieceColor.Yellow => Color.yellow,
         PieceColor.Green => Color.green,
         PieceColor.Purple => new Color(128, 0, 128),
         PieceColor.Red => Color.red,
         _ => throw new ArgumentException("Unknown Color was added to the enum."),
      };
   }
}
