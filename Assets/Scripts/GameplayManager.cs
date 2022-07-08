namespace AFSInterview
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")] 
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject simpleTowerPrefab;
        [SerializeField] private GameObject towerDeluxePrefab;

        [Header("Settings")] 
        [SerializeField] private Vector2 boundsMin;
        [SerializeField] private Vector2 boundsMax;
        [SerializeField] private float enemySpawnRate;

        [Header("UI")] 
        [SerializeField] private GameObject enemiesCountText;
        [SerializeField] private GameObject scoreText;
        
        private List<Enemy> enemies;
        private float enemySpawnTimer;
        private int score;

        private void Awake()
        {
            enemies = new List<Enemy>();
            InvokeRepeating("SpawnEnemy", 0.0f, enemySpawnRate);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnTowerAtCursorPoint(simpleTowerPrefab);
            }
            if (Input.GetMouseButtonDown(1))
            {
                SpawnTowerAtCursorPoint(towerDeluxePrefab);
            }
        }

        private void SpawnEnemy()
        {
            var position = new Vector3(Random.Range(boundsMin.x, boundsMax.x), enemyPrefab.transform.position.y, Random.Range(boundsMin.y, boundsMax.y));
            
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
            enemy.OnEnemyDied += Enemy_OnEnemyDied;
            enemy.Initialize(boundsMin, boundsMax);

            enemies.Add(enemy);
            enemiesCountText.GetComponent<TextMeshProUGUI>().text = $"Enemies: {enemies.Count}";
        }

        private void Enemy_OnEnemyDied(Enemy enemy)
        {
            enemies.Remove(enemy);
            score++;
            if (scoreText) scoreText.GetComponent<TextMeshProUGUI>().text = $"Score: {score}";
        }

        private void SpawnTowerAtCursorPoint(GameObject towerPrefab)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Ground")))
            {
                var spawnPosition = hit.point;
                spawnPosition.y = towerPrefab.transform.position.y;

                SpawnTower(spawnPosition, towerPrefab);
            }
        }

        private void SpawnTower(Vector3 position, GameObject towerPrefab)
        {
            var tower = Instantiate(towerPrefab, position, Quaternion.identity).GetComponent<Tower>();
            tower.Initialize(enemies);
        }
    }
}