using System;
using System.Collections.Generic;

public class GeneticAlgorithm<T>
{

    List<DNA<T>> m_newPopulation;
    public List<DNA<T>> m_population { get; private set; }

    int m_dnaSize;
    public int m_generation { get; private set; }
    public int m_elitism;

    float m_fitnessSum; 

    public float m_bestFitness { get; private set; }
    public float m_mutationRate;

    public T[] m_bestGenes { get; private set; }

    Random m_random;

    Func<T> m_getRandomGene;
    Func<int, float> m_fitnessFunction;

    public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, int elitism, float mutationRate = 0.01f)
    {
        m_generation = 1;
        m_mutationRate = mutationRate;
        m_elitism = elitism;
        m_dnaSize = dnaSize;

        m_population = new List<DNA<T>>(populationSize);
        m_newPopulation = new List<DNA<T>>(populationSize);

        m_random = random;

        m_bestGenes = new T[dnaSize];

        m_getRandomGene = getRandomGene;
        m_fitnessFunction = fitnessFunction;

        //Create the population
        for(int i = 0; i < populationSize; ++i)
        {
            m_population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction));
        }
    }

    //Create a new generation
    public void NewGeneration(int numNewDNA = 0, bool crossover = false)
    {
        int finalcount = m_population.Count + numNewDNA;
        //Checks if the population is 0
        if(finalcount <= 0)
        {
            return;
        }

        if (m_population.Count > 0)
        {
            CalculateFitness();

            m_population.Sort(CompareDNA);
        }

        m_newPopulation.Clear();
        
        //creates a new child and adds them to the population
        for (int i = 0; i < finalcount; ++i)
        {
            //Grabs the best from previous population
            if(i < m_elitism && i < finalcount)
            {
                m_newPopulation.Add(m_population[i]);
            }
            else if (i < finalcount || crossover)
            {
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);

                child.Mutate(m_mutationRate);

                m_newPopulation.Add(child);
            }
            else
            {
                m_newPopulation.Add(new DNA<T>(m_dnaSize, m_random, m_getRandomGene, m_fitnessFunction));
            }
        }

        //Holds previous generations population
        List<DNA<T>> tmpList = m_population;
        m_population = m_newPopulation;
        m_newPopulation = tmpList;

        ++m_generation;
    }

    public int CompareDNA(DNA<T> a, DNA<T> b)
    {
        if(a.m_fitness > b.m_fitness)
        {
            return -1;
        }
        else if (a.m_fitness < b.m_fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    //Calls the CalculateFitness function on all of the population
    public void CalculateFitness()
    {
        m_fitnessSum = 0;

        DNA<T> best = m_population[0];
        for (int i = 0; i < m_population.Count; ++i)
        {
            m_fitnessSum +=m_population[i].CalculateFitness(i);
            //Checks for the strongest member of the population
            if(m_population[i].m_fitness > best.m_fitness)
            {
                best = m_population[i];
            }
        }

        m_bestFitness = best.m_fitness;
        best.m_genes.CopyTo(m_bestGenes, 0);
    }

    //Chooses parent for the child
    private DNA<T> ChooseParent()
    {
        double randNumber = m_random.NextDouble() * m_fitnessSum;
        for (int i = 0; i < m_population.Count; ++i)
        {
            //randomly choose a parent
            if(randNumber < m_population[i].m_fitness)
            {
                return m_population[i];
            }
            randNumber -= m_population[i].m_fitness;
        }
        return null;
    }
}
