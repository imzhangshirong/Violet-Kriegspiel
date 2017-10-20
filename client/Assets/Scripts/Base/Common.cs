using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Common
{
    public class UI{
        public static void OpenTips(string msg)
        {
            Intent intent = new Intent();
            intent.Push("tipContent", msg);
            AppInterface.UIManager.OpenView("UITips", intent);
        }
    }
    
}
