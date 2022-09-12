using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace proto
{
    public class PlayerControl : MonoBehaviour
    {
        #region Inspector

        Rigidbody rb;
        public float playerVelocity = 2f;
        public float playerSpeed = 2f;
        public float turnSpeed = 3;
        public bool isGrounded = true;
        [SerializeField] bool playerCanMovePos = true;
        [SerializeField] bool playerCanMoveRot = true;
        public LayerMask mouseToWorldIgnoredLayers;
        [SerializeField] bool useGamepad = false;
        public bool moveDuringAttack = false;
        public bool inAttack = false;
        [SerializeField] float previousPlayerMoveValue;
        [SerializeField] float moveDuringAttackTimer = 0;
        public Animator playerAnimator;
        public float fallMultiplier = 2.0f;
        public Vector3 jumpDirection;

        #endregion


        #region Hidden

        Ray cameraRay;
        RaycastHit cameraRayHit;
        PlayerInput playerInput;
        Vector2 move;
        Vector3 input;
        Vector3 playerInitialPos, playerInitialRot;
        float playerMoveValue = 0;
        Vector3 targetPosition;
        public float moveDuringAttackDuration = 0.5f;

        #endregion


        #region Updates

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            playerInput = GetComponent<PlayerInput>();
            playerAnimator = GetComponent<Animator>();

            playerInitialPos = transform.position;
            playerInitialRot = transform.eulerAngles;

            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (!useGamepad)
            {
                move = playerInput.actions["MoveForwardAndTurn_Joystick"].ReadValue<Vector2>();
                input = new Vector3(move.x, 0, move.y);
            }

            if (playerCanMoveRot)
            {
                Look();
            }

            if (transform.position.y <= -20f && !isGrounded)
            {
                //gameManager.CancelDeadlyAction();
            }
        }

        void FixedUpdate()
        {
            if (playerCanMovePos)
            {
                Move();

                if ((move.x < 0.01f && move.x > -0.01f) && (move.y < 0.01f && move.y > -0.01f)
                && playerMoveValue < 1f || !isGrounded)
                {
                    playerAnimator.SetBool("walk", false);
                    playerAnimator.SetFloat("walkSpeed", 0f);
                }
                else
                {
                    playerAnimator.SetBool("walk", true);
                    if (useGamepad)
                    {
                        playerAnimator.SetFloat("walkSpeed", input.magnitude);
                    }
                    else
                    {
                        playerAnimator.SetFloat("walkSpeed", 1f);
                    }
                }
            }
            else
            {
                playerAnimator.SetBool("walk", false);
                playerAnimator.SetFloat("walkSpeed", 0f);
            }

            Ray checkGround = new Ray(transform.position + (Vector3.up * 0.1f), -transform.up);
            RaycastHit hit;
            float rayLenght = 0.5f;

            if (Physics.Raycast(checkGround, out hit, rayLenght))
            {
                isGrounded = true;
                playerAnimator.SetBool("isGrounded", true);
            }
            else
            {
                isGrounded = false;
                playerAnimator.SetBool("isGrounded", false);

                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
                }
            }
        }

        #endregion


        #region Main

        public void FreezePlayerAction(bool freezePos, bool freezeRot)
        {
            if (freezePos)
            {
                playerCanMovePos = false;
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            else
            {
                playerCanMovePos = true;
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            if (freezeRot)
            {
                playerCanMoveRot = false;
            }
            else
            {
                playerCanMoveRot = true;
            }
        }

        public void MoveForwardMouse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //gameManager.UpdateCurrentInput(context);
                playerMoveValue = 1f;
            }
            if (context.canceled)
            {
                playerMoveValue = 0f;
            }
        }

        void Move()
        {
            if (useGamepad)
            {
                if (!isGrounded)
                {
                    rb.MovePosition(transform.position + (transform.forward * jumpDirection.magnitude) * playerSpeed * Time.deltaTime);
                }
                else
                {
                    rb.MovePosition(transform.position + (transform.forward * input.magnitude) * playerSpeed * Time.deltaTime);
                    previousPlayerMoveValue = playerMoveValue;
                }
            }
            else
            {
                if (!isGrounded)
                {
                    rb.MovePosition(transform.position + (transform.forward * playerMoveValue) * playerSpeed * Time.deltaTime);
                }
                else
                {
                    if (!inAttack)
                    {
                        if (!moveDuringAttack)
                        {
                            rb.MovePosition(transform.position + (transform.forward * playerMoveValue) * playerSpeed * Time.deltaTime);
                            moveDuringAttackTimer = moveDuringAttackDuration;
                        }
                    }
                    else
                    {
                        if (moveDuringAttack)
                        {
                            if (moveDuringAttackTimer > 0)
                            {
                                rb.MovePosition(transform.position + (transform.forward * previousPlayerMoveValue) * (playerSpeed * 1.5f) * Time.deltaTime);
                                moveDuringAttackTimer -= Time.deltaTime;
                            }
                            else
                            {
                                previousPlayerMoveValue = playerMoveValue;
                                moveDuringAttackTimer = moveDuringAttackDuration;
                            }
                        }
                    }
                }
            }
        }

        void Look()
        {
            if (useGamepad)
            {
                if (input != Vector3.zero && isGrounded && playerCanMoveRot)
                {
                    //Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45f,0));
                    //Vector3 skewedInput = matrix.MultiplyPoint3x4(input);

                    Vector3 relative = (transform.position /*+ skewedInput*/) - transform.position;
                    Quaternion rot = (Quaternion.LookRotation(relative, Vector3.up));

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, (turnSpeed * 360) * Time.deltaTime);
                }
            }
            else
            {
                if (isGrounded && !moveDuringAttack)
                {
                    cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(cameraRay, out cameraRayHit, mouseToWorldIgnoredLayers))
                    {
                        targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        Quaternion.LookRotation(targetPosition - transform.position), (turnSpeed * 360) * Time.deltaTime);
                    }
                }
            }
        }

        public void ResetPlayerTransform()
        {
            transform.position = playerInitialPos;
            transform.rotation = Quaternion.Euler(playerInitialRot);
        }

        #endregion
    }
}
