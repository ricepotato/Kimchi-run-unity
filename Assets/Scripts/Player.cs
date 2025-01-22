using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce;

    [Header("References")]
    public Rigidbody2D PlayerRigidbody;
    public Animator PlayerAnimator;

    private bool isGrounded = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public BoxCollider2D PlayerCollider;

    private bool isInvincible = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            //PlayerRigidbody.linearVelocityX = 10;
            //PlayerRigidbody.linearVelocityY = 20;
            PlayerRigidbody.AddForceY(jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            PlayerAnimator.SetInteger("state", 1);
        }        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.name == "Platform"){
            if(!isGrounded){
                PlayerAnimator.SetInteger("state", 2);
            }
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "enermy"){
            if(!isInvincible){
                Destroy(collider.gameObject);
            }
            Hit();
        }

        else if(collider.gameObject.tag == "food"){
            Destroy(collider.gameObject);
            Heal();
        }

        else if(collider.gameObject.tag == "golden"){
            Destroy(collider.gameObject);
            StartInvincible();
        }
    }

    void Hit(){
        //lives -= 1;
        GameManager.Instance.lives -= 1;
    }

    void Heal(){
        GameManager.Instance.lives = Mathf.Min(3, GameManager.Instance.lives + 1);
    }

    void StartInvincible(){
        isInvincible = true;
        Invoke("StopInvincible", 5);
    }

    void StopInvincible(){
        isInvincible = false;
    }

    public void KillPlayer(){
        PlayerCollider.enabled = false;
        PlayerAnimator.enabled = false;
        PlayerRigidbody.AddForceY(jumpForce, ForceMode2D.Impulse);
    }
}
