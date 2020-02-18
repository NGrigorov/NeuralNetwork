﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Manager : MonoBehaviour {

    public GameObject boomerPrefab;
    public GameObject hex;
    public TextMeshProUGUI textGen;

    private bool isTraning = false;
    private int populationSize = 50;
    private int generationNumber = 0;
    private int[] layers = new int[] { 1, 10, 10, 1 }; //1 input and 1 output
    private List<Brain> nets;
    private bool leftMouseDown = false;
    private List<Boomerang> boomerangList = null;

    void Timer()
    {
        isTraning = false;
    }


	void Update ()
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
                    nets[i] = new Brain(nets[i+(populationSize / 2)]);
                    nets[i].Mutate();

                    nets[i + (populationSize / 2)] = new Brain(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].Setfitness(0f);
                }
            }

           
            generationNumber++;
            textGen.text = "Generation: " + generationNumber;
            isTraning = true;
            Invoke("Timer",15f);
            CreateBoomerangBodies();
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(hex.transform.position, hit.point);
                hex.transform.position = hit.point;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }

        if(leftMouseDown == true)
        {

            
            
        }
    }


    private void CreateBoomerangBodies()
    {
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
            Boomerang boomer = ((GameObject)Instantiate(boomerPrefab, new Vector3(UnityEngine.Random.Range(-10f,10f), UnityEngine.Random.Range(-10f, 10f), 0),boomerPrefab.transform.rotation)).GetComponent<Boomerang>();
            boomer.Init(nets[i],hex.transform);
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

        nets = new List<Brain>();
        

        for (int i = 0; i < populationSize; i++)
        {
            Brain net = new Brain(layers);
            net.Mutate();
            nets.Add(net);
        }
    }
}
