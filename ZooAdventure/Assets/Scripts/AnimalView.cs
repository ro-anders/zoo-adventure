using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimalView : MonoBehaviour
{
    public AnimalChooser animalChooser;
    public GameObject scannerPanel;
    public GameObject foundPanel;
    public GameObject notfoundPanel;
    public GameObject videoScreen;
    public GameObject duringAudioImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioPlayer;
    public Text statusMesage;

    private AnimalConfig activeAnimal;

    void Start()
    {
        scannerPanel.SetActive(false);
    }

    public void FindAnimal(AnimalConfig animal)
    {
        activeAnimal = animal;
        scannerPanel.SetActive(true);
        foundPanel.SetActive(false);
        notfoundPanel.SetActive(false);
        // TODO: Setup GPS to look for coordinates
        CheckInRegions(animal.Regions, OnGpsFinderReturned);
    }

    public void OnCloseClicked()
    {
        scannerPanel.SetActive(false);
        foundPanel.SetActive(false);
        notfoundPanel.SetActive(false);
        this.DismissVideo();
        animalChooser.OnAnimalViewClosed();
    }

    private void OnGpsFinderReturned(bool success, bool error)
    {
        // If not found, show the not found page
        // If found, start the video playing
        // TODO: Handle an error
        UnityEngine.Debug.Log("Animal search was " + (success ? "succesful" : "a failure"));
        foundPanel.SetActive(success);
        notfoundPanel.SetActive(!success);
        scannerPanel.SetActive(false);
        if (success)
        {
            PlayVideo();
        }
    }

    private void PlayVideo()
    {
        statusMesage.text = "Loading " + activeAnimal.ResourceName + " clip";
        VideoClip clipToPlay = Resources.Load<VideoClip>("Videos/" + activeAnimal.ResourceName) as VideoClip;
        if (clipToPlay != null)
        {
            UnityEngine.Debug.Log("Playing " + activeAnimal.ResourceName + " clip");
            statusMesage.text = "Playing " + activeAnimal.ResourceName + " clip";
            videoScreen.SetActive(true);
            videoPlayer.clip = clipToPlay;
            videoPlayer.loopPointReached += OnVideoDone;
            videoPlayer.Play();
            statusMesage.text = "Currently playing " + activeAnimal.ResourceName + " clip";
        }
    }

    private void OnVideoDone(UnityEngine.Video.VideoPlayer vp)
    {
        UnityEngine.Debug.Log(activeAnimal.ResourceName + " clip done playing");
        statusMesage.text = activeAnimal.ResourceName + " clip done playing";
        // Remove the video and start the sound playing
        this.DismissVideo();
        this.PlayAudio();
    }

    private void DismissVideo()
    {
        videoPlayer.Stop();
        videoScreen.SetActive(false);
    }

    private void PlayAudio()
    {
        UnityEngine.Debug.Log("Loading " + activeAnimal.ResourceName + " blip");
        AudioClip blipToPlay = Resources.Load<AudioClip>("Sounds/" + activeAnimal.ResourceName) as AudioClip;
        if (blipToPlay != null)
        {
            UnityEngine.Debug.Log("Playing " + activeAnimal.ResourceName + " blip");
            duringAudioImage.SetActive(true);
            audioPlayer.clip = blipToPlay;
            //videoPlayer.loopPointReached += OnVideoDone;
            audioPlayer.Play();
            Invoke("OnAudioDone", blipToPlay.length);
        }
    }

    private void OnAudioDone()
    {
        UnityEngine.Debug.Log(activeAnimal.ResourceName + " blip done playing");
        // Remove the audio icons
        this.DismissAudio();
    }

    private void DismissAudio()
    {
        duringAudioImage.SetActive(false);
    }


    // GPS STUFF ////////////////////////////////////////

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
            Input.location.Start(5);
            int maxWait = 10;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            ok = maxWait > 0;
        }

        float DESIRED_ACCURACY = 5;
        float UNACCEPTABLE_ACCURACY = 50;
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
        double meters_per_degree_lat = 111014.94; // This approximation most accurate at Wash DC latitude
        double meters_per_degree_long = 111467.13 * Math.Cos(latitude * Mathf.Deg2Rad);  // This approximation most accurate at Wash DC latitude
        double latAccuracy = accuracy / meters_per_degree_lat;
        double longAccuracy = accuracy / meters_per_degree_long;
        foreach (Region region in regions)
        {
            // TODO: Take accuracy into account
            if ((latitude <= region.north + latAccuracy) && (latitude  >= region.south - latAccuracy) &&
                (longitude <= region.east + longAccuracy) && (longitude >= region.west - longAccuracy))
            {
                statusMesage.text = latitude + "N " + longitude + "W is IN region with accuracy " + accuracy;
                return true;
            } else
            {
                statusMesage.text = "";
                if (latitude > region.north)
                {
                    statusMesage.text = latitude + "N " + " is NORTH of " + region.north + "N with accuracy " + accuracy;
                }
                if (latitude < region.south)
                {
                    statusMesage.text = latitude + "N " + " is SOUTH of " + region.south + "N with accuracy " + accuracy;
                }
                if (longitude > region.east)
                {
                    statusMesage.text = longitude + "W " + " is EAST of " + region.east + "W with accuracy " + accuracy;
                }
                if (longitude < region.west)
                {
                    statusMesage.text = longitude + "W " + " is WEST of " + region.east + "W with accuracy " + accuracy;
                }
            }
        }
        return false;
    }



}
