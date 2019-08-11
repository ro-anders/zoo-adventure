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
        StartCoroutine(MockGps(animal));
    }

    public void OnCloseClicked()
    {
        scannerPanel.SetActive(false);
        foundPanel.SetActive(false);
        notfoundPanel.SetActive(false);
        this.DismissVideo();
        animalChooser.OnAnimalViewClosed();
    }

    private bool mockSuccess = false;
    private IEnumerator MockGps(AnimalConfig animal)
    {
        yield return new WaitForSeconds(3f);
        mockSuccess = !mockSuccess;
        OnGpsFinderReturned(animal, mockSuccess);
    }

    private void OnGpsFinderReturned(AnimalConfig animal, bool success)
    {
        // If not found, show the not found page
        // If found, start the video playing
        UnityEngine.Debug.Log("Animal search was " + (success ? "succesful" : "a failure"));
        foundPanel.SetActive(success);
        notfoundPanel.SetActive(!success);
        scannerPanel.SetActive(false);
        if (success)
        {
            PlayVideo();
        }
    }

    private void OnVideoDone(UnityEngine.Video.VideoPlayer vp)
    {
        UnityEngine.Debug.Log(activeAnimal.ResourceName + " clip done playing");
        // Remove the video and start the sound playing
        this.DismissVideo();
        this.PlayAudio();
    }

    private void PlayVideo()
    {
        UnityEngine.Debug.Log("Loading " + activeAnimal.ResourceName + " clip");
        VideoClip clipToPlay = Resources.Load<VideoClip>("Videos/" + activeAnimal.ResourceName) as VideoClip;
        if (clipToPlay != null)
        {
            UnityEngine.Debug.Log("Playing " + activeAnimal.ResourceName + " clip");
            videoScreen.SetActive(true);
            videoPlayer.clip = clipToPlay;
            videoPlayer.loopPointReached += OnVideoDone;
            videoPlayer.Play();
        }
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
        }
    }

}
