using System.Collections;
using UnityEngine;

public class Sc_WeaponHandler : MonoBehaviour {

    #region ★彡[ Getters and Setters ]彡★
    private float Weapon_Damage = 10f;
    public float WeaponDamage {

        get  { return Weapon_Damage; }
        set { Weapon_Damage = value; }
    }

    private float Weapon_Range = 100f;
    public float WeaponRange {

        get  { return Weapon_Range; }
        set { Weapon_Range = value; }
    }

    private float Weapon_Impact_Force = 4f;
    public float WeaponImpactForce {

        get  { return Weapon_Impact_Force; }
        set { Weapon_Impact_Force = value; }
    }

    private float Weapon_Fire_Rate = 0.7f;
    public float WeaponFireRate {

        get  { return Weapon_Fire_Rate; }
        set { Weapon_Fire_Rate = value; }
    }

    private float Weapon_Reload_Time = 3.0f;
    public float WeaponReloadTime {

        get { return Weapon_Reload_Time; }
        set { Weapon_Reload_Time = value; }
    }

    private int Weapon_Magazine_Size = 10;
    public int WeaponMagazineSize {

        get { return Weapon_Magazine_Size; }
        set { Weapon_Magazine_Size = value; }
    }

    private int Weapon_Bullets;
    public int WeaponBullets {

        get { return Weapon_Bullets; }
        set { Weapon_Bullets = value; }
    }

    private Transform Look_Root_Object;
    public Transform LookRootObject {

        get { return Look_Root_Object; }
        set { Look_Root_Object = value; }
    }

    private GameObject Impact_Effect_Object;
    public GameObject ImpactEffectObject {

        get { return Impact_Effect_Object; }
        set { Impact_Effect_Object = value; }
    }

    private ParticleSystem Muzzle_Flash_Particle;
    public ParticleSystem MuzzleFlashParticle {

        get { return Muzzle_Flash_Particle; }
        set { Muzzle_Flash_Particle = value; }
    }

    private Animator Weapon_Animator_Component;
    public Animator WeaponAnimatorComponent {

        get { return Weapon_Animator_Component; }
        set { Weapon_Animator_Component = value; }
    }

    private Sc_WeaponSoundManager Weapon_Audio_Script;
    public Sc_WeaponSoundManager WeaponAudioScript {
        
        get { return Weapon_Audio_Script; }
        set { Weapon_Audio_Script = value; }
    }

    private Sc_Target Target_Script;
    public Sc_Target TargetScript {

        get { return Target_Script; }
        set { Target_Script = value; }
    }

    #endregion ★彡[ Getters and Setters ]彡★

    private float nextFireTime = 0f;

    public void Fire() {

        if ( Time.time >= nextFireTime ) {

            if( WeaponBullets == 0 ) {

                Debug.Log( "Magzine is empty" );
                return;
            }

            WeaponBullets--;

            nextFireTime = Time.time + 1 / WeaponFireRate;

            Shoot();
        }
    } // ★彡[ Fire ]彡★

    public void Shoot() {


        RaycastHit _hitInfo;

        if ( Physics.Raycast(LookRootObject.transform.position, LookRootObject.transform.forward, out _hitInfo, WeaponRange) ) {

            Debug.Log( _hitInfo.transform.name );

            TargetScript =  _hitInfo.transform.GetComponent<Sc_Target> ();

            if( TargetScript != null ) {

                TargetScript.TakeDamage( WeaponDamage );
            }

            if( _hitInfo.rigidbody != null ) {

                _hitInfo.rigidbody.AddForce( -_hitInfo.normal * WeaponImpactForce, ForceMode.Impulse );
            }

            PlayFireAnimationAndSound();
            MuzzleFlashParticle.Play();

            GameObject impactGameObject = Instantiate( ImpactEffectObject, _hitInfo.point, Quaternion.LookRotation(_hitInfo.normal), _hitInfo.transform );

            Destroy( impactGameObject, 2f );

        }
    } // ★彡[ Shoot ]彡★

    public IEnumerator ReloadGun() {

        PlayReloadAnimationAndSound();
        yield return new WaitForSeconds( WeaponReloadTime );
        WeaponBullets = WeaponMagazineSize;

    } // ★彡[ Reload ]彡★

    public void ScopeIn() {

        WeaponAnimatorComponent.SetBool( AnimationTags.AimBool, true );
    } // ★彡[ Scope In ]彡★

    public void ScopeOut() {

        WeaponAnimatorComponent.SetBool( AnimationTags.AimBool, false );
    } // ★彡[ Scioe Out ]彡★
    
    public void PlayFireAnimationAndSound() {

        WeaponAnimatorComponent.SetTrigger( AnimationTags.fireTrig );
        WeaponAudioScript.FireSound();
    } // ★彡[ Play Fire Animation And Sound ]彡★

    public void PlayReloadAnimationAndSound() {

        WeaponAnimatorComponent.SetTrigger( AnimationTags.reloadTrig );
        WeaponAudioScript.ReloadSound();
    } // ★彡[ Play Reload Animation And Sound ]彡★
}
