using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDestroyer : MonoBehaviour
{
    private bool destroying;
    private float dueTime = 0.6f;
    private Transform _transform;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (dueTime > 0)
        {
            dueTime -= Time.deltaTime;
            if (dueTime <= 0)
            {
                destroying = true;
            }
        }
        if (destroying)
        {
            _transform.localScale = new Vector3(1, transform.localScale.y - 2.5f * Time.deltaTime, 1);
            if (_transform.localScale.y <= 0)
            {
                Destroy(this);
            }
        }
    }
}
