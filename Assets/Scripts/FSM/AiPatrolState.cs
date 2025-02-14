using UnityEngine;

public class AiPatrolState : AiState
{
    //Vector3 destination = Vector3.zero;


    public AiPatrolState(StateAgentScript agent) : base(agent)
    {
        CreateTransition(nameof(AiIdleState)).
            AddCondition(agent.destinationDistence, Condition.Predicate.Less, 1.25f);
    }


    public override void OnEnter()
    {
        //agent.movement.Destination = NavNode.GetRandomNavNode().transform.position;
        agent.movement.Destination = NavNode.GetRandomNavNode().transform.position;
        agent.movement.Resume();
    }


    public override void OnUpdate()
    {
        
        //if(agent.destinationDistence <= 1.25f)
        //{
        //    agent.stateMachine.SetState(nameof(AiIdleState)); //nameof helps get around how error prone strings can be
        //}
    }
    public override void OnExit()
    {
        //
    }
}
