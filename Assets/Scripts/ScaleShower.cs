using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ScaleShower : MonoBehaviour
{
    private bool showing;
    private float dueTime = 27.5f;
    private Transform _transform;
    private float oldY;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        oldY = _transform.localPosition.y;
        _transform.localPosition = new Vector3(_transform.localPosition.x, -100, _transform.localPosition.z);
        _transform.localScale = new Vector3(1, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (dueTime > 0)
        {
            dueTime -= Time.deltaTime;
            if (dueTime <= 0)
            {
                showing = true;
                _transform.localPosition = new Vector3(_transform.localPosition.x, oldY, _transform.localPosition.z);
            }
        }
        if (showing)
        {
            if (_transform.localScale.y < 1)
            {
                _transform.localScale = new Vector3(1, _transform.localScale.y + 2.5f * Time.deltaTime, 1);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
