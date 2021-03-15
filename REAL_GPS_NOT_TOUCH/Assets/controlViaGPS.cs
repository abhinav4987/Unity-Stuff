using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class controlViaGPS : MonoBehaviour
{
    public Text GPSStatus;
    public Text latitudeValue;
    public Text longitudeValue;
    public Text altitudeValue; 
    public Text horizontalAccuracyValue;
    public Text timestampValue;
    public Vector2 speed = new Vector2(5,5);
    // Start is called before the first frame update
    void Start()
    {
        GPSStatus.text = "Running not";
        if (!Permission.HasUserAuthorizedPermission (Permission.FineLocation))
        {
                Permission.RequestUserPermission (Permission.FineLocation);
        }
        StartCoroutine(GPSLoc());
    }

    // Update is called once per frame
    IEnumerator GPSLoc()
    {
        if(!Input.location.isEnabledByUser) {
            GPSStatus.text = "GPS not Enabled";
            yield break;
        }

        Input.location.Start();
        
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait < 1) {
            GPSStatus.text = "Time Out.";
            yield break;
        }
        
        if(Input.location.status == LocationServiceStatus.Failed) {
            GPSStatus.text = "Connection Failed";
            yield break;
        } else {
            // granted
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData",0.05f,1f);
        }

    }

    private void UpdateGPSData() {
        if(Input.location.status == LocationServiceStatus.Running) {
            // Access granted to GPS values and it has been init
            GPSStatus.text = "Running";

            // float lastLattitude = float.parse(latitudeValue);
            // float lastLongitude = float.parse(longitudeValue);

            // float newLattitude = Input.location.lastData.latitude;
            // float newLongitude = Input.location.lastData.longitude;

            // Vector3 movement = new Vector3(
            //     newLatitude - lastLatitude,
            //     newLongitude - lastLongitude,
            //     0
            // );
            // transform.Translate(movement);
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timestampValue.text = Input.location.lastData.timestamp.ToString();


        } else {
            // service is stopped
            GPSStatus.text = "Not Running";
        }
    }

    // public string getLatitude() {
    //     return latitudeValue.text;
    // }

    // public string getLongitude() {
    //     return longitudeValue.text;
    // }

    // public String getAltitude() {
    //     return altitudeValue.text;
    // }

    // public String getHorizontalAccuracy() {
    //     return horizontalAccuracyValue.text;
    // }

    // public String getTimestamp() {
    //     return timestampValue.text;
    // }


}
