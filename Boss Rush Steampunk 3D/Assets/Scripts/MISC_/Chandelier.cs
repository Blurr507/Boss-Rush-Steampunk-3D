using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chandelier : MonoBehaviour
{
    public float height = 1;    //  The height that the chandelier should swing to
    public int dir = 1;         //  1 for right, -1 for left
    private Vector2 prevVel;    //  Save the velocity from last frame to figure out when the chandelier hits the bottom of its arc
    private Rigidbody2D body;   //  A reference to the Rigidbody2D
    public Transform hinge;    //  The object that the chandelier hangs from

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        prevVel = body.velocity;
    }

    void Update()
    {
        //  If we used to be going down, and we're now going up, we must have hit the bottom of the pendulum
        if(prevVel.y < 0 && body.velocity.y >= 0 || prevVel == Vector2.zero && body.velocity == Vector2.zero)
        {
            //  Since we're at the bottom, set the velocity to correct any rounding errors caused by the physics engine
            dir *= -1;
            body.velocity = Vector2.right * dir * Mathf.Sqrt(-Physics2D.gravity.y * height * 2); 
        }
        prevVel = body.velocity;

        if(transform.position.y <= 0)
        {
            foreach(Enemy enemy in FindObjectsOfType<Enemy>())
            {
                if(!enemy.effects.Contains("burning"))
                {
                    enemy.effects.Add("burning");
                    enemy.UpdateAnimatorBools();
                }
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
