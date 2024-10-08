using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Manager
    public GameManager gameManager;

    //Adudio
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    //State
    public float maxSpeed;
    public float jumpPower;

    //Comonent
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCol;
    AudioSource audioSource;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        
        audioSource.Play();
    }

    private void Update()
    {
        //NOTE : Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);

            //Sound
            PlaySound("JUMP");
        }

        //NOTE : Stop Speed
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f , rigid.velocity.y);

        //NOTE : Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //NOTE : Animation
        //if (rigid.velocity.normalized.x == 0)
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    private void FixedUpdate()
    {
        //NOTE : Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //NOTE : Max Speed
        if (rigid.velocity.x > maxSpeed)    // RIght Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))    // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, Color.green);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                //NOTE : Coliider 크기의 반값
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //Attack
            if(rigid.velocity.y < 0 && 
                transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            //Damaged
            else
                OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze)
                gameManager.stagePoint += 50;
            else if(isSilver)
                gameManager.stagePoint += 100;
            else if (isGold)
                gameManager.stagePoint += 300;

            // Deactive Item
            collision.gameObject.SetActive(false);

            //Sound
            PlaySound("ITEM");
        }
        else if(collision.gameObject.tag == "Finish")
        {
            //Sound
            PlaySound("FINISH");

            // Next Stage
            gameManager.NextStage();
        }
    }

    private void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;

        // Reaction Force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();

        //Sound
        PlaySound("ATTACK");
    }
    private void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown();

        // Change Layer(Immortal Active)
        gameObject.layer = 11;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dir, 1) * 7, ForceMode2D.Impulse);

        // Animation
        anim.SetTrigger("doDamaged");

        //Sound
        PlaySound("DAMAGED");

        Invoke("OffDamaged", 3f);
    }

    private void OffDamaged()
    {
        // Change Layer(Immortal Active)
        gameObject.layer = 10;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
         //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;

        //Collider Disable
        capsuleCol.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Sound
        PlaySound("DIE");
    }

    public void VelocityZero()
    {
        //NOTE : 낙하속도를 0으로 바꿈
        rigid.velocity = Vector2.zero;
    }
}
