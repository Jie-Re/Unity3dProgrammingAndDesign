using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEmission : MonoBehaviour
{
    GameObject smoke = null;
    GameObject barrier = null;
    float engineCoeffient = 2.5f;
    float speedZ = 15F;
    float old_pos_z;
    float shift = 0;
    bool break_down = false;
    int recover_time = 500;
    Vector3 barrier_pos;

    private void Start()
    {
        //barrier_pos = new Vector3(this.gameObject.transform.position.x,
        //    this.gameObject.transform.position.y, this.gameObject.transform.position.z + 100);
        //barrier = Instantiate(Resources.Load<GameObject>("Prefabs/Barrier"));
        old_pos_z = this.gameObject.transform.position.z;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1)
        {
            smoke = Instantiate(Resources.Load<GameObject>("Prefabs/Smoke"), new Vector3(this.gameObject.transform.position.x - 0.635f,
                this.transform.position.y + 0.17f, this.transform.position.z - 1.6f), Quaternion.identity);
            float tranlationZ = Input.GetAxis("Vertical") * speedZ;
            smoke.GetComponent<ParticleSystem>().emissionRate = engineCoeffient * tranlationZ;
            //Debug.Log(smoke.GetComponent<ParticleSystem>().emissionRate);
        }

        shift += (this.gameObject.transform.position.z - old_pos_z);
        old_pos_z = this.gameObject.transform.position.z;

        if (shift >= 100)
        {
            barrier_pos = new Vector3(this.gameObject.transform.position.x-1,
                this.gameObject.transform.position.y, this.gameObject.transform.position.z + 30);
            barrier = Instantiate(Resources.Load<GameObject>("Prefabs/Barrier"), barrier_pos, Quaternion.identity);
            shift = 0;
        }
        //Debug.Log(Vector3.Distance(this.gameObject.transform.position, barrier_pos));
        if (Vector3.Distance(this.gameObject.transform.position, barrier_pos) < 1.5f)
        {
            //car is break down
            break_down = true;
            engineCoeffient = 10;
        }
        if (break_down)
        {
            if (recover_time == 0)
            {
                break_down = false;
                recover_time = 500;
                engineCoeffient = 2.5f;
            }
            else
            {
                recover_time--;
            }
        }
        Debug.Log(break_down);
    }
}
