using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyShuriken : MonoBehaviour
{
    private static Fighter _fighter;
    private static Enemy _enemy;

	AnimationsManager _myAnim = new AnimationsManager();
	HealthManager _hm = new HealthManager();
	DeathManager _dm = new DeathManager();

    float shurikenDamage1 = 5f; // normal shuriken damage
    float shurikenDamage2 = 2.5f; // dist > 7 + parry
    float shurikenDamage3 = 1f; // dist <= 7
    float shurikenDamage4 = 0.5f; // dist <= 7 + parry

    float distanceBetweenPlayers = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _fighter = GameObject.FindGameObjectWithTag("Fighter").GetComponent<Fighter>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    private void Update()
    {
        // when the shuriken reaches the limits, destroy it
        if (gameObject.transform.position.x >= 10f || gameObject.transform.position.x <= -10f) {
            Destroy(gameObject);
            _enemy.CanShoot = true;
        } else {
            _enemy.CanShoot = false;
        }

        // distance between players
        distanceBetweenPlayers = Math.Abs(_fighter.transform.position.x - _enemy.transform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.name.Equals("Fighter")) {
            if (_myAnim.GetIsFighterParry()) {
                if (distanceBetweenPlayers <= 7)
                {
                    print("Fighter in parry mode and distance less than 7f. Dealing a biiiiit damage.");
                    _hm.SetFighterHealth(shurikenDamage4);
                    Destroy(gameObject);
                    _enemy.CanShoot = true;
                } else
                {
                    print("Fighter in parry mode and distance more than 7f. Dealing 2.5 damage.");
                    _hm.SetFighterHealth(shurikenDamage2);
                    Destroy(gameObject);
                    _enemy.CanShoot = true;
                }
            } else {
                if (distanceBetweenPlayers <= 7f)
                {
                    print("Fighter hit by shuriken and distance less than 7f. Dealing 1 damage.");
                    _enemy.PlayHurtAnim_Fighter(shurikenDamage3);
                    _fighter.ShurikenHit = false;
                    Destroy(gameObject);
                    _enemy.CanShoot = true;
                } else
                {
                    print("Fighter hit by shuriken and distance more than 7f. Dealing normal damage.");
                    _enemy.PlayHurtAnim_Fighter(shurikenDamage1);
                    _fighter.ShurikenHit = false;
                    Destroy(gameObject);
                    _enemy.CanShoot = true;
                }
            }
        }

        // Fighter dead
        if (collision.gameObject.name.Equals("Fighter") && _hm.GetFighterHealth() <= 0f) {
            print("Fighter dead!");
            _dm.setIsDeadParam(false);
            _myAnim.GetFighterAnimator().SetTrigger("isDead");
            Destroy(gameObject);
        }
    }
}
