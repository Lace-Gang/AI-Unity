using System;
using System.Threading;
using UnityEngine;

public class AiIdleState : AiState
{
    //float timer;

    ValueCondition<float> timerCheck;

    public AiIdleState(StateAgentScript agent) : base(agent)
    {
        ////an example of how to use the Conditions
        ////var distanceCheck = new ValueCondition<float>(agent.destinationDistence, Condition.Predicate.Less, 5);
        ////distanceCheck.IsTrue();
        //timerCheck = new ValueCondition<float>(agent.timer, Condition.Predicate.LessOrEqual, 0);
        ////timerCheck.IsTrue();
        //
        //var transition = new StateTransition(nameof(AiPatrolState)); //example of roughly how to construct this.
        //transition.AddCondition(new ValueCondition<float>(agent.timer, Condition.Predicate.LessOrEqual, 0));
        ////transition.AddCondition(timerCheck);
        ///

        CreateTransition(nameof(AiPatrolState)).
        AddCondition(agent.timer, Condition.Predicate.LessOrEqual, 0);//.AddCondition
        
    }


    public override void OnEnter()
    {
        ////timer = UnityEngine.Random.Range(1, 3);
        agent.timer.value = UnityEngine.Random.Range(1, 3); //probably a way to put this somewhere else? But I'm not sure
        //agent?.movement.Stop();
    }

    public override void OnExit()
    {
       //
    }

    public override void OnUpdate()
    {
        //Console.WriteLine("update"); //As far as I can tell, this line of code is useless.
        
        //timer -= Time.deltaTime; //supposed to be able to take this out and replace with agent.timer, but not sure yet how to do this (with the random times)

        //transition
        //if (agent.timer.value <= 0)
        //if (timerCheck.IsTrue()) 
        //{
        //    agent.stateMachine.SetState(nameof(AiPatrolState));
        //}
    }
}
