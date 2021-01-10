using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    private float dueTime = 1f;
    private Transform _transform;
    public bool shouldSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GameObject.Find("Star").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldSpawn)
        {
            if (dueTime > 0)
            {
                dueTime -= Time.deltaTime;
            }
            else
            {
                if (_transform.localPosition.y < 1.5f)
                {
                    _transform.localPosition = new Vector3(_transform.localPosition.x, 
                    _transform.localPosition.y + 0.4f * Time.deltaTime, 
                    _transform.localPosition.z);
                }
            }
        }

        _transform.localRotation = Quaternion.Euler(_transform.localEulerAngles.x,
            _transform.localEulerAngles.y + 120 * Time.deltaTime, 
            _transform.localEulerAngles.z);
    }
}