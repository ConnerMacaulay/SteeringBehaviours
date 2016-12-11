using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{

    public GameObject[] agentOBJ;
    public GameObject target;
   
   
    public float maxSpeed = 50.0f;
    public float maxSteering = 1.0f;
    public float distance;
    public float slowRadius = 30.0f;
    public float delay = 0.025f;
    public float delayTimer;
    public Vector2 desiredVelocity;
    public Vector2 steeringVelocity;
    public Vector2 randomVelocity;
         


    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    protected Rigidbody2D rigidBody2D;

    // Use this for initialization
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("HealthKit");
        
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called once per physics frame
    private void FixedUpdate()
    {
        CooperativeArbitration();
    }

    // Update is called once per game frame
    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }

        if (delayTimer < 0)
        {
            randomVelocity = Random.insideUnitCircle.normalized;
        }

        

        NearestAgent();
        SetDirection();

       
    }

    #region Helper functions
    /// <summary>
    /// Returns the mouse position in 2d space
    /// </summary>
    /// <returns>the mouse position in 2d space</returns>
    protected Vector2 GetMousePosition()
    {
        Vector3 temp = target.transform.position;
        return new Vector2(temp.x, temp.x);
    }

    /// <summary>
    /// Sets the direction of the triangle to the direction it is moving in to give the illusion it is turning. Trying taking out the function
    /// call in Update() to see what happens
    /// </summary>
    protected void SetDirection()
    {
        // Don't set the direction if no direction
        if (rigidBody2D.velocity.sqrMagnitude > 0.0f)
        {
            transform.up = new Vector3(rigidBody2D.velocity.x, rigidBody2D.velocity.y, 0.0f);
        }

        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 50);

    }


    protected Vector2 LimitSteering(Vector2 steeringVelocity)
    {
        // This limits the steering velocity to maxSteering. sqrMagnitude is used rather than magnitude as in magnitude a square root must be computed which is a slow operation.
        // By using sqrMagnitude and comparing with maxSteering squared we can around using the expensive square root operation.
        if (steeringVelocity.sqrMagnitude > maxSteering * maxSteering)
        {
            steeringVelocity.Normalize();
            steeringVelocity *= maxSteering;
        }
        return steeringVelocity;
    }

    protected Vector2 LimitVelocity(Vector2 velocity)
    {
        // This limits the velocity to max speed. sqrMagnitude is used rather than magnitude as in magnitude a square root must be computed which is a slow operation.
        // By using sqrMagnitude and comparing with maxSpeed squared we can around using the expensive square root operation.
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
        return velocity;
    }
    #endregion

    #region Behaviours: Feel free to add more to this section
    protected Vector2 Seek(Vector2 currentVelocity, Vector2 targetPosition)
    {
        // These 3 lines are equivalent to: desiredVelocity = Normalise(targetPosition - currentPoisition) * MaxSpeed
        Vector2 desiredVelocity = targetPosition - new Vector2(transform.position.x, transform.position.y);
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Calculate steering velocity
        Vector2 steeringVelocity = desiredVelocity - currentVelocity;

        // A way to control steering speed. This one is based on the mass of an agent (Can't devide a vector so thats why its 1/mass and then multiplied)
        // Could do this in many different ways: limited by degrees per second, propotionionally limited to current speed, or simply don't limit it and see what happens
        steeringVelocity *= (1.0f / rigidBody2D.mass);
        steeringVelocity = LimitSteering(steeringVelocity);

        // Useful for showing directions in scene view to visualise what the AI is doing
        Debug.DrawRay(transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(transform.position, currentVelocity);

        return steeringVelocity;
    }

    protected Vector2 Flee(Vector2 currentVelocity, Vector2 obsticlePosition)
    {
        // Notice for flee that this is identical to seek except the targetPosition and current position is swapped round
        // These 3 lines are equivalent to: desiredVelocity = Normalise(currentPoisition - targetPosition) * MaxSpeed
        Vector2 desiredVelocity = new Vector2(transform.position.x, transform.position.y) - obsticlePosition;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Calculate steering velocity
        Vector2 steeringVelocity = desiredVelocity - currentVelocity;

        // A way to control steering speed. This one is based on the mass of an agent (Can't devide a vector so thats why its 1/mass and then multiplied)
        // Could do this in many different ways: limited by degrees per second, propotionionally limited to current speed, or simply don't limit it and see what happens
        steeringVelocity *= (1.0f / rigidBody2D.mass);
        steeringVelocity = LimitSteering(steeringVelocity);

        // Useful for showing directions in scene view to visualise what the AI is doing
        Debug.DrawRay(transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(transform.position, currentVelocity);

        return steeringVelocity;
    }

    protected Vector2 Arrival(Vector2 currentVelocity, Vector2 targetPosition)
    {

        // These 3 lines are equivalent to: desiredVelocity = Normalise(targetPosition - currentPoisition) * MaxSpeed
        Vector2 desiredVelocity = targetPosition - new Vector2(transform.position.x, transform.position.y);
        distance = desiredVelocity.magnitude;

        if (distance <= slowRadius)
        {
            distance = distance / slowRadius;

            Debug.Log(distance);
            desiredVelocity.Normalize();
            desiredVelocity *= 1 * distance;
        }
        else
        {
            desiredVelocity *= 1;
        }

        // Calculate steering velocity
        Vector2 steeringVelocity = desiredVelocity - currentVelocity;

        // A way to control steering speed. This one is based on the mass of an agent (Can't devide a vector so thats why its 1/mass and then multiplied)
        // Could do this in many different ways: limited by degrees per second, propotionionally limited to current speed, or simply don't limit it and see what happens
        //steeringVelocity *= (1.0f / rigidBody2D.mass);
        steeringVelocity = LimitSteering(steeringVelocity);

        // Useful for showing directions in scene view to visualise what the AI is doing
        Debug.DrawRay(transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(transform.position, currentVelocity);



        return steeringVelocity;
    }

    protected Vector2 wander(Vector2 currentVelocity, Vector2 targetPosition)
    {
        // These 3 lines are equivalent to: desiredVelocity = Normalise(targetPosition - currentPoisition) * MaxSpeed

        desiredVelocity = randomVelocity;
        desiredVelocity.Normalize();

        desiredVelocity *= maxSpeed;

        // Calculate steering velocity
        steeringVelocity = desiredVelocity - currentVelocity;

        // A way to control steering speed. This one is based on the mass of an agent (Can't devide a vector so thats why its 1/mass and then multiplied)
        // Could do this in many different ways: limited by degrees per second, propotionionally limited to current speed, or simply don't limit it and see what happens
        steeringVelocity *= (1.0f / rigidBody2D.mass);
        steeringVelocity = LimitSteering(steeringVelocity);

        // Useful for showing directions in scene view to visualise what the AI is doing
        Debug.DrawRay(transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(transform.position, currentVelocity);
        delayTimer = delay;

        return steeringVelocity;
    }

    #endregion

    #region BehaviorManagement
    /// <summary>
    /// This is responsible for how to deal with multiple behaviours and selecting which ones to use. Please see this link for some decent descriptions of below:
    /// https://alastaira.wordpress.com/2013/03/13/methods-for-combining-autonomous-steering-behaviours/
    /// Remember some options for choosing are:
    /// 1 Finite state machines which can be part of the steering behaviours or not (Not the best approach but quick)
    /// 2 Weighted Truncated Sum
    /// 3 Prioritised Weighted Truncated Sum
    /// 4 Prioritised Dithering
    /// 5 Context Behaviours: https://andrewfray.wordpress.com/2013/03/26/context-behaviours-know-how-to-share/
    /// 6 Any other approach you come up with
    /// </summary>
    protected void CooperativeArbitration()
    {
        // Get a new target position which is just the mouse position for now. This could be other game agents - think about the Dog, cat, mouse example
        Vector2 targetPosition = GetMousePosition();


        // Currently just choosing seeking so no arbitration is happening
        Vector2 currentVelocity = rigidBody2D.velocity;
        //currentVelocity += Seek(currentVelocity, targetPosition);

        // Uncomment the next line to activate flee. See what happens when you do both behaviours and observe why Cooperative Arbitration is needed
        //currentVelocity += Flee(currentVelocity, avoidPosition);

        currentVelocity += Arrival(currentVelocity, targetPosition);

        //currentVelocity += wander(currentVelocity, targetPosition);

        currentVelocity = LimitVelocity(currentVelocity);
        rigidBody2D.velocity = currentVelocity;
    }
    #endregion

    public void NearestAgent()
    {
        agentOBJ = GameObject.FindGameObjectsWithTag("Agent");


        float dist = Mathf.Infinity;
        foreach (GameObject agent in agentOBJ)
        {
            Vector2 diff = transform.position - agent.transform.position;
            float curDis = diff.sqrMagnitude;
            if (curDis < dist)
            {
                target = agent;
                dist = curDis;
            }
           
        }
    }
}