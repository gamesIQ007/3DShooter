using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Солдат пришельцев
    /// </summary>
    public class AlienSoldier : Destructible, ISoundListener
    {
        /// <summary>
        /// Оружие
        /// </summary>
        [SerializeField] private Weapon weapon;

        /// <summary>
        /// Риг разброса
        /// </summary>
        [SerializeField] private SpreadShootRig spreadShootRig;

        /// <summary>
        /// ИИ солдата
        /// </summary>
        [SerializeField] private AIAlienSoldier aiAlienSoldier;

        /// <summary>
        /// Дистанция слуха
        /// </summary>
        [SerializeField] private float hearingDistance;


        /// <summary>
        /// Выстрел
        /// </summary>
        /// <param name="target">Цель выстрела</param>
        public void Fire(Vector3 target)
        {
            if (weapon.CanFire == false) return;

            weapon.FirePointLookAt(target);
            weapon.Fire();

            spreadShootRig.Spread();
        }


        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();
        }


        /// <summary>
        /// Класс для состояния объекта
        /// </summary>
        [System.Serializable]
        public class AIAlienState
        {
            /// <summary>
            /// Расположение
            /// </summary>
            public Vector3 Position;
            /// <summary>
            /// Здоровье
            /// </summary>
            public int HitPoints;
            /// <summary>
            /// Поведение
            /// </summary>
            public int Behaviour;

            public AIAlienState() { }
        }

        /// <summary>
        /// Услышать
        /// </summary>
        /// <param name="distance">Расстояние</param>
        public void Heard(float distance)
        {
            if (distance <= hearingDistance)
            {
                aiAlienSoldier.OnHeard();
            }
        }

        public override string SerializableState()
        {
            AIAlienState s = new AIAlienState();

            s.Position = transform.position;
            s.HitPoints = HitPoints;
            s.Behaviour = (int) aiAlienSoldier.Behaviour;

            return JsonUtility.ToJson(s);
        }

        public override void DeserializeState(string state)
        {
            AIAlienState s = JsonUtility.FromJson<AIAlienState>(state);

            aiAlienSoldier.SetPosition(s.Position);
            SetHitPoint(s.HitPoints);
            aiAlienSoldier.Behaviour = (AIAlienSoldier.AIBehaviour) s.Behaviour;
        }
    }
}