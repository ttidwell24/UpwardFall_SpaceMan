using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int currentHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 25;

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public EnemyStats Stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmt = 0.1f;
    public float shakeLen = 0.1f;

    public string deathSoundName = "Explosion";

    //public int moneyDrop = 20;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start()
    {
        Stats.Init();

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(Stats.currentHealth, Stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if (deathParticles == null)
        {
            Debug.LogError("No death particles referenced on enemy!");
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<EnemyAI>().enabled = !active;
    }

    public void DamageEnemy(int damage)
    {
        Stats.currentHealth -= damage;
        if (Stats.currentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(Stats.currentHealth, Stats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(Stats.damage);
            DamageEnemy(99999999);
        }
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
