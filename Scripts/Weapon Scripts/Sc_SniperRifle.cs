using UnityEngine;

public class Sc_SniperRifle : Sc_WeaponHandler {

    [Header( "Refrences" )]
    public GameObject _impactEffect;
    public Transform _lookRoot;
    public ParticleSystem _muzzleFlash;
    private Sc_WeaponSoundManager _weaponAudio;
    private Animator _weaponAnimator;

    [Header( "Weapon Attributes" )]
    public float _reloadTime = 4.1f;
    public float _damage = 100.0f;
    public float _range = 1000.0f;
    public float _impactForce = 5.0f;
    public float _fireRate = 0.5f;
    public int _magazineSize = 7;

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
        WeaponReloadTime = _reloadTime;
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
