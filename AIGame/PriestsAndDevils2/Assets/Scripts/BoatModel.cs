using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel
{
    private const int boatCapacity = 2;//船容量为2

    GameObject boat;//船对象
    int boat_sign = 1;//船在开始还是结束陆地
    RoleModel[] roles = new RoleModel[boatCapacity];//乘客数组
    Vector3[] start_empty_pos;//船在开始陆地的座位坐标
    Vector3[] end_empty_pos;//船在结束陆地的座位坐标
    Click click;

    //Move move;
    //动作分离版
    public float move_speed = 250;
    public GameObject getGameObject() { return boat; }

    //坐标设置相关参数
    private Vector3 boat_position = new Vector3(12, 0, 40);
    private Vector3 boat_position2 = new Vector3(-12, 0, 40);
    private Vector3 boat_seat1 = new Vector3(6, 4, 40);
    private Vector3 boat_seat2 = new Vector3(18, 4, 40);

    public BoatModel()
    {
        boat = Object.Instantiate(Resources.Load("Boat", typeof(GameObject)), boat_position, Quaternion.identity) as GameObject;
        boat.name = "boat";

        //move = boat.AddComponent(typeof(Move)) as Move;

        click = boat.AddComponent(typeof(Click)) as Click;
        click.SetBoat(this);

        Vector3 end_pos2 = boat_seat1;
        end_pos2.x = -1 * boat_seat1.x;
        Vector3 end_pos1 = boat_seat2;
        end_pos1.x = -1 * boat_seat2.x;
        start_empty_pos = new Vector3[] { boat_seat1, boat_seat2 };
        end_empty_pos = new Vector3[] { end_pos1, end_pos2 };
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] != null)
                return false;
        }
        return true;
    }

    //public void BoatMove()
    //动作分离版
    public Vector3 BoatMoveToPosition()
    {
        boat_sign = -boat_sign;
        if (boat_sign == -1)
        {
            return boat_position2;
        }
        else
        {
            return boat_position;
        }
    }

    public int GetBoatSign() { return boat_sign; }

    public RoleModel DeleteRoleByName(string role_name)
    {
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] != null && roles[i].GetName() == role_name)
            {
                RoleModel role = roles[i];
                roles[i] = null;
                return role;
            }
        }
        return null;
    }

    public int GetEmptyNumber()
    {
        Debug.Log("Boat GetEmptyNumber()");
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == null)
            {
                Debug.Log(i);
                if (roles[1 - i] == null) Debug.Log("something wrong");
                return i;
            }
        }
        Debug.Log(-1);
        return -1;
    }

    public Vector3 GetEmptyPosition()
    {
        Debug.Log("GetEmptyPosition()");
        Vector3 pos;
        if (boat_sign == -1)
            pos = end_empty_pos[GetEmptyNumber()];
        else
            pos = start_empty_pos[GetEmptyNumber()];
        Debug.Log(pos);
        return pos;
    }

    public void AddRole(RoleModel role)//登上船
    {
        roles[GetEmptyNumber()] = role;
    }

    public int[] GetRoleNumber()
    {
        int[] count = { 0, 0 };//count[0]是牧师数，count[1]是魔鬼数
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == null)
                continue;
            if (roles[i].GetSign() == 0)
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public GameObject GetBoat() { return boat; }

    public void Reset()
    {
        if (boat_sign == -1)
            boat.transform.position = BoatMoveToPosition();
        roles = new RoleModel[2];
    }
}
