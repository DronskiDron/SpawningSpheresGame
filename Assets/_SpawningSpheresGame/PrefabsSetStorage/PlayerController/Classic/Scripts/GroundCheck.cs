using UnityEngine;

namespace PlayerController
{
    public class GroundCheck : MonoBehaviour
    {
        public LayerMask groundLayer;
        public Transform groundPoint;
        public float checkRadius = 0.3f;

        private Collider[] groundColliders = new Collider[1];
        public bool IsGrounded { get; private set; }

        private void FixedUpdate()
        {
            IsGrounded = Physics.OverlapSphereNonAlloc(groundPoint.position, checkRadius, groundColliders, groundLayer) > 0;
        }
    }
}
