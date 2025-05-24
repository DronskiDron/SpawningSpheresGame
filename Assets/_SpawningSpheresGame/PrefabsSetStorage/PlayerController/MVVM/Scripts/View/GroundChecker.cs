using UnityEngine;

namespace PlayerController.MVVM
{
    public class GroundChecker : MonoBehaviour
    {
        public LayerMask groundLayer;
        public Transform groundPoint;
        public float checkRadius = 0.3f;

        private Collider[] groundColliders = new Collider[1];


        public bool GetIsGrounded()
        {
            var isGrounded = Physics.OverlapSphereNonAlloc(groundPoint.position, checkRadius, groundColliders, groundLayer) > 0;
            return isGrounded;
        }
    }
}