using UnityEngine;

public class KinematicMovement : Movement
{
    public override void ApplyForce(Vector3 force)
    {
        Acceleration += force;
    }

    private void LateUpdate() //What is late update?
    {
        Velocity += Acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);

        transform.position += Velocity * Time.deltaTime;

        Acceleration = Vector3.zero; //zero to recalculate
    }
}
