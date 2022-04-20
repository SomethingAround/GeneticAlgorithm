using System;

public class DNA<T>
{
	//The stored data for the member of the species
	public T[] m_genes { get; private set; }

	public float m_fitness { get; private set; }

	//Creates random
	Random m_random;

	Func<T> m_getRandomGene;

	Func<int, float> m_fitnessFunction;
	//Create member
	public DNA(int size, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool initGenes = true)
	{
		m_genes = new T[size];
		m_random = random;
		m_getRandomGene = getRandomGene;
		m_fitnessFunction = fitnessFunction;

		if (initGenes)
		{
			for (int i = 0; i < m_genes.Length; ++i)
			{
				m_genes[i] = m_getRandomGene();
			}
		}
	}

	//Calculate the fitness of the member
	public float CalculateFitness(int index)
    {
		m_fitness = m_fitnessFunction(index);
		return m_fitness;
    }

	//Create offspring off of 2 from the species
	public DNA<T> Crossover(DNA<T> otherParent)
    {
		//Creates offspring
		DNA<T> child = new DNA<T>(m_genes.Length, m_random, m_getRandomGene, m_fitnessFunction, false);

		for(int i = 0; i < m_genes.Length; ++i)
        {
			//Does a 50/50 check for which gene to take from each parent
			child.m_genes[i] = m_random.NextDouble() < 0.5 ? m_genes[i] : otherParent.m_genes[i];
		}

		return child;
    }

	//Mutate the species
	public void Mutate(float mutationRate)
    {
		for (int i = 0; i < m_genes.Length; ++i)
		{
			if (m_random.NextDouble() < mutationRate)
            {
				m_genes[i] = m_getRandomGene();
            }
		}
	}
}
