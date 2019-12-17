using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour {

    public float width;
    public float height;

    private void Awake ()
    {
        width = transform.lossyScale.x;
        height = transform.lossyScale.y;
    }
	
}
