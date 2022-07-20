//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    private Renderer _myRenderer;
    //private Vector3 _startingPosition;
    private bool gazedMain;
    private bool startTimer;
    private bool glowToggle;
    private float duration;

    public string playerCoordinate;

    public static string playerID { set; get; } = "0";

    public RecordGazing RecordPlayer;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        //_startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();

        gazedMain = false;
        startTimer = false;
        glowToggle = false;
        duration = 0.0f;
    }

    public void Update()
    {
        // record duration
        if (startTimer)
        {
            duration += Time.deltaTime;
        }
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        SetMaterial(false);
    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMaterial(bool gazedAt)
    {
        //_myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
            
        if ( gazedAt )
        {
            gazedMain = true;
            playerCoordinate = RecordPlayer.RecordData();

            Debug.Log("player coordinate: " + playerCoordinate);
            Debug.Log("gaze object: " + _myRenderer.name);

            StartCoroutine(toggleCountdown(3f));

            startTimer = true;
        }
        else
        {
            StartCoroutine(PostData("http://localhost/unityconnect/recordgaze.php"));

            gazedMain = false;
            glowToggle = false;
            startTimer = false;

            Debug.Log("duration: " + duration.ToString());
            duration = 0.0f;

            _myRenderer.material = InactiveMaterial;
        }
    }

    IEnumerator toggleCountdown(float timeLeft)
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (gazedMain == false)
            {
                break;
            }
         yield return null;
        }
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedMain ? GazedAtMaterial : InactiveMaterial;
        }
        if (InactiveMaterial != GazedAtMaterial)
        {
            glowToggle = gazedMain ? true : false;
        }

        Debug.Log("glowToggle: " + glowToggle.ToString());
    }

    
    IEnumerator PostData(string uri)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", playerID);
        form.AddField("playerCoordinate", playerCoordinate);
        //form.AddField("objectName", playerCoordinate[1]);
        form.AddField("objectName", _myRenderer.name);
        form.AddField("duration", duration.ToString());
        form.AddField("glowToggle", glowToggle.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("User data posted failed. Error #" + webRequest.result);
            }
            else
            {
                // Get text content like this:
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
