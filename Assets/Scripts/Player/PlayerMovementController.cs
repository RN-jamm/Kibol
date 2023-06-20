using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private float startingMovementSpeed;
    [SerializeField] private float startingHealth;
    public Animator animator;
    // public UnityEvent OnAttackPerformed;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    // private bool mouseHit = false;
    private bool weaponsNearby = false;
    private PlayerSpriteController spriteController;
    // private float lastBeer = 0.0f;
    // private float beerCooldown = 10f;
    // private int beersDrunk = 0;
    // private int beerLimit = 2;
    private bool isPuking = false;

    public Transform CircleAttack;
    public float RadiusAttack;
    public Transform CircleWeapon;
    public float RadiusWeapon;
    public float currentHealth { get; private set; }
    public GameController gameController;
    // public Text textBeerCounter;
    // public Text textBeerCooldown;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        spriteController = gameObject.GetComponent<PlayerSpriteController>();
        startingMovementSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPuking) {
            movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetMouseButtonDown(0)) {
                StartCoroutine(mouseHitAnim());
            }

            DetectWeaponsNearby();
            PickupWeapon();

            animator.SetFloat("Speed", rb.velocity.magnitude);
        }

        
        // if (lastBeer >= 0) {
        //     lastBeer -= Time.deltaTime;
        //     textBeerCooldown.text = Math.Round(lastBeer, 1).ToString();
        // } else {
        //     beersDrunk = 0;
        //     textBeerCooldown.text = "0.0";
        // }
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDirection * movementSpeed;
        if (!isPuking) {
            followMouse();
        }
    }

    private IEnumerator mouseHitAnim() {
        animator.Play("PlayerAttack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void followMouse() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        
        float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
        
        transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle));
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg + 180;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Vector3 position = CircleAttack == null ? Vector3.zero : CircleAttack.position;
        Gizmos.DrawWireSphere(position, RadiusAttack);

        Gizmos.color = Color.green;
        Vector3 position2 = CircleWeapon == null ? Vector3.zero : CircleWeapon.position;
        Gizmos.DrawWireSphere(position2, RadiusWeapon);
    }

    public void DetectColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleAttack.position, RadiusAttack))
        {
            EnemyController enemy;
            if(enemy = collider.GetComponent<EnemyController>())
            {
                StartCoroutine(enemy.GetHit(spriteController.GetWeaponDamage()));
            }
        }
    }

    public void DetectWeaponsNearby() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleWeapon.position, RadiusWeapon)) {
            WeaponController weapon;
            if (weapon = collider.GetComponent<WeaponController>()) {
                weapon.pickupHighlight();
                weaponsNearby = true; 
            } else { 
                weaponsNearby = false; 
            }
        }
    }

    public void PickupWeapon() {
        if (weaponsNearby) {
            if (Input.GetKeyDown(KeyCode.E)) {
                foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleWeapon.position, RadiusWeapon)) {
                    WeaponController weapon;
                    if (weapon = collider.GetComponent<WeaponController>()) {
                        GetComponent<PlayerSpriteController>().addPickedUpWeapon(weapon.gameObject.name);
                        Destroy(weapon.gameObject);
                        break;
                    }
                }
            }
        }
    }



    public void GetHit() {
        currentHealth = Mathf.Clamp(currentHealth - 1, 0, startingHealth);
        if (currentHealth <= 0)
            gameController.GameOver();
            //Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("beer"))
        {
            // beersDrunk++;
            // lastBeer = beerCooldown;

            Destroy(other.gameObject);
        
            // if (lastBeer > 0 && beersDrunk > beerLimit) {
            if (currentHealth >= startingHealth) {
                StartCoroutine(puke());
                this.GetHit();
            } else {
                currentHealth = Mathf.Clamp(currentHealth + 1, 0, startingHealth);
            }
        
        }
    }

    private IEnumerator puke() {
        animator.Play("PlayerPuke");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public void startPuking() {
        isPuking = true;
        movementSpeed = 0;
    }

    public void stopPuking() {
        isPuking = false;
        movementSpeed = startingMovementSpeed;
    }
}
