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
            App.Manager.UI.OpenView("UITips", intent);
        }
        public static void BackPage()
        {
            App.Manager.UI.PageBack();
        }
        public static void OpenAlert(string title,string content, string positiveString, AlertWindowClick positive, string negativeString, AlertWindowClick negative, AlertWindowMode mode,bool autoClose = true)
        {
            Intent intent = new Intent();
            intent.Push("title", title);
            intent.Push("content", content);
            intent.Push("positiveStr", positiveString);
            intent.Push("negativeStr", negativeString);
            intent.Push("positive", positive);
            intent.Push("negative", negative);
            intent.Push("mode", mode);
            intent.Push("autoClose", autoClose);
            App.Manager.UI.OpenView("UIAlertWindow", intent);
        }
        public static void OpenAlert(string title, string content, string positiveString, AlertWindowClick positive, bool autoClose = true)
        {
            OpenAlert(title, content, positiveString, positive, "", null, AlertWindowMode.OneButton, autoClose);
        }
        public static void OpenAlert(string title, string content, string positiveString, AlertWindowClick positive, string negativeString, AlertWindowClick negative, bool autoClose = true)
        {
            OpenAlert(title, content, positiveString, positive, negativeString, negative, AlertWindowMode.TwoButton, autoClose);
        }
        public static void OpenWaiting(){

        }
        public static void OpenRetry(){
            
        }
    }
    
}
