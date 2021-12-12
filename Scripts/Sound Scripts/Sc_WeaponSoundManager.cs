using System.Collections.Generic;
using UnityEngine;

public class Sc_WeaponSoundManager : MonoBehaviour {

    private AudioSource _weaponSound;
    [SerializeField] private List<AudioClip> _fireSoundClips;
    [SerializeField] private List<AudioClip> _reloadSoundClips;

    // ★彡[ Awake is called when the script instance is being loaded. ]彡★
    void Start() {
        
        _weaponSound = GetComponent<AudioSource> ();
    } // ★彡[ Awake ]彡★

    public void FireSound() {

        _weaponSound.PlayOneShot( _fireSoundClips[0], 0.8f );
    } // ★彡[ Fire Sound ]彡★
    
    public void ReloadSound() {
        
        _weaponSound.PlayOneShot( _reloadSoundClips[0], 0.8f );
    } // ★彡[ Reload Sound ]彡★

    

} // ★彡[ Class ]彡★
