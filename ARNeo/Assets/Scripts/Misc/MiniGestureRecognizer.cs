using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class MiniGestureRecognizer : MonoBehaviour
{

    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    public static bool isActive=true;
    public static event Action<SwipeDirection> Swipe;
    private bool swiping = false;
    private bool eventSent = false;
    private Vector2 lastPosition;
    public static Vector2 direction;

    void Update()
    {
        if (isActive)
        {
            if (Input.touchCount == 0)
                direction = Vector2.zero;
                return;

            if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
            {
                if (swiping == false)
                {
                    swiping = true;
                    lastPosition = Input.GetTouch(0).position;
                    return;
                }
                else
                {
                    if (!eventSent)
                    {
                        if (Swipe != null)
                        {
                            direction += Input.GetTouch(0).position - lastPosition;



                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                if (direction.x > 100)
                                    Swipe(SwipeDirection.Right);
                                else if (direction.x < -100)
                                    Swipe(SwipeDirection.Left);
                            }
                            else
                            {
                                if (direction.y > 0)
                                    Swipe(SwipeDirection.Up);
                                else
                                    Swipe(SwipeDirection.Down);
                            }

                            eventSent = true;
                        }
                    }
                }
            }
            else
            {
                swiping = false;
                eventSent = false;
            }
        }
    }
}