using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    AudioSource m_AudioSource;

    private void Awake () {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
    }

    private void FixedUpdate () {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");

        m_Movement.Set (horizontal, 0, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);

        if (isWalking) {
            if (!m_AudioSource.isPlaying) {
                m_AudioSource.Play ();
            }
        } else {
            if (m_AudioSource.isPlaying) {
                m_AudioSource.Stop ();
            }
        }

        Vector3 desiredForard = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0);
        m_Rotation = Quaternion.LookRotation (desiredForard);
    }

    private void OnAnimatorMove () {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}