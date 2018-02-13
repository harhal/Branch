using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProcessComponent : MonoBehaviour {

    public CircleProgressBar progressbarPrototype;
    public float BuildTime = 3;
    [SerializeField]
    float CurrentTime;
    CircleProgressBar progressbar;

    // Use this for initialization
    void OnEnable()
    {
        progressbar = Instantiate(progressbarPrototype.gameObject).GetComponent<CircleProgressBar>();
        progressbar.transform.position += transform.position;
        progressbar.Progress = 0;
    }

    private void OnDisable()
    {
        GameObject.Destroy(progressbar.gameObject);
    }

    // Update is called once per frame
    void Update () {
        CurrentTime += Time.deltaTime;
        if (CurrentTime < BuildTime)
            progressbar.Progress = CurrentTime / BuildTime;
        else
        {
            GetComponent<Building>().enabled = true;
            enabled = false;
        }
    }
}
