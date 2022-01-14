using UnityEngine;

namespace BloodLessStudio.ArtificialPsycho { //Namespace

    [RequireComponent(typeof(PlayerMotor))] //Require PlayerMotor Componement
    public class PlayerController : MonoBehaviour
    {
        public float speed = 20f;
        public float mouseSensitivity = 3f;

        [SerializeField]
        private PlayerMotor Motor;

        private Vector3 velocity;
        private Vector3 xRotation;
        private Vector3 yRotation;

        private void Update()
        {
            /*--------- Player Mouvement Input ---------*/
            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");    //Get inputs

            velocity = (transform.right * xMov + transform.forward * zMov).normalized * speed; //Does math thing

            Motor.SetVelocity(velocity); //Set new velocity in PlayerMotor

            /*--------- Camera Mouvement Input ---------*/
            float yRot = Input.GetAxisRaw("Mouse X");
            float xRot = Input.GetAxisRaw("Mouse Y");

            yRotation = new Vector3(0, yRot, 0);
            xRotation = new Vector3(xRot, 0, 0) * mouseSensitivity;

            Motor.SetCameraY(yRotation);
            Motor.SetCameraX(xRotation);
        }
    }
}
