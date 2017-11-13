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
    
}
