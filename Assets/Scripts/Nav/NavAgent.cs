using UnityEngine;

// Ensure this component requires a NavPath component
[RequireComponent(typeof(NavPath))]
public class NavAgent : AiAgent
{
    // Reference to the navigation path component
    private NavPath path;

    private void Start()
    {
        // Get the NavPath component attached to this GameObject
        path = GetComponent<NavPath>();

        // Find the nearest navigation node based on the agent's current position
        var startNode = NavNode.GetNearestNavNode(transform.position);

        // If a valid navigation node is found, set the path's destination to that node
        if (startNode != null)
        {
            path.Destination = startNode.transform.position;
        }
    }

    private void Update()
    {
        // If the agent has a target destination, move towards it
        if (path.HasTarget)
        {
            movement.MoveTowards(path.Destination);
        }
        else
        {
            // If there is no target, assign a random navigation node as the destination
            NavNode destinationNode = NavNode.GetRandomNavNode();

            // Ensure the node is valid before setting it as the destination
            if (destinationNode != null)
            {
                path.Destination = destinationNode.transform.position;
            }
        }

        // Ensure the agent faces the direction of movement
        transform.forward = movement.Direction;
    }
}