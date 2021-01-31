using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JointDriveConfig{

    [SerializeField] private float _positionSpring, _positionDamper, _maximumForce;
    public float PositionSpring { get { return _positionSpring; } }
    public float PositionDamper { get { return _positionDamper; } }
    public float MaximumForce { get { return _maximumForce; } }


    public static explicit operator JointDrive(JointDriveConfig config) {
        JointDrive jointDrive = new JointDrive {
            positionSpring = config._positionSpring,
            positionDamper = config._positionDamper,
            maximumForce = config._maximumForce
        };

        return jointDrive;
    }

    public static explicit operator JointDriveConfig(JointDrive jointDrive) {
        JointDriveConfig jointDriveConfig = new JointDriveConfig {
            _positionSpring = jointDrive.positionSpring,
            _positionDamper = jointDrive.positionDamper,
            _maximumForce = jointDrive.maximumForce
        };

        return jointDriveConfig;
    }

    public readonly static JointDriveConfig ZERO = new JointDriveConfig { _positionSpring = 0, _positionDamper = 0, _maximumForce = 0 };

    public static JointDriveConfig operator *(JointDriveConfig config, float multiplier) {
        return new JointDriveConfig {
            _positionSpring = config.PositionSpring * multiplier,
            _positionDamper = config.PositionDamper * multiplier,
            _maximumForce = config.MaximumForce * multiplier
        };
    }
}
