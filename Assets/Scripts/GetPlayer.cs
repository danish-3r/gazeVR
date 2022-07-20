using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GetPlayer : MonoBehaviour
{
    public TMP_Text nameField;

    private string playerID;

    public void Start()
    {
        nameField = GameObject.Find("PlayerName").GetComponent<TMP_Text>();

        playerID = ObjectController.playerID;

        StartCoroutine(GetName("http://localhost/unityconnect/getplayername.php"));
    }

    IEnumerator GetName(string uri)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", playerID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection error. Error #" + webRequest.result);
            }
            else
            {
                nameField.text = "Your name: " + webRequest.downloadHandler.text;
            }
        }
    }
}
