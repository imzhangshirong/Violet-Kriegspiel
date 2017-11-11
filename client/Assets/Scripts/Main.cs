using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    private void Awake()
    {
    }
    void Start () {
		App.UIManager.OpenView("UITopTest");
        App.UIManager.OpenView("UIMainPanel");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
