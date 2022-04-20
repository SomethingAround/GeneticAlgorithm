using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCreation : MonoBehaviour
{
    public int m_xSize;
    public int m_zSize;
    static int m_x = 0;
    static int m_z = 0;

    public float m_xDistance = 15.0f;
    public float m_zDistance = 15.0f;

    public GameObject m_parent;
    public GameObject m_area;

    Vector3 m_pos;
    // Start is called before the first frame update
    void Awake()
    { 
        m_x = m_xSize;
        m_z = m_zSize;
        m_pos = m_area.transform.position;
        //Creates the area
        for (int i = 0; i < m_x; ++i)
        {
            m_pos.x = m_area.transform.position.x + (Mathf.Abs(m_xDistance + (m_area.transform.localScale.x) * 10) * i);
            for (int j = 0; j < m_z; ++j)
            {
                m_pos.z = m_area.transform.position.z + m_zDistance * j;
                Instantiate(m_area, m_pos, Quaternion.Euler(0, 0, 0), m_parent.transform);
            }
        }

        Destroy(this);
    }
}
