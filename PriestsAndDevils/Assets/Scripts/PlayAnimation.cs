using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public void Play()                                  //播放gameover状态的动画
    {
        Animator anim = this.GetComponent<Animator>();
        anim.SetBool("isgameover", true);
    }
    public void NotPlay()                              //播放Idle状态的动画
    {
        Animator anim = this.GetComponent<Animator>();
        anim.SetBool("isgameover", false);
    }
}
