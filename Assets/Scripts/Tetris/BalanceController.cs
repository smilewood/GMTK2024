using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BalanceController : MonoBehaviour
{
   public RectTransform LeftObjects, RightObjects;//, BalanceArm;
   public float MaxOffsetDistance, MaxArmAngle;
   public float MaxWeightDiference;
   public TetrisGameView LeftGame, RightGame;
   public float ScaleSpeed;


   // Update is called once per frame
   void Update()
   {
      int diference = RightGame.BoardWeight - LeftGame.BoardWeight;

      if(Mathf.Abs(diference) > MaxWeightDiference)
      {
         MenuFunctions.Instance.ShowMenu("Game Over");
         TetrisGame.GameOver = true;
      }

      float ratio = diference / MaxWeightDiference;
      float offset = ratio * MaxOffsetDistance;
      //float targetAngle = ratio * MaxArmAngle;

      LeftObjects.anchoredPosition = Vector2.Lerp(LeftObjects.anchoredPosition, new Vector2(LeftObjects.anchoredPosition.x, offset), ScaleSpeed * Time.deltaTime);
      RightObjects.anchoredPosition = Vector2.Lerp(RightObjects.anchoredPosition, new Vector2(RightObjects.anchoredPosition.x, -offset), ScaleSpeed * Time.deltaTime);
      //BalanceArm.rotation = Quaternion.Lerp(BalanceArm.rotation, Quaternion.Euler(0, 0, targetAngle), ScaleSpeed * Time.deltaTime);
   }
}
