using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsManager : MonoBehaviour
{
    private static Fighter _fighter;
    private static Enemy _enemy;

    private static Rigidbody2D _fighterRb;
    private static Rigidbody2D _enemyRb;

    private void Start()
    {
        _fighter = GameObject.FindGameObjectWithTag("Fighter").GetComponent<Fighter>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        _fighterRb = GameObject.FindGameObjectWithTag("Fighter").GetComponent<Rigidbody2D>();
        _enemyRb = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody2D>();
    }

    public Rigidbody2D GetEnemyRigidbody() { return _enemyRb; }

    public Rigidbody2D GetFighterRigidbody() { return _fighterRb; }

    public IEnumerator Hurt(bool fromFighter) // bool isHurting, 
    {
        // knock back the Enemy
        if (fromFighter) {
            _enemy.IsHurting = true;
            _enemyRb.velocity = Vector2.zero;

            if (_enemy.FacingRight)  {
                _enemyRb.AddForce(new Vector2(200f, 200f));
            } else {
                _enemyRb.AddForce(new Vector2(-200f, 200f));
            }

            yield return new WaitForSeconds(0.5f);
            _enemy.IsHurting = false;
        } else // knock back the Fighter
        {
            _fighter.IsHurting = true;
            _fighterRb.velocity = Vector2.zero;

            if (_fighter.FacingRight) {
                _fighterRb.AddForce(new Vector2(-200f, 200f));
            } else {
                _fighterRb.AddForce(new Vector2(200f, 200f));
            }

            yield return new WaitForSeconds(0.5f);
            _fighter.IsHurting = false;
        }
    }
}
