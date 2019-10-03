using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel
{
    GameObject role;//role对象
    int role_sign;//0为牧师，1为恶魔
    bool on_boat;//是否在船上
    Click click;//点击触发的脚本
    //Move move;//移动的脚本
    PlayAnimation play_ani;//动画脚本
    BankModel bank_model = (SSDirector.GetInstance().CurrentSceneController as Controller).start_land;//所在陆地模型

    public float move_speed = 300;

    public RoleModel(string role_name)
    {
        if (role_name == "priest")
        {
            role = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, 180,0)) as GameObject;
            role_sign = 0;
        }
        else
        {
            role = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;
            role_sign = 1;
        }
        //move = role.AddComponent(typeof(Move)) as Move;
        click = role.AddComponent(typeof(Click)) as Click;
        play_ani = role.AddComponent(typeof(PlayAnimation)) as PlayAnimation;
        click.SetRole(this);
    }

    public int GetSign() { return role_sign; }
    public BankModel GetBankModel() { return bank_model; }
    public string GetName() { return role.name; }
    public bool IsOnBoat() { return on_boat; }
    public void SetName(string name) { role.name = name; }
    public void SetPosition(Vector3 pos) { role.transform.position = pos; }
    //动作分离版新增
    public GameObject getGameObject() { return role; }

    public void PlayGameOver()
    {
        play_ani.Play();
    }

    public void PlayIdle()
    {
        play_ani.NotPlay();
    }

    /*public void Move(Vector3 vec)
    {
        move.MovePosition(vec);
    }*/

    public void GoLand(BankModel bank)//登上陆地
    {
        role.transform.parent = null;
        bank_model = bank;
        on_boat = false;
    }

    public void GoBoat(BoatModel boat)//登上船
    {
        role.transform.parent = boat.GetBoat().transform;
        bank_model = null;
        on_boat = true;
    }

    public void Reset()
    {
        bank_model = (SSDirector.GetInstance().CurrentSceneController as Controller).start_land;
        GoLand(bank_model);
        SetPosition(bank_model.GetEmptyPosition());
        bank_model.AddRole(this);
    }
}
