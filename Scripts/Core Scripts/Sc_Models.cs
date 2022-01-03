using System;
using UnityEngine;

public static class Sc_Models {

    
    #region ★彡[ Player ]彡★

    public enum PlayerStance {

        Stand,
        Crouch,
        Prone
    } // ★彡[ Player Stance ]彡★

    [Serializable]
    public class PlayerModelSettings {

        [Header( "View Settings" )]
        public float xSensitivity = 5f;
        public float ySensitivity = 5f;
        public float minViewAngleY = -70;
        public float maxViewAngleY = 70;

        public bool xInverted = false;
        public bool yInverted = false;

        [Header( "Movement Settings" )]
        public bool holdToSprint = true;
        public float smoothMovement = 0.2f;

        [Header( "Walking Movement Settings" )]
        public float walkingForwardSpeed = 4f;
        public float walkingBackwardSpeed = 2f;
        public float walkingStrafeSpeed = 3f;

        [Header( "Running Movement Settings" )]
        public float runningForwardSpeed = 8f;
        public float runningStrafeSpeed = 5f;

        [Header( "Jump Settings" )]
        public float jumpHeight = 12.0f;
        public float jumpFalloff = 0.2f;
        public float smoothfalling = 0.4f;

        [Header( "Speed Changer" )]
        public float speedChanger = 1;
        public float crouchSpeed = 0.6f;
        public float proneSpeed = 0.2f;
        public float fallingSpeed = 0.4f;

        [Header( "Gravity Settings" )]
        public float gravityAmount = 0.7f;
        public float minGravity = -3f;
        [HideInInspector]
        public float playerGravity;

        [Header( "Stance Settings" )]
        public float smoothPlayerStance = 0.15f;
        public ModelStance playerStandStance;
        public ModelStance playerCrouchStance;
        public ModelStance playerProneStance;
    } // ★彡[ Player Model Settings ]彡★

    [Serializable]
    public class ModelStance {

        public float cameraHeight;
        public CapsuleCollider stanceCollider;
    } // ★彡[ Model Stance ]彡★

    public class PlayerAttributesSettings {

        // ★彡[ Rotation ]彡★
        public Vector3 newCameraRotation;
        public Vector3 newPlayerRotation;
        
        // ★彡[ Camera ]彡★
        public float cameraHeight;
        public float cameraHeightVelocity;

        // ★彡[ Jumping ]彡★
        public float initialJumpHeight;
        public Vector3 jumpForce;
        public Vector3 jumpForceVelocity;

        // ★彡[ Movement ]彡★
        public Vector3 newMovementSpeed;
        public Vector3 newMovementSpeedVelocity;

        // ★彡[ Capsule Collider Check ]彡★
        public float stanceCheckErrorMargin = 0.05f;
        public float stanceCapsuleHeightVelocity;
        public Vector3 stanceCapsuleCenterVelocity;

        public bool isSprinting;

    } // ★彡[ Player Attributes Setting ]彡★

    #endregion

    #region ★彡[ Sound Settings ]彡★

    public class SoundSettings {

        // ★彡[ Default Settings ]彡★
        public float minVolume, maxVolume;
        public float accumulatedDistance;
        public float stepDistance;

        // ★彡[ Sound volume ]彡★
        public float sprintVolume = 0.7f;
        public float crouchVolume = 0.1f;
        public float proneVolume = 0.05f;
        public float jumpVolume = 0.3f;
        public float walkMinVolume = 0.2f, walkMaxVolume = 0.4f;

        // ★彡[ Step Distances ]彡★
        public float walkStepDistance = 0.40f;
        public float sprintStepDistance = 0.20f;
        public float crouchStepDistance = 0.55f;
        public float proneStepDistance = 0.8f;

    } // ★彡[ Sound Settings ]彡★

    #endregion

    #region ★彡[ Mouse ]彡★

    [Serializable]
    public class MouseLookSettings {

        // ★彡[ Bool checks ]彡★
        public bool invert;
        public bool canUnlock = true;

        // ★彡[ Sensitivity and smoothing ]彡★
        [Range( 0, 30 )] public float sensitivity = 5;
        [Range( 0, 30 )] public float smoothWeight = 0.4f;
        [Range( 0, 50 )] public float rollAngle = 10.0f;
        [Range( 0, 50 )] public float rollSpeed = 3.0f;
        [Range( 0, 50 )] public float currentRollAngle;
        [Range( 0, 50 )] public int lastLookFrame;
        [Range( 0, 50 )] public int smoothSteps = 10;

        // ★彡[ limit view in Y axis ]彡★
        public Vector2 defaultLookLimits = new Vector3( -70f, 80f );

        // ★彡[ Mouse look and smoothing ]彡★
        public Vector2 lookAngles;
        public Vector2 currentMouseLook;
        public Vector2 smoothMove;
    }  // ★彡[ Mouse look settings ]彡★

    #endregion
}
