using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Sprite[] spriteArray;
    public Transform CirclePlayerDetection;
    public float RadiusPlayerDetection;
    private bool highlighted = false;
    private bool currentHighlight = false;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        DetectPlayerNearby();

        if (!highlighted && currentHighlight) {
            spriteRenderer.sprite = spriteArray[0];
            currentHighlight = false;
        } else if (highlighted && !currentHighlight) {
            spriteRenderer.sprite = spriteArray[1];
            currentHighlight = true;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Vector3 position = CirclePlayerDetection == null ? Vector3.zero : CirclePlayerDetection.position;
        Gizmos.DrawWireSphere(position, RadiusPlayerDetection);
    }

    public void DetectPlayerNearby() {
        if (highlighted) {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(CirclePlayerDetection.position, RadiusPlayerDetection)) {
                PlayerMovementController player;
                if (player = collider.GetComponent<PlayerMovementController>()) {
                    float distance = Vector3.Distance (transform.position, player.transform.position);
                    if (distance > player.RadiusWeapon) {
                        highlighted = false;
                    }
                }

            }
        }
    }

    public void pickupHighlight() {
        highlighted = true;
    }
}
