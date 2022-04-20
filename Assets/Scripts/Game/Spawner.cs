using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int m_xPos;
    public int m_zPos;

    public float m_offset;

    public float m_xDistance = 15.0f;
    public float m_zDistance = 15.0f;

    public GameObject m_parent;
    public GameObject m_sphere;
    public GameObject m_unit;

    Area m_area;

    Vector3 m_pos;
    Vector3 m_unitPos;

    // Start is called before the first frame update
    void Awake()
    {
        m_area = m_parent.GetComponent<Area>();
        m_unitPos = m_parent.transform.position;
        m_unitPos.y += 1;
        m_pos = m_parent.transform.position;
        m_pos.y += 1;
        for (int i = -3; i < 4; ++i)
        {
            m_pos.x = m_parent.transform.position.x + i;
            for (int j = -3; j < 4; ++j)
            {
                if(i == 0 && j == 0)
                {
                    m_area.m_unit = Instantiate(m_unit, m_unitPos, Quaternion.Euler(0, 0, 0), m_parent.transform);
                    continue;
                }
                m_pos.z = m_parent.transform.position.z + j;
                m_area.m_spheres.Add(Instantiate(m_sphere, m_pos, Quaternion.Euler(0, 0, 0), m_parent.transform));
            }
        }
        Destroy(this);
    }
}
