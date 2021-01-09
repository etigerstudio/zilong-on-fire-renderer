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
    private bool _die = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetState(bool walk, bool jump, bool slash, bool die)
    {
        _animator.SetBool("walk", walk);
        _animator.SetBool("jump", jump);
        _animator.SetBool("slash", slash);
        if (!_die && die)
        {
            _die = true;
            _animator.SetTrigger("die");
        }
        // print($"{walk} {jump} {slash} {die} ");
    }
}
