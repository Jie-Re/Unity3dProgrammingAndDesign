using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    IUserAction action;
    RoleModel role = null;
    BoatModel boat = null;
    public void SetRole(RoleModel role)
    {
        this.role = role;
    }
    public void SetBoat(BoatModel boat)
    {
        this.boat = boat;
    }
    void Start()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
    void OnMouseDown()
    {
        if (boat == null && role == null) return;
        if (role != null)
        {
            action.MoveRole(role);
            Debug.Log("moveRole");

        }
        else if (boat != null)
        {
            action.MoveBoat();
            Debug.Log("moveBoat");
        }
    }
}
