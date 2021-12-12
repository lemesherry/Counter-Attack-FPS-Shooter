using UnityEngine;
using static Sc_Models;

public class Sc_PlayerController : MonoBehaviour {
    
    #region ★彡[ Variables ]彡★

    [Header( "Refrences" )]
    public Transform lookRoot;
    public Transform feetObject;
    // public Animator playerAnimator;

    [Header( "Settings" )]
    public PlayerModelSettings playerSettings;
    public LayerMask playerMask;
    private CharacterController characterController;
    private Sc_PlayerSoundManager playerAudio;
    private PlayerAttributesSettings settings;

    // ★彡[ Stance ]彡★
    private PlayerStance playerStance;

    // ★彡[ Inputs ]彡★
    [HideInInspector]
    public Vector2 _movementInput;
    
    [HideInInspector]
    public float _verticalSpeed;
    
    [HideInInspector]
    public float _horizontalSpeed;

    #endregion

    // ★彡[ Awake is called when the script instance is being loaded. ]彡★
    private void Awake() {

        settings = new PlayerAttributesSettings();

        // ★彡[ Getting the lookRoot eularAngle rotation ]彡★
        settings.newCameraRotation = lookRoot.localRotation.eulerAngles;
        settings.newPlayerRotation = transform.localRotation.eulerAngles;

        // ★彡[ Getting the Character Controller component of the gameObject ]彡★
        characterController = GetComponent<CharacterController> ();

        // ★彡[ Getting the Player Sound Manager component in the children objects ]彡★
        playerAudio = GetComponentInChildren<Sc_PlayerSoundManager> ();

        // ★彡[ Setting the defualt camera height to lookRoot GameObject's poisition in Y axis ]彡★
        settings.cameraHeight = lookRoot.localPosition.y;
        // ★彡[ Setting initial jump height to the defined height in the settings ]彡★
        settings.initialJumpHeight = playerSettings.jumpHeight;

    } // ★彡[ Awake ]彡★

    // ★彡[ Update is called every frame, if the MonoBehaviour is enabled. ]彡★
    private void Update() {

        GetInputs();
        CalculateMovement();
        CalculateJump();
        CalculateStance();

    } // ★彡[ Update ]彡★

    #region ★彡[ Local Functions ]彡★

    private void GetInputs() {

        // ★彡[ Getting Inputs in X and Y axis ]彡★
        _movementInput = new Vector2( Input.GetAxis(Axis.horizontal), Input.GetAxis(Axis.vertical) );

        // ★彡[ Checking if space key is pressed ]彡★
        if( Input.GetKeyDown(KeyCode.Space) ) {

            Jump();
            playerAudio.JumpSound();
        } // ★彡[ Jump ]彡★

        // ★彡[ Checking if left control key is pressed ]彡★
        if( Input.GetKeyDown(KeyCode.LeftControl) ) {

            Crouch();
        } // ★彡[ Crouch ]彡★

        // ★彡[ Checking if Z key is pressed ]彡★
        if( Input.GetKeyDown(KeyCode.Z) ) {

            Prone();
        } // ★彡[ Prone ]彡★

        // ★彡[ Checking if Left shift key is pressed ]彡★
        if( Input.GetKeyDown(KeyCode.LeftShift) ) {

            ToggleSprint();

        } // ★彡[ Sprint ]彡★
        else if ( Input.GetKeyUp(KeyCode.LeftShift) ) {

            StopSprint();
        } // ★彡[ Stop sprinting ]彡★

    } // ★彡[ Get Input ]彡★

