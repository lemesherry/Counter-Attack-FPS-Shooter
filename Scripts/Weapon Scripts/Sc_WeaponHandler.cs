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

            if( Weapon_Bullets == 0 ) {

                Debug.Log( "Magzine is empty" );
                return;
            }

            Weapon_Bullets--;

            PlayFireAnimationAndSound();
            nextFireTime = Time.time + 1 / Weapon_Fire_Rate;

            Shoot();
        }
    } // ★彡[ Fire ]彡★

    public void Shoot() {

        Muzzle_Flash_Particle.Play();

        RaycastHit _hitInfo;

        if ( Physics.Raycast(Look_Root_Object.transform.position, Look_Root_Object.transform.forward, out _hitInfo, Weapon_Range) ) {

            Debug.Log( _hitInfo.transform.name );

            Target_Script =  _hitInfo.transform.GetComponent<Sc_Target> ();

            if( Target_Script != null ) {

                Target_Script.TakeDamage( Weapon_Damage );
            }

            if( _hitInfo.rigidbody != null ) {

                _hitInfo.rigidbody.AddForce( -_hitInfo.normal * Weapon_Impact_Force, ForceMode.Impulse );
            }

            GameObject impactGameObject = Instantiate( Impact_Effect_Object, _hitInfo.point, Quaternion.LookRotation(_hitInfo.normal), _hitInfo.transform );

            Destroy( impactGameObject, 1f );

        }
    } // ★彡[ Shoot ]彡★

    public IEnumerator ReloadGun() {

        PlayReloadAnimationAndSound();
        yield return new WaitForSeconds( Weapon_Reload_Time );
        Weapon_Bullets = Weapon_Magazine_Size;

    } // ★彡[ Reload ]彡★

    public void ScopeIn() {

        Weapon_Animator_Component.SetBool( AnimationTags.AimBool, true );
    } // ★彡[ Scope In ]彡★

    public void ScopeOut() {

        Weapon_Animator_Component.SetBool( AnimationTags.AimBool, false );
    } // ★彡[ Scioe Out ]彡★
    
    public void PlayFireAnimationAndSound() {

        Weapon_Animator_Component.SetTrigger( AnimationTags.fireTrig );
        Weapon_Audio_Script.FireSound();
    } // ★彡[ Play Fire Animation And Sound ]彡★

    public void PlayReloadAnimationAndSound() {

        Weapon_Animator_Component.SetTrigger( AnimationTags.reloadTrig );
        Weapon_Audio_Script.ReloadSound();
    } // ★彡[ Play Reload Animation And Sound ]彡★
}
