using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 6.5f;

    private Rigidbody2D rb;
    private Transform playerTrans;
    private Animator animator;
    private Vector2 currentPos;
    private Vector2 targetPos;
    private Vector2 inputVector;
    private Vector2 movementVector;
    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTrans = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Checks for player input
        horizontalInput = 0;
        verticalInput = 0;
        if (Input.GetKey(PlayerPrefData.Up)) verticalInput += 1;
        if (Input.GetKey(PlayerPrefData.Down)) verticalInput -= 1;
        if (Input.GetKey(PlayerPrefData.Left)) horizontalInput -= 1;
        if (Input.GetKey(PlayerPrefData.Right)) horizontalInput += 1;

        //Sets appropriate animation depending on movement
        if (horizontalInput > 0) animator.SetInteger("cardinalDirection", 1);
        else if (horizontalInput < 0) animator.SetInteger("cardinalDirection", 3);
        else if (verticalInput > 0) animator.SetInteger("cardinalDirection", 0);
        else if (verticalInput < 0) animator.SetInteger("cardinalDirection", 2);

        //Sets the current position of the player
        currentPos = playerTrans.position;

        //Makes sure to only run this if there is input from player
        if (horizontalInput != 0 || verticalInput != 0)
        {
            //Calculates player movement based on input
            CalculateMovement();
            //Moves the character
            rb.MovePosition(targetPos);
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetFloat("velocity", 0);
        }
    }

    private void CalculateMovement()
    {
        inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1); //Makes sure diagonal movement is not faster
        movementVector = inputVector * movementSpeed;
        targetPos = currentPos + movementVector * Time.fixedDeltaTime;

        animator.SetFloat("velocity", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); //Sets the animation as moving
    }
}
