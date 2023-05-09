using UnityEngine;

namespace Shooter3D
{
    public class Projectile : Entity
    {
        [SerializeField] private float velocity;

        [SerializeField] private float lifeTime;

        [SerializeField] private int damage;

        [SerializeField] private ImpactEffect[] impactEffectPrefabs;

        private float timer;

        private Destructible parent;
        public Destructible Parent => parent;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > lifeTime)
            {
                Destroy(gameObject);
            }

            float stepLength = velocity * Time.deltaTime;
            Vector3 step = transform.forward * stepLength;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, stepLength))
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                int targetMaterial = ((int)hit.collider.transform.root.GetComponent<ObjectMaterial>().CurrentObjectMaterial);

                if (dest != null && dest != parent)
                {
                    dest.ApplyDamage(damage);
                }

                OnProjectileLifeEnd(hit.collider, hit.point, hit.normal, targetMaterial);
            }

            transform.position += new Vector3(step.x, step.y, step.z);
        }

        public void SetParentShooter(Destructible parent)
        {
            this.parent = parent;
        }

        private void OnProjectileLifeEnd(Collider col, Vector3 pos, Vector3 normal, int targetMaterial)
        {
            if (impactEffectPrefabs[targetMaterial] != null)
            {
                ImpactEffect impact = Instantiate(impactEffectPrefabs[targetMaterial], pos, Quaternion.LookRotation(normal));
                impact.transform.SetParent(col.transform);
            }

            Destroy(gameObject);
        }
    }
}