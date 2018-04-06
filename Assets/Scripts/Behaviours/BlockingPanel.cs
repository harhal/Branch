using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class BlockingPanel : MonoBehaviour {

    RectTransform rectTranform;

    public bool IsOverlapPoint(Vector2 point)
    {
        if (!gameObject.activeInHierarchy) return false;
        Vector3[] corners = new Vector3[4];
        rectTranform.GetWorldCorners(corners);
        /*for (int i = 0; i < 4; i++)
            print(corners[i]);*/
        Rect panelRect = new Rect(corners[0], corners[2] - corners[0]);
        //Vector2 LocalPoint = (Vector2)(rectTranform.worldToLocalMatrix * (Vector3)point);
        //return rectTranform.rect.Contains(LocalPoint);
        return panelRect.Contains(point);
    }
    
	void Awake ()
    {
        rectTranform = GetComponent<RectTransform>();
    }

    void Start()
    {
        PlayerController.MainController.blockingInputPanels.Add(this);
    }
}
