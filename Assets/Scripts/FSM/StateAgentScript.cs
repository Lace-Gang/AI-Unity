using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateAgentScript : AiAgent
{
    [SerializeField] Perception perception;

    public StateMachine stateMachine = new StateMachine();

    public ValueRef<float> timer = new ValueRef<float>();
    public ValueRef<float> health = new ValueRef<float>();
    public ValueRef<float> destinationDistence = new ValueRef<float>();

    public ValueRef<bool> enemySeen = new ValueRef<bool>();
    public ValueRef<float> enemyDistence = new ValueRef<float>();

    public AiAgent enemy;


    private void Start()
    {
        //print("start");
        stateMachine.AddState(nameof(AiIdleState), new AiIdleState(this));
        stateMachine.AddState(nameof(AiPatrolState), new AiPatrolState(this));

        stateMachine.SetState(nameof(AiIdleState));
    }

    private void Update()
    {
        //print("AgentUpdate");

        //update parameters
        timer.value -= Time.deltaTime;

        //stateMachine.CurrentState?.CheckTransition();
        stateMachine.CurrentState?.CheckTransitions();
        stateMachine.Update();

            //Enemies
        if (perception != null)
        {
            var gameObjects = perception.GetGameObjects();
            //gameObject is what this component/script is attatched to
            //so in the bellow, it needs to be something else (in this case 'go')
            enemySeen.value = gameObjects.Length > 0;


            if (gameObjects.Length > 0) //if it sees any gameobjects
            {
                gameObjects[0].TryGetComponent<AiAgent>(out enemy);
                enemyDistence.value = transform.position.DistanceXZ(gameObjects[0].transform.position);
                movement.Destination = gameObjects[0].transform.position;
            }
        }

        destinationDistence.value = transform.position.DistanceXZ(movement.Destination);


        //Vector3 direction = (agent.transform.position - destination);
        
        //float distance  = (agent.transform.position - destination).magnitude;  
        //float distance = direction.magnitude; 

        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);
        }
    }


    private void OnGUI()
    {
        // draw label of current state above agent
        GUI.backgroundColor = Color.black;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        Rect rect = new Rect(0, 0, 100, 20);
        // get point above agent
        Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
        rect.x = point.x - (rect.width / 2);
        rect.y = Screen.height - point.y - rect.height - 20;
        // draw label with current state name
        GUI.Label(rect, stateMachine.CurrentState.Name);
    }

}
