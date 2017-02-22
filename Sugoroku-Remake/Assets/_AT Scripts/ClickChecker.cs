using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickChecker : MonoBehaviour
{
    private GameControl gameScript;

	// Use this for initialization
	void Start ()
    {
        gameScript = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
	}

    private void OnMouseDown()
    {
        gameScript.AttackCharacterClicked(this.gameObject.GetComponent<CharacterControl>());
    }
}
