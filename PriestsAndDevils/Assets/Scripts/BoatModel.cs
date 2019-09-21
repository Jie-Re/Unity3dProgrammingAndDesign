using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel
{
    Move move;
    Click click;
    int boat_sign = 1;                              //船在开始还是结束陆地
    RoleModel[] roles = new RoleModel[2];           //船上的角色（只能坐俩人）
    GameObject boat;

    //position
    //船在开始陆地的位置
    Vector3 startBoatPostion = new Vector3(11, 0.5f, 2);
    //船在结束陆地的位置
    Vector3 endBoatPosition = new Vector3(-11, 0.5f, 2);
    //船在开始陆地的空位位置（船中的座位位置）
    Vector3[] start_empty_pos = new Vector3[] {new Vector3(12, 2, 2), new Vector3(10, 2, 2) };
    //船在结束陆地的空位位置（船中的座位位置）
    Vector3[] end_empty_pos = new Vector3[] { new Vector3(-12, 2, 2), new Vector3(-10, 2, 2) };

    public BoatModel()
    {
        boat = Object.Instantiate(Resources.Load("Boat", typeof(GameObject)),startBoatPostion, Quaternion.identity) as GameObject;
        boat.name = "boat";
        move = boat.AddComponent(typeof(Move)) as Move;
        click = boat.AddComponent(typeof(Click)) as Click;
        click.SetBoat(this);
    
    }

    //封装了一个MovePosition，传进要移动到的位置就可以移动啦
    public void BoatMove()
    {
        if (boat_sign == -1)
        {
            move.MovePosition(startBoatPostion);
            boat_sign = 1;
        }
        else
        {
            move.MovePosition(endBoatPosition);
            boat_sign = -1;
        }
    }

    //移除船中的角色
    public RoleModel DeleteRoleModeByName(string roleName)
    {
        for (int i = 0; i < roles.Length; i ++)
        {
            if (roles[i] != null && roles[i].GetName() == roleName)
            {
                RoleModel role = roles[i];
                roles[i] = null;
                return role;
            }
        }
        return null;
    }

    //添加船中的角色
    public void AddRole(RoleModel role)
    {
        roles[GetEmptyNumber()] = role;
    }

    public int GetEmptyNumber()
    {
        for (int i = 0; i < roles.Length; i ++)
        {
            if (roles[i] == null) return i;
        }
        return -1;
    }

    public Vector3 GetEmptyPosition()
    {
        if (boat_sign == -1)
            return end_empty_pos[GetEmptyNumber()];
        else
            return start_empty_pos[GetEmptyNumber()];
    }

    public int[] GetRoleNumber()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == null) continue;
            if (roles[i].GetSign() == 0) count[0]++;//priest
            else count[1]++;//devil
        }
        return count;
    }

    public int GetBoatSign()
    {
        return boat_sign;
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < roles.Length; i ++)
        {
            if (roles[i] != null)
                return false;
        }
        return true;
    }

    public GameObject GetBoat()
    {
        return boat;
    }

    public void Reset()
    {
        if (boat_sign == -1) BoatMove();
        roles = new RoleModel[2];
    }

}
