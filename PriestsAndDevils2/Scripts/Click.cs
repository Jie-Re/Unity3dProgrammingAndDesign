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
        if (this.role == null)
            Debug.Log("SetRole failed");
    }

    public void SetBoat(BoatModel boat)
    {
        this.boat = boat;
        if (this.boat == null)
            Debug.Log("SetRole failed");
    }

    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    void OnMouseDown()
    {
        if (boat == null && role == null) return;
        if (boat != null)
            action.MoveBoat();
        else if (role != null)
            action.MoveRole(role);
    }
}
