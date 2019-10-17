using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraft
{
    GameObject plane;
    JoyStick joystick;

    public SpaceCraft()
    {
        plane = Object.Instantiate(Resources.Load("Plane", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        joystick = plane.AddComponent(typeof(JoyStick)) as JoyStick;
    }

    public Vector3 GetJoyStickPosition()
    {
        return joystick.transform.position;
    }
}
