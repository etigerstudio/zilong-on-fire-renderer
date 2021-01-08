using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;

    public float currentAngle;
    // private bool _walk = false;
    // private bool _jump = false;
    // private bool _slash = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // if (Input.GetKey(KeyCode.W))
    //     // {
    //     //     if (!_walk)
    //     //     {
    //     //         _animator.SetBool("walk", true);
    //     //         _walk = true;
    //     //     }
    //     // }
    //     // else
    //     // {
    //     //     if (_walk)
    //     //     {
    //     //         _animator.SetBool("walk", false);
    //     //         _walk = false;
    //     //     }
    //     // }
    //     //
    //     // if (Input.GetKeyDown(KeyCode.Space))
    //     // {
    //     //     if (!_jump)
    //     //     {
    //     //         _animator.SetBool("jump", true);
    //     //         _jump = true;
    //     //     }
    //     // }
    //     // else
    //     // {
    //     //     if (_jump)
    //     //     {
    //     //         _animator.SetBool("jump", false);
    //     //         _jump = false;
    //     //     }
    //     // }
    //     // if (Input.GetKeyDown(KeyCode.S))
    //     // {
    //     //     if (!_slash)
    //     //     {
    //     //         _animator.SetBool("slash", true);
    //     //         _slash = true;
    //     //     }
    //     // }
    //     // else
    //     // {
    //     //     if (_slash)
    //     //     {
    //     //         _animator.SetBool("slash", false);
    //     //         _slash = false;
    //     //     }
    //     // }
    // }

    public void SetState(bool walk, bool jump, bool slash)
    {
        _animator.SetBool("walk", walk);
        _animator.SetBool("jump", jump);
        _animator.SetBool("slash", slash);
        print($"{walk} {jump} {slash}");
    }
}
