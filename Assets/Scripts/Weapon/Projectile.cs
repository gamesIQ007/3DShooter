using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Снаряд
    /// </summary>
    public class Projectile : Entity
    {
        /// <summary>
        /// Скорость
        /// </summary>
        [SerializeField] private float velocity;

        /// <summary>
        /// Время жизни снаряда
        /// </summary>
        [SerializeField] private float lifeTime;

        /// <summary>
        /// Наносимый урон
        /// </summary>
        [SerializeField] private int damage;

        /// <summary>
        /// Массив эффектов
        /// </summary>
        [SerializeField] private ImpactEffect[] impactEffectPrefabs;

        /// <summary>
        /// Таймер
        /// </summary>
        private float timer;

        /// <summary>
        /// Родитель снаряда
        /// </summary>
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
                if (hit.collider.isTrigger == false)
                {
                    Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                    int targetMaterial = ((int)hit.collider.transform.root.GetComponent<ObjectMaterial>().CurrentObjectMaterial);

                    if (dest != null && dest != parent)
                    {
                        dest.ApplyDamage(damage, parent);
                    }

                    OnProjectileLifeEnd(hit.collider, hit.point, hit.normal, targetMaterial);
                }
            }

            transform.position += new Vector3(step.x, step.y, step.z);
        }


        /// <summary>
        /// Задать родителя снаряда
        /// </summary>
        /// <param name="parent">Родительский объект</param>
        public void SetParentShooter(Destructible parent)
        {
            this.parent = parent;
        }


        /// <summary>
        /// Действие при конце жизни снаряда
        /// </summary>
        /// <param name="col">Коллайдер</param>
        /// <param name="pos">Позиция</param>
        /// <param name="normal">Нормаль</param>
        /// <param name="targetMaterial">Материал цели</param>
        private void OnProjectileLifeEnd(Collider col, Vector3 pos, Vector3 normal, int targetMaterial)
        {
            if (col is CharacterController) return;

            if (impactEffectPrefabs[targetMaterial] != null)
            {
                ImpactEffect impact = Instantiate(impactEffectPrefabs[targetMaterial], pos, Quaternion.LookRotation(normal));
                impact.transform.SetParent(col.transform);
            }

            Destroy(gameObject);
        }
    }
}