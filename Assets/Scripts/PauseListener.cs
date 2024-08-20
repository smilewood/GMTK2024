using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseListener : MonoBehaviour
{
   public TwoGameController Controller;
   bool paused = false;
   // Update is called once per frame
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         SetPause(!paused);  
      }
   }
   public void SetPause(bool pause)
   {
      if (paused)
      {
         MenuFunctions.Instance.HideMenu("Pause");
         Controller.PauseGames(false);
         paused = false;
      }
      else
      {
         MenuFunctions.Instance.ShowMenu("Pause");
         Controller.PauseGames(true);
         paused = true;
      }
   }
}
