using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private static Animator _fighterAnim;
    private static Animator _enemyAnim;

    static bool fighterParry = false;
    static bool enemyParry = false;

    private static Fighter _fighter;
    private static Enemy _enemy;

    private void Start()
    {
        _fighterAnim = GameObject.FindGameObjectWithTag("Fighter").GetComponent<Animator>();
        _enemyAnim = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();

        _fighter = GameObject.FindGameObjectWithTag("Fighter").GetComponent<Fighter>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    public Animator GetEnemyAnimator() { return _enemyAnim; }

    public Animator GetFighterAnimator() { return _fighterAnim; }

    public bool GetIsFighterParry() { print(fighterParry);  return fighterParry; }

    public bool GetIsEnemyParry() { return enemyParry; }

    public void IdleAnim(float dirX, Animator anim, Rigidbody2D rb)
    {
        // not moving at all => play idle
        if (dirX == 0) {
            anim.SetBool("isWalking", false);
        }

        // not in the air
        if (rb.velocity.y == 0) {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
    }

    public void WalkAnim(float dirX, Rigidbody2D rb, Animator anim)
    {
        // character is walking
        if (Mathf.Abs(dirX) == 5 && rb.velocity.y == 0) {
            anim.SetBool("isWalking", true);
        }
    }

    public void JumpAnim(Rigidbody2D rb, Animator anim, KeyCode key)
    {
        // character is going up
        if (rb.velocity.y > 5) {
            anim.SetBool("isJumping", true);
            //print(rb.velocity.y);
        }

        // character is going down
        if (rb.velocity.y < 0) {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }

    }

    public void CrouchAnim(Animator anim, KeyCode key, Transform player_transform)
    {
        float dirY_current = -2.699f;
        float dirY_crouch = -3.0f;
        // character stays down
        if (Input.GetKey(key)) {
            //print("here???");
            anim.SetBool("isDown", true);
            // change the y position a bit because the animation is not accurate
            player_transform.position = new Vector3(player_transform.position.x, dirY_crouch, player_transform.position.z);
        }
        if (Input.GetKeyUp(key)) {
            anim.SetBool("isDown", false);
            player_transform.position = new Vector3(player_transform.position.x, dirY_current, player_transform.position.z);
        }
    }

    public float ParryAnim(Animator anim, KeyCode key, float moveSpeed, bool isFighter)
    {
        // parry
        if (Input.GetKey(key)) { // if user holds E key
            anim.SetBool("isParry", true);
            moveSpeed = 0f; // can't move while parry
            if (isFighter) {
                //print("Fighter parry.");
                fighterParry = true;
            } else {
                //print("Enemy parry.");
                enemyParry = true;
            }
        }
        if (Input.GetKeyUp(key)) {
            anim.SetBool("isParry", false);
            moveSpeed = 5f;

            fighterParry = false;
            enemyParry = false;
        }

        return moveSpeed;
    }

    public bool FistAnim(Animator anim, bool fistHit, KeyCode key)
    {
        // fist
        if (Input.GetKeyDown(key)) {
            anim.SetTrigger("isFist");
            fistHit = true;
        }

        return fistHit;
    }

    public bool FootHit(Animator anim, bool footHit, KeyCode key)
    {
        // kick
        if (Input.GetKeyDown(key)) {
            anim.SetTrigger("isKick");
            footHit = true;
        }

        return footHit;
    }

    public bool ThrowShuriken(Animator anim, bool shurikenHit, KeyCode key, bool isFighter)
    {
        // throw a shuriken at the opponent
        // the player cannot throw a shuriken if the other one was not destroyed yet
        if (Input.GetKeyDown(key))
        {
            anim.SetTrigger("isShuriken");
            shurikenHit = true;

            if (isFighter) {
                if (_fighter.CanShoot) {
                    _fighter.SpawnShuriken();
                    _fighter.CanShoot = false;
                }
            } else {
                if (_enemy.CanShoot) {
                    _enemy.SpawnShuriken();
                    _enemy.CanShoot = false;
                }
            }
        }

        return shurikenHit;
    }
}
