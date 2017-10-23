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
        public static void OpenAlert(string title,string content, string positiveString, AlertWindowClick positive, string negativeString, AlertWindowClick negative, AlertWindowMode mode)
        {
            Intent intent = new Intent();
            intent.Push("title", title);
            intent.Push("content", content);
            intent.Push("positiveStr", positiveString);
            intent.Push("negativeStr", negativeString);
            intent.Push("positive", positive);
            intent.Push("negative", negative);
            intent.Push("mode", mode);
            AppInterface.UIManager.OpenView("UIAlertWindow", intent);
        }
    }
    
}
