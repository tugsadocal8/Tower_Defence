using UnityEngine;
using UnityEngine.UI;

namespace PathCreation.Examples
{
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed;
        public float distanceTravelled;
        public float timer;
        public float health;
        public Image healthBar;
        public float monetaryValue;
        void Start()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }
        void Update()
        {
            if (pathCreator != null)
            {

                timer -= Time.deltaTime;
                if (timer < 0.0f)
                {
                    distanceTravelled += speed * Time.deltaTime;
                    transform.position = GetTargetLocation(Time.deltaTime);
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    if (transform.position.x > 23.0f || health <= 0)
                    {

                        Destroy(this.gameObject);
                        GameManager.Instance.EarnMoney(monetaryValue);
                        GameManager.Instance.RemoveEnemyList(this.gameObject);
                    }
                }
            }
        }
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
        public void TakeDamage(float damage)
        {
            health -= damage;
            healthBar.fillAmount -= 1 / health;
        }

        public Vector3 GetTargetLocation(float seconds)
        {
            float dist = distanceTravelled + speed * seconds;
            return pathCreator.path.GetPointAtDistance(dist, endOfPathInstruction);
        }

    }
}