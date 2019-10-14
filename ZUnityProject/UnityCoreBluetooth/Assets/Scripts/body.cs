using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body : MonoBehaviour {

    public GvrArmModel armModel;
    public GameObject neck;
    public GameObject torso;
    public GameObject shoulder;
    public GameObject elbow;
    public GameObject wrist;

    public GameObject unitychan_torso;
    public GameObject unitychan_shoulder;
    public GameObject unitychan_elbow;
    public GameObject unitychan_wrist;

    Quaternion unitychan_torso_rot;
    Quaternion unitychan_shoulder_rot;
    Quaternion unitychan_elbow_rot;
    Quaternion unitychan_wrist_rot;


    // Use this for initialization
    void Start () {
        unitychan_torso_rot = unitychan_torso.transform.rotation;
        unitychan_shoulder_rot = unitychan_shoulder.transform.rotation;
        unitychan_elbow_rot = unitychan_elbow.transform.rotation;
        unitychan_wrist_rot = unitychan_wrist.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {

        neck.transform.position = armModel.NeckPosition;

        torso.transform.rotation = armModel.TorsoRotation;

        shoulder.transform.position = armModel.ShoulderPosition;
        shoulder.transform.rotation = armModel.ShoulderRotation;

        elbow.transform.position = armModel.ElbowPosition;
        elbow.transform.rotation = armModel.ElbowRotation;

        wrist.transform.position = armModel.WristPosition;
        wrist.transform.rotation = armModel.WristRotation;


        Vector3 euler = armModel.TorsoRotation.eulerAngles;
        unitychan_torso.transform.rotation = Quaternion.Euler(0, euler.y, 0) * unitychan_torso_rot;

        //        unitychan_shoulder.transform.position = armModel.ShoulderPosition;
        unitychan_shoulder.transform.rotation =  armModel.ShoulderRotation * unitychan_shoulder_rot;

        //        unitychan_elbow.transform.position = armModel.ElbowPosition;
        unitychan_elbow.transform.rotation = armModel.ElbowRotation * unitychan_elbow_rot;

        //        unitychan_wrist.transform.position = armModel.WristPosition;
        unitychan_wrist.transform.rotation = armModel.WristRotation * unitychan_wrist_rot;


    }
}
