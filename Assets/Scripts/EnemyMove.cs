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

        //NOTE : 5�ʵ� �Լ� ȣ��.(Coroutine, Invoke)
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        // �������� �ݴ���Ⱚ
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        //NOTE : Invoke �Լ��� ���߰� �����
        //NOTE : �ڷ�ƾ�Ͻ� StopCoroutine, StartCourine
        CancelInvoke();
        Invoke("Think", 5);
    }
}
