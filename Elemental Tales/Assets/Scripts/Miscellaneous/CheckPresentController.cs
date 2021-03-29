using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.4.0
 *    @version 1.0.1
 *
 *    Class purpose built to be extended by other classes which need to check for player presence in a variety of ways.
 */

public class CheckPresentController : MonoBehaviourPun
{
    /// <summary>
    /// Checks for player presence based on a passed in position, length, height, angle and layer, and returns true / false.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="lengthX"></param>
    /// <param name="lengthY"></param>
    /// <param name="angle"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public bool CheckPresentBox(Transform pos, float lengthX, float lengthY, float angle, LayerMask layer)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, new Vector2(lengthX, lengthY), angle, layer);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Player" && c.gameObject.GetPhotonView().IsMine)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks for player presence based on a passed in position, radius and layer, and returns true / false.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="radius"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public bool CheckPresentCircle(Transform pos, float radius, LayerMask layer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Player" && c.gameObject.GetPhotonView().IsMine)
            {
                return true;
            }
        }

        return false;
    }
}