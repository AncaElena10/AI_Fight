using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FighterShuriken : MonoBehaviour
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
        if (gameObject.transform.position.x >= 10f || gameObject.transform.position.x <= -10f)
        {
            Destroy(gameObject);
            _fighter.CanShoot = true;
        } else {
            _fighter.CanShoot = false;
        }

        // distance between players
        distanceBetweenPlayers = Math.Abs(_fighter.transform.position.x - _enemy.transform.position.x);

        //print(distanceBetweenPlayers);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Enemy")) {
            if (_myAnim.GetIsEnemyParry()) {
                if (distanceBetweenPlayers <= 7f)
                {
                    print("Enemy in parry mode and distance less than 7f. Dealing a biiiiiit damage.");
                    _hm.SetEnemyHealth(shurikenDamage4);
                    Destroy(gameObject);
                    _fighter.CanShoot = true;
                }
                else
                {
                    print("Enemy in parry mode and distance more than 7f. Dealing 2.5 damage.");
                    _hm.SetEnemyHealth(shurikenDamage2);
                    Destroy(gameObject);
                    _fighter.CanShoot = true;
                }
            } else {
                if (distanceBetweenPlayers <= 7f)
                {
                    print("Enemy hit by shuriken and distance less than 7f. Dealing 1 damage.");
                    _fighter.PlayHurtAnim_Enemy(shurikenDamage3);
                    _enemy.ShurikenHit = false;
                    Destroy(gameObject);
                    _fighter.CanShoot = true;
                } else
                {
                    print("Enemy hit by shuriken and distance more than 7f. Dealing normal damage.");
                    _fighter.PlayHurtAnim_Enemy(shurikenDamage1);
                    _enemy.ShurikenHit = false;
                    Destroy(gameObject);
                    _fighter.CanShoot = true;
                }
            }
        }

        // Enemy dead
        if (collision.gameObject.name.Equals("Enemy") && _hm.GetEnemyHealth() <= 0f) {
            print("Enemy dead!");
            _dm.setIsDeadParam(true);
            _myAnim.GetEnemyAnimator().SetTrigger("isDead");
            Destroy(gameObject);
        }
    }
}
