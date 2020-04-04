using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Components
    {
        [ExecuteInEditMode]
        public class BillboardingComponent : MonoBehaviour
        {
            #region Inspector Variables
            [SerializeField]
            private bool isUpdateEveryFrame = true;
            [SerializeField]
            private UnityEngine.Vector3 angleOffSet = UnityEngine.Vector3.zero;

            [Header("References")]
            [SerializeField]
            private Camera mainCamera;
            #endregion

            #region MonoBehaviour Functions
            private void Awake()
            {
                this.mainCamera = Camera.main;
            }

            private void LateUpdate()
            {
                if (this.isUpdateEveryFrame)
                {
                    UnityEngine.Vector3 directionToLookAt = this.transform.position - Camera.main.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(directionToLookAt);
                    this.transform.eulerAngles = rotation.eulerAngles + this.angleOffSet;
                }
            }
            #endregion
        }
    }
}
