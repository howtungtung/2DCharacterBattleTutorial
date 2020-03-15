using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float groundDistance = 0.1f;
    public float footOffset;
    public float playerWidth;
    public LayerMask groundLayer;

    private Rigidbody2D rigid2D;
    private Animator animator;
    private float horizontalInput;
    private int direction = 1;
    private bool jumpPressed;
    private bool attackHeld;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        attackHeld = Input.GetButton("Fire1");
        animator.SetFloat("speed", Mathf.Abs(horizontalInput));
        animator.SetBool("isAttacking", attackHeld);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        GroundMovement();
        AirMovement();
    }

    private void GroundCheck()
    {
        isGrounded = false;
        RaycastHit2D leftCheck = Raycast(new Vector2(-playerWidth, footOffset), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(playerWidth, footOffset), Vector2.down, groundDistance);
        if(leftCheck || rightCheck)
        {
            isGrounded = true;
        }
    }

    private void GroundMovement()
    {
        if (isGrounded)
        {
            float xVelocity = moveSpeed * horizontalInput;
            rigid2D.velocity = new Vector2(xVelocity, rigid2D.velocity.y);
        }
        if (horizontalInput * direction < 0)
        {
            Flip();
        }
    }

    private void AirMovement()
    {
        if (jumpPressed && isGrounded)
        {
            rigid2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        jumpPressed = false;
    }

    private void Flip()
    {
        direction *= -1;
        transform.eulerAngles = direction == 1 ? Vector3.zero : new Vector3(0, 180, 0);
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, groundLayer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}
