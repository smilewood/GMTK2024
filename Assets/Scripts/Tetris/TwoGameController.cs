using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwoGameController : MonoBehaviour
{
   public TetrisGameView LeftGame, RightGame;
   public bool LeftGameActive;
   public TMP_Text ScoreText, gameoverscore;
   private int CombinedScore;
   // Start is called before the first frame update
   void Start()
   {
      //RestartGame();
      LeftGame.OnScoreUpdate.AddListener(UpdateScore);
      RightGame.OnScoreUpdate.AddListener(UpdateScore);
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

   public void UpdateScore(int scoreChange)
   {
      CombinedScore += scoreChange;
      ScoreText.text = CombinedScore.ToString();
      gameoverscore.text = CombinedScore.ToString();
   }

   public void RestartGame()
   {
      CombinedScore = 0;
      UpdateScore(0);
      ActiveGame.RemoveAllBoxes();
      ActiveGame.RemoveActivePiece();
      InactiveGame.RemoveAllBoxes();
      LeftGameActive = true;
      ActiveGame.TryAddActivePiece(TetrisGame.GetNextPiece());
      TetrisGame.GameOver = false;
      ActiveGame.StartGame();
      InactiveGame.StartGame();
   }

   public void PauseGames(bool pause)
   {
      ActiveGame.PauseGame(pause);
      InactiveGame.PauseGame(pause);
   }
}
