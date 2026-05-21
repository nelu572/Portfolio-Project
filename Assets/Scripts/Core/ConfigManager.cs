using System;
using UnityEngine;

namespace PortfolioFilling.Core
{
    [Serializable]
    public sealed class HarnessGameConfig
    {
        public float walkSpeed = 5.5f;
        public float sprintSpeed = 8f;
        public float jumpHeight = 1.1f;
        public float gravity = -20f;
        public float lookSensitivity = 0.12f;
        public float playerMaxHealth = 100f;
        public float interactRange = 3f;
        public float zombieMoveSpeed = 2.6f;
        public float zombieAttackDamage = 8f;
        public float zombieAttackRange = 1.35f;
        public float zombieAttackCooldown = 1f;
        public float zombieMaxHealth = 40f;
        public int startingScrap = 50;
    }

    public sealed class ConfigManager : MonoBehaviour
    {
        [SerializeField] private HarnessGameConfig config = new();

        public HarnessGameConfig Runtime => config;
    }
}
