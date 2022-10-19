using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class Movement2D : MonoBehaviour
{
    [SerializeField] private InputAction movementInput;
    private Vector2 movement;
    private Rigidbody rb;

    private void OnEnable()
    {
        movementInput.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        print(movement);

        movement = movementInput.ReadValue<Vector2>().normalized;

        if(movement.x != 0)
        {
            transform.position += new Vector3(0, 0, -movement.x) * Time.deltaTime * 10;
        }

        if(movement.y != 0)
        {
            rb.AddForce(new Vector3(0, 100,0));
        }
    }
}
