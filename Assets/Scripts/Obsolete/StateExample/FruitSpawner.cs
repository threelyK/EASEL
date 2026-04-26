
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateExample
{
    public class FruitSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _fruitPrefabs;
        [SerializeField] private float _radius;
        private Vector3 _position;
        private float _y;
        
        [SerializeField] private float _spawnInterval = 3f;

        private void Start()
        {
            _position = transform.position;
            _y = _position.y;
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(spawnFruit), 0f, _spawnInterval);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(spawnFruit));
        }

        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        public void spawnFruit()
        {
            Instantiate(_fruitPrefabs[Random.Range(0, _fruitPrefabs.Length)], GetRandomPosition(), GetRandomRotation());
        }
        
        private Vector3 GetRandomPosition()
        {
            return new Vector3(_position.x + Random.Range(-_radius, _radius), _y, _position.z + Random.Range(-_radius, _radius));
        }

        private Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
}
