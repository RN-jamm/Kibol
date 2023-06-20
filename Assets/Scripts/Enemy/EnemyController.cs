using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public float movementSpeed;
    public float distanceBetween;
    private NavMeshAgent agent;
    private Vector3 target;
    private Rigidbody2D rb;
    private float distance;
    private float cooldown;
    private float lastHit;
    public bool isDead;
    public float health = 8 ;
    public float RadiusAttack;
    public Transform CircleAttack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        cooldown = 1.5f;
        lastHit = 2f;
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (distance < distanceBetween) {
                agent.SetDestination(player.transform.position);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            } else {
                rb.velocity = new Vector2(0,0);
            }
            animator.SetFloat("Speed", agent.velocity.magnitude/agent.speed);

            lastHit += Time.deltaTime;
            if (distance < 1.5 && lastHit > cooldown) {
                StartCoroutine(mouseHitAnim());
                lastHit = 0;
            }
        }
    }

    private IEnumerator mouseHitAnim() {
        animator.Play("EnemyAttackBat");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Vector3 position = CircleAttack == null ? Vector3.zero : CircleAttack.position;
        Gizmos.DrawWireSphere(position, RadiusAttack);
    }

    public void DetectColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleAttack.position, RadiusAttack))
        {
            PlayerMovementController player;
            if(player = collider.GetComponent<PlayerMovementController>())
            {
                player.GetHit();
            }
        }
    }

    public IEnumerator GetHit(int weaponDamage) {
        if (health>weaponDamage){
            health -= weaponDamage;
        }else{
            agent.SetDestination(transform.position);
            isDead = true;
            transform.parent.GetComponent<AllEnemiesController>().KillChildren(gameObject);
            animator.SetBool("isDead", true);
            animator.Play("EnemyDeadFromBat");
            transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 180);
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
}
