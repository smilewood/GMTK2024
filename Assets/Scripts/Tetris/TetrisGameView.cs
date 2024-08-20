using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnScoreUpdate : UnityEvent<int> {}

[RequireComponent (typeof(RectTransform))]
public class TetrisGameView : MonoBehaviour
{
   public GameObject SquarePrefab;
   public Dictionary<int, RectTransform> squares;
   private RectTransform myTransform;
   private TetrisGame game;
   private Vector2 scaleFactor;
   public OnScoreUpdate OnScoreUpdate;

   private void Awake()
   {
      squares = new Dictionary<int, RectTransform>();
      myTransform = GetComponent<RectTransform>();
      scaleFactor = new Vector2(myTransform.rect.width / TetrisBoard.BoardSize.x, myTransform.rect.height / TetrisBoard.BoardSize.y);
      OnScoreUpdate = new OnScoreUpdate();
      game = new TetrisGame();

      game.OnSquareAdded.AddListener(AddSquare);
      game.OnSquareMoved.AddListener((s, pos) => MoveSquare(s.ID, pos));
      game.OnSquareRemoved.AddListener(s => ClearSquare(s.ID));
      game.OnLinesCleared.AddListener(UpdateScore);
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

   public void PauseGame(bool pause)
   {
      game.Paused = pause;
   }
   public void StartGame()
   {
      level = 0;
      IncreaseSpeed();
      linesThisLevel = 0;
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

   public TMP_Text SpeedText;
   private int linesThisLevel = 0;
   private int level;

   private void UpdateScore(int clearedLines)
   {
      int scoreIncrease = 0;
      switch (clearedLines)
      {
         case 1:
         scoreIncrease += level * 40;
         break;
         case 2:
         scoreIncrease += level * 100;
         break;
         case 3:
         scoreIncrease += level * 300;
         break;
         case 4:
         scoreIncrease += level * 1200;
         break;
      }

      OnScoreUpdate.Invoke(scoreIncrease);

      linesThisLevel += clearedLines;
      if(linesThisLevel > 8)
      {
         IncreaseSpeed();
         linesThisLevel = 0;
      }
   }


   public void IncreaseSpeed()
   {
      ++level;
      game.tps = 1 / Mathf.Pow(.8f - ((level - 1) * .007f), level - 1f);
      SpeedText.text = (level).ToString();
   }
}
