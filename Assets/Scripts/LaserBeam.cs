using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserBeam
{
    GameObject laserObj;
    LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();
    LayerMask layerThatIgnoresLaser;
    int laserDamage = 1;
    private int maxDistance = 100;

    public LaserBeam(Vector3 pos, Vector3 dir, Material laserMaterial, LayerMask layerThatIgnoresLaser)
    {
        laser = new LineRenderer();
        laserObj = new GameObject
        {
            name = "Laser Beam"
        };

        laser = laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        laser.material = laserMaterial;
        laser.endWidth = laser.startWidth = 0.2f;
        laser.endColor = laser.startColor = Color.green;
        this.layerThatIgnoresLaser = layerThatIgnoresLaser;

        CastRay(pos, dir, laser, null);
    }

    void CastRay(Vector2 pos, Vector2 dir, LineRenderer laser, Collider2D lastHitCollider)
    {
        laserIndices.Add(pos);

        Ray2D ray = new Ray2D(pos, dir);
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, maxDistance, ~layerThatIgnoresLaser);
        if (lastHitCollider != null)
        {
            lastHitCollider.enabled = true;
        }
        if (hit)
        {
            CheckHit(hit, dir, laser);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(maxDistance));
            UpdateLaser();
        }
    }

    void UpdateLaser()
    {
        laser.positionCount = laserIndices.Count;
        for (int i = 0; i < laserIndices.Count; i++)
        {
            laser.SetPosition(i, laserIndices[i]);
        }
    }

    void CheckHit(RaycastHit2D hitInfo, Vector3 direction, LineRenderer laser)
    {
        if (hitInfo.collider.gameObject.CompareTag("Reflective") && laserIndices.Count < 20)
        {
            hitInfo.collider.enabled = false;
            Vector2 newPos = hitInfo.point;
            Vector2 newDir = Vector2.Reflect(direction, hitInfo.normal);

            CastRay(newPos, newDir, laser, hitInfo.collider);
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLaser();
            CheckEnemy(hitInfo);
            CheckSelfDamage(hitInfo);
        }
    }

    void CheckEnemy(RaycastHit2D hitInfo) {
        Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
        if (enemy) {
            enemy.DealDamage(laserDamage);
        }
    }

    void CheckSelfDamage(RaycastHit2D hitInfo) {
        Shooter shooter = hitInfo.collider.GetComponent<Shooter>();
        if (shooter) {
            shooter.DealSelfDamage(laserDamage);
        }
    }
}
