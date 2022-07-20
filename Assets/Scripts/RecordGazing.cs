using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class RecordGazing : MonoBehaviour
{
    //public TMP_InputField nameField;

    public Vector3 playerCoordinate;
    //public string  gazeObjectName;

    //public string[] coordinateList = new string[2];

    public string RecordData()
    {
        //nameField = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        playerCoordinate = GameObject.Find("XR Origin").transform.position;
        Debug.Log("player coordinate: " + playerCoordinate.ToString());
        //coordinateList[0] = playerCoordinate.ToString();

        return playerCoordinate.ToString();

        //gazeCoordinate = GameObject.Find("Main Camera").transform.rotation.eulerAngles;
        //gazeObjectName = GameObject.Find("Main Camera").name;
        //Debug.Log("gazeObjectName: " + gazeObjectName);
        //coordinateList[1] = gazeObjectName;

        //return coordinateList;
    }
}