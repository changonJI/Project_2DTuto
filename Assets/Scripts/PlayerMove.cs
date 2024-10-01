using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //NOTE : Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
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
        Debug.Log(h);
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
}
