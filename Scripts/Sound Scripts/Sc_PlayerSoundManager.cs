using System.Collections.Generic;
using UnityEngine;
using static Sc_Models;

public class Sc_PlayerSoundManager : MonoBehaviour {

    private AudioSource _playerSound;
    [SerializeField] private List<AudioClip> _footstepClips;
    [SerializeField] private List<AudioClip> _jumpSoundClips;
    private CharacterController _characterController;
    public SoundSettings settings;

    // ★彡[ Awake is called when the script instance is being loaded. ]彡★
    void Awake() {
        
        _playerSound = GetComponent<AudioSource> ();
        _characterController = GetComponentInParent<CharacterController> ();
        settings = new SoundSettings();
    } // ★彡[ Awake ]彡★

    // ★彡[ Start is called on the frame when a script is enabled just before any of the Update methods is called the first time. ]彡★    
    void Start() {

        WalkSound();
    } // ★彡[ Start ]彡★

    // ★彡[ Update is called every frame, if the MonoBehaviour is enabled. ]彡★
    void Update() {

        CalculateFootstepsAndPlaySound();
    } // ★彡[ Update ]彡★

    void CalculateFootstepsAndPlaySound() {

        // ★彡[ Cheking if the player on ground ]彡★
        if( !_characterController.isGrounded )
            return;

        // ★彡[ Checking if the player is moving ]彡★
        if( _characterController.velocity.sqrMagnitude > 0 ) {

            // ★彡[ Setting accumulated distance according to the current time spent ]彡★
            settings.accumulatedDistance += Time.deltaTime;

            // ★彡[ Checking if the accumulated time is greater than step distance ]彡★
            if( settings.accumulatedDistance > settings.stepDistance ) {

                // ★彡[ Setting random amount of volume ]彡★
                _playerSound.volume = Random.Range( settings.minVolume, settings.maxVolume );

                // ★彡[ Making the player sound equal to random clip from the sound clips array ]彡★
                _playerSound.clip = _footstepClips[Random.Range( 0, _footstepClips.Count )];

                // ★彡[ Playing sound ]彡★
                _playerSound.Play();

                // ★彡[ Setting accumulated distance back to zero after playing song once to recheck and replay again accoring to the specific time ]彡★
                settings.accumulatedDistance = 0f;
            }

        } else {

            // ★彡[ Setting accumulated distance back to zero after playing song once to recheck and replay again accoring to the specific time ]彡★
            settings.accumulatedDistance = 0f;
        }

    } // ★彡[ Calculate Footsteps And Play Sound ]彡★

    
    public void WalkSound() {

        // ★彡[ Setting volume according to walking volume ]彡★
        settings.minVolume = settings.walkMinVolume;
        settings.maxVolume = settings.walkMaxVolume;
        // ★彡[ Setting step distance according to the walk step distance ]彡★
        settings.stepDistance = settings.walkStepDistance;
    } // ★彡[ Walk sound ]彡★

    public void SprintSound() {
        
        // ★彡[ Setting step distance according to the sprint step distance ]彡★
        settings.stepDistance = settings.sprintStepDistance;
        // ★彡[ Setting volume according to sprint volume ]彡★
        settings.minVolume = settings.sprintVolume;
        settings.maxVolume = settings.sprintVolume;
    } // ★彡[ Sprint sound ]彡★

    public void CrouchSound() {

        // ★彡[ Setting step distance according to the crouch step distance ]彡★
        settings.stepDistance = settings.crouchStepDistance;
        // ★彡[ Setting volume according to crouch volume ]彡★
        settings.minVolume = settings.crouchVolume;
        settings.maxVolume = settings.crouchVolume;
    } // ★彡[ Crouch sound ]彡★

    public void ProneSound() {

        // ★彡[ Setting step distance according to the prone step distance ]彡★
        settings.stepDistance = settings.proneStepDistance;
        // ★彡[ Setting volume according to prone volume ]彡★
        settings.minVolume = settings.proneVolume;
        settings.maxVolume = settings.proneVolume;
    } // ★彡[ Prone sound ]彡★
    
    public void JumpSound() {

        // ★彡[ Playing jump audio when jump key is pressed ]彡★
        _playerSound.PlayOneShot( _jumpSoundClips[1], settings.jumpVolume );

        // ★彡[ Playing jump audio when playing has landed ]彡★
        if( _characterController.isGrounded )
            _playerSound.PlayOneShot( _jumpSoundClips[0], settings.jumpVolume );

    } // ★彡[ Jump sound ]彡★

} // ★彡[ Class ]彡★
