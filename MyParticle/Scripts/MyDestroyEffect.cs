using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDestroyEffect : MonoBehaviour
{
    public float t = 0f;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > 1.5f)
        {
            Destroy(transform.gameObject);
        }
    }
}
