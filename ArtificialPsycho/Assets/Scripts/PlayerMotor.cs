using UnityEngine;

namespace BloodLessStudio.ArtificialPsycho { //Namespace

    [RequireComponent(typeof(Rigidbody))] //Require PlayerMotor Componement
    public class PlayerMotor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private Rigidbody Rb;

        [Header("Settings")]
        public float maxJump = 2f;
        public float jumpHeight = 5f;

        private int jumpNB = 0;
        private bool isGrounded = true;

        private Vector3 velocity;
        private Vector3 cameraX;
        private Vector3 cameraY;

        public void SetVelocity(Vector3 _velocity)
        {
            velocity = _velocity;
        }

        public void SetCameraX(Vector3 _cameraX)
        {
            cameraX = _cameraX;
        }

        public void SetCameraY(Vector3 _cameraY)
        {
            cameraY = _cameraY;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleCameraMovement();
        }
        private void Update()
        {
            HandleSpecificMovement();
        }

        private void HandleMovement()
        {
            if (velocity != Vector3.zero)
            {
                Rb.MovePosition(Rb.position + velocity * Time.fixedDeltaTime);
            }
        }

        private void HandleCameraMovement()
        {
            Rb.MoveRotation(Rb.rotation * Quaternion.Euler(cameraY));
            playerCamera.transform.Rotate(-cameraX);
        }

        private void HandleSpecificMovement()
        {
            if (jumpNB >= maxJump) isGrounded = false;
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
                jumpNB++;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            isGrounded = true;
            jumpNB = 0;
        }
    }
}