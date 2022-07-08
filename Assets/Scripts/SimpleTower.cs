namespace AFSInterview
{
    using UnityEngine;

    public class SimpleTower : Tower
    {
        protected override void Shoot()
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.Initialize(targetEnemy.gameObject);
        }
    }
}
