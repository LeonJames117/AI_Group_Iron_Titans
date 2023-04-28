using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flocking_Behavior : ScriptableObject
{
   public abstract Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform>World_Context, Flocking_Controller Controller, Flocking_Leader Leader);
   
}
