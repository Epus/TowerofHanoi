using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public List<Plate> plates = new List<Plate>();
    private Vector3 top;

    public bool IsStartingTower;

    private void Awake ()
    {
        Reset();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset ()
    {
        plates = new List<Plate>();
        top = transform.position + Vector3.down * (transform.lossyScale.y / 2);
        IsStartingTower = false;
    }

    public bool Push (Plate plate)
    {
        if (plate == null)
        {
            return false;
        }
        if(plates.Count > 0 && plate.width > plates[plates.Count -1].width)
        {
            Game.instance.Log("Cannot place larger plate on top of smaller plate", 2);
            return false;
        }
        plates.Add(plate);
        plate.transform.position = top + Vector3.up * plate.height / 2;
        top += Vector3.up * (plate.height);
        if (plates.Count == Game.instance.numPlates && !IsStartingTower)
            Game.instance.Completed();
        return true;
    }

    public Plate Pop()
    {
        if(plates.Count == 0)
        {
            Game.instance.Log("No plates in this tower", 2);
            return null;
        }
        var plate = plates[plates.Count - 1];
        top += Vector3.down * (plate.height);
        plates.RemoveAt(plates.Count - 1);
        return plate;

    }

}


