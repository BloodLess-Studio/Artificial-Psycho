using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodLessStudio.ArtificialPsycho {
    public class Movement : MonoBehaviour
    {
        public float speed;

        [SerializeField]
        private Rigidbody PlayerBody;

        private Vector3 direction;

        void FixedUpdate()
        {
            float horizon = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            direction = new Vector3(horizon, 0, vertical);
            direction.Normalize();

            PlayerBody.velocity = transform.TransformDirection(direction) * speed * Time.fixedDeltaTime;
        }

    }
}
