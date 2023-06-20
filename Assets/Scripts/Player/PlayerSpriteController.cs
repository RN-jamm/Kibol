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
            {"Axe", "PlayerAxe"},
            {"Pan", "PlayerPan"},
        };

    private RuntimeAnimatorController currentWeaponController;
    private string currentWeapon = "Unarmed";
    
    private Dictionary<string, int> weaponsDamage = new Dictionary<string, int> 
        {
            {"Unarmed", 1},
            {"Bat", 2},
            {"Axe", 4},
            {"Pan", 10},
        };

    private List<string> obtainedWeapons = new List<string>{
        "Unarmed",
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentWeaponController = Resources.Load<RuntimeAnimatorController>("unarmed");
    }

    // Update is called once per frame
    void Update()
    {
        for ( int i = 0; i < obtainedWeapons.Count(); ++i ) {
            if ( Input.GetKeyDown( "" + (i+1)) ) {
                currentWeaponController = Resources.Load<RuntimeAnimatorController>(weaponsAnimations[obtainedWeapons[i]]);
                currentWeapon = obtainedWeapons[i];
            }
        }
        if (currentWeaponController != null)
        {
            animator.runtimeAnimatorController = currentWeaponController;
        }
    }

    public void addPickedUpWeapon(string weaponName) {
        if (!obtainedWeapons.Contains(weaponName)) {
            obtainedWeapons.Add(weaponName);
        }
    }

    public int GetWeaponDamage() {
        return weaponsDamage[currentWeapon];
    }
}
