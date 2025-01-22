using UnityEngine;
using UnityEngine.Rendering;

public class AutonomousAgent : AiAgent
{
    [Header("Wander")]
    [SerializeField] float displacement;
    [SerializeField] float distance;
    [SerializeField] float radius;

    [Header("Perception")]
    //public Perception perception;
    public Perception seekPerception;
    public Perception fleePerception;

    //FloatRangeParameter angle; //I think this was a typo, but just in case
    float angle;

    private void Update()
    {
        //movement.ApplyForce(Vector3.forward * 10 );


        //Debug.DrawRay(transform.position, transform.forward * perception.maxDistance, Color.yellow);
       
        //SEEK
        if(seekPerception != null )
        {
            var gameObjects = seekPerception.GetGameObjects();
            //gameObject is what this component/script is attatched to
            //so in the bellow, it needs to be something else (in this case 'go')

            if (gameObjects.Length > 0) //if it sees any gameobjects
            {
                Vector3 force = Seek(gameObjects[0]); //go towards the first in the array
                movement.ApplyForce(force);

            }
        }


        //FLEE
        if( fleePerception != null ) 
            {
            var gameObjects = fleePerception.GetGameObjects();

            if (gameObjects.Length > 0) 
            {
                Vector3 force = Seek(gameObjects[0]); //go away from the first in the array
                movement.ApplyForce(-force);

            }

        }

        //WANDER - if not moving (seeking/fleeing)
        if(movement.Acceleration.sqrMagnitude == 0)
        {

            //angle += Random.Range(-displacement, displacement);
            //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            //Vector3 point = rotation * (Vector3.forward * radius);

            //Vector3 force = GetSteeringForce(forwa)


            //randomly adjust angle +/- displacement
            angle += Random.Range(-displacement, displacement);

            //create rotation quaternion around y axis (up)
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 point = rotation * (Vector3.forward * radius);

            //set point in front of agent
            Vector3 forward =  movement.Direction * distance;

            //spply force towards point in front
            Vector3 force = GetSteeringForce(forward + point);
            movement.ApplyForce(force);

        }

        //because otherwise they might start to spontaneously fly.
        Vector3 acceleration = movement.Acceleration;
        acceleration.y = 0;
        movement.Acceleration = acceleration;

        //foreach (var go in gameObjects)
        //{
        //    Debug.DrawLine(transform.position, go.transform.position, Color.red);
        //}
        
        
        float size = 25;
        transform.position = Utilities.Wrap(transform.position, new Vector3( -size, -size, -size ), new Vector3( size, size, size ));


    }


    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }


    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;

        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);

        return force;
    }
}
