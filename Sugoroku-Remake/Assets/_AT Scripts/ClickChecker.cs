using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickChecker : MonoBehaviour
{
    public bool moveCheck;

    private GameControl gameScript;

	// Use this for initialization
	void Start ()
    {
        gameScript = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
	}

    private void OnMouseDown()
    {
        if (moveCheck)
        {
            foreach (GameObject mb in GameObject.FindGameObjectsWithTag("MoveBlock"))
            {
                GetComponent<SpriteRenderer>().color = Color.blue;
            }
            GetComponent<SpriteRenderer>().color = Color.green;
            gameScript.MoveBlockClicked(new Vector2(transform.position.x, transform.position.y));
        }
        else
        {
            gameScript.AttackCharacterClicked(this.gameObject.GetComponent<CharacterControl>());
        }
    }
}
