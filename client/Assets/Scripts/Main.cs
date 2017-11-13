using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    private void Awake()
    {
    }
    void Start () {
        App.Manager.UI.OpenView("UIMainPanel");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
