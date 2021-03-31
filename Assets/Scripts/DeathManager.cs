using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private static Fighter _fighter;
    private static Enemy _enemy;

    CollisionsManager _cm = new CollisionsManager();

    private void Start()
    {
        _fighter = GameObject.FindGameObjectWithTag("Fighter").GetComponent<Fighter>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    public void setIsDeadParam(bool isFighter)
    {
        // set the isDead for Enemy
        if (isFighter) {
            _enemy.IsDead = true;
            _cm.GetEnemyRigidbody().isKinematic = true;

        } else // set the isDead for Fighter
        {
            _fighter.IsDead = true;
            _cm.GetFighterRigidbody().isKinematic = true;
        }
    }
}