    private void CalculateMovement() {

        // ★彡[ Check if the movement input in y axis is lower than specific value to make isSprinting false to make smooth transition between walking and sprinting ]彡★
        if( _movementInput.y <= 0.2f ){

            settings.isSprinting = false;
        }

        // ★彡[ Getting the input in y and x axis to make player move or strafe ]彡★
        _verticalSpeed = playerSettings.walkingForwardSpeed * _movementInput.y * Time.deltaTime;
        _horizontalSpeed = playerSettings.walkingStrafeSpeed * _movementInput.x * Time.deltaTime;

        // ★彡[ Check if isSprinting to make player run according to the specified running speeds ]彡★
        if( settings.isSprinting ) {

            _verticalSpeed = playerSettings.runningForwardSpeed* _movementInput.y * Time.deltaTime;
            _horizontalSpeed = playerSettings.runningStrafeSpeed * _movementInput.x * Time.deltaTime;

        } else {

            // ★彡[ making jump height same as initial to stop making player jump high when its not sprinting anymore ]彡★
            playerSettings.jumpHeight = settings.initialJumpHeight;
        }

        if( !characterController.isGrounded ) {

            // ★彡[ Changing the movement speed while in the air ]彡★
            playerSettings.speedChanger = playerSettings.fallingSpeed;
        }
        else if ( playerStance == PlayerStance.Crouch ) {

            // ★彡[ Changing the movement speed while crouching ]彡★
            playerSettings.speedChanger = playerSettings.crouchSpeed;
        }
        else if ( playerStance == PlayerStance.Prone ) {

            // ★彡[ Changing the movement speed while prone ]彡★
            playerSettings.speedChanger = playerSettings.proneSpeed;
        }
        else {

            // ★彡[ Resting the speed back to normal when none of above condition are true ]彡★
            playerSettings.speedChanger = 1;
        }

        // ★彡[ Changing the input according to the speed changer (to differentiate speed between normal, jumping, crouching or prone) ]彡★
        _verticalSpeed *= playerSettings.speedChanger;
        _horizontalSpeed *= playerSettings.speedChanger;

        // ★彡[ Smoothing the movement speed ]彡★
        settings.newMovementSpeed = Vector3.SmoothDamp( settings.newMovementSpeed, new Vector3( _horizontalSpeed, 0, _verticalSpeed), ref settings.newMovementSpeedVelocity, characterController.isGrounded ? playerSettings.smoothMovement : playerSettings.smoothfalling );

        // ★彡[ Making the global transform direction equal to players transform direction to make player move according to its rotation ]彡★
        var movementDirection = transform.TransformDirection( settings.newMovementSpeed );

        GravityCheck();
        // ★彡[ Making player jump and fall on the ground (using custom specified gravity) ]彡★
        movementDirection.y += playerSettings.playerGravity;
        movementDirection += settings.jumpForce * Time.deltaTime;

        // ★彡[ Making player move according to the inputs ]彡★
        characterController.Move( movementDirection );

    } // ★彡[ Calculate Movement ]彡★

    private void GravityCheck() {

        // ★彡[ Check if the player gravity is greater than minimum gravity to make player fall down according to the gravity amount ]彡★
        if( playerSettings.playerGravity > playerSettings.minGravity ) {

            playerSettings.playerGravity -= playerSettings.gravityAmount * Time.deltaTime;
        }

        // ★彡[ cheking if the player gravity is too less than what it should be (in order to avoid player sticking in the ground (bug fixed)) ]彡★
        if( playerSettings.playerGravity < -0.1f && characterController.isGrounded ) {

            playerSettings.playerGravity = -0.1f;
        }
    } // ★彡[ Gravity Check ]彡★

    private void CalculateJump() {

        // ★彡[ Calculating smooth jump using Vector3 smooth damp ]彡★
        settings.jumpForce = Vector3.SmoothDamp( settings.jumpForce, Vector3.zero, ref settings.jumpForceVelocity, playerSettings.jumpFalloff );
    } // ★彡[ Calculate Jump ]彡★

    private void CalculateStance() {

        var currentStance = playerSettings.playerStandStance;

        // ★彡[ Changing player current stance according to the PlayerStance selected enum ]彡★
        switch ( playerStance ) {

            case PlayerStance.Crouch:
            currentStance = playerSettings.playerCrouchStance;
            break;

            case PlayerStance.Prone:
            currentStance = playerSettings.playerProneStance;
            break;

            default:
            currentStance = playerSettings.playerStandStance;
            break;
        }

        // ★彡[ Smoothing the camera movement in Y axis according to the current stance ]彡★
        settings.cameraHeight = Mathf.SmoothDamp( lookRoot.localPosition.y, currentStance.cameraHeight, ref settings.cameraHeightVelocity, playerSettings.smoothPlayerStance );

        // ★彡[ Changing camera position according to the calculated smooth movement ]彡★
        lookRoot.localPosition = new Vector3( lookRoot.localPosition.x, settings.cameraHeight, lookRoot.localPosition.z );

        // ★彡[ Smoothly changing the Character Controller collider height according to the current stance collider height ]彡★
        characterController.height = Mathf.SmoothDamp( characterController.height, currentStance.stanceCollider.height, ref settings.stanceCapsuleHeightVelocity, playerSettings.smoothPlayerStance );

        // ★彡[ Smoothly changing the Character Controller collider center point according to the current stance collider center ]彡★
        characterController.center = Vector3.SmoothDamp( characterController.center, currentStance.stanceCollider.center, ref settings.stanceCapsuleCenterVelocity, playerSettings.smoothPlayerStance );
    } // ★彡[ Calculate Stance ]彡★

