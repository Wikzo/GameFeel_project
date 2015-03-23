using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {

    // Use this for initialization
    void Start () {

        const string projectId = "5f287fd0-45fc-42f2-9e3a-f7e04a20faba";
        UnityAnalytics.StartSDK (projectId);

        //Debug.Log("analytics started");

    }

}