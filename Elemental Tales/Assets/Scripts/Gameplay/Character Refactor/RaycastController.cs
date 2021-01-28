﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
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

    private void Awake()
    {
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

    // Initialises with the collider and starts the raycasting
    public virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	// Updates the raycasting locations
	public void UpdateRaycastOrigins() { 
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	// Calculates the needed spacing between raycasts based off the number of rays and the size of the object
	public void CalculateRaySpacing()
	{
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
