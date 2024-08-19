using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTileIndicator : MonoBehaviour
{
   private GameObject[] previews;
   private GameObject ActivePreview;
   public GameObject SquarePrefab;
   public GameObject ParentPrefab;
   public Vector2Int ScaleFactor;

   // Start is called before the first frame update
   void Start()
   {
      TetrisGame.NextPieceUpdated.AddListener(UpdateNextPreview);

      previews = new GameObject[Enum.GetValues(typeof(TetrisShape)).Length];
      foreach(TetrisShape shape in Enum.GetValues(typeof(TetrisShape)))
      {
         GameObject previewParent = Instantiate(ParentPrefab, this.transform);
         previewParent.name = shape.ToString();

         CreateShape(shape, previewParent);

         previewParent.SetActive(false);
         previews[(int)shape] = previewParent;
      }
      ActivePreview = previews[0];
   }

   private void CreateShape(TetrisShape shape, GameObject previewParent)
   {
      TetrisPiece piece = new TetrisPiece(shape, Vector2Int.zero);
      foreach((Vector2Int offset, Square s) in piece.squares)
      {
         GameObject newSquare = Instantiate(SquarePrefab, previewParent.transform);
         RectTransform rc = newSquare.GetComponent<RectTransform>();
         rc.anchoredPosition = offset * ScaleFactor;
         rc.sizeDelta = ScaleFactor;
         rc.pivot = Vector2.zero;
         newSquare.GetComponent<Image>().color = Square.GetPieceColor(s.color);
      }

   }

   private void UpdateNextPreview(TetrisShape newShape)
   {
      ActivePreview.SetActive(false);
      ActivePreview = previews[(int)newShape];
      ActivePreview.SetActive(true);
   }
}