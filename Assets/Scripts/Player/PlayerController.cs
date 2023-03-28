using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    public Animator animator;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private bool mouseHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // target = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetMouseButtonDown(0)) {
            // animator.SetBool("isMouseHit", true);
            mouseHit = true;
            StartCoroutine(mouseHitAnim());
            // animator.SetBool("isMouseHit", false);
            // mouseHit = false;
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * movementSpeed;
        mouseHit = false;
        followMouse();
    }

    private IEnumerator mouseHitAnim() {
        animator.Play("PlayerAttack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void followMouse() {

        //Mouse Position in the world. It's important to give it some distance from the camera. 
        //If the screen point is calculated right from the exact position of the camera, then it will
        //just return the exact same position as the camera, which is no good.
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        
        //Angle between mouse and this object
        float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
        
        //Ta daa
        transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle));
    }
    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg + 180;
    }
}
