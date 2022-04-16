using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovimentation : MonoBehaviour
{
    public float speedForce = 4f;
    public float jumpForce = 4f;
    public float jumpTime = 0.28f;

    private Rigidbody2D rigidbody2;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D collider2d;

    private bool canRun = true;
    private bool inGround = false;
    private float jumpTimer = 0f;
    private bool jumping = false;

    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<BoxCollider2D>();

        rigidbody2.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInGround();
        JumpAnimation();
        RunAnimation();
    }

    void CheckInGround()
    {
        RaycastHit2D raycastHit2 = (
            Physics2D.BoxCast(
                collider2d.bounds.center,
                collider2d.bounds.size,
                0f,
                Vector2.down,
                0.1f,
                LayerMask.GetMask("Ground"))
            );

        inGround = raycastHit2.collider != null;
    }

    void RunAnimation()
    {
        if (!inGround || !canRun) return;

        float input = Input.GetAxisRaw("Horizontal");

        rigidbody2.velocity = new Vector2(input * speedForce, rigidbody2.velocity.y);
        spriteRenderer.flipX = input < 0;

        if (input != 0)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    void JumpAnimation()
    {
        if (jumping)
        {
            jumpTimer += Time.deltaTime;
        }

        if (jumpTimer >= jumpTime)
        {
            jumpTimer = 0;
            canRun = true;
            jumping = false;
        }

        if (!inGround) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, jumpForce);
            animator.Play("Jump");
            canRun = false;
            jumping = true;
        }
    }

    public void DisableRun()
    {
        canRun = false;
        rigidbody2.velocity = new Vector2(0, rigidbody2.velocity.y);
    }

    public void EnableRun()
    {
        canRun = true;
    }
}
