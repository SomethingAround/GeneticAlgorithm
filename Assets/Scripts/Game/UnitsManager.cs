using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager m_instance = null;

    int m_unitsDone;
    public int m_popSize = 6;
    public int m_moveSize = 80;
    public int m_elitism = 1;
    public float m_mutationRate = 0.01f;

    public Unit[] m_units;
    public Area[] m_areas;
    public GameObject[] units;
    public GameObject[] areas;

    public Text m_bestFitness;
    public Text m_generation;
    public Text m_targetMinMoves;

    GeneticAlgorithm<int> m_ga;
    System.Random m_random;
    private void Awake()
    {
        m_instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");
        areas = GameObject.FindGameObjectsWithTag("Area");
        m_units = new Unit[units.Length];
        m_areas = new Area[areas.Length];
        m_random = new System.Random();

        //Creates the Genetic Algorithm
        m_ga = new GeneticAlgorithm<int>(m_popSize, m_moveSize, m_random, RandomGene, FitnessFunction, m_elitism, m_mutationRate);

        m_bestFitness.text = "Best Fitness: " + m_ga.m_bestFitness;
        m_generation.text = "Generation: " + m_ga.m_generation;
        m_targetMinMoves.text = "Target Collected: " + m_moveSize;

        //Gets the units and area scripts
        for (int i = 0; i < units.Length; ++i)
        {
            m_units[i] = units[i].GetComponent<Unit>();
            m_areas[i] = areas[i].GetComponent<Area>();
            m_units[i].m_moves = m_ga.m_population[i].m_genes;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        //Checks if all units have done all their moves
        for(int i = 0; i < m_popSize; ++i)
        {
            if (m_units[i].m_done)
            {
                ++m_unitsDone;
            }
            else
            {
                m_unitsDone = 0;
                break;
            }
        }
        //resets all of them with the new genes
        if (m_unitsDone == m_popSize)
        {
            m_ga.NewGeneration();
            m_bestFitness.text = "Best Fitness: " + m_ga.m_bestFitness;
            m_generation.text = "Generation: " + m_ga.m_generation;
            for (int i = 0; i < m_popSize; ++i)
            {
                m_units[i].ResetUnit();
                m_areas[i].ResetArea();
                m_units[i].m_moves = m_ga.m_population[i].m_genes;
            }
            print("----------------------");
        }
    }

    //Gets a random gene
    public int RandomGene()
    {
        //print("Creating Random Gene");
        int i = m_random.Next(4);
        return i;
    }

    //Fitness function that is being used to calculate the fitness
    private float FitnessFunction(int index)
    {
        float score = 0;
        //NA<int> dna = m_ga.m_population[index];

        score = m_units[index].m_collected;

        score = score / m_moveSize;
        score = (Mathf.Pow(2, score) - 1) / (2 - 1);
        print("Score: " + score);
        return score;
    }
}
