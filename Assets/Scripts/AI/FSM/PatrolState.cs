using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public override State RunCurrentState(StateManager manager)
    {
        return this;
    }
}
