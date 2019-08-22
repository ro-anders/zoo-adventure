using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSCoordinates : MonoBehaviour
{

    public Text coordinatesText;
    public Text messageText;
    private bool gettingGpsData = false;
    private float firstAccuracy = -1;
    private double lastTimestamp = -1;

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
        messageText.text = ("Starting location service");
        Input.location.Start();
        messageText.text = ("Location service started");

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
            gettingGpsData = true;
            messageText.text = "Location received";
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (gettingGpsData && (Input.location.lastData.timestamp > lastTimestamp))
        {
            // Access granted and location value could be retrieved
            if (firstAccuracy < 0)
            {
                firstAccuracy = Input.location.lastData.horizontalAccuracy;
            }
            coordinatesText.text = "GPS=" + Input.location.lastData.latitude + "N " + Input.location.lastData.longitude + "W, " +
                "Alt=" + Input.location.lastData.altitude +
                "\nAccuracy=" + Input.location.lastData.horizontalAccuracy + " improved by " + (firstAccuracy - Input.location.lastData.horizontalAccuracy) +
                "\nTime=" + Input.location.lastData.timestamp;
            lastTimestamp = Input.location.lastData.timestamp;
        }
    }
}
