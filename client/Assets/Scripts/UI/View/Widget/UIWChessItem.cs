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
        int dx = (toPoint.x-x)*128;
        int dy = (toPoint.y-y)*64;
        float rotate = Mathf.Atan2(y,x);
        Debuger.Log(rotate/Mathf.PI*180);
        GameObject arrowGo = App.Manager.Resource.Load<GameObject>("UI/Game/Arrow");
        arrowGo = Instantiate(arrowGo);
        arrowGo.transform.SetParent(this.gameObject.transform);
        arrowGo.transform.localScale = Vector3.one;
        arrowGo.transform.Rotate(new Vector3(0,0,rotate/Mathf.PI*180));

    }
    
    void CleanArrow(object content){
        Transform tran = transform.Find("Arrow");
        while(tran!=null){
            Destroy(tran.gameObject);
            tran = transform.Find("Arrow");
        }
    }
}
