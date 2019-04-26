using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public GameObject boomerPrefab;
    public GameObject hex;
    public TextMeshProUGUI timeScaleText;
    public TextMeshProUGUI generationText;
    public TextMeshProUGUI timeLeftText;
    public float fitness;
    private bool isTraning = false;
    private int populationSize = 50;
    private int generationNumber = 0;
    private int[] layers = new int[] { 1, 10, 10, 1 }; //1 input and 1 output
    private List<NeuralNetwork> nets;
    private bool leftMouseDown = false;
    private List<Boomerang> boomerangList = null;

    private float timer = 15;
    void Timer()
    {
        isTraning = false;
        timer = 15;
    }

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        // If we aren't training
        if (isTraning == false)
        {
            // Check if our generation number is 0
            if (generationNumber == 0)
            {
                // InitBoomerangNeuralNetworks
                InitBoomerangNeuralNetworks();
            }
            else
            {
                // Sort the nets
                nets.Sort();

                //Iterate through the half of the population size
                for (int i = 0; i < populationSize / 2; i++)
                {

                    nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                    // Mutate it
                    nets[i].Mutate();


                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                }

                // Iterate through the population size
                for (int i = 0; i < populationSize; i++)
                {
                    // Set the fitness to 0
                    nets[i].SetFitness(0f);

                }
            }

            // Increase the generation number
            generationNumber++;
            generationText.text = "Generation " + generationNumber.ToString();
            // Training to true
            isTraning = true;
            //Invoke the timer method
            Invoke("Timer", 15f);
            //Create Boomerang bodies
            CreateBoomerangBodies();

        }


        //Check if our mouse button is down
        if (Input.GetMouseButtonDown(0))
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Set the mouse button down boolean to true
                leftMouseDown = true;
            }


        }

        else if (Input.GetMouseButtonUp(0))
        {
            // Set the mouse down to false
            leftMouseDown = false;
        }

        if (leftMouseDown == true)
        {

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hex.transform.position = mousePosition;
        }

        timeLeftText.text = "Time Left: " + timer.ToString("0.0s");
        timer -= Time.deltaTime;
    }


    //Create boomerang bodies
    private void CreateBoomerangBodies()
    {
        // Set the food position to a random point
        hex.transform.position = new Vector2(Random.Range(-37, 37), Random.Range(-15, 15));
        // if our boomerang list is not null
        if (boomerangList != null)
        {

            // Destroy every Boomerang
            for (int i = 0; i < boomerangList.Count; i++)
            {
                GameObject.Destroy(boomerangList[i].gameObject);
            }

        }

        // Create a new boomerang list
        boomerangList = new List<Boomerang>();

        // Iterate through the population size
        for (int i = 0; i < populationSize; i++)
        {
            // Instantiate a boomerang at a random position and get the component
            Boomerang boomer = ((GameObject)Instantiate(boomerPrefab, new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0), boomerPrefab.transform.rotation)).GetComponent<Boomerang>();
            //Call the Init method
            boomer.Init(nets[i], hex.transform);
            //Add it the list
            boomerangList.Add(boomer);
        }

    }

    // This is only called once per run.
    void InitBoomerangNeuralNetworks()
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize % 2 != 0)
        {
            populationSize = 20;
        }

        // Creates a new list of neural networks
        nets = new List<NeuralNetwork>();

        // Create neural networks based off of the population size
        for (int i = 0; i < populationSize; i++)
        {
            // Create a new neural network
            NeuralNetwork net = new NeuralNetwork(layers);
            //Mutate it
            net.Mutate();

            //Add the net
            nets.Add(net);
        }

    }


    public void TimeScale(Slider slider)
    {


        Time.timeScale = slider.value;
        if (slider.value > 1)
        {
            timeScaleText.text = "Time Scale: " + slider.value + "x";
        }
        else
        {
            timeScaleText.text = "Time Scale: " + slider.value;
        }
    }

    //Quit the program
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    //Restart the program
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
