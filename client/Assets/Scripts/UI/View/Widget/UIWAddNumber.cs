using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIWAddNumber : UIWidgetBase
{
    public void AddNumberOp()
    {
        Random random = new Random();
        Push("_addNumber", random.Next(1,20));
    }
}