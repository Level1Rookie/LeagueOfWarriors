using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Dota.Core;

public class SkillShot : NetworkBehaviour
{
    [SerializeField] float speed = 8;
    [SerializeField] Vector3 direction;
    [SerializeField] float damage = 50;
    Vector3 startPos;
    Vector3 destroyPoint;

    [SyncVar]
    NetworkIdentity owner;

    // OnTriggerEnter IsSometimes called twice or more, this prevents it
    bool hasHit;


    #region Server

    [Server]
    public void ServerDealDamageTo(Health health, float damage)
    {
        health.ServerTakeDamage(damage);
    }

    [Server]
    public void ServerSetDirection(Vector3 startPos, Vector3 direction, float travelDist)
    {
        this.direction = new Vector3(direction.normalized.x, 0, direction.normalized.z);
        this.startPos = startPos;
        destroyPoint = startPos + this.direction.normalized * travelDist;
    }

    [Server]
    public void ServerSetOwner(NetworkIdentity owner)
    {
        this.owner = owner;
    }

    [Server]
    public void ServerSetSpeed(float speed)
    {
        this.speed = speed;
    }

    [Server]
    public void ServerSetDamage(float damage)
    {
        this.damage = damage;
    }

    [Server]
    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NetworkServer.Destroy(gameObject);
    }

    IEnumerator HitAfter(Health health, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NetworkServer.Destroy(gameObject);
        health.ServerTakeDamage(damage);
    }

    [ServerCallback]
    private void Update()
    {
        transform.forward = destroyPoint - transform.position;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, destroyPoint) < Vector3.Distance(startPos, transform.position))
        {
            StartCoroutine(DestroyAfter(0.1f));
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        NetworkIdentity otherIdentity = other.gameObject.GetComponent<NetworkIdentity>();
        Health health = other.GetComponent<Health>();
        if (health && !hasHit && otherIdentity != owner)
        {
            if (health.IsDead()) { return; }

            hasHit = true;
            StartCoroutine(HitAfter(health, 0.1f));
        }
    }
    #endregion
}