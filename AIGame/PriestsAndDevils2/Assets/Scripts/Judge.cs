using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge
{
    private BoatModel boat;

    public Judge(BoatModel b)
    {
        boat = b;
    }

    public int Check(int sp, int sd, int ep, int ed)
    {
        int start_priest = sp;
        int start_devil = sd;
        int end_priest = ep;
        int end_devil = ed;

        if (end_priest + end_devil == 6)//win
            return 2;

        int[] boat_role_num = boat.GetRoleNumber();
        if (boat.GetBoatSign() == 1)//在开始岸和船上的角色
        {
            start_priest += boat_role_num[0];
            start_devil += boat_role_num[1];
        }
        else//在结束岸和船上的角色
        {
            end_priest += boat_role_num[0];
            end_devil += boat_role_num[1];
        }
        if (start_priest > 0 && start_priest < start_devil) //fail
            return 1;
        if (end_priest > 0 && end_priest < end_devil)//fail
            return 1;
        return 0;//未结束
    }
}
