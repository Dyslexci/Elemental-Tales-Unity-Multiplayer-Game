using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Provides raycasting logic needed for the custom physics work. Determines raycast locations and orientation, and is designed to be extended by controller classes.
 */

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviourPunCallbacks
{
	[Header("Layer Masks")]
	public LayerMask collisionMask;

	[Header("Raycast Variables")]
	public const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing;
	[HideInInspector]
	public BoxCollider2D boxCollider;
	[HideInInspector]
	public RaycastOrigins raycastOrigins;

	/// <summary>
	/// On object awake, sets the collider of the object and calculates the spacing of the raycast lines.
	/// </summary>
    private void Awake()
    {
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	/// <summary>
	/// On object start, sets the collider of the object and calculates the spacing of the raycast lines. A backup to combat the awake method sometimes not triggering correctly.
	/// </summary>
	public virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	/// <summary>
	/// Updates the raycasting origin locations.
	/// </summary>
	public void UpdateRaycastOrigins() { 
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	/// <summary>
	/// Calculates the needed spacing between raycasts based off the number of rays and the size of the object.
	/// </summary>
	public void CalculateRaySpacing()
	{
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	/// <summary>
	/// Stores the origins of the raycasts.
	/// </summary>
	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
