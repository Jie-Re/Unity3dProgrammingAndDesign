using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllor : MonoBehaviour, ISceneController, IUserAction
{
    public LandModel start_land;            //开始陆地
    public LandModel end_land;              //结束陆地
    public BoatModel boat;                  //船
    private RoleModel[] roles;              //角色
    UserGUI user_gui;

    void Start()
    {
        SSDirector director = SSDirector.GetInstance();      //得到导演实例
        director.CurrentScenceController = this;             //设置当前场景控制器
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;  //添加UserGUI脚本作为组件
        LoadResources();                                     //加载资源
    }


    //创建水，陆地，角色，船
    public void LoadResources()
    {
        GameObject water = Instantiate(Resources.Load("Water", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        Debug.Log("waterload");
        water.name = "water";
        start_land = new LandModel("start");
        end_land = new LandModel("end");
        boat = new BoatModel();

        roles = new RoleModel[6];
        //priests
        for (int i = 0; i < 3; i ++)
        {
            RoleModel role = new RoleModel("priest");
            role.SetName("priest" + i);
            role.SetPosition(start_land.GetEmptyPosition());
            role.GoLand(start_land);
            start_land.AddRole(role);
            roles[i] = role;
        }
        //devils
        for (int i = 0; i < 3; i++)
        {
            RoleModel role = new RoleModel("devil");
            role.SetName("devil" + i);
            role.SetPosition(start_land.GetEmptyPosition());
            role.GoLand(start_land);
            start_land.AddRole(role);
            roles[i+3] = role;
        }
    }
    //移动船
    public void MoveBoat()
    {
        if (boat.IsEmpty() || user_gui.sign != 0) return;
        boat.BoatMove();
        user_gui.sign = Check();
        if (user_gui.sign == 1)
        {
            for (int i = 0; i < 3; i ++)
            {
                roles[i].PlayGameOver();
                roles[i + 3].PlayGameOver();
            }
        }
    }
    //移动角色
    public void MoveRole(RoleModel role)
    {
        if (user_gui.sign != 0) return;
        if (role.IsOnBoat())
        {
            Debug.Log("role is on Boat");
            LandModel land;
            if (boat.GetBoatSign() == -1) land = end_land;
            else land = start_land;
            boat.DeleteRoleModeByName(role.GetName());
            role.Move(land.GetEmptyPosition());
            role.GoLand(land);
            land.AddRole(role);
        }
        else
        {
            LandModel land = role.GetLandModel();
            //船上无空位或船在移动过程中，不可移动角色
            if (boat.GetEmptyNumber() == -1 || land.GetLandSign() != boat.GetBoatSign()) return;

            land.DeleteRoleByName(role.GetName());
            role.Move(boat.GetEmptyPosition());
            role.GoBoat(boat);
            boat.AddRole(role);
        }
        user_gui.sign = Check();
        if (user_gui.sign == 1)
        {
            for (int i = 0; i < 3; i ++)
            {
                roles[i].PlayGameOver();
                roles[i + 3].PlayGameOver();
            }
        }
    }
    //重新开始游戏
    public void Restart()
    {
        start_land.Reset();
        end_land.Reset();
        boat.Reset();
        for (int i = 0; i < roles.Length; i++) roles[i].Reset();

        if (user_gui.sign == 1)
        {
            for (int i = 0; i < 3; i ++)
            {
                roles[i + 3].PlayIdle();
                roles[i].PlayIdle();
            }
        }

        Debug.Log("Restart");
    }
    //检测游戏是否结束，返回0表示游戏仍在进行，返回1表示失败，返回2表示成功
    public int Check()
    {
        int start_priest = (start_land.GetRoleNum())[0];
        int start_devil = (start_land.GetRoleNum())[1];
        int end_priest = (end_land.GetRoleNum())[0];
        int end_devil = (end_land.GetRoleNum())[1];

        if (end_priest + end_devil == 6) return 2;

        int[] boat_role_num = boat.GetRoleNumber();
        if (boat.GetBoatSign() == 1)
        {
            start_priest += boat_role_num[0];
            start_devil += boat_role_num[1];
        }
        else
        {
            end_priest += boat_role_num[0];
            end_devil += boat_role_num[1];
        }

        if (start_priest > 0 && start_priest < start_devil) return 1;
        if (end_priest > 0 && end_priest < end_devil) return 1;

        return 0;
    }
}