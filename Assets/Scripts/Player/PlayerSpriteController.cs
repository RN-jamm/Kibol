using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    private Animator animator;
    private Dictionary<string, string> weaponsAnimations = new Dictionary<string, string>
        {
            {"Unarmed", "PlayerUnarmed"},
            {"Bat", "PlayerBat"},
        };

    private RuntimeAnimatorController currentWeaponController;

    private List<string> obtainedWeapons = new List<string>{
        "Unarmed",
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
        for ( int i = 0; i < obtainedWeapons.Count(); ++i ) {
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

    public void addPickedUpWeapon(string weaponName) {
        if (!obtainedWeapons.Contains(weaponName)) {
            obtainedWeapons.Add(weaponName);
        }
    }
}
