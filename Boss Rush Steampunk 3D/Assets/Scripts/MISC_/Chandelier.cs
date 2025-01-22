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
    public float failPos = -3.5f, failRange = 1.5f;     //  The position and range in which we fail and hit the heroes
    public float targetPos = 3.5f, targetRange = 0.5f;  //  The position and range in which we succeed and hit the heroes
    public float critSpeed = 2, critSpeedRange = 0.5f;  //  The horizontal speed needed for the chandelier to crit
    private Vector2 prevVel;    //  Save the velocity from last frame to figure out when the chandelier hits the bottom of its arc
    private Rigidbody2D body;   //  A reference to the Rigidbody2D
    public Transform hinge;     //  The object that the chandelier hangs from
    private LineRenderer line;  //  A reference to the LineRenderer
    public bool showColors = false;
    public bool cut = false;
    public Color missColor = Color.white;
    public Color failColor = Color.red;
    public Color targetColor = Color.green;
    public Color critColor = Color.yellow;
    private SpriteRenderer renderer;
    private Vector3 endPos = Vector3.zero;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        renderer = GetComponent<SpriteRenderer>();
        prevVel = body.velocity;
        endPos = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        if(result == -1)
        {
            //  Update the rope
            line.SetPosition(0, transform.position + Vector3.up * 0.7f);
            line.SetPosition(1, hinge.position);

            if (showColors)
            {
                int tempResult = -1;

                //  Calculate how far we'll go if stopped now.
                float h = transform.position.y;
                float vx = body.velocity.x;
                float vy = body.velocity.y;
                float a = Physics2D.gravity.y;
                //  Thank you desmos (https://www.desmos.com/calculator/ktxyzexqiu)
                float dist = -vx * (vy + Mathf.Sqrt(vy * vy - 2 * a * h)) / a;
                endPos.x = transform.position.x + dist;

                //  Check if we'll land in the target range
                if (Mathf.Abs(endPos.x - targetPos) <= targetRange)
                {
                    //  If so, check if our speed is in the crit speed range
                    if (Mathf.Abs(vx - critSpeed) <= critSpeedRange)
                    {
                        //  If so, set colour to crit colour
                        renderer.color = critColor;
                        tempResult = 3;
                    }
                    else
                    {
                        //  If not, set colour to target colour
                        renderer.color = targetColor;
                        tempResult = 2;
                    }
                }
                else if (Mathf.Abs(endPos.x - failPos) <= failRange)
                {
                    //  If we didn't hit, check if we're hitting the player, and if so, set the colour to the fail colour
                    renderer.color = failColor;
                    tempResult = 1;
                }
                else
                {
                    //  If we're not hitting anything, set the colour to normal
                    renderer.color = missColor;
                    tempResult = 0;
                }
                if(cut)
                {
                    result = tempResult;
                }
            }
            else if(!cut)
            {
                renderer.color = missColor;
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(endPos, 0.5f);
    }
}
