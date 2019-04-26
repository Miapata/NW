using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonAnimator : MonoBehaviour {
    bool increasingSize = true;
    Material mat;

    private int count;
	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
        mat.color = Color.yellow;
    }
	
	// Update is called once per frame
	void Update () {

     // Create a float called delta and assign it deltaTime
        float delta = Time.deltaTime;

        // Create a vector3 for our euler angles
        Vector3 angles = transform.eulerAngles;

        //Rotate the z axis
        angles.z += delta * 50f;
        transform.eulerAngles = angles;
        
        // Get the localScale;
        Vector3 localScale = transform.localScale;
        if (increasingSize == true)
        {
            // Increase the scale
            localScale += new Vector3(delta, delta ,0f);
            if (localScale.x >= 2f)
            {
                // Set the increasing scale boolean to false
                increasingSize = false;
            }
        }

        // Decrease the scale
        else if (increasingSize == false)
        {
            localScale -= new Vector3(delta, delta, 0f);
            if (localScale.x <=1f)
            {
                increasingSize = true;
            }
        }

        //Set the scale
        transform.localScale = localScale;

        
    }

}
