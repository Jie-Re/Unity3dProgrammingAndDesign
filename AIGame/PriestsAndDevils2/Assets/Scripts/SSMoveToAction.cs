using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSMoveToAction : SSAction//移动
{
    public Vector3 target;        //移动到的目的地
    public float speed;           //移动的速度

    private SSMoveToAction() { }
    public static SSMoveToAction GetSSAction(Vector3 target, float speed)
    {
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();//让unity自己创建一个MoveToAction实例，并自己回收
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);      //告诉动作管理或动作组合这个动作已完成
        }
    }

    public override void Start()
    {
        //移动动作建立时候不做任何事情
    }
}
