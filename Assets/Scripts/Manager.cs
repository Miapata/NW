using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public GameObject boomerPrefab;
    public GameObject hex;
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
        if (isTraning == false)
        {
            if (generationNumber == 0)
            {
                InitBoomerangNeuralNetworks();
            }
            else
            {
                nets.Sort();
                for (int i = 0; i < populationSize / 2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                    nets[i].Mutate();

                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
               
                }
            }


            generationNumber++;

            isTraning = true;
            Invoke("Timer", 15f);
            CreateBoomerangBodies();
           
        }


        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }

        if (leftMouseDown == true)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hex.transform.position = mousePosition;
        }

        timer -= Time.deltaTime;
    }


    private void CreateBoomerangBodies()
    {
        hex.transform.position = new Vector2(Random.Range(-39, 39), Random.Range(-15, 15));
        if (boomerangList != null)
        {
            for (int i = 0; i < boomerangList.Count; i++)
            {
                GameObject.Destroy(boomerangList[i].gameObject);
            }

        }

        boomerangList = new List<Boomerang>();

        for (int i = 0; i < populationSize; i++)
        {
            Boomerang boomer = ((GameObject)Instantiate(boomerPrefab, new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0), boomerPrefab.transform.rotation)).GetComponent<Boomerang>();
            boomer.Init(nets[i], hex.transform);
            boomerangList.Add(boomer);
        }

    }

    void InitBoomerangNeuralNetworks()
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize % 2 != 0)
        {
            populationSize = 20;
        }

        nets = new List<NeuralNetwork>();


        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
        
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 22;
        
        GUI.Label(new Rect(10, 10, 100, 200), timer.ToString("0.0s"), style);
        GUI.Label(new Rect(10, 40, 100, 200), fitness.ToString("0.0"), style);
    }
}