    private void Jump() {

        // ★彡[ check if the player has not grounded yet ]彡★
        if( !characterController.isGrounded ) {

            return;
        }

        if ( settings.isSprinting ) {

            // ★彡[ Increasing the jump height while sprinting to make player jump higher than normal speed ]彡★
            playerSettings.jumpHeight += 3;
        }
        
        // ★彡[ check if the player is prone to make it stand ]彡★
        if( playerStance == PlayerStance.Prone ) {

            // ★彡[ Checking if the player can stand or not ]彡★
            if ( CheckStance( playerSettings.playerStandStance.stanceCollider.height )) {

                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        
        // ★彡[ check if the player is crouching to first make it stand ]彡★
        if( playerStance == PlayerStance.Crouch ) {

            // ★彡[ Checking if the player can stand or not ]彡★
            if ( CheckStance( playerSettings.playerStandStance.stanceCollider.height )) {

                return;
            }

            playerStance = PlayerStance.Stand;
        }

        // ★彡[ Making player jump ]彡★
        settings.jumpForce = Vector3.up * playerSettings.jumpHeight;
        // ★彡[ Making the player gravity 0 in order to perform relevant jump force and gravity ]彡★
        playerSettings.playerGravity = 0;
    } // ★彡[ Jump ]彡★
    
    private void Crouch() {

        // ★彡[ Changing PlayerStance enum according to the input ]彡★
        switch ( playerStance ) {

            case PlayerStance.Crouch:
            // ★彡[ Checking if the player can stand or not ]彡★
            if ( CheckStance( playerSettings.playerStandStance.stanceCollider.height )) {

                Debug.Log( "Player can't not stand");
                return;
            }
            // ★彡[ if player can stand only then we are changing the PlayerStance to stand ]彡★
            playerStance = PlayerStance.Stand;
            playerAudio.WalkSound();
            break;

            default:
            // ★彡[ Checking if the player can crouch or not ]彡★
            if ( CheckStance( playerSettings.playerCrouchStance.stanceCollider.height )) {

                Debug.Log( "Player can't not crouch");
                return;
            }
            // ★彡[ if player can crouch only then we are changing the PlayerStance to crouch ]彡★
            playerStance = PlayerStance.Crouch;
            playerAudio.CrouchSound();
            break;
        }
    } // ★彡[ Crouch ]彡★

    private void Prone() {

        switch ( playerStance ) {

            case PlayerStance.Prone:
            // ★彡[ Checking if the player can stand or not ]彡★
            if ( CheckStance( playerSettings.playerStandStance.stanceCollider.height )) {

                Debug.Log( "Player can't stand" );
                return;
            }
            // ★彡[ if player can stand only then we are changing the PlayerStance to stand ]彡★
            playerStance = PlayerStance.Stand;
            playerAudio.WalkSound();
            break;

            default:
            // ★彡[ Making the PlayerStance to prone ]彡★
            playerStance = PlayerStance.Prone;
            playerAudio.ProneSound();
            break;
        }
    } // ★彡[ Prone ]彡★

    // ★彡[ Function to check if the player can change its stance or not ]彡★
    private bool CheckStance( float CheckStanceHeight ) {

        Vector3 feetPosition = new Vector3 ( feetObject.position.x, feetObject.position.y, feetObject.position.z );

        // ★彡[ declaring the start and end of collider check and keeping it slightly above according to the margin variable in order to avoid weird collisions ]彡★
        var start = new Vector3( feetPosition.x, feetPosition.y + characterController.radius + settings.stanceCheckErrorMargin, feetPosition.z );
        var end = new Vector3( feetPosition.x, feetPosition.y - characterController.radius - settings.stanceCheckErrorMargin + CheckStanceHeight, feetPosition.z );

        // ★彡[ Checking the collider from start to the end point according to the character controller radius with exception of player (because we are masking its layer out) ]彡★
        return Physics.CheckCapsule( start, end, characterController.radius, playerMask );
    } // ★彡[ Check Stance ]彡★

    private void ToggleSprint() {

        // ★彡[ To smooth the transition between sprint and walk ]彡★
        if( _movementInput.y <= 0.2f ) {

            settings.isSprinting = false;
            return;
        }

       settings.isSprinting = !settings.isSprinting;
       playerAudio.SprintSound();
    } // ★彡[ Toggle Sprint ]彡★
    
    private void StopSprint() {

        // ★彡[ Check if hold to sprint is true to make sprint function stop when sprint key is not held anymore ]彡★
        if( playerSettings.holdToSprint ) {

            settings.isSprinting = !settings.isSprinting;
            playerAudio.WalkSound();
        }
    } // ★彡[ Stop Sprint ]彡★

    #endregion

} // ★彡[ class ]彡★
