// Copyright (C) 2022 Nicholas Maltbie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;
using UnityEngine.InputSystem;

namespace com.nickmaltbie.MinimapTools.MinimapFPS
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        /// <summary>
        /// Minimum pitch for camera movement.
        /// </summary>
        public const float minPitch = -90;

        /// <summary>
        /// Maximum pitch for camera movement.
        /// </summary>
        public const float maxPitch = 90;

        /// <summary>
        /// Action realted to moving the camera, should be a two component vector.
        /// </summary>
        [Tooltip("Action with two axis to rotate player camera around.")]
        public InputActionReference lookAround;

        /// <summary>
        /// Action realted to moving the player, should be a two component vector.
        /// </summary>
        [Tooltip("Action with two axis used to move the player around.")]
        public InputActionReference movePlayer;

        /// <summary>
        /// How fast the player can rotate in degrees per second.
        /// </summary>
        [Tooltip("Rotation speed when moving the camera.")]
        [SerializeField]
        public float rotateSpeed = 90f;

        /// <summary>
        /// Player movement speed.
        /// </summary>
        [Tooltip("Move speed of the player character.")]
        [SerializeField]
        public float moveSpeed = 5.0f;

        /// <summary>
        /// Speed at which the player falls.
        /// </summary>
        [Tooltip("Speed at which the player falls.")]
        [SerializeField]
        public Vector3 gravity = new Vector3(0, -20, 0);

        private Vector2 cameraAngle;
        private Vector3 velocity;
        private Transform cameraTransform;
        private CharacterController characterController;

        public void Awake()
        {
            characterController = GetComponent<CharacterController>();
            cameraTransform = GetComponentInChildren<Camera>().transform;
            lookAround?.action.Enable();
            movePlayer?.action.Enable();
        }

        // Update is called once per frame
        public void Update()
        {
            // Read input values from player
            Vector2 cameraMove = lookAround.action.ReadValue<Vector2>();
            Vector2 playerMove = movePlayer.action.ReadValue<Vector2>();

            // Camera move on x (horizontal movement) controls the yaw (look left or look right)
            // Camera move on y (vertical movement) controls the pitch (look up or look down)
            cameraAngle.x += -cameraMove.y * rotateSpeed * Time.deltaTime;
            cameraAngle.y += cameraMove.x * rotateSpeed * Time.deltaTime;

            cameraAngle.x = Mathf.Clamp(cameraAngle.x, minPitch, maxPitch);
            cameraAngle.y %= 360;

            // Rotate player based on mouse input, ensure pitch is bounded to not overshoot
            transform.rotation = Quaternion.Euler(0, cameraAngle.y, 0);
            cameraTransform.rotation = Quaternion.Euler(cameraAngle.x, cameraAngle.y, 0);

            // Check if the player is falling
            bool falling = !characterController.isGrounded;

            // If falling, increase falling speed, otherwise stop falling.
            if (falling)
            {
                velocity += Physics.gravity * Time.deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
            }

            // Read player input movement
            var inputVector = new Vector3(playerMove.x, 0, playerMove.y);

            // Rotate movement by current viewing angle
            var viewYaw = Quaternion.Euler(0, cameraAngle.y, 0);
            Vector3 rotatedVector = viewYaw * inputVector;
            Vector3 normalizedInput = rotatedVector.normalized * Mathf.Min(rotatedVector.magnitude, 1.0f);

            // Attempt to move the player based on player movement
            characterController.Move((normalizedInput * moveSpeed + velocity) * Time.deltaTime);
        }
    }
}
