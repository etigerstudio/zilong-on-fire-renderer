using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChild : MonoBehaviour
{   
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // print("1231");
        GameObject go = GameObject.Find("WorldRoot/ActorZilong(Clone)");
        foreach (Transform child in go.transform)
        { 
            print(child.gameObject.transform.position);
        }

    }
}
