using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        private Vector3 offset;

        private Transform _transform;

        void Start()
        {
            offset = transform.position - target.position;

            _transform = transform;
        }
        


        private void LateUpdate()
        {
            _transform.position = target.position + offset;
        }
    }
}
