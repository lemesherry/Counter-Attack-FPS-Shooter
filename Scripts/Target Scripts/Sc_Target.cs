using UnityEngine;

public class Sc_Target : MonoBehaviour {

    public float health = 40f;

    public void TakeDamage( float amountToDamage ) {
        
        health -= amountToDamage;

        if( health <= 0f ) {

            Die();
        }
    }

    private void Die() {

        Destroy( gameObject );
    }
}
