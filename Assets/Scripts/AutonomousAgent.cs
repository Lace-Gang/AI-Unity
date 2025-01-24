using UnityEngine;
using UnityEngine.Rendering;

public class AutonomousAgent : AiAgent
{
    //___replaced to AutonomousDataAgent
    //[Header("Wander")]
    //[SerializeField] float displacement;
    //[SerializeField] float distance;
    //[SerializeField] float radius;
    [SerializeField] AutonomousAgentData data;

    [Header("Perception")]
    //public Perception perception;
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;

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
                Vector3 force = Seek(gameObjects[0]); //move towards the first in the array
                movement.ApplyForce(force);
            }
        }


        //FLEE
        if( fleePerception != null ) 
        {
            var gameObjects = fleePerception.GetGameObjects();

            if (gameObjects.Length > 0) 
            {
                Vector3 force = Flee(gameObjects[0]); //will run away from the first thing in the array
                movement.ApplyForce(force);
            }
        }


        //FLOCK
        if( flockPerception != null )
        {
            var gameObjects = flockPerception.GetGameObjects();

            if (gameObjects.Length > 0)
            {
                //flocking requires three different actions IN THIS ORDER
                movement.ApplyForce(Cohesion(gameObjects) * data.cohesionWeight);
                movement.ApplyForce(Seperation(gameObjects, data.seperationRadius) * data.seperationWeight);
                movement.ApplyForce(Alignment(gameObjects) * data.alignmentWeight);
            }
        }


        //WANDER - if not moving (seeking/fleeing)
        if(movement.Acceleration.sqrMagnitude == 0)
        {
            //will pick a semi-random direction to move in
            Vector3 force = Wander();
            movement.ApplyForce(force);

        }

        //because otherwise they might start to spontaneously fly.
        Vector3 acceleration = movement.Acceleration;
        acceleration.y = 0;
        movement.Acceleration = acceleration;


        //so that they face the direction they are moving in (looks strange otherwise
        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);   
        }


        //foreach (var go in gameObjects)
        //{
        //    Debug.DrawLine(transform.position, go.transform.position, Color.red);
        //}
        
        
        //keeps the agents from just completely leaving the game area.
        float size = 25;
        transform.position = Utilities.Wrap(transform.position, new Vector3( -size, -size, -size ), new Vector3( size, size, size ));


    }



    //calucaltes force vector to move towards a specific object
    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }


    //calucaltes force vector to move away from a specific object

    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return -force;
    }

    //calucaltes force vector to move towards an average center of a group
    private Vector3 Cohesion(GameObject[] neighbors)
    {
        //find the average center point of all neighbors
        Vector3 positions = Vector3.zero;
        foreach(GameObject neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }

        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    //calucaltes force vector to move far enough (radius) away from the other members of the group (so they aren't colliding/phasing through each other)

    private Vector3 Seperation(GameObject[] neighbors, float radius)
    {
        return Vector3.zero;
    }

    //calucaltes force vector to move in the same average direction as the group
    private Vector3 Alignment(GameObject[] neighbors)
    {
        return Vector3.zero;
    }

    //calucaltes force vector to move in a semi-random direction.
    private Vector3 Wander()
    {
        //angle += Random.Range(-displacement, displacement);
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //Vector3 point = rotation * (Vector3.forward * radius);

        //Vector3 force = GetSteeringForce(forwa)


        //randomly adjust angle +/- displacement
        angle += Random.Range(-data.displacement, data.displacement);

        //create rotation quaternion around y axis (up)
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 point = rotation * (Vector3.forward * data.radius);

        //set point in front of agent
        Vector3 forward = movement.Direction * data.distance;

        //spply force towards point in front
        Vector3 force = GetSteeringForce(forward + point);
        return force;
    }

    //when given a direction, calculates and returns the force vector necessary to move in that direction
    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;

        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxForce);

        return force;
    }
}
