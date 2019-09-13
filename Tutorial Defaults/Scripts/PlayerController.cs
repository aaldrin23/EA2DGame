using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public LayerMask groundLayers;

  public float speed = 5;

  public float jumpForce = 7;

  private Rigidbody rb;

  private CapsuleCollider col;
  private GameObject currentHitObject;
  float hitDistance = 0f;

  #region Monobehaviour API

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    col = GetComponent<CapsuleCollider>();
  }

  void Update()
  {
    float moveHorizontal = Input.GetAxis("Horizontal");

    rb.velocity = new Vector3(moveHorizontal * speed, rb.velocity.y);

    if (IsGrounded() && Input.GetButtonDown("Jump"))
    {
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Debug.Log(IsGrounded());
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
