using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : CharaBehaviour
{
    //[Header ("Input Controller")]
    //[SerializeField] private KeyCode moveUp;
    //[SerializeField] private KeyCode moveDown;
    //[SerializeField] private KeyCode moveRight;
    //[SerializeField] private KeyCode moveLeft;
    //[SerializeField] private KeyCode dash;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Clamp();
        Dash();
        KeyboardMovement();
        Action();
    }
    public void KeyboardMovement()
    {
        direction = Vector2.zero;
        /*note : 1 : up , 2 : down, 3 : left , 4 : right*/
        if (Input.GetKey(InputManager.instance.moveUp))
        {
            
            isAccelerating = true;
            direction += Vector2.up;
            lastDirection = Vector2.up;
        }
        if (Input.GetKeyUp(InputManager.instance.moveUp))
        {
            if (isAccelerating && lastDirection == Vector2.up)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        if (Input.GetKey(InputManager.instance.moveDown))
        {
            
            isAccelerating = true;
            direction += Vector2.down;
            lastDirection = Vector2.down;
        }
        if (Input.GetKeyUp(InputManager.instance.moveDown))
        {
            if (isAccelerating && lastDirection == Vector2.down)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        if (Input.GetKey(InputManager.instance.moveLeft))
        {
       
            isAccelerating = true;
            direction += Vector2.left;
            lastDirection = Vector2.left; ;
        }
        if (Input.GetKeyUp(InputManager.instance.moveLeft))
        {
            if (isAccelerating && lastDirection == Vector2.left)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        if (Input.GetKey(InputManager.instance.moveRight))
        {
            
            isAccelerating = true;
            direction += Vector2.right;
            lastDirection = Vector2.right;
        }
        if (Input.GetKeyUp(InputManager.instance.moveRight))
        {
            if (isAccelerating && lastDirection == Vector2.right)
            {
                isAccelerating = false;
                timeMoveElapsed = timeToStop;
            }
        }

        MoveAccelerate();
    }

    public void Action()
    {
        if (Input.GetKeyDown(InputManager.instance.dash))
        {
            if (canDash)
            {
                dashTime = startDashTime;
                isDashed = true;
                data.IsDashing = true;
                canDash = false;
                this.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
}
