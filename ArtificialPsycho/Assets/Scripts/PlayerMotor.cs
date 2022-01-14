using UnityEngine;

namespace BloodLessStudio.ArtificialPsycho { //Namespace

    [RequireComponent(typeof(Rigidbody))] //Require PlayerMotor Componement
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private Rigidbody Rb;

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
            Movement();
            CameraMovement();
        }

        private void Movement()
        {
            if (velocity != Vector3.zero)
            {
                Rb.MovePosition(Rb.position + velocity * Time.fixedDeltaTime);
            }
        }

        private void CameraMovement()
        {
            Rb.MoveRotation(Rb.rotation * Quaternion.Euler(cameraY));
            playerCamera.transform.Rotate(-cameraX);
        }
    }
}