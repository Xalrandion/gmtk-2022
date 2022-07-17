using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class ProjectileSpawner : MonoBehaviour
{
    public Transform? projectilePrefab;
    public Transform? projectileSpawn;
    [HideInInspector] public Transform? projectile;
    private Vector3 targetPosition;
    public float travelTime;
    private float travelStart;
    void Start()
    {
        if ( projectilePrefab == null) {
            enabled = false;
            Debug.LogError("missing projectilePrefeab");
        }
        if ( projectileSpawn == null) {
            enabled = false;
            Debug.LogError("missing projectileSpawn");
        }
    }

    void Update()
    {
        if (projectile != null) {
            projectile.position = Vector3.Lerp(projectileSpawn!.position, targetPosition, (Time.time - travelStart) / travelTime);
            if (Time.time - travelStart > travelTime) {
                Destroy(projectile.gameObject);
                projectile = null;
            }
        }
    }

    public void Spawn(Vector3 targetPosition)
    {
        if (projectile != null) {
            Destroy(projectile.gameObject);
            projectile = null;
        }
        this.targetPosition = targetPosition;
        projectile = Instantiate(projectilePrefab! , projectileSpawn!.position, projectileSpawn!.rotation);
        projectile.forward = targetPosition - projectileSpawn.position;
        travelStart = Time.time;
    }
}
