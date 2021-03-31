using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
	private Rigidbody2D enemyShurikenPrefab;
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
		if (Input.GetKeyDown(KeyCode.RightControl) && !isDead && rb.velocity.y == 0) {
			rb.AddForce(Vector2.up * 600f);
		}

		SetAnimationState();

		if (!isDead) {
			dirX = Input.GetAxisRaw("Horizontal2") * moveSpeed;
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

		_myAnim.CrouchAnim(anim, KeyCode.DownArrow, transform);

		_myAnim.JumpAnim(rb, anim, KeyCode.RightControl);

        moveSpeed = _myAnim.ParryAnim(anim, KeyCode.Keypad0, moveSpeed, false);

        FistHit = (_myAnim.FistAnim(anim, fistHit, KeyCode.Keypad1));

        FootHit = (_myAnim.FootHit(anim, footHit, KeyCode.Keypad2));

		ShurikenHit = (_myAnim.ThrowShuriken(anim,shurikenHit, KeyCode.Keypad3, false));
	}

	public void SpawnShuriken()
	{
		print("Fighter shuriken instantiated.");
		float shurikenX_offset = 0.95f;
		float shurikenY_offset = 0.45f;
		if (facingRight) {
			var spawnedShuriken = Instantiate(enemyShurikenPrefab, new Vector3(parentObject.transform.position.x - shurikenX_offset, parentObject.transform.position.y + shurikenY_offset, parentObject.transform.position.z), Quaternion.identity);
			spawnedShuriken.AddForce(-spawner.up * shurikenSpeed);
		} else {
			var spawnedShuriken = Instantiate(enemyShurikenPrefab, new Vector3(parentObject.transform.position.x + shurikenX_offset, parentObject.transform.position.y + shurikenY_offset, parentObject.transform.position.z), Quaternion.identity);
			spawnedShuriken.AddForce(spawner.up * shurikenSpeed);
		}
	}

	void CheckWhereToFace()
	{
		if (dirX > 0) {
			facingRight = false;
		} else {
			if (dirX < 0) {
				facingRight = true;
			}
		}

		if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0))) {
			localScale.x *= -1;
		}

		transform.localScale = localScale;
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Equals("Fighter")) {
			if (footHit) {
				if (_myAnim.GetIsFighterParry()) {
					print("Fighter in parry mode. Dealing less damage.");
					_hm.SetFighterHealth(parryDamage);
				} else {
					print("Fighter hit by foot!");
					PlayHurtAnim_Fighter(damage);
					footHit = false;
				}
			}
			if (fistHit) {
				if (_myAnim.GetIsFighterParry()) {
					print("Fighter in parry mode. Dealing less damage.");
					_hm.SetFighterHealth(parryDamage);
				} else {
					print("Fighter hit by fist!");
					PlayHurtAnim_Fighter(damage);
					fistHit = false;
				}
			}
		}

		// Fighter dead
		if (collision.gameObject.name.Equals("Fighter") && _hm.GetFighterHealth() <= 0f) {
			print("Fighter dead!");
			_dm.setIsDeadParam(false);
			_myAnim.GetFighterAnimator().SetTrigger("isDead");
		}
	}

	public void PlayHurtAnim_Fighter(float dmg)
    {
		_hm.SetFighterHealth(dmg);
		// play hurting animation from Fighter
		_myAnim.GetFighterAnimator().SetTrigger("isHurt");
		StartCoroutine(_cm.Hurt(false));
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
}
