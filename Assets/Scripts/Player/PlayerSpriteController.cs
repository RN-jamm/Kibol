using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    private Animator animator;
    private Dictionary<string, string> weaponsAnimations = new Dictionary<string, string>
        {
            {"unarmed", "PlayerUnarmed"},
            {"bat", "PlayerBat"},
        };

    private RuntimeAnimatorController currentWeaponController;

    private List<string> obtainedWeapons = new List<string>{
        "unarmed",
        "bat",
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentWeaponController = Resources.Load<RuntimeAnimatorController>("unarmed");
        // animator.runtimeAnimatorController = currentWeaponController;
    }

    // Update is called once per frame
    void Update()
    {
        for ( int i = 0; i < 2; ++i ) {
            if ( Input.GetKeyDown( "" + (i+1)) ) {
                Debug.Log(i);
                Debug.Log(currentWeaponController);
                // Debug.Log(animator.runtimeAnimatorController);
                currentWeaponController = Resources.Load<RuntimeAnimatorController>(weaponsAnimations[obtainedWeapons[i]]);
                Debug.Log(currentWeaponController);
                
                // animator.runtimeAnimatorController = currentWeaponController;
                // Debug.Log(animator.runtimeAnimatorController);
            }
        }
        if (currentWeaponController != null)
        {
            animator.runtimeAnimatorController = currentWeaponController;
        }
        // animator.runtimeAnimatorController = currentWeaponController;
    }
}
