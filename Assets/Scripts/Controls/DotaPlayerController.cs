﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Dota.Core;
using Dota.Combat;
using Dota.Movement;

namespace Dota.Controls
{
    public class DotaPlayerController : NetworkBehaviour
    {

        [SerializeField] DotaMover mover = null;
        [SerializeField] DotaFighter fighter = null;
        [SerializeField] Health health = null;
        [SerializeField] AbilityStore abilityStore = null;

        private void Update()
        {
            if (!isLocalPlayer) { return; }

            if (health.IsDead()) { return; }

            // Use Abilities
            if (Input.GetKeyDown(KeyCode.Q))
            {
                abilityStore.Use(0, gameObject);
            }
            

            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(GetMouseRay(), out RaycastHit hit, Mathf.Infinity))
                {
                    GameObject go = hit.collider.gameObject;
                    if(go == gameObject) { return; }

                    if (fighter.CanAttack(go))
                    {
                        fighter.CmdTryAttack(go);
                        return;
                    }
                    else
                    {
                        fighter.CmdStopAttack();
                        mover.CmdMoveTo(hit.point);
                    }
                }
            }

        }



        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}