using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int m_index = 0;
    int m_move;
    int m_horizontal, m_vertical;
    int m_originX, m_originZ;
    public int m_collected;
    public int[] m_moves;
    public float m_speed;

    float m_timer;

    public bool m_moving = true;
    public bool m_done = false;

    public System.Random m_random;

    Vector3 m_moveTowards;
    Vector3 m_origin;
    Vector3 m_lastPos;
    Vector3 m_Extents = new Vector3(0.9f, 1f, 0.9f);
    // Start is called before the first frame update
    void Start()
    {
        m_moveTowards = transform.position;
        m_origin = m_moveTowards;
    }

    // Update is called once per frame
    public void Update()
    {
        //Checks if it is colliding with the pellet
        Collider[] overlapNode = Physics.OverlapBox(transform.position, m_Extents / 2, Quaternion.identity);
        //Waits for the timer so their is a bit of wait time before a new generation starts
        if (m_timer > 1.0f)
        {
            //Checks if they have reached their final move
            if (m_index < m_moves.Length)
            {
                //Checks if they are still moving
                if (!m_moving)
                {
                    m_lastPos = m_moveTowards;
                    //Checks what move the gene is
                    switch (m_moves[m_index])
                    {
                        //Left -x
                        case 0:
                            {
                                --m_moveTowards.x;
                                if (m_moveTowards.x < m_origin.x)
                                {
                                    ++m_horizontal;
                                }
                                else if (m_moveTowards.x > m_origin.x)
                                {
                                    --m_horizontal;
                                }
                                else if (m_moveTowards.x == m_origin.x)
                                {
                                    m_horizontal = 0;
                                }

                                break;
                            }
                        //Right +x
                        case 1:
                            {
                                ++m_moveTowards.x;
                                if (m_moveTowards.x > m_origin.x)
                                {
                                    ++m_horizontal;
                                }
                                else if (m_moveTowards.x < m_origin.x)
                                {
                                    --m_horizontal;
                                }
                                else if (m_moveTowards.x == m_origin.x)
                                {
                                    m_horizontal = 0;
                                }
                                break;
                            }
                        //Up +z
                        case 2:
                            {
                                ++m_moveTowards.z;
                                if (m_moveTowards.z > m_origin.z)
                                {
                                    ++m_vertical;
                                }
                                else if (m_moveTowards.z < m_origin.z)
                                {
                                    --m_vertical;
                                }
                                else if (m_moveTowards.z == m_origin.z)
                                {
                                    m_vertical = 0;
                                }
                                break;
                            }
                        //Down -z
                        case 3:
                            {
                                --m_moveTowards.z;
                                if (m_moveTowards.z < m_origin.z)
                                {
                                    ++m_vertical;
                                }
                                else if (m_moveTowards.z > m_origin.z)
                                {
                                    --m_vertical;
                                }
                                else if (m_moveTowards.z == m_origin.z)
                                {
                                    m_vertical = 0;
                                }
                                break;
                            }
                    }


                    m_moving = true;
                    //Checks if they have reached the edge of the area
                    if (m_horizontal > 3)
                    {
                        --m_horizontal;
                        m_moveTowards = m_lastPos;
                        m_moving = false;
                    }
                    else if (m_vertical > 3)
                    {
                        --m_vertical;
                        m_moveTowards = m_lastPos;
                        m_moving = false;
                    }
                    ++m_index;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_moveTowards, m_speed);
                    //Checks if they have reached the pellet
                    if (transform.position == m_moveTowards)
                    {
                        //Checks if the are inside of a pellet
                        if (overlapNode.Length > 0)
                        {
                            for (int i = 0; i < overlapNode.Length; ++i)
                            {
                                if (overlapNode[i].gameObject.CompareTag("Pellet"))
                                {
                                    overlapNode[i].gameObject.SetActive(false);
                                    ++m_collected;
                                }
                            }
                        }
                        m_moving = false;

                    }
                }
            }
            else
            {
                m_done = true;
            }
        }
        else
        {
            m_timer += Time.deltaTime;
        }
    }

    //Resets the Unit
    public void ResetUnit()
    {
        gameObject.transform.position = m_origin;
        m_moveTowards = m_origin;
        m_horizontal = 0;
        m_vertical = 0;
        m_collected = 0;
        m_index = 0;
        m_done = false;
        m_timer = 0.0f;
    }
}
