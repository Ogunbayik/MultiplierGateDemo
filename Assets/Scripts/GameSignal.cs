using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSignal
{ 

    public class PlayerTriggeredGateSignal
    {
        public Gate Gate;
        public PlayerTriggeredGateSignal(Gate gate) => Gate = gate;
    }

    public class SoldierDeadSignal
    {
        public Soldier DeadSoldier;
        public SoldierDeadSignal(Soldier deadSoldier) => DeadSoldier = deadSoldier;
    }

}
