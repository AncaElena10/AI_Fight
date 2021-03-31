using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
	Rigidbody2D rb;
	Animator anim;
	float dirX, moveSpeed = 5f;
	static bool isHurting;
	static bool isDead;
	static bool facingRight = true;
	Vector3 localScale;

    public static bool fistHit = false;
	public static bool footHit = false;
	public static bool shurikenHit = false;

	float damage = 10f;
	float parryDamage = 3f;

	static bool canShoot = true;

	AnimationsManager _myAnim = new AnimationsManager();
	HealthManager _hm = new HealthManager();
	CollisionsManager _cm = new CollisionsManager();
	DeathManager _dm = new DeathManager();

	public GameObject parentObject;

	[SerializeField]
	private Rigidbody2D fighterShurikenPrefab;
	[SerializeField]
	private Transform spawner;

	private float shurikenSpeed = 500f;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		localScale = transform.localScale;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Jump") && !isDead && rb.velocity.y == 0) {
			rb.AddForce(Vector2.up * 600f);
		}

		SetAnimationState();

		if (!isDead) {
			dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
		} else {
			transform.position = new Vector3(transform.position.x, -3.5f, transform.position.z);
		}
	}

	void FixedUpdate()
	{
		if (!isHurting) {
			rb.velocity = new Vector2(dirX, rb.velocity.y);
		}
	}

	void LateUpdate()
	{
		CheckWhereToFace();
	}

	void SetAnimationState()
	{
		_myAnim.IdleAnim(dirX, anim, rb);

		_myAnim.WalkAnim(dirX, rb, anim);

		_myAnim.CrouchAnim(anim, KeyCode.S, transform);

		_myAnim.JumpAnim(rb, anim, KeyCode.Space);

        moveSpeed = _myAnim.ParryAnim(anim, KeyCode.E, moveSpeed, true);

		FistHit = (_myAnim.FistAnim(anim, fistHit, KeyCode.Mouse0)); // left click

		FootHit = (_myAnim.FootHit(anim, footHit, KeyCode.Mouse1)); // right click

		ShurikenHit = (_myAnim.ThrowShuriken(anim, shurikenHit, KeyCode.Mouse2, true)); // wheel click
	}

	public void SpawnShuriken()
	{
		print("Fighter shuriken instantiated.");
		float shurikenX_offset = 0.95f;
		float shurikenY_offset = 0.45f;
		if (facingRight) {
			var spawnedShuriken = Instantiate(fighterShurikenPrefab, new Vector3(parentObject.transform.position.x + shurikenX_offset, parentObject.transform.position.y + shurikenY_offset, parentObject.transform.position.z), Quaternion.identity);
			spawnedShuriken.AddForce(spawner.up * shurikenSpeed);
		} else {
			var spawnedShuriken = Instantiate(fighterShurikenPrefab, new Vector3(parentObject.transform.position.x - shurikenX_offset, parentObject.transform.position.y + shurikenY_offset, parentObject.transform.position.z), Quaternion.identity);
			spawnedShuriken.AddForce(-spawner.up * shurikenSpeed);
		}
	}

	void CheckWhereToFace()
	{
		if (dirX > 0) {
			facingRight = true;
		} else {
			if (dirX < 0) {
				facingRight = false;
			}
		}

		if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0))) {
			localScale.x *= -1;
		}

		transform.localScale = localScale;
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Equals("Enemy")) {
			if (footHit) {
				// if the enemy is in parry
				if (_myAnim.GetIsEnemyParry()) {
					print("Enemy in parry mode. Dealing less damage.");
					_hm.SetEnemyHealth(parryDamage);
				} else {
					print("Enemy hit by foot!");
					PlayHurtAnim_Enemy(damage);
					footHit = false;
				}
            }
            if (fistHit) {
				if (_myAnim.GetIsEnemyParry()) {
					print("Enemy in parry mode. Dealing less damage.");
					_hm.SetEnemyHealth(parryDamage);
				} else {
					print("Enemy hit by fist!");
					PlayHurtAnim_Enemy(damage);
					fistHit = false;
				}
            }
		}

		// Enemy dead
		if (collision.gameObject.name.Equals("Enemy") && _hm.GetEnemyHealth() <= 0f) {
			print("Enemy dead!");
			_dm.setIsDeadParam(true);
			_myAnim.GetEnemyAnimator().SetTrigger("isDead");
		}
	}

	public void PlayHurtAnim_Enemy(float dmg)
    {
		_hm.SetEnemyHealth(dmg);
		// play hurting animation from Enemy
		_myAnim.GetEnemyAnimator().SetTrigger("isHurt");
		StartCoroutine(_cm.Hurt(true));
	}

	public bool FootHit
	{
		get { return footHit; }
		set { footHit = value; }
	}

	public bool FistHit
	{
		get { return fistHit; }
		set { fistHit = value; }
	}

	public bool ShurikenHit
	{
		get { return shurikenHit; }
		set { shurikenHit = value; }
	}

	public bool CanShoot
	{
		get { return canShoot; }
		set { canShoot = value; }
	}

	public bool FacingRight
	{
		get { return facingRight; }
		set { facingRight = value; }
	}

	public bool IsHurting
	{
		get { return isHurting; }
		set { isHurting = value; }
	}

	public bool IsDead
	{
		get { return isDead; }
		set { isDead = value; }
	}

	/*
	public bool FootHit { get; set;  }

	public bool FistHit { get; set; }

	public bool ShurikenHit { get; set; }

	public bool CanShoot { get; set; }

	public bool FacingRight { get; set; }

	public bool IsHurting { get; set; }
	public bool IsDead { get; set; }*/
}
