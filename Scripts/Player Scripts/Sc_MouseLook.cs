using UnityEngine;
using static Sc_Models;

public class Sc_MouseLook : MonoBehaviour {

    [SerializeField] private Transform _playerRoot, _lookRoot;

    public MouseLookSettings settings;

    private void Start() {
        
        // ★彡[ locking the cursor ]彡★
        Cursor.lockState = CursorLockMode.Locked;
    } // ★彡[ Start ]彡★

    private void Update() {
        
        LockUnlockCursor();

        // ★彡[ check if the cursor is locked ]彡★
        if( Cursor.lockState == CursorLockMode.Locked ) {

            LookAround();
        }
    } // ★彡[ Update ]彡★

    private void LockUnlockCursor() {

        // ★彡[ check if the escape is pressed ]彡★
        if( Input.GetKeyDown(KeyCode.Escape) ) {
            // ★彡[ check if the cursor is already locked ]彡★
            if( Cursor.lockState == CursorLockMode.Locked ) {
                // ★彡[ unlocking the cursor ]彡★
                Cursor.lockState = CursorLockMode.None;

            } else {
                // ★彡[ locking the cursor ]彡★
                Cursor.lockState = CursorLockMode.Locked;
                // ★彡[ making cursor invisible ]彡★
                Cursor.visible = false;
            }
        }
    } // ★彡[ lock unlock cursor ]彡★

    private void LookAround() {

        // ★彡[ Getting mouse inputs ]彡★
        settings.currentMouseLook = new Vector2( Input.GetAxis(MouseAxis.mouseY), Input.GetAxis(MouseAxis.mouseX) );

        // ★彡[ Setting look in upward/downward direction accroding to the sensitivity ]彡★
        settings.lookAngles.x += settings.currentMouseLook.x * settings.sensitivity * ( settings.invert ? 1f : -1f );

        // ★彡[ Setting look in right/left direction accroding to the sensitivity ]彡★
        settings.lookAngles.y += settings.currentMouseLook.y * settings.sensitivity;

        // ★彡[ setting upward/downward look limits  ]彡★
        settings.lookAngles.x = Mathf.Clamp( settings.lookAngles.x, settings.defaultLookLimits.x, settings.defaultLookLimits.y );

        // ★彡[ Setting the Z rotation value just to make player look drunk/wounded (not a necessary thing) ]彡★
        // settings.currentRollAngle = Mathf.Lerp( settings.currentRollAngle, Input.GetAxisRaw( MouseAxis.mouseX) * settings.rollAngle, Time.deltaTime * settings.rollSpeed );

        // ★彡[ Rotating LookRoot gameObject in Y axis ]彡★
        _lookRoot.localRotation = Quaternion.Euler( settings.lookAngles.x, 0f, settings.currentRollAngle );

        // ★彡[ Rotating playerRoot gameObject in X axis ]彡★
        _playerRoot.localRotation = Quaternion.Euler( 0f, settings.lookAngles.y, 0f );

    } // ★彡[ look around ]彡★

}
