using System.Collections.Generic;
using System;

public class Brain : IComparable<Brain>
{
    private int[] layers; //Each layer holds Neurons
    private float[][] neurons; //2D jagged array Neutrons per layer
    private float[][][] weights; //3D jagged Array
    private float fitness;

   // private Random random;

    public Brain(int[] layers)
    {
        this.layers = new int[layers.Length];
        for(int i = 0; i< layers.Length;i++)
        {
            this.layers[i] = layers[i];
        }

        //random = new Random(System.DateTime.Today.Millisecond);

        initNeuros();
        initWeights();
    }

    public Brain(Brain copyBrain)
    {
        this.layers = new int[copyBrain.layers.Length];
        for (int i = 0; i < copyBrain.layers.Length; i++)
        {
            this.layers[i] = copyBrain.layers[i];
        }
        initNeuros();
        initWeights();
        CopyWeights(copyBrain.weights);
    }

    public void Addfitness(float fit)
    {
        fitness += fit;
    }

    public void Setfitness(float fit)
    {
        fitness = fit;
    }

    public float Getfitness()
    {
        return fitness;
    }

    public int CompareTo(Brain other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness) return 1;

        else if (fitness < other.fitness) return -1;

        else return 0;
    }

    public void CopyWeights(float[][][] copyweights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyweights[i][j][k];
                }
            }
        }
    }

    private void initNeuros() //Initializing the parts that either hold a value
    {
        List<float[]> neuronsList = new List<float[]>();

        for(int i = 0; i < layers.Length; i++) //Run through all layers
        {
            neuronsList.Add(new float[layers[i]]); //add layer to neuronlist
        }

        neurons = neuronsList.ToArray();//List of array to array of array
    }

    private void initWeights() //The connections between each Layer, and the likelyhood of using said connection.
    {
        List<float[][]> weightsList = new List<float[][]>();

        //Iterate over all neuros who have weight connection
        for (int i = 1; i < layers.Length; i++) 
        {
            List<float[]> layerWeightsList = new List<float[]>(); //Temp list for the current layer

            int neuronsInPrevLayer = layers[i - 1];

            //Iterate over all neuros in this layer
            for(int j = 0; j < neurons[i].Length;j++)
            {
                float[] neuronWeights = new float[neuronsInPrevLayer]; //Neuron Weights

                //Iterate over all neurons in the previous layer and set random weights between range value
                for(int k = 0; k < neuronsInPrevLayer; k++)
                {
                    //Random weights to Neurons weights
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights); // Add current neuron weights layer to layer weights
            }

            weightsList.Add(layerWeightsList.ToArray()); // add this layer weights into weight list
        }

        weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs) //Feed forward this neural network with a given input array
    {
        //add input to neuron martix
        for(int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f;
                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }

                neurons[i][j] = (float)Math.Tanh(value);
            }
        }
            return neurons[neurons.Length-1];
    }

    public void Mutate() //A-sexual mutation
    {
        for(int i = 0; i < weights.Length;i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //mutate value
                    float randomNumber = UnityEngine.Random.Range(0f, 1000f);

                    if (randomNumber <= 2f)
                    {
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    {
                        weight *= UnityEngine.Random.Range(-0.5f,0.5f);
                    }
                    else if (randomNumber <= 6f)
                    {
                        weight *= UnityEngine.Random.Range(0f, 1f) + 1f;
                    }
                    else if (randomNumber <= 8f)
                    {
                        weight *= UnityEngine.Random.Range(0f, 1f);
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

}
