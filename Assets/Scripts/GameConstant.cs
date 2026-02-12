using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstant
{
    public class PlayerInput
    {
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
    }
    public class PoolGroups
    {
        public const string BULLETS = "BulletsPool";
        public const string ENEMIES = "EnemyPool";
    }
    public class Tag
    {
        public const string PLAYER = "Player";
    }
    public class Timers
    {
        public const float GATE_DESTROY_TIME = 0.1f;
    }
    public class PlayerSettings
    {
        public const int MINIMUM_SOLDIER_COUNT = 1;
    }
    public class GatePartTypes
    {
        public const string GATE_PILLAR = "Pillar";
        public const string GATE_PANEL = "Panel";
    }
    public class EnemyAnimation
    {
        private const string DieKey = "isDead";
        private const string RunKey = "isRun";

        public static readonly int IsDeadHash = Animator.StringToHash(DieKey);
        public static readonly int IsRunHash = Animator.StringToHash(RunKey);

        public const float SinkWaitDuration = 2.0f;
        public const float SinkMoveDuration = 4.0f;
        public const float SinkDepth = -1.5f;
    }
}
