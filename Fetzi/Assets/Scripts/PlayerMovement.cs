using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public Animator anim;
    public FixedJoystick joystick;
    public GameObject climpEffect;
    public AudioSource audioSource;
    [Range(0f, 5f)]
    public float audioSourceVolume = 0.7f;

    private float horizontal;
    public float speed = 5f;
    public float shiftSpeed = 2f;
    public float controllSpeed = 8f;
    public float decayRate = 1.5f; // Adjust this value to control the rate of decay
    private float speedDefault;
    private float jumpingPower = 12f;
    private bool isFacingRight = true;
    private bool canDoubleJump = false;
    private AudioManager audio;

    private float wallBoost;
    public float rate = 0.1f;
    public float threshold = 0.05f;

    private void Start() {
        speedDefault = speed;
        wallBoost = 0f;

        audioSource.volume = audioSourceVolume * PlayerPrefs.GetFloat("volume", 0.5f) * PlayerPrefs.GetFloat("volumeEffects", 0.5f);

        audio = FindObjectOfType<AudioManager>();
    }

    private float MoveHorizontal;
    // Update is called once per frame
    void Update() {

        bool isWalled = IsWalled();
        bool isGroundet = IsGrounded();

        if (isGroundet)
        {
            canDoubleJump = true;
            wallBoost = 0;
        }

        climpEffect.SetActive(isWalled && (horizontal != 0 || wallBoost != 0) && rb.velocity.y <= 0f);

        anim.SetBool("isJumping", !isGroundet);

        horizontal = Input.GetAxis("Horizontal");
        horizontal += joystick.Horizontal;

        if (horizontal > 1)
        {
            horizontal = 1;
        }else
        if (horizontal < -1)
        {
            horizontal = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            speed = controllSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            speed = speedDefault;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            speed = shiftSpeed;
            anim.SetBool("isSneaking", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            speed = speedDefault;
            anim.SetBool("isSneaking", false);
        }

        if (wallBoost > 2f || wallBoost < -2f)
        {
            horizontal /= 4;

            if (!isFacingRight && wallBoost > 0f)
            {
                Flip();
            }
            else if (isFacingRight && wallBoost < 0f)
            {
                Flip();
            }
        }
        else
        {
            if (!isFacingRight && horizontal > 0f)
            {
                Flip();
            }
            else if (isFacingRight && horizontal < 0f)
            {
                Flip();
            }
        }

        float tempHorizonz = horizontal;
        if (isWalled)
        {
            tempHorizonz = 0;
        }

        MoveHorizontal = tempHorizonz;

        if (tempHorizonz != 0f)
        {
            anim.SetBool("isRunning", true);
            if (isGroundet)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
            audioSource.Stop();
        }


        if (rb.velocity.y <= 0f && isWalled && (horizontal != 0 || wallBoost != 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, -1f);
            anim.SetBool("isClimping", true);
        }
        else
        {
            anim.SetBool("isClimping", false);
        }

    }

    private void FixedUpdate() {

        if (wallBoost != 0)
        {
            if (Mathf.Abs(wallBoost) < threshold)
            {
                wallBoost = 0;
            }
            else
            {
                float sign = Mathf.Sign(wallBoost);
                wallBoost -= rate * Mathf.Abs(wallBoost) * sign * Time.deltaTime;
                if (sign == -1 && wallBoost > 0)
                {
                    wallBoost = 0;
                }
            }
        }

        rb.velocity = new Vector2(MoveHorizontal * speed + wallBoost, rb.velocity.y);
    }

    public void Jump() {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            FindObjectOfType<AudioManager>().Play("jump");
        }
        else if (IsWalled() && (horizontal != 0 || wallBoost != 0))
        {
            // Wall Jump
            float wallJumpDirection = isFacingRight ? -1f : 1f; // determine the direction to jump based on the player's facing direction
            rb.velocity = new Vector2(wallJumpDirection * speed, jumpingPower * 0.65f);
            FindObjectOfType<AudioManager>().Play("jump");

            if (isFacingRight)
            {
                wallBoost = -12f;
            }
            else
            {
                wallBoost = 12f;
            }

            Flip(); // flip the player's facing direction after the wall jump

        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            FindObjectOfType<AudioManager>().Play("jump");
            anim.SetBool("isJumping", false);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public Vector2 boxSize = new Vector2(1f, 1f);
    private bool IsWalled()
    {
        return Physics2D.OverlapBox(wallCheck.position, boxSize, 0f, groundLayer);
    }

    private void OnDrawGizmosSelected() {
        // Draw a wireframe box in the editor to show the area being checked
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(wallCheck.position, boxSize);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void StartSprint(Image image) {
        if (speed == speedDefault)
        {
            speed = controllSpeed;
            Color newColor = image.color;
            newColor.a = 0.5f;
            image.color = newColor;
        }
        else
        {
            speed = speedDefault;
            Color newColor = image.color;
            newColor.a = 1f;
            image.color = newColor;
        }
    }
    
    public void StopSprint() {
        //speed = speedDefault;
    }

}
