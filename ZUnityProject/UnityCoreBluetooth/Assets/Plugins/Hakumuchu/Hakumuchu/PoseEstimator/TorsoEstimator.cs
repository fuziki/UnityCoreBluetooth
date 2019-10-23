using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TorsoEstimator {

    protected static readonly Vector3 NECK_OFFSET = new Vector3(0.0f, 0.075f, 0.08f);

    public struct Input
    {
        public bool IsLockedToNeck;
        public Vector3 HeadPosition;
        public Quaternion HeadRotation;
        public float AngularVelocity;
        public bool ForceImmediate;
    }

    public struct Output
    {
        public Vector3 NeckPosition;
        public Quaternion TorsoRotation;
    }

    private Vector3 torsoDirection = Vector3.forward;
    public Output Estimate(Input input)
    {
        // Determine the gaze direction horizontally.
        Vector3 gazeDirection = input.HeadRotation * Vector3.forward;
        gazeDirection.y = 0.0f;
        gazeDirection.Normalize();

        // Use the gaze direction to update the forward direction.
        if (input.ForceImmediate)
        {
            torsoDirection = gazeDirection;
        }
        else
        {
            float gazeFilterStrength = Mathf.Clamp((input.AngularVelocity - 0.2f) / 45.0f, 0.0f, 0.1f);
            torsoDirection = Vector3.Slerp(torsoDirection, gazeDirection, gazeFilterStrength);
        }

        Vector3 neckPosition = CalculateNeckPosition(input.IsLockedToNeck, input.HeadPosition, input.HeadRotation);

        // Calculate the torso rotation.
        return new Output 
        {
            TorsoRotation = Quaternion.FromToRotation(Vector3.forward, torsoDirection),
            NeckPosition = neckPosition
        };
    }

    protected virtual Vector3 CalculateNeckPosition(bool isLockedToNeck, Vector3 headPosition, Quaternion headRotation)
    {
        if (isLockedToNeck)
        {
            Vector3 rotatedNeckOffset =
                (headRotation * NECK_OFFSET) - (NECK_OFFSET.y * Vector3.up);
            headPosition -= rotatedNeckOffset;
            return headPosition;
        }
        return Vector3.zero;
    }
}
