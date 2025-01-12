using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chandelier : MonoBehaviour
{
    public float height = 1;    //  The height that the chandelier should swing to
    public int dir = 1;         //  1 for right, -1 for left
    public int result = -1;     //  An int to specify how you hit. -1 for unfinished, 0 for nothing, 1 for player, 2 for enemies (normal), 3 for enemies (critical)
    public int critDamage = 10; //  The amount of fire damage done if the hit crits
    private Vector2 prevVel;    //  Save the velocity from last frame to figure out when the chandelier hits the bottom of its arc
    private Rigidbody2D body;   //  A reference to the Rigidbody2D
    public Transform hinge;     //  The object that the chandelier hangs from
    private LineRenderer line;  //  A reference to the LineRenderer

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        prevVel = body.velocity;
    }

    void Update()
    {
        if(result == -1)
        {
            line.SetPosition(0, transform.position + Vector3.up * 0.55f);
            line.SetPosition(1, hinge.position);
        }
        else if(line.enabled)
        {
            line.enabled = false;
        }

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
            switch(result)
            {
                case 1:
                    foreach(CanSelect hero in BattleStateManager.me.heroes)
                    {
                        hero.GetComponent<Health>().AddEffect("burning");
                    }
                    break;
                case 2:
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                    {
                        enemy.AddEffect("burning");
                    }
                    break;
                case 3:
                    foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                    {
                        enemy.AddEffect("burning");
                        enemy.SubtractHealth(critDamage, 1);
                    }
                    break;
            }
            
            Destroy(transform.parent.gameObject);
        }
    }
}
