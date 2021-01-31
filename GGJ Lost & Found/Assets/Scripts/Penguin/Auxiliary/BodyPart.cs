using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyPart : MonoBehaviour{

    public string bodyPartName;

    [SerializeField] private List<ConfigurableJoint> _joints;
    private List<JointDriveConfig> XjointDriveConfigs;
    private List<JointDriveConfig> YZjointDriveConfigs;

    [SerializeField] private float _strengthScale = 1;
    public float StrengthScale { get { return _strengthScale; } }


    public BodyPart(string name, List<ConfigurableJoint> joints) {
        bodyPartName = name;
        _joints = joints;
    }

    public void Init() {
        XjointDriveConfigs = new List<JointDriveConfig>();
        YZjointDriveConfigs = new List<JointDriveConfig>();

        foreach (ConfigurableJoint joint in _joints) {
            XjointDriveConfigs.Add((JointDriveConfig)joint.angularXDrive);
            YZjointDriveConfigs.Add((JointDriveConfig)joint.angularYZDrive);
        }

        _strengthScale = 1;
    }

    public void SetStrengthScale(float scale) {
        for (int i = 0; i < _joints.Count; i++) {
            _joints[i].angularXDrive = (JointDrive)(XjointDriveConfigs[i] * scale);
            _joints[i].angularYZDrive = (JointDrive)(YZjointDriveConfigs[i] * scale);
        }

        _strengthScale = scale;
    }
}
