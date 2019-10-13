using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body : MonoBehaviour {

    public GvrArmModel armModel;
    public GameObject neck;
    public GameObject shoulder;
    public GameObject elbow;
    public GameObject wrist;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        neck.transform.position = armModel.NeckPosition;
        shoulder.transform.position = armModel.ShoulderPosition;
        shoulder.transform.rotation = armModel.ShoulderRotation;
        elbow.transform.position = armModel.ElbowPosition;
        elbow.transform.rotation = armModel.ElbowRotation;
        wrist.transform.position = armModel.WristPosition;
        wrist.transform.rotation = armModel.WristRotation;


    }
}
