using UnityEngine;

namespace Shooter3D
{
    public class Drone : Destructible
    {
        [Header("Main")]
        [SerializeField] private Transform mainMesh;

        [Header("View")]
        [SerializeField] private GameObject[] meshComponents;
        [SerializeField] private Renderer[] meshRenderers;
        [SerializeField] private Material[] deadMaterials;

        [Header("Movement")]
        [SerializeField] private float hoverAmplitude;
        [SerializeField] private float hoverSpeed;

        protected override void Update()
        {
            base.Update();

            mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime, 0);
        }

        protected override void OnDeath()
        {
            eventOnDeath?.Invoke();

            enabled = false;

            for (int i = 0; i < meshComponents.Length; i++)
            {
                if (meshComponents[i].GetComponent<Rigidbody>() == null)
                {
                    meshComponents[i].AddComponent<Rigidbody>();
                }
            }

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = deadMaterials[i];
            }
        }
    }
}