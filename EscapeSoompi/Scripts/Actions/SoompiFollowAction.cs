using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoompiFollowAction : SSAction
{
    private float speed = 2f;//跟随王一博的速度
    private GameObject player;
    private SoompiData soompi_data;

    private SoompiFollowAction() { }
    public static SoompiFollowAction GetSSAction(GameObject wyb)
    {
        SoompiFollowAction action = CreateInstance<SoompiFollowAction>();
        action.player = wyb;
        return action;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        this.gameobject.GetComponent<Animator>().SetBool("run", true);
        soompi_data = this.gameobject.GetComponent<SoompiData>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        Follow();

        //如果私生饭没有跟踪对象，或者王一博不在其区域内
        if (!soompi_data.follow_player || soompi_data.wall_sign != soompi_data.sign)
        {
            this.gameobject.GetComponent<Animator>().SetBool("run", false);
            this.destroy = true;
            this.callback.SSActionEvent(this, 1, this.gameobject);
        }
    }

    void Follow()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        this.transform.LookAt(player.transform.position);
    }
}
