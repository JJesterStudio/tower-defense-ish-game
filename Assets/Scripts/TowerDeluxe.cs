namespace AFSInterview
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TowerDeluxe : Tower
    {
        [SerializeField] protected int burstSize;
        [SerializeField] protected float burstRate;

        private Vector3 targetPosition;

        public override void Initialize(IReadOnlyList<Enemy> enemies)
        {
            base.Initialize(enemies);
            ChangeInitialFiringRate(burstRate * burstSize);
        }

        protected override void Shoot()
        {
            StartCoroutine(ShootBurst());
        }

        protected Vector3 FutureTargetPosition()
        {
            if (targetEnemy != null)
            {
                targetPosition = targetEnemy.transform.position + targetEnemy.GetDirection();
            }
            return targetPosition;
        }

        protected IEnumerator ShootBurst()
        {
            for (int i = 0; i < burstSize; i++)
            {
                var arrow = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Arrow>();
                arrow.Initialize(FutureTargetPosition());
                yield return new WaitForSeconds(burstRate);
            }
        }
    }
}
