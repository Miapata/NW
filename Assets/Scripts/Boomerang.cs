using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Boomerang : MonoBehaviour
{
    private bool initilized = false;
    private Transform hex;

    public NeuralNetwork net;
    private Rigidbody2D rBody;
    private Material[] mats;

    public TextMeshProUGUI fitnessText;
    public bool entered;
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        mats = new Material[transform.childCount];
        for (int i = 0; i < mats.Length; i++)
            mats[i] = transform.GetChild(i).GetComponent<Renderer>().material;
    }

    void OnMouseover()
    {
        Manager.instance.fitness=net.fitness;
    }

    void FixedUpdate()
    {
        // If we started
        if (initilized == true)
        {
            // Get distance
            float distance = Vector2.Distance(transform.position, hex.position);

            // If our distance is greater than 20
            // Set it to 20
            if (distance > 20f)
                distance = 20f;
            
            // Change the color
            for (int i = 0; i < mats.Length; i++)
                mats[i].color = new Color(distance / 20f, (1f - (distance / 20f)), (1f - (distance / 20f)));

            float[] inputs = new float[1];


            float angle = transform.eulerAngles.z % 360f;
            if (angle < 0f)
                angle += 360f;


            Vector2 deltaVector = (hex.position - transform.position).normalized;


            float rad = Mathf.Atan2(deltaVector.y, deltaVector.x);
            rad *= Mathf.Rad2Deg;

            rad = rad % 360;
            if (rad < 0)
            {
                rad = 360 + rad;
            }

            rad = 90f - rad;
            if (rad < 0f)
            {
                rad += 360f;
            }
            rad = 360 - rad;
            rad -= angle;
            if (rad < 0)
                rad = 360 + rad;
            if (rad >= 180f)
            {
                rad = 360 - rad;
                rad *= -1f;
            }
            rad *= Mathf.Deg2Rad;

            inputs[0] = rad / (Mathf.PI);


            // Create a new float array and set it to the result of FeedForward
            float[] output = net.FeedForward(inputs);
            // Set the velocity forward
            rBody.velocity = 3 * transform.up;
            //Set the angular velocity, this determines rotation
            rBody.angularVelocity = 500f * output[0];
             // Add fitness
            net.AddFitness((1f - Mathf.Abs(inputs[0])));
        }
    }

    // Init the game
    public void Init(NeuralNetwork net, Transform hex)
    {
        this.hex = hex;
        this.net = net;
        initilized = true;
    }


}
