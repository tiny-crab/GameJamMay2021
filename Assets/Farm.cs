using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public Datastore datastore;
    public GameObject garden;

    // Start is called before the first frame update
    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
    }

    // Update is called once per frame
    void Update() {

    }
}
