using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

struct Movement
{
    public const string L = "LEFTWARD";
    public const string R = "RIGHTWARD";
    public const string F = "FORWARD";
    public const string B = "BACKWARD";
    public const string I = "IDLE";
    
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
    public const string DIE = "DIE";
    public const string I = "WALK";
  
    
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
    private RPGWorldBuilder _world;
    private ControlSequence _cs;
    private float remainingTime = 0f;
    private Tuple<Movement, Spell> _ms;
    private float movementSpeed = 2f;
    private float angleOffset = 0f;
    private float angluarSpeed = 360f;
    private bool dead;
    private float deadAfter = 0f;
    private int jumpStride = 2;
    public string[] actions;
    private ActorAnimationController _ani;
    
    // Start is called before the first frame update
    void Start()
    {
        actions = GetData.getActions();
        _cs = GetControlSequence();
        _world = GameObject.Find("WorldBuilder").GetComponent<RPGWorldBuilder>();
        _ani = GetComponent<ActorAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            deadAfter -= Time.deltaTime;
            if (deadAfter <= 0)
            {
                _ani.SetState(false, false, false, true);
                return;
            }
        }
        
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            if (_cs.HasNext())
            {
                _ms = _cs.GetNext();
                remainingTime = GetRemainingTime(_ms);
                HandleInteraction(_ms);
            }
            else
            {
                return;
            }
        }

        if (_ms != null)
        {
            UpdateMovement(_ms);
        }
    }

    void HandleInteraction(Tuple<Movement, Spell> ms)
    {
        Entity actor = _world.GetActorEntity();
        int dx = 0, dy = 0;
        switch (ms.Item1.type)
        {
            case Movement.L:
                dx = -1;
                break;
            case Movement.R:
                dx = 1;
                break;
            case Movement.B:
                dy = -1;
                break;
            case Movement.F:
                dy = 1;
                break;
            default:
                break;
        }

        switch (ms.Item2.type)
        {
            case Spell.I:
            {
                Entity e = _world.FindEntity(actor.x + dx, actor.y + dy);
                if (e != null)
                {
                    switch (e.type)
                    {
                        case Entity.SPIKE:
                            dead = true;
                            deadAfter = 0.6f;
                            break;
                        case Entity.BARRIER:
                            dead = true;
                            deadAfter = 0.4f;
                            break;
                    }
                }

                actor.x += dx;
                actor.y += dy;
                break;
            }
            case Spell.SLASH:
            {
                Entity e = _world.FindEntity(actor.x + dx, actor.y + dy);
                if (e != null)
                {
                    switch (e.type)
                    {
                        case Entity.BARRIER:
                            e.obj.AddComponent<ScaleDestroyer>();
                            _world.RemoveEntity(e);
                            break;
                    }
                }
                break;
            }
            case Spell.JUMP:
            {
                for (int i = 1; i <= jumpStride; i++)
                {
                    Entity e = _world.FindEntity(actor.x + dx * i, actor.y + dy * i);
                    if (e != null)
                    {
                        switch (e.type)
                        {
                            case Entity.BARRIER:
                                dead = true;
                                deadAfter = i - 1 + 0.4f;
                                goto Done;
                                break;
                            case Entity.SPIKE:
                                if (i == jumpStride)
                                {
                                    dead = true;
                                    deadAfter = i;
                                    goto Done;
                                }
                                break;
                        }
                    }
                }
                
                Done:
                actor.x += jumpStride * dx;
                actor.y += jumpStride * dy;
                break;
            }
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
                _ani.SetState(ms.Item1.type != Movement.I, false, false, false);
                transform.position += offset;
                
                break;
            case Spell.SLASH:
                _ani.SetState(false, false, true, false);
                break;
            case Spell.JUMP:
                _ani.SetState(false, true, false, false);
                transform.position += offset;
                break;
            case Spell.DIE:
                _ani.SetState(false, false, false, die:true);
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
        cs._controls = new Tuple<Movement, Spell>[actions.Length];
        for (int i = 0; i < actions.Length; i ++)
        {            
            string[] s_action = System.Text.RegularExpressions.Regex.Replace(actions[i], @"\s+", ",").Split(',');
            string move_action = s_action[0];
            string spell_action = s_action[1];
            cs._controls[i] = new Tuple<Movement, Spell>(new Movement(move_action), new Spell(spell_action));
        }
        // cs._controls = new Tuple<Movement, Spell>[]
        // {
        //     // new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.I)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.R), new Spell(Spell.I)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.I)),
        //     // new Tuple<Movement, Spell>(new Movement(Movement.R), new Spell(Spell.I)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.JUMP)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.R), new Spell(Spell.SLASH)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.I), new Spell(Spell.I)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.I), new Spell(Spell.DIE)), 
        //     // new Tuple<Movement, Spell>(new Movement(Movement.I), new Spell(Spell.JUMP)), 
        //     
        //     // new Tuple<Movement, Spell>(new Movement(Movement.I), new Spell(Spell.EXIT)), 
        //     new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.SLASH)), 
        //     new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.I)), 
        //     new Tuple<Movement, Spell>(new Movement(Movement.R), new Spell(Spell.JUMP)), 
        //     new Tuple<Movement, Spell>(new Movement(Movement.B), new Spell(Spell.I)),
        //     // new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.I)),
        //     // new Tuple<Movement, Spell>(new Movement(Movement.F), new Spell(Spell.SLASH)),
        //     
        //     
        // };
        return cs;
    }
}
