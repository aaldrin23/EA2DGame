using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public LayerMask groundLayers;

  public float runSpeed = 4;
  public float walkSpeed = 2;

  public float jumpForce = 7;

  private Rigidbody rb;

  private CapsuleCollider col;
  private GameObject currentHitObject;
  private Animator animator;

  private bool facingRight = true;

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

    if (IsGrounded() && Input.GetButtonDown("Jump"))
    {
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
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

  private bool IsGrounded()
  {
    RaycastHit hit;
    float maxDistance = 1f;
    if (Physics.SphereCast(transform.position, col.height / 2, Vector3.down, out hit, maxDistance, groundLayers))
    {
      currentHitObject = hit.transform.gameObject;
      hitDistance = hit.distance;

      return hitDistance >= .1f;
    }
    else
    {
      return false;
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Debug.DrawLine(transform.position, transform.position + transform.forward * hitDistance);
    Gizmos.DrawWireSphere(transform.position + transform.forward * hitDistance, col.radius);
  }

  #endregion
}
