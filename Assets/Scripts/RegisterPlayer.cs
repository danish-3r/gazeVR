using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class RegisterPlayer : MonoBehaviour
{
    public TMP_InputField nameField;
    public Button submitButton;
    
    public void CallRegister()
    {
        nameField = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        Debug.Log(nameField.text);
        StartCoroutine(Register("http://localhost/unityconnect/register.php"));
    }

    IEnumerator Register(string uri)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("User creation failed. Error #" + webRequest.result);
            }
            else
            {
                // Get text content like this:
                Debug.Log("User created successfully");
                Debug.Log(webRequest.downloadHandler.text);

                ObjectController.playerID = webRequest.downloadHandler.text;

                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 1);
    }
}
