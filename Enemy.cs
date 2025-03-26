using UnityEngine;

public class Enemy : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    
    }

    // Update is called once per frame
    void Update()
    {   
        EnemyMove(Vector2.down);
        
    }
    private void EnemyMove(Vector2 direction)
    {
        rigidbody2D.linearVelocity = moveSpeed * direction;
    }
}
