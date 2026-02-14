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
        private const string DIE_KEY = "isDead";
        private const string RUN_KEY = "isRun";

        public static readonly int IS_DEAD_HASH = Animator.StringToHash(DIE_KEY);
        public static readonly int IS_RUN_HASH = Animator.StringToHash(RUN_KEY);

        public const float SinkWaitDuration = 2.0f;
        public const float SinkMoveDuration = 4.0f;
        public const float SinkDepth = -1.5f;
    }
    public class SoldierAnimation
    {
        public const string MOVEMENT_KEY = "MovementSpeed";
        public const string BLEND_MOVE_X = "MoveX";
        public const string BLEND_MOVE_Y = "MoveY";
        public const string DIE_KEY = "Die";
        public const string FIRE_KEY = "Fire";
        public const string RELOAD_KEY = "Reload";

        public const float ReloadWaitDuration = 3.5f;
        public const float AttackWaitDuration = 0.4f;
        public const float DieWaitDuration = 3.5f;
    }
}
