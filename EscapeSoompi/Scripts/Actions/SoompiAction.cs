using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoompiAction : SSAction
{
    private enum Direction { EAST, NORTH, WEST, SOUTH };
    private float pos_x, pos_z;//移动前的初始x和z方向坐标
    private float move_length;//移动的长度
    private float move_speed = 1.2f;//移动速度
    private bool move_sign = true;//是否到达目的地
    private Direction direc = Direction.EAST;//移动的方向
    private SoompiData soompi_data;//私生饭的数据

    private SoompiAction() { }
    public static SoompiAction GetSSAction(Vector3 location)
    {
        SoompiAction action = CreateInstance<SoompiAction>();
        action.pos_x = location.x;
        action.pos_z = location.z;
        //设定移动矩形的边长
        action.move_length = 10;
        return action;
    }
    public override void Start()
    {
        this.gameobject.GetComponent<Animator>().SetBool("walk", true);
        soompi_data = this.gameobject.GetComponent<SoompiData>();
    }
    public override void Update()
    {
        //防止碰撞后发生的旋转
        if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        //移动
        MoveSoompi();
        if (soompi_data.follow_player && soompi_data.wall_sign == soompi_data.sign)
        {
            this.gameobject.GetComponent<Animator>().SetBool("walk", false);
            this.destroy = true;
            this.callback.SSActionEvent(this, 0, this.gameobject);
        }
    }
    //凸多边形移动
    void MoveSoompi()
    {
        if (move_sign)
        {
            //不需要转向则设定一个目的地，按照矩形移动
            switch (direc)
            {
                case Direction.EAST:
                    pos_x -= move_length;
                    break;
                case Direction.NORTH:
                    pos_z += move_length;
                    break;
                case Direction.WEST:
                    pos_x += move_length;
                    break;
                case Direction.SOUTH:
                    pos_z -= move_length;
                    break;
            }
            move_sign = false;
        }
        this.transform.LookAt(new Vector3(pos_x, 0, pos_z));
        float distance = Vector3.Distance(transform.position, new Vector3(pos_x, 0, pos_z));
        //当前位置与目的地距离浮点数的比较
        if (distance > 0.9)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(pos_x, 0, pos_z), move_speed * Time.deltaTime);
        }
        else
        {
            direc += 1;
            if (direc > Direction.SOUTH)
            {
                direc = Direction.EAST;
            }
            move_sign = true;
        }
    }
}
