using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackNEW : MonoBehaviour {

	private bool facingRight = true;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private KeyCode shoot;
	//[SerializeField] private Transform firePoint;
	[SerializeField] private float bulletSpeed = 50;
	[SerializeField] private int bulletDamage = 2;
	[SerializeField] private int numberOfBullets = 1;
	private bool canShoot = true;
	[SerializeField] private float timeBetweenShots;
	private float timeAfterShot;
	[SerializeField] private float bulletSpread = 0.01f;
	[SerializeField] private float maxBulletTime = 1f;
	private Transform firePoint;
	private bool attackPlayer;
	private float playerAngle;
	bool rotated = false;

	[SerializeField] private float attackRange = 15;
	private GameObject player;
	private float speed;
	private Rigidbody2D rb;
	[SerializeField] private float minDistance = 15;


void Awake() {
	firePoint = transform.Find ("Fire Point");
	player = GameObject.FindWithTag ("Player");
	rb = GetComponent<Rigidbody2D>(); //Used to give the unit movement

		if (transform.parent.GetComponent<AIPatrol> ().FacingRight) {
			//rotated = true;
		}
}


// Update is called once per frame
void Update () {


		if (!rotated) {
			firePoint.Rotate (new Vector3(0, 180, 0));
			rotated = true;
		}

		//GetComponent<Animator>().SetBool ("Attack", true);
		attackPlayer = false;
		transform.parent.GetComponent<AIPatrol> ().CanMove = true;
		Vector3 tempAngle = Vector3.zero;
		Vector2 direction = player.transform.position - firePoint.transform.position;
		RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction);
		//Debug.DrawRay (firePoint.transform.position, direction, Color.red, 2);
		if (hit.transform == player.transform) {
			attackPlayer = true;
			transform.parent.GetComponent<AIPatrol> ().CanMove = false;
			//transform.rotation = hit.transform        //firePoint.transform.rotation;//hit.transform.rotation 
			tempAngle = Vector3.zero;
			tempAngle.z = player.transform.position.y - firePoint.transform.position.y;
				Vector2 temp = Vector2.zero;

			temp.x = player.transform.position.x - firePoint.transform.position.x;
			temp.y = player.transform.position.y - firePoint.transform.position.y;
			playerAngle = Mathf.Atan2 (temp.y, temp.x) * Mathf.Rad2Deg;

			tempAngle.z = playerAngle;

			Quaternion rotation = Quaternion.LookRotation (hit.transform.position - transform.position, transform.TransformDirection (Vector3.up));
			transform.rotation = new Quaternion (0, 0, rotation.z, rotation.w);
		}


	timeAfterShot += Time.deltaTime;

	if (timeAfterShot > timeBetweenShots) {
		canShoot = true;
			//Debug.Log ("Enemy can fire");
	}

	if (attackPlayer) {
		if (canShoot) {
			timeAfterShot = 0;
			Shoot ();
			canShoot = false;
		}
	}
		//transform.rotation = Vector3.zero;
}

void Shoot() {
		transform.parent.GetComponent<Animator> ().Play ("Enemy Blink");
	//Debug.Log ("Attacking player");

	float tempSpread = bulletSpread;

		Quaternion modifiedRotation = firePoint.rotation;

		modifiedRotation = firePoint.rotation;
		Debug.Log (playerAngle);
		Debug.Log(modifiedRotation);
		//Debug.Log ("Player angle = " + playerAngle);


		float rotationModifier = tempSpread; /// numberOfBullets;
	//modifiedRotation.z -= rotationModifier * (numberOfBullets / 2);
	
	for (int i = 0; i < numberOfBullets; i++) {


		//Debug.Log (modifiedRotation.z);
			var bullet = (GameObject)Instantiate (
				            bulletPrefab,
				            firePoint.position,
				            modifiedRotation);

		var bulletBehavior = bullet.GetComponent<BulletBehavior> ();

		if (bulletBehavior) {
			bulletBehavior.FacingRight = transform.parent.GetComponent<AIPatrol> ().FacingRight;
			bulletBehavior.Speed = bulletSpeed;
			bulletBehavior.Damage = bulletDamage;
			bulletBehavior.BulletTime = maxBulletTime;
		} else {
			var rocketBehavior = bullet.GetComponent<RocketBehavior> ();

			rocketBehavior.FacingRight = transform.parent.GetComponent<AIPatrol> ().FacingRight;
			rocketBehavior.Speed = bulletSpeed;
			rocketBehavior.Damage = bulletDamage;
		}

	
		Destroy(bullet, maxBulletTime);

		tempSpread = tempSpread / 2;
		modifiedRotation.z += rotationModifier;
		} 
	}

}