using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistencePerception : Perception
{
    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();


        //get all colliders inside sphere
        var colliders = Physics.OverlapSphere(transform.position, maxDistance);
        //var is the c# version of auto


        foreach (var collider in colliders)
        {
            //do not include ourseives
            if (collider.gameObject == gameObject) continue;
            //check for matching tag
            if(tagName == "" || collider.tag == tagName)
            {
                //check if anble within max angle range
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle <= maxAngle)
                {
                    //add gameobject to result
                    result.Add(collider.gameObject);
                }
            }
        }

        return result.ToArray();
    }
}
