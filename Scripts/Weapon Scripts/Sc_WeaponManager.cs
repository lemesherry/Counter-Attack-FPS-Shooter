using System.Collections.Generic;
using UnityEngine;

public class Sc_WeaponManager : MonoBehaviour {
    
    [SerializeField] private List<Sc_WeaponHandler> weapons;

    private Sc_PlayerController player;
    private Animator currentWeaponAnimator;
    private int currentSelectedWeapon;

    // ★彡[ Start is called on the frame when a script is enabled just before any of the Update methods is called the first time. ]彡★
    void Start() {
        
        currentSelectedWeapon = 0;
        weapons[currentSelectedWeapon].gameObject.SetActive( true );
        player = GetComponent<Sc_PlayerController> ();
        currentWeaponAnimator = weapons[0].GetComponent<Animator>();
    }

    // ★彡[ Update is called every frame, if the MonoBehaviour is enabled. ]彡★
    void Update() {
        

        if( Input.GetKeyDown(KeyCode.Alpha1) ) {

            SelectNewWeapon( 0 );
            currentWeaponAnimator = weapons[0].GetComponent<Animator>();
        }

        if( Input.GetKeyDown(KeyCode.Alpha2) ) {

            SelectNewWeapon( 1 );
            currentWeaponAnimator = weapons[1].GetComponent<Animator>();
        }

        if( Input.GetKeyDown(KeyCode.Alpha3) ) {

            SelectNewWeapon( 2 );
            currentWeaponAnimator = weapons[2].GetComponent<Animator>();
        }

        if( Input.GetKeyDown(KeyCode.Alpha4) ) {

            SelectNewWeapon( 3 );
            currentWeaponAnimator = weapons[3].GetComponent<Animator>();
        }

        var movementAnimationSpeed = ( player._verticalSpeed == 0 ? player._horizontalSpeed : player._verticalSpeed ) / 2;

        currentWeaponAnimator.SetFloat( AnimationTags.speedF, movementAnimationSpeed, 0.01f, Time.deltaTime );

    }

    void SelectNewWeapon( int WeaponIndex ) {

        if( currentSelectedWeapon == WeaponIndex )
            return;

        weapons[currentSelectedWeapon].gameObject.SetActive( false );

        weapons[WeaponIndex].gameObject.SetActive( true );

        currentSelectedWeapon = WeaponIndex;
    }

    public Sc_WeaponHandler GetCurrentWeaponInfo() {

        return weapons[currentSelectedWeapon];
    }
}
