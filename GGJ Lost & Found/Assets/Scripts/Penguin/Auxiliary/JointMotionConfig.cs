using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JointMotionConfig{
    public ConfigurableJointMotion angularXMotion, angularYMotion, angularZMotion;
    public float angularXLimit, angularYLimit, angularZLimit;

    public void ApplyTo(ref ConfigurableJoint joint) {
        joint.angularXMotion = angularXMotion;
        joint.angularYMotion = angularYMotion;
        joint.angularZMotion = angularZMotion;

        var softJointLimit = new SoftJointLimit();

        softJointLimit.limit = angularXLimit / 2;
        joint.highAngularXLimit = softJointLimit;

        softJointLimit.limit = -softJointLimit.limit;
        joint.lowAngularXLimit = softJointLimit;

        softJointLimit.limit = angularYLimit;
        joint.angularYLimit = softJointLimit;

        softJointLimit.limit = angularZLimit;
        joint.angularZLimit = softJointLimit;
    }
}
