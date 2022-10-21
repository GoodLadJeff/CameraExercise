using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Movement2D : MonoBehaviour
{
    [SerializeField] private InputAction movementInput;
    private Vector2 velocity = Vector3.zero;
    private Vector2 acceleration = new Vector2(5,0);
    private Rigidbody rb;
    private float distToGround;

    private void OnEnable()
    {
        movementInput.Enable();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0, -velocity.x) * Time.deltaTime * 10;

        if (movementInput.ReadValue<Vector2>().x != 0)
        {
            velocity.x += movementInput.ReadValue<Vector2>().normalized.x * acceleration.x * Time.deltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -3, 3);
        }
        else
        {
            velocity.x -= velocity.normalized.x * acceleration.x * Time.deltaTime;
        }

        if(movementInput.ReadValue<Vector2>().y != 0 && IsGrounded())
        {
            rb.AddForce(new Vector3(0, 10, 0));
        }
    }
}