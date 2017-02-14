using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelX : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void XButtonClicked()
    {
        Debug.Log(transform.parent.gameObject.name + "-> XButtonClicked()");
        transform.parent.gameObject.SetActive(false);
    }


}
