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
        intent.Push("id", chessId);
        intent.Push("gameObject", gameObject);
        Push("_chessClick", intent);
        
    }
    
}
