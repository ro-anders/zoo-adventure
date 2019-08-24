using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSFinder: MonoBehaviour
{
    private bool lastMockValue = false;

    public void CheckInRegions(Region[] regions, Action<bool, bool> callback)
    {
        // TODO: Determine if GPS is turned on with something like this
        //if (!Input.location.isEnabledByUser)
        StartCoroutine(DoCheckInRegions(regions, callback));
    }

    private IEnumerator DoCheckInRegions(Region[] regions, Action<bool, bool> callback)
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        yield return new WaitForSeconds(2f);
        lastMockValue = !lastMockValue;
        callback(true, lastMockValue);
        yield break;
#else
        // TODO: This should be checked elsewhere where we can give a better
        // message to the user
        bool ok = Input.location.isEnabledByUser;

        if (ok)
        {
            Input.location.Start();
            int maxWait = 10;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            ok = maxWait > 0;
        }

        float DESIRED_ACCURACY = 30;
        float UNACCEPTABLE_ACCURACY = 1000;
        float bestAccuracy = UNACCEPTABLE_ACCURACY;
        bool successAtBestAccuracy = false;
        if (ok)
        {
            float MAX_WAIT_SECONDS = 5;
            float timeToWait = MAX_WAIT_SECONDS;
            double lastReading = 0;
            while ((timeToWait > 0) && (bestAccuracy > DESIRED_ACCURACY))
            {
                if (timeToWait < MAX_WAIT_SECONDS)
                {
                    yield return new WaitForSeconds(1);
                }
                if (Input.location.lastData.timestamp > lastReading)
                {
                    lastReading = Input.location.lastData.timestamp;
                    if (Input.location.lastData.horizontalAccuracy < bestAccuracy)
                    {
                        bestAccuracy = Input.location.lastData.horizontalAccuracy;
                        successAtBestAccuracy = checkCoordsInRegions(
                            Input.location.lastData.latitude,
                            Input.location.lastData.longitude,
                            Input.location.lastData.horizontalAccuracy,
                            regions);
                    }

                }
                timeToWait -= 1;
            }
        }

        ok = (bestAccuracy <= UNACCEPTABLE_ACCURACY);

        Input.location.Stop();

        callback(ok && successAtBestAccuracy, !ok);
        yield break;
#endif
    }

    private bool checkCoordsInRegions(float latitude, float longitude, float accuracy, Region[] regions)
    {
        foreach (Region region in regions)
        {
            // TODO: Take accuracy into account
            if ((latitude <= region.north) && (latitude >= region.south) &&
                (longitude <= region.east) && (longitude >= region.west))
            {
                return true;
            }
        }
        return false;
    }


}
