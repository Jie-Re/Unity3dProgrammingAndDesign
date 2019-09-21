using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel
{
    GameObject role;
    int role_sign;             //0为牧师，1为恶魔
    Click click;               //点击触发的脚本
    bool on_boat;              //是否在船上       
    Move move;                 //移动的脚本
    PlayAnimation play_ani;    //动画脚本
    LandModel land_model = (SSDirector.GetInstance().CurrentScenceController as Controllor).start_land;     //所在陆地模型

    public RoleModel(string role_name)
    {
        if (role_name == "priest")
        {
            role = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
            role_sign = 0;
        }
        else
        {
            role = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
            role_sign = 1;
        }
        move = role.AddComponent(typeof(Move)) as Move;
        click = role.AddComponent(typeof(Click)) as Click;
        play_ani = role.AddComponent(typeof(PlayAnimation)) as PlayAnimation;
        click.SetRole(this);
    }

    public void SetName(string name)
    {
        role.name = name;
    }
    public int GetSign() { return role_sign; }
    public LandModel GetLandModel() { return land_model; }
    public string GetName() { return role.name; }
    public bool IsOnBoat() { return on_boat; }
    public void SetPosition(Vector3 pos) { role.transform.position = pos; }

    public void PlayGameOver()
    {
        play_ani.Play();
    }

    public void PlayIdle()
    {
        play_ani.NotPlay();
    }

    public void Move(Vector3 vec)
    {
        move.MovePosition(vec);
    }

    public void GoLand(LandModel land)
    {
        role.transform.parent = null;
        land_model = land;
        on_boat = false;
    }

    public void GoBoat(BoatModel boat)
    {
        role.transform.parent = boat.GetBoat().transform;
        land_model = null;
        on_boat = true;
    }

    public void Reset()
    {
        land_model = (SSDirector.GetInstance().CurrentScenceController as Controllor).start_land;
        GoLand(land_model);
        SetPosition(land_model.GetEmptyPosition());
        land_model.AddRole(this);
    }
}
