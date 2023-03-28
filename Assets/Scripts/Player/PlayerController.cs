using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 6f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;

    // private Vector3 mouse_pos;
    // private Transform target;
    // private Vector3 object_pos;
    // private float angle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // target = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * movementSpeed;
        followMouse();
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

        // mouse_pos = Input.mousePosition;
        // mouse_pos.z = -20;
        // object_pos = Camera.main.WorldToScreenPoint(target.position);
        // mouse_pos.x = mouse_pos.x - object_pos.x;
        // mouse_pos.y = mouse_pos.y - object_pos.y;
        // angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(new Vector3(0f,0f,angle));
    }
    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg +90;
    }
}
