using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSCoordinates : MonoBehaviour
{

    public Text coordinatesText;
    public Text messageText;

    IEnumerator Start()
    {
        UnityEngine.Debug.Log("Starting");
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            messageText.text = "Location not enabled on device";
            yield break;
        }

        // Start service before querying location
        UnityEngine.Debug.Log("Starting location service");
        Input.location.Start();
        UnityEngine.Debug.Log("Location service started");

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            messageText.text = "Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            messageText.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            coordinatesText.text = "" + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
