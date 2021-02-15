using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CalculationTools
{
    public class CalcUtils
    {
        public static float GetRotationAngle(Vector3 vTarget, Vector3 vPos)
        {
            if (vTarget != Vector3.zero)
            {
                float fVal = GetLookAtValue(vTarget, vPos);

                fVal *= Mathf.Rad2Deg;

                return fVal;
            }

            return 0.0f;
        }

        public static float GetLookAtValue(Vector3 vTarget, Vector3 vPos)
        {
            Vector3 vTargetDir = (vTarget - vPos);
            float fDotResult = Vector3.Dot(vTargetDir, Vector3.forward);

            return Mathf.Acos((fDotResult) / (vTargetDir.magnitude * Vector3.forward.magnitude)) * Mathf.Sin(vTargetDir.x);
        }

        public static Quaternion RotateTowardsTarget(Vector3 vTarget, Vector3 vPos)
        {
            return Quaternion.Euler(0f, GetRotationAngle(vTarget, vPos), 0f);
        }

        public static Quaternion UpdateProjectileFacingRotation(Vector3 vTarget, Vector3 vPos)
        {
            return RotateTowardsTarget(vTarget, vPos);
        }
    }
}