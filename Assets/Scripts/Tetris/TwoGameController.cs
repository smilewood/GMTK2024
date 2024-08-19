using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoGameController : MonoBehaviour
{
   public TetrisGameView LeftGame, RightGame;
   public bool LeftGameActive;

   // Start is called before the first frame update
   void Start()
   {
      RestartGame();
   }

   private TetrisGameView ActiveGame => LeftGameActive ? LeftGame : RightGame;
   private TetrisGameView InactiveGame => LeftGameActive ? RightGame : LeftGame;

   // Update is called once per frame
   void Update()
   {
      //TODO move cooldown?
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
         ActiveGame.MovePieceLeft();
      }
      if (Input.GetKeyDown(KeyCode.RightArrow))
      {
         ActiveGame.MovePieceRight();
      }
      if (Input.GetKey(KeyCode.DownArrow))
      {
         ActiveGame.MovePieceDown();
      }
      if (Input.GetKeyDown(KeyCode.UpArrow))
      {
         ActiveGame.RotatePieceLeft();
      }
      if (Input.GetKeyDown(KeyCode.Tab))
      {
         TetrisPiece piece = ActiveGame.RemoveActivePiece();
         if (!InactiveGame.TryAddActivePiece(piece))
         {
            ActiveGame.TryAddActivePiece(piece);
         }
         else
         {
            LeftGameActive = !LeftGameActive;
         }
      }
   }

   public void RestartGame()
   {
      ActiveGame.RemoveAllBoxes();
      ActiveGame.RemoveActivePiece();
      InactiveGame.RemoveAllBoxes();
      LeftGameActive = true;
      ActiveGame.TryAddActivePiece(TetrisGame.GetNextPiece());
      TetrisGame.GameOver = false;
      ActiveGame.StartGame();
      InactiveGame.StartGame();
   }
}
