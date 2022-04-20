using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<GameObject> m_spheres;
    public GameObject m_unit;

    public void ResetArea()
    {
        for(int i = 0; i < m_spheres.Count; ++i)
        {
            m_spheres[i].SetActive(true);
        }
    }
}
