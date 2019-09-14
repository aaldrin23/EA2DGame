using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public LayerMask groundLayers;

  public float runSpeed = 4;
  public float walkSpeed = 2;

  public float jumpForce = 5;

  private Rigidbody rb;

  private CapsuleCollider col;
  private GameObject currentHitObject;
  private Animator animator;

  private bool facingRight = true;
  private bool grounded = true;
  private bool landing = false;

  float hitDistance = 0f;

  #region Monobehaviour API

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    col = GetComponent<CapsuleCollider>();
    animator = GetComponent<Animator>();
  }

  void Update()
  {

  }

  void FixedUpdate()
  {
    float move = Input.GetAxis("Horizontal");

    CheckDirection(move);

    float speed = move;
    bool shift = Input.GetButton("Shift");

    speed = move * (shift ? walkSpeed : runSpeed);
    rb.velocity = new Vector3(speed, rb.velocity.y);


    animator.SetBool("walk", shift);
    animator.SetFloat("speed", Mathf.Abs(speed));

    GroundCheck();
    animator.SetBool("grounded", grounded);
    animator.SetBool("landing", landing);

    if (grounded && Input.GetButtonDown("Jump"))
    {
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    animator.SetFloat("vspeed", rb.velocity.y);
  }

  private void CheckDirection(float move)
  {
    if (move > .5 && !facingRight)
    {
      Flip();
    }
    else if (move < -0.5 && facingRight)
    {
      Flip();
    }
  }

  void Flip()
  {
    facingRight = !facingRight;
    Vector3 scale = transform.localScale;
    scale.z *= -1;
    transform.localScale = scale;
  }

  private void GroundCheck()
  {
    RaycastHit hit;
    float maxDistance = 5f;
    Vector3 origin = new Vector3(transform.position.x, transform.position.y + (col.radius), transform.position.z);
    if (Physics.SphereCast(origin, col.radius, Vector3.down, out hit, maxDistance, groundLayers))
    {
      currentHitObject = hit.transform.gameObject;
      hitDistance = hit.distance;
      Debug.Log(hitDistance);

      grounded = hitDistance <= 0.01f;
      landing = !grounded && hitDistance <= 0.2f;
    }
    else
    {
      grounded = false;
      landing = false;
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + (col.radius), transform.position.z), col.radius);
  }

  #endregion
}
