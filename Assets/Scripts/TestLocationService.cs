using UnityEngine;
using System.Collections;

public class TestLocationService : MonoBehaviour {
    public UnityEngine.UI.Text textBox1;
    public UnityEngine.UI.Text textBox2;

    bool gpsInit = false;
    LocationInfo currentGPSPosition;

    void Start()
    {
        print("start");

        //Starting the Location service before querying location
        Input.location.Start(0.5f); // Accuracy of 0.5 m

        int wait = 1000; // Per default

        // Checks if the GPS is enabled by the user (-> Allow location )
        if (Input.location.isEnabledByUser)
        {
            while (Input.location.status == LocationServiceStatus.Initializing && wait > 0)
            {
                wait--;
            }


            if (Input.location.status == LocationServiceStatus.Failed)
            {

            }
            else
            {
                gpsInit = true;
                // We start the timer to check each tick (every 3 sec) the current gps position
                InvokeRepeating("RetrieveGPSData", 0, 3);
            }
        }
        else
        {
            textBox1.text = "GPS not available";
        }
    }

    void RetrieveGPSData()
    {
        string gpsString1 = "retrieve";
        //currentGPSPosition = Input.location.lastData;
        //string gpsString = "::" + currentGPSPosition.latitude + "//" + currentGPSPosition.longitude;
        gpsString1 += "Lat: " + Input.location.lastData.latitude + " Lon: " + Input.location.lastData.longitude + " HorAcc: " + Input.location.lastData.horizontalAccuracy + " Alt: " + Input.location.lastData.altitude + " VerAcc: " + Input.location.lastData.verticalAccuracy;
        string gpsString2 = "Lat: " + Input.location.lastData.latitude + "\nLon: " + Input.location.lastData.longitude + "\nHorAcc: " + Input.location.lastData.horizontalAccuracy + "\nAlt: " + Input.location.lastData.altitude + "\nVerAcc: " + Input.location.lastData.verticalAccuracy;
        print(gpsString1);
        textBox2.text = gpsString2;
    }
}