using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
{
    private float speedX = 150F;
    private float speedZ = 15F;

    // Update is called once per frame
    void Update()
    {
        float translationZ = Input.GetAxis("Vertical") * speedZ;
        float translationX = Input.GetAxis("Horizontal") * speedX;
        translationZ *= Time.deltaTime;
        translationX *= Time.deltaTime;
        transform.Translate(0, 0, translationZ);
        transform.Rotate(0, translationX, 0);
    }
}
