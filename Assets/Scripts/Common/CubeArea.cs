﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Shooter3D
{
    /// <summary>
    /// Область кубической формы
    /// </summary>
    public class CubeArea : MonoBehaviour
    {
        /// <summary>
        /// Вектор, задающий область
        /// </summary>
        [SerializeField] private Vector3 area;


        /// <summary>
        /// Получить случайную точку внутри области
        /// </summary>
        /// <returns>Точка внутри области</returns>
        public Vector3 GetRandomInsideZone()
        {
            Vector3 result = transform.position;

            result.x += Random.Range(-area.x / 2, area.x / 2);
            result.y += Random.Range(-area.y / 2, area.y / 2);
            result.z += Random.Range(-area.z / 2, area.z / 2);

            return result;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.green;
            Handles.DrawWireCube(transform.position, area);
        }
#endif

    }
}