using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Управление стреляющим транспортом
    /// </summary>
    public class ShootingVehicleInputControl : VehicleInputControl
    {
        /// <summary>
        /// Стрельба игрока
        /// </summary>
        [SerializeField] private PlayerShooter playerShooter;

        /// <summary>
        /// Точка прицеливания
        /// </summary>
        [SerializeField] private Transform aimPoint;


        protected override void Update()
        {
            base.Update();

            aimPoint.position = playerShooter.Camera.transform.position + playerShooter.Camera.transform.forward * 30;

            if (Input.GetMouseButton(0))
            {
                playerShooter.Shoot();
            }
        }
    }
}