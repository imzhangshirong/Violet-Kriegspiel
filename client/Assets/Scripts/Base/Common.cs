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
        public static void BackPage()
        {
            AppInterface.UIManager.PageBack();
        }
        public static void OpenAlert(string title,string content, string positiveString, AlertWindowClick positive, string nagetiveString, AlertWindowClick nagetive,AlertWindowMode mode)
        {
            Intent intent = new Intent();
            intent.Push("title", title);
            intent.Push("content", content);
            intent.Push("positiveStr", positiveString);
            intent.Push("nagetiveStr", nagetiveString);
            intent.Push("positive", positive);
            intent.Push("nagetive", nagetive);
            intent.Push("mode", mode);
            AppInterface.UIManager.OpenView("UIAlertWindow", intent);
        }
    }
    
}
