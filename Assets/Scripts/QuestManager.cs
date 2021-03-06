﻿#pragma warning disable 0649
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    // GPS CONSTANTS
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

    public bool FakingLocation, DebugCoords;

    [SerializeField]
    private Text objLoc, curLoc;
    public Text user1, user1info, user2, user2info;
    public GameObject playerActual, forceArriveButton, objectiveMarker;

    private float lastLatitude = 0;
    private string objectiveLocation;
    private bool hasArrived, forceArrived;

    Dictionary<Vector2, string> locs;
    Dictionary<string, Vector2> coords;
    // TODO: Dictionary to tuple

    void Start ()
    {
        // Fake GPS to real location
        locs = new Dictionary<Vector2, string>();
        locs.Add(new Vector2(1.3058f, 103.7723f), "Saga1");
        locs.Add(new Vector2(1.3058f, 103.7721f), "Saga2");
        locs.Add(new Vector2(1.3056f, 103.7723f), "Saga3");

        locs.Add(new Vector2(1.3064f, 103.7722f), "Elm1");
        locs.Add(new Vector2(1.3062f, 103.7722f), "Elm2");

        locs.Add(new Vector2(1.3068f, 103.7725f), "Library1");
        locs.Add(new Vector2(1.3069f, 103.7726f), "Library2");
        locs.Add(new Vector2(1.3071f, 103.7725f), "Library3");
        locs.Add(new Vector2(1.3071f, 103.7728f), "Library4");

        locs.Add(new Vector2(1.3076f, 103.7725f), "MPH1");
        locs.Add(new Vector2(1.3075f, 103.7725f), "MPH2");
        locs.Add(new Vector2(1.3073f, 103.7726f), "MPH3");

        locs.Add(new Vector2(1.3077f, 103.7724f), "Cendana Staff Area");
        locs.Add(new Vector2(1.3082f, 103.7726f), "MCS Classroom");
        locs.Add(new Vector2(1.3080f, 103.7725f), "MCS Classroom");
        locs.Add(new Vector2(1.3081f, 103.7723f), "Cendana1");
        locs.Add(new Vector2(1.3082f, 103.7723f), "Cendana2");

        locs.Add(new Vector2(1.3078f, 103.7723f), "Dylan's Room"); // Latest         

        // Real location to onscreen GPS
        coords = new Dictionary<string, Vector2>();
        coords.Add("Saga", new Vector2(103.7724f, 1.3059f));
        coords.Add("Saga1", new Vector2(103.7724f, 1.3059f));
        coords.Add("Saga2", new Vector2(103.7723f, 1.3058f));
        coords.Add("Saga3", new Vector2(103.7723f, 1.3056f));

        coords.Add("Dean of Faculty Office", new Vector2(103.7725f, 1.3061f));
        coords.Add("Dean of Faculty Office1", new Vector2(103.7725f, 1.3061f));

        coords.Add("Elm", new Vector2(103.7724f, 1.3066f));
        coords.Add("Elm1", new Vector2(103.7726f, 1.3065f));
        coords.Add("Elm2", new Vector2(103.7725f, 1.3063f));

        coords.Add("Library", new Vector2(103.77275f, 1.30715f));
        coords.Add("Library1", new Vector2(103.7728f, 1.30717f));
        coords.Add("Library2", new Vector2(103.77275f, 1.30715f));
        coords.Add("Library3", new Vector2(103.77275f, 1.30713f));

        coords.Add("MPH", new Vector2(103.7728f, 1.30765f));
        coords.Add("MPH1", new Vector2(103.77285f, 1.3077f));
        coords.Add("MPH2", new Vector2(103.7728f, 1.30765f));
        coords.Add("MPH3", new Vector2(103.7727f, 1.30766f));

        coords.Add("Cendana", new Vector2(103.7725f, 1.3080f));
        coords.Add("Cendana1", new Vector2(103.7730f, 1.3080f));
        coords.Add("Cendana2", new Vector2(103.7730f, 1.3080f));

        coords.Add("MCS Classroom", new Vector2(103.7728f, 1.308076f));
        coords.Add("Dylan's Room", new Vector2(103.7727f, 1.3080f));
        

        hasArrived = false;
        float longitude, latitude, x, y;

        if (!DebugCoords)
        {
            objectiveLocation = GameManager.instance.questObj;
            objLoc.text = "Objective:\n" + objectiveLocation;
            curLoc.text = "Current:\n<color=blue>Initializing GPS</color>";
            // Send my coordinates
            InvokeRepeating("HasArrived", 0, GameManager.instance.PING_FREQUENCY);
        }
        else
        {
            objectiveLocation = "Library";
            objLoc.text = "Objective:\n" + objectiveLocation;
            curLoc.text = "Current:\nCendana";
            InvokeRepeating("HasArrived", 0, GameManager.instance.PING_FREQUENCY);
        }

        Vector2 vector = new Vector2(0f, 0f);
        if (coords.TryGetValue(objectiveLocation, out vector))
        {
            longitude = vector.x;
            latitude = vector.y;
            x = (longitude - MIN_LONG) * SCALE_X - 3f;
            y = (latitude - MIN_LAT) * SCALE_Y - 5f;
            objectiveMarker.transform.position = new Vector3(x, y, 0);
        }

        if (FakingLocation)
        {
            // Method to calculate "correct" GPS
            print((1.1f + 3f) / SCALE_X + MIN_LONG);
            print((2.6f + 5f) / SCALE_Y + MIN_LAT);

            // Method to check distances
            //print(Vector2.Distance(new Vector2(1.306181f, 103.7722f), new Vector2(1.308547f, 103.7715f)));
            //print(Vector2.Distance(new Vector2(1.306181f, 103.7722f), new Vector2(1.307993f, 103.7725f)));
            
            longitude = 105.4725f;
            latitude = 2.407161f;
            x = (longitude - MIN_LONG) * SCALE_X - 3f;
            y = (latitude - MIN_LAT) * SCALE_Y - 5f;
            playerActual.transform.position = new Vector3(x, y, 0);
        } else
        {
            Input.location.Start(5f, 5f); // Accuracy of 5m, update if moved 5m
            Input.compass.enabled = true;
            int wait = 1000; // Per default 

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
                    // We start the timer to check each tick (every 5 sec) the current gps position
                    InvokeRepeating("RetrieveGPSData", 0, 5);
                }
            } else
            {
                curLoc.text = "Current:\n<color=red>GPS Unavailable</color>";
            }
        }
        Button btn = forceArriveButton.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            forceArrived = true;
            HasArrived();
            forceArriveButton.SetActive(false);
        });
    }

    private void HasArrived()
    {
        JSONObject data = new JSONObject(JSONObject.Type.BOOL);
        data.b = hasArrived || forceArrived;
        GameManager.socket.Emit("has arrived", data);
    }

    void RetrieveGPSData()
    {
        Input.location.Start(5f, 5f); // Accuracy of 5m, update if moved 5m
        string gpsString1 = "";
        if (lastLatitude != Input.location.lastData.latitude)
        {
            lastLatitude = Input.location.lastData.latitude;

            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float x = (longitude - MIN_LONG) * SCALE_X - 3f;
            float y = (latitude - MIN_LAT) * SCALE_Y - 5f;
            playerActual.transform.position = new Vector3(x, y, 0);
            gpsString1 += "Lat: " + latitude + " Lon: " + longitude + " Time: " + Input.location.lastData.timestamp;
            //string gpsString2 = "Lat: " + latitude + "\nLon: " + longitude + "\n Time: " + Input.location.lastData.timestamp + "\nx: " + x + "\ny: " + y;
            //textBox1.text = gpsString2;

            string location = where(new Vector2(latitude, longitude));
            curLoc.text = "Current:\n" + location.Remove(location.Length-1);
            if (location.StartsWith(objectiveLocation))
            {
                hasArrived = true;
                HasArrived();
            } else
            {
                hasArrived = false;
                HasArrived();
            }

            Vector2 vector = new Vector2(0f, 0f);
            if (coords.TryGetValue(location, out vector))
            {
                longitude = vector.x;
                latitude = vector.y;
                x = (longitude - MIN_LONG) * SCALE_X - 3f;
                y = (latitude - MIN_LAT) * SCALE_Y - 5f;
                playerActual.transform.position = new Vector3(x, y, 0);
                print(gpsString1);
            } else { // Unable to find vector
                playerActual.transform.position = new Vector3(x, y, 0);
                if (x < MIN_X || x > MAX_X || y < MIN_Y || y > MAX_Y)
                {
                    curLoc.text = "Current:\nOut of Bounds";
                    print("OOB: " + gpsString1);
                } else
                {
                    curLoc.text = "Current:\nUnlisted Location";
                    print("UNL: " + gpsString1);
                }
            }
        }
        Input.location.Stop();
    }

    void Update()
    {
        // Arrow Rotation
        playerActual.transform.rotation = Quaternion.Euler(0, 0, -Input.compass.trueHeading);
    }

    string where(Vector2 input)
    {
        float epsilon = 0.0002f;
        KeyValuePair<Vector2, string> closest = new KeyValuePair<Vector2, string> (new Vector2(103.770f,1.307f), "Not found");
        foreach (KeyValuePair<Vector2, string> pair in locs)
        {
            Vector2 coord = pair.Key;
            string location = pair.Value;
            //if (Vector2.Distance(input, coord) <= epsilon) return location;
            if (Vector2.Distance(input, coord) <= epsilon && Vector2.Distance(input, coord) < Vector2.Distance(closest.Key, coord))
            {
                closest = new KeyValuePair<Vector2, string>(coord, location);
            }
        }
        //return "Not found";
        return closest.Value;
    }

    public void Disconnected()
    {
        user1info.text = "<color=red>Disconnected</color>";
        user2info.text = "";
    }

    public void UpdateArriveUI(string s1, bool b2, bool b3, string s4, bool b5, bool b6) {
        user1.text = s1 + ":";
        user2.text = s4 + ":";

        string connected1, connected2;
        string arrived1, arrived2;
        connected1 = b2 ? "<color=green>Connected</color>" : "<color=red>Disconnected</color>";
        connected2 = b5 ? "<color=green>Connected</color>" : "<color=red>Disconnected</color>";
        arrived1 = b3 ? "<color=green>Arrived</color>" : "<color=red>Not yet</color>";
        arrived2 = b6 ? "<color=green>Arrived</color>" : "<color=red>Not yet</color>";

        if (GameManager.instance.myUsername == s1)
        {
            user1info.text = connected1 + "|" + arrived1;
            user2info.text = connected2 + "|" + arrived2;
        } else
        {
            user1info.text = connected1 + "|" + arrived1;
            user2info.text = connected2 + "|" + arrived2;
        }
    }
}
