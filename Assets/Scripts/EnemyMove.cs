using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public int nextMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    // Recursive
    private void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //sprite animation
        anim.SetInteger("WalkSpeed", nextMove);

        //flip Sprite
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        //NOTE : 5초뒤 함수 호출.(Coroutine, Invoke)
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        // 목적지의 반대방향값
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        //NOTE : Invoke 함수를 멈추고 재실행
        //NOTE : 코루틴일시 StopCoroutine, StartCourine
        CancelInvoke();
        Invoke("Think", 5);
    }
}
