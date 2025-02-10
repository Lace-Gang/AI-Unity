using UnityEngine;

public class StateAgentScript : AiAgent
{
    [SerializeField] Perception perception;


    private void Update()
    {
        //SEEK
        if (perception != null)
        {
            var gameObjects = perception.GetGameObjects();
            //gameObject is what this component/script is attatched to
            //so in the bellow, it needs to be something else (in this case 'go')

            if (gameObjects.Length > 0) //if it sees any gameobjects
            {
                movement.Destination = gameObjects[0].transform.position;
            }
        }
    }

}
