using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIWChessItem : UIWidgetBase
{
    public int chessId;
    private void Start()
    {

        BindEvent("_cleanArrow",CleanArrow);//清除路径
    }
    public void OnClick()
    {
        Intent intent = new Intent();
        int x = chessId % 5;
        int y = chessId / 5;
        intent.Push("point", new ChessPoint(x,y));
        intent.Push("gameObject", gameObject);
        Push("_chessClick", intent);
        
    }
    public void SetArrow(ChessPoint toPoint){
        if(toPoint == null){
            CleanArrow(null);
            return;
        }
        int x = chessId % 5;
        int y = chessId / 5;
        float dx = (toPoint.x-x)*128;
        float dy = (toPoint.y-y)*64;
        float rotateA = Mathf.Atan2(dy,dx);
        GameObject arrowGo = App.Manager.Resource.Load<GameObject>("UI/Game/Arrow");
        arrowGo = Instantiate(arrowGo);
        arrowGo.name = "Arrow";
        arrowGo.transform.SetParent(this.gameObject.transform);
        arrowGo.transform.localScale = Vector3.one;
        arrowGo.transform.localPosition = Vector3.zero;
        arrowGo.transform.Rotate(0,0,rotateA/Mathf.PI*180-90);
    }
    
    void CleanArrow(object content){
        for(int i=transform.childCount-1;i>=0;i--){
            GameObject go = transform.GetChild(i).gameObject;
            if(go.name=="Arrow"){
                Destroy(go);
            }
        }
    }
}
