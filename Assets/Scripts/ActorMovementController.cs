using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

struct Movement
{
    public const string L = "L";
    public const string R = "R";
    public const string F = "F";
    public const string B = "B";
    public const string I = "I"; // Idle
    
    public string type;

    public Movement(string type)
    {
        this.type = type;
    }
}

struct Spell
{
    public const string JUMP = "JUMP";
    public const string SLASH = "SLASH";
    public const string I = "I";
    
    public string type;

    public Spell(string type)
    {
        this.type = type;
    }
}

class ControlSequence
{
    private int i = 0;
    
    public Tuple<Movement, Spell>[] _controls;
    public string terminal;

    public bool HasNext()
    {
        return _controls.Length > i;
    }
    
    public Tuple<Movement, Spell> GetNext()
    {
        Tuple<Movement, Spell> next = _controls[i];
        ++i;
        return next;
    }
}
public class ActorMovementController : MonoBehaviour
{
    private ControlSequence _cs;
    private float remainingTime = 0f;
    private Tuple<Movement, Spell> ms;
    private float movementSpeed = 2f;
    private float angleOffset = 0f;
    private float angluarSpeed = 360f;

    private ActorAnimationController _ani;
    
    // Start is called before the first frame update
    void Start()
    {
        _cs = GetControlSequence();
        _ani = GetComponent<ActorAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            if (_cs.HasNext())
            {
                ms = _cs.GetNext();
                remainingTime = GetRemainingTime(ms);
            }
            else
            {
                return;
            }
        }

        if (ms != null)
        {
            UpdateMovement(ms);
        }
    }
    
    void UpdateMovement(Tuple<Movement, Spell> ms)
    {
        Vector3 offset;
        float currentAngle = transform.rotation.eulerAngles.y;
        float targetAngle;
        switch (ms.Item1.type)
        {
            case Movement.L:
                offset = Vector3.left * (Time.deltaTime * movementSpeed);
                targetAngle = -90f;
                break;
            case Movement.R:
                offset = Vector3.right * (Time.deltaTime * movementSpeed);
                targetAngle = 90f;
                break;
            case Movement.B:
                offset = Vector3.back * (Time.deltaTime * movementSpeed);
                targetAngle = -180f;
                break;
            case Movement.F:
                offset = Vector3.forward * (Time.deltaTime * movementSpeed);
                targetAngle = 0f;
                break;
            default:
                targetAngle = currentAngle;
                offset = Vector3.zero;
                break;
        }

        float factor = 0f;
        float offsetAngleL = targetAngle - currentAngle ;
        float offsetAngleR = targetAngle - currentAngle + 360;
        float offsetAngle = offsetAngleL;
        if (Math.Abs(offsetAngle) >= 2f)
        {
            if (Math.Abs(offsetAngleL) > Math.Abs(offsetAngleR))
                offsetAngle = offsetAngleR;
            if (offsetAngle > 0)
                factor = 1f;
            else if (offsetAngle < 0)
                factor = -1f;
            transform.rotation = Quaternion.Euler(
                0,
                transform.rotation.eulerAngles.y + 360f * Time.deltaTime * factor,
                0);
        }
        switch (ms.Item2.type)
        {
            case Spell.I:
                _ani.SetState(ms.Item1.type != Movement.I, false, false);
                transform.position += offset;
                break;
            case Spell.SLASH:
                _ani.SetState(false, false, true);
                break;
            case Spell.JUMP:
                _ani.SetState(false, true, false);
                transform.position += offset;
                break;
        }
    }
    
    float GetRemainingTime(Tuple<Movement, Spell> ms)
    {
        if (ms.Item2.type == Spell.JUMP)
        {
            return 2f;
        }

        return 1f;
    }
    
    ControlSequence GetControlSequence()
    {
        ControlSequence cs = new ControlSequence();
        cs._controls = new Tuple<Movement, Spell>[]
        {
            new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.I)), 
            new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.I)),
            new Tuple<Movement, Spell>(new Movement(Movement.R), new Spell(Spell.I)), 
            new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.JUMP)), 
            new Tuple<Movement, Spell>(new Movement(Movement.R), new Spell(Spell.SLASH)), 
            new Tuple<Movement, Spell>(new Movement(Movement.B), new Spell(Spell.I)), 
        };
        return cs;
    }
}
