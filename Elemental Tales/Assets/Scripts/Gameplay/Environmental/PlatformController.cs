using System.Collections.Generic;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.2.7
 *
 *    Provides logic for platforms the player can jump up through and drop down through, as well as platforms which can move between an indeterminate number of waypoints.
 *    Known issues: When moving horizontally, a player wallclimbing on the platform tends to bounce around uncontrollably. Possibly needs to be fixed by always extending raycasts
 *	  on the horizontal axis.
 */

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;
    public bool isSwitchable;
    public Switch switchToggle;

    [Header("Waypoints")]
    public Vector3[] localWaypoints;

    private Vector3[] globalWaypoints;

    [Header("Movement Variables")]
    public float speed;

    public bool cyclic;
    public float waitTime;

    [Range(0, 2)]
    public float easeAmount;

    private int fromWaypointIndex;
    private float percentBetweenWaypoints;
    private float nextMoveTime;

    private bool isMoving;
    private bool wasMoving;
    public AudioSource platformAudio;

    private List<PassengerMovement> passengerMovement;
    private Dictionary<Transform, CharacterControllerRaycast> passengerDictionary = new Dictionary<Transform, CharacterControllerRaycast>();

    /// <summary>
    /// Initialises the platform waypoints and raycasting logic.
    /// </summary>
    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Updates raycasting logic and sets velocity based off the current destination/location, as well as moving any passengers
    /// </summary>
    private void Update()
    {
        if (isSwitchable && !switchToggle.getLeverState())
            return;

        wasMoving = isMoving;
        UpdateRaycastOrigins();
        Vector3 velocity = CalculatePlatformMovement();
        isMoving = (velocity != new Vector3(0, 0, 0)) ? true : false;
        PlayAudio();
        CalculatePassengerMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    private void PlayAudio()
    {
        if (!wasMoving && isMoving)
            platformAudio.Play(0);
        if (wasMoving && !isMoving)
            platformAudio.Stop();
    }

    /// <summary>
    /// Slows down the platform as it approaches its destination, returning the new movement as a float.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    /// <summary>
    /// Calculates platform movement based off its current and next locations, returning the movement as a vector 3.
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    /// <summary>
    /// Logic for moving passengers with the platform
    /// </summary>
    /// <param name="beforeMovePlatform"></param>
    private void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<CharacterControllerRaycast>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    /// <summary>
    /// Logic for calculating how the passengers should move based off the movement of the platform
    /// </summary>
    /// <param name="velocity"></param>
    private void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Contains variables for the movement of the passengers standing on the platform.
    /// </summary>
    private struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    /// <summary>
    /// Draws the raycasts of the platform in the editor for debugging purposes.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}