using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GpsSystem : MonoBehaviour {
    // Define 103.7717,1.3054 = -2.31,-4.45
    // Define 103.7729,1.30865 = 2.22, 4.78
    const float MIN_LONG = 103.7717f;
    const float MAX_LONG = 103.7729f;
    const float MIN_LAT = 1.3054f;
    const float MAX_LAT = 1.30865f;
    const float MIN_X = -2.31f;
    const float MAX_X = 2.22f;
    const float MIN_Y = -4.45f;
    const float MAX_Y = 4.78f;
    const float SCALE_X = (MAX_X - MIN_X) / (MAX_LONG - MIN_LONG); 
    const float SCALE_Y = (MAX_Y - MIN_Y) / (MAX_LAT - MIN_LAT);

    public bool FakingLocation;

    public UnityEngine.UI.Text textBox1;
    public UnityEngine.UI.Text textBox2;
    public GameObject playerActual;

    //LocationInfo currentGPSPosition;
    //double lastTimestamp = 0;
    float lastLatitude = 0;

    Dictionary<Vector2, string> locs;
    Dictionary<string, Vector2> coords;
    // TODO: Dictionary to tuple

    void Start ()
    {
        print("Start");

        // Fake GPS to real location
        locs = new Dictionary<Vector2, string>();
        //locs.Add(new Vector2(1.3080f, 103.7725f), "Cendana Courtyard");
        locs.Add(new Vector2(1.3081f, 103.7723f), "Cendana Dining Hall");
        locs.Add(new Vector2(1.3082f, 103.7723f), "Cendana Dining Hall");
        locs.Add(new Vector2(1.3085f, 103.7715f), "Dylan's Room");
        locs.Add(new Vector2(1.3078f, 103.7723f), "Dylan's Room");

        locs.Add(new Vector2(1.3076f, 103.7725f), "MPH1");
        locs.Add(new Vector2(1.3075f, 103.7725f), "MPH2");
        locs.Add(new Vector2(1.3074f, 103.7726f), "MPH3");

        locs.Add(new Vector2(1.3068f, 103.7725f), "Library1");
        locs.Add(new Vector2(1.3069f, 103.7725f), "Library2");
        locs.Add(new Vector2(1.3070f, 103.7725f), "Library3");
        locs.Add(new Vector2(1.3072f, 103.7725f), "Library4");

        locs.Add(new Vector2(1.3064f, 103.7722f), "Elm Courtyard1");
        locs.Add(new Vector2(1.3062f, 103.7722f), "Elm Courtyard2");
        locs.Add(new Vector2(1.3061f, 103.7724f), "Between Elm and Saga");
        locs.Add(new Vector2(1.3058f, 103.7723f), "Saga Courtyard1");
        locs.Add(new Vector2(1.3058f, 103.7721f), "Saga Courtyard2");
        locs.Add(new Vector2(1.3056f, 103.7723f), "Saga Courtyard3");

        // Real location to real GPS
        coords = new Dictionary<string, Vector2>();
        coords.Add("Cendana Dining Hall", new Vector2(103.7724f, 1.3084f));
        coords.Add("Dylan's Room", new Vector2(103.7727f, 1.3080f));
        coords.Add("MPH1", new Vector2(103.77285f, 1.30770f));
        coords.Add("MPH2", new Vector2(103.7728f, 1.30765f));
        coords.Add("MPH3", new Vector2(103.7728f, 1.3076f));
        coords.Add("Library1", new Vector2(103.7728f, 1.30720f));
        coords.Add("Library2", new Vector2(103.77275f, 1.30715f));
        coords.Add("Library3", new Vector2(103.77275f, 1.30710f));
        coords.Add("Library4", new Vector2(103.7728f, 1.30720f));
        coords.Add("Elm Courtyard1", new Vector2(103.7726f, 1.3065f));
        coords.Add("Elm Courtyard2", new Vector2(103.7725f, 1.3063f));
        coords.Add("Between Elm and Saga", new Vector2(103.7725f, 1.3061f));
        coords.Add("Saga Courtyard1", new Vector2(103.7724f, 1.3059f));
        coords.Add("Saga Courtyard2", new Vector2(103.7723f, 1.3058f));
        coords.Add("Saga Courtyard3", new Vector2(103.7723f, 1.3056f));

        if (FakingLocation)
        {
            // Method to calculate "correct" GPS
            print((1.1f + 3f) / SCALE_X + MIN_LONG);
            print((1.35f + 5f) / SCALE_Y + MIN_LAT);

            // Method to check distances
            //print(Vector2.Distance(new Vector2(1.306181f, 103.7722f), new Vector2(1.308547f, 103.7715f)));
            //print(Vector2.Distance(new Vector2(1.306181f, 103.7722f), new Vector2(1.307993f, 103.7725f)));
            
            float longitude = 105.4725f;
            float latitude = 2.407161f;
            float x = (longitude - MIN_LONG) * SCALE_X - 3f;
            float y = (latitude - MIN_LAT) * SCALE_Y - 5f;
            this.transform.position = new Vector3(x, y, 0);
        } else
        {
            Input.location.Start(5f, 5f); // Accuracy of .1m, update if moved .1m
            Input.compass.enabled = true;
            int wait = 1000; // Per default 

            // Checks if the GPS is enabled by the user (-> Allow location)
            if (Input.location.isEnabledByUser)
            {
                while (Input.location.status == LocationServiceStatus.Initializing && wait > 0)
                {
                    wait--;
                }
                if (Input.location.status == LocationServiceStatus.Failed)
                {
                    print("Initializiing failed, try again.");
                } else
                {
                    // We start the timer to check each tick (every 4 sec) the current gps position
                    InvokeRepeating("RetrieveGPSData", 0, 4);
                }
            } else
            {
                textBox2.text = "GPS not available";
            }
        }
    }

    void RetrieveGPSData()
    {
        Input.location.Start(10f, 10f); // Accuracy of 10m, update if moved 10m
        string gpsString1 = "retrieve: ";
        if (lastLatitude != Input.location.lastData.latitude)
        {
            lastLatitude = Input.location.lastData.latitude;

            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float x = (longitude - MIN_LONG) * SCALE_X - 3f;
            float y = (latitude - MIN_LAT) * SCALE_Y - 5f;
            playerActual.transform.position = new Vector3(x, y, 0);

            gpsString1 += "Lat: " + latitude + " Lon: " + longitude;
            //string gpsString2 = "Lat: " + latitude + "\nLon: " + longitude + "\nTime: " + Input.location.lastData.timestamp + "\nx: " + x + "\ny: " + y;
            //textBox1.text = gpsString2;

            string location = where(new Vector2(latitude, longitude));
            textBox2.text = location;

            Vector2 vector = new Vector2(0f, 0f);
            if (coords.TryGetValue(location, out vector))
            {
                longitude = vector.x;
                latitude = vector.y;
                x = (longitude - MIN_LONG) * SCALE_X - 3f;
                y = (latitude - MIN_LAT) * SCALE_Y - 5f;
                this.transform.position = new Vector3(x, y, 0);
            } else {
                if (x < MIN_X || x > MAX_X || y < MIN_Y || y > MAX_Y)
                {
                    textBox2.text = "Out of bounds.";
                } //else Not Found
                this.transform.position = new Vector3(x, y, 0);
            }
            print(gpsString1);
        }
        Input.location.Stop();
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, -Input.compass.trueHeading);
    }

    string where(Vector2 input)
    {
        float epsilon = 0.0002f;
        foreach (KeyValuePair<Vector2, string> pair in locs)
        {
            Vector2 coord = pair.Key;
            string location = pair.Value;
            if (Vector2.Distance(input, coord) <= epsilon) return location;
        }
        return "Not found";
    }
}
