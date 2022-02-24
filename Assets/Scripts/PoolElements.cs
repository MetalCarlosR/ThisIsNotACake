using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolElements
{
    List<GameObject> m_Elements;
    int m_CurrentElementId;

    public PoolElements(int count, Transform parent, GameObject prefab)
    {
        m_Elements = new List<GameObject>();
        m_CurrentElementId = 0;
        for (int i = 0; i < count; i++)
        {
            GameObject l_Elements = GameObject.Instantiate(prefab, parent);
            l_Elements.SetActive(false);
            l_Elements.transform.SetParent(null);
            m_Elements.Add(l_Elements);
        }
    }

    public GameObject GetNextElement()
    {
        GameObject l_Element = m_Elements[m_CurrentElementId];
        ++m_CurrentElementId;
        if (m_CurrentElementId >= m_Elements.Count) m_CurrentElementId = 0;
        return l_Element;
    }
}
