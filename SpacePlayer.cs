using UnityEngine;
using UnityEngine.InputSystem;

public class SpacePlayer : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 4;
    [SerializeField] GameObject spawnPosition;

    [SerializeField] float leftLimit = -5;
    [SerializeField] float rightLimit = 5;

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform.position = spawnPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = transform.position;

        if (playerPosition.x > rightLimit)
        {
            playerPosition.x = rightLimit;
        }
        else if (playerPosition.x < leftLimit)
        {
            playerPosition.x = leftLimit;
        }

        transform.position = playerPosition;
    }

    public void OnMove(InputValue inputValue)
    {
        float input = inputValue.Get<Vector2>().x;

        // Debug.Log("Key : " + input);

        if (input > 0)
        {
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        }
        else if (input < 0)
        {
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }

        if (Mathf.Abs(input) > 0)
        {
            rigidbody2D.linearVelocity = input * Vector2.right * moveSpeed;
        }
        else{
            rigidbody2D.linearVelocity = Vector2.zero;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
    }
}
