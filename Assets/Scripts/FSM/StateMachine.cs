using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<string, AiState> states = new Dictionary<string, AiState>();

    public AiState CurrentState { get; private set; }


    public void Update()
    {
        //Console.WriteLine("updateSM");
        //print("AgentUpdate");


        CurrentState?.OnUpdate();
    }

    public void AddState(string name, AiState state)
    {
        //The assert is so that the same state is not added twice
        //and so that two states with the same name are not added
        Debug.Assert(!states.ContainsKey(name), "State machine already contains state " + name);
        states[name] = state;
    }

    public void SetState(string name)
    {
        //assert that state exists before tryint to set it
        Debug.Assert(states.ContainsKey(name), "State machine does not contain state " + name);

        var newState = states[name];
        if (newState == CurrentState) return;

        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();

    }
}
