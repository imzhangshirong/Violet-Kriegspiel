using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    private void Awake()
    {
    }
    void Start () {
		AppInterface.UIManager.OpenView("UITopTest");
        AppInterface.UIManager.OpenView("UIMainPanel");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
