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
        float delta = Time.deltaTime;
        Vector3 angles = transform.eulerAngles;
        angles.z += delta * 50f;
        transform.eulerAngles = angles;

        Vector3 localScale = transform.localScale;
        if (increasingSize == true)
        {
            localScale += new Vector3(delta, delta ,0f);
            if (localScale.x >= 2f)
            {
                increasingSize = false;
            }
        }
        else if (increasingSize == false)
        {
            localScale -= new Vector3(delta, delta, 0f);
            if (localScale.x <=1f)
            {
                increasingSize = true;
            }
        }

        transform.localScale = localScale;

        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider!=null)
        if (collider.tag == "Boomerang")
        {
            if (collider.GetComponent<Boomerang>().entered == true)
                return;

            collider.GetComponent<Boomerang>().entered = true;
            count++;
            if (count > 5)
            {
                transform.position= new Vector2(Random.Range(-39, 39), Random.Range(-15, 15));
            }
        }
    }
}
