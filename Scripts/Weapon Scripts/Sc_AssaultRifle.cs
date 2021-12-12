using UnityEngine;

public class Sc_AssaultRifle : Sc_WeaponHandler {

    [Header( "Refrences" )]
    public GameObject _impactEffect;
    public Transform _lookRoot;
    public ParticleSystem _muzzleFlash;
    private Sc_WeaponSoundManager _weaponAudio;
    private Animator _weaponAnimator;

    [Header( "Weapon Attributes" )]
    public float _reloadTime = 2.6f;
    public float _Damage = 30.0f;
    public float _range = 500.0f;
    public float _impactForce = 3.0f;
    public float _fireRate = 15f;
    public int _magazineSize = 30;

    // ★彡[ Awake is called when the script instance is being loaded. ]彡★
    void Start() {

        _weaponAnimator = GetComponent<Animator> ();
        _weaponAudio = GetComponent<Sc_WeaponSoundManager> ();

        LookRootObject = _lookRoot;
        ImpactEffectObject = _impactEffect;
        MuzzleFlashParticle = _muzzleFlash;
        WeaponAnimatorComponent = _weaponAnimator;
        WeaponAudioScript = _weaponAudio;

        WeaponDamage = _Damage;
        WeaponRange = _range;
        WeaponImpactForce = _impactForce;
        WeaponFireRate = _fireRate;
        WeaponReloadTime = _reloadTime;
        WeaponMagazineSize = _magazineSize;
        WeaponBullets = _magazineSize;
    }

    private void Update() {

        if( Input.GetKeyDown(KeyCode.R) ) {
            StartCoroutine( ReloadGun() );
        }

        if( Input.GetKey(KeyCode.Mouse0) ) {
            Fire();
        }

        if( Input.GetKey(KeyCode.Mouse1) )
            ScopeIn();

        else
            ScopeOut();

    } // ★彡[ Update ]彡★
    
} // ★彡[ class ]彡★
