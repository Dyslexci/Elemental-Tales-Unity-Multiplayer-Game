﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class TerrainDamage : MonoBehaviourPun
{
    [SerializeField] private string damageType;
    [SerializeField] private int damage;
    private CharacterControllerPlayer1 playerController;
    private float hitLast = 0;
    private float hitDelay = 2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !collision.GetComponent<CharacterControllerPlayer1>().getElement().Equals(damageType))
        {
            if (Time.time - hitLast < hitDelay | (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true))
            {
                return;
            }
            playerController = collision.gameObject.GetComponent<CharacterControllerPlayer1>();
            playerController.DamageHealth(-damage);
            hitLast = Time.time;
        }
    }
}
