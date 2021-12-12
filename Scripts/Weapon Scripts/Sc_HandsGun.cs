using UnityEngine;

public class Sc_HandsGun : Sc_WeaponHandler {

    [Header( "Refrences" )]
    public GameObject _impactEffect;
    public Transform _lookRoot;
    public ParticleSystem _muzzleFlash;
    private Sc_WeaponSoundManager _weaponAudio;
    private Animator _weaponAnimator;

    [Header( "Weapon Attributes" )]
    public float _weaponReloadTime = 4.0f;
    public float _damage = 15.0f;
    public float _range = 350.0f;
    public float _impactForce = 2.0f;
    public float _fireRate = 5f;
    public int _magazineSize = 10;

    // ★彡[ Awake is called when the script instance is being loaded. ]彡★
    void Start() {

        _weaponAnimator = GetComponent<Animator> ();
        _weaponAudio = GetComponent<Sc_WeaponSoundManager> ();

        LookRootObject = _lookRoot;
        ImpactEffectObject = _impactEffect;
        MuzzleFlashParticle = _muzzleFlash;
        WeaponAnimatorComponent = _weaponAnimator;
        WeaponAudioScript = _weaponAudio;

        WeaponDamage = _damage;
        WeaponRange = _range;
        WeaponImpactForce = _impactForce;
        WeaponFireRate = _fireRate;
        WeaponReloadTime = _weaponReloadTime;
        WeaponMagazineSize = _magazineSize;
        WeaponBullets = _magazineSize;
    }

    private void Update() {

        if( Input.GetKeyDown(KeyCode.R) ) {
            StartCoroutine( ReloadGun() );
        }

        if( Input.GetKeyDown(KeyCode.Mouse0) ) {
            Fire();
        }

        if( Input.GetKey(KeyCode.Mouse1) )
            ScopeIn();

        else
            ScopeOut();

    } // ★彡[ Update ]彡★

}
