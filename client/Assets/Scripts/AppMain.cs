using UnityEngine;
using System.Collections;

public class AppMain : MonoBehaviour {
	void Start () {
		Global.MUI.OpenView("UIPlayerInfo");
		Global.MUI.OpenView("UIPlayerInfo2");
		Global.MUI.OpenView("leftTest");

		Global.MUI.OpenView("UIMainPanel");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
