using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUIHealthBar : MonoBehaviour
{
    private Transform father;
    // 当前血量
    public float health = 0.75f;
    public float maxHealth = 1.0f;
    // 增/减后血量
    private float resulthealth;

    private Rect HealthBar;
    private Rect HealthUp;
    private Rect HealthDown;

    void Start()
    {
        father = this.transform.parent.transform;
        resulthealth = health;
    }

    void OnGUI()
    {
        //血条区域
        HealthBar = new Rect(Screen.width / 2 + father.position.x * father.localScale.x * 100 - father.localScale.x * 10, 
            Screen.height / 2 + (father.position.z - father.localScale.y * 7) * father.localScale.z * 10, 
            father.localScale.x * 100, father.localScale.z * 10);
        //加血按钮区域  
        HealthUp = new Rect(Screen.width / 2 + father.position.x * father.localScale.x * 100 - father.localScale.x * 30
            , Screen.height / 2 + (father.position.z - father.localScale.y * 7) * father.localScale.z * 10
            , father.localScale.x * 20, father.localScale.z * 10);
        //减血按钮区域
        HealthDown = new Rect(Screen.width / 2 + father.position.x * father.localScale.x * 100 + father.localScale.x * 90, 
            Screen.height / 2 + (father.position.z - father.localScale.y * 7) * father.localScale.z * 10, 
            father.localScale.x * 20, father.localScale.z * 10);
        if (GUI.Button(HealthUp, "+"))
        {
            resulthealth = resulthealth + 0.1f > 1.0f ? 1.0f : resulthealth + 0.1f;
        }
        if (GUI.Button(HealthDown, "-"))
        {
            resulthealth = resulthealth - 0.1f < 0.0f ? 0.0f : resulthealth - 0.1f;
        }
        //插值计算health值，以实现血条值平滑变化
        health = Mathf.Lerp(health, resulthealth, 0.05f);
        // 用水平滚动条的宽度作为血条的显示值
        GUI.color = Color.red;
        GUI.HorizontalScrollbar(HealthBar, 0, health, 0.0f, maxHealth);
    }
}
