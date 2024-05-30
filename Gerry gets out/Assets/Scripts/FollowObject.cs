using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [Tooltip("Attached Object to follow")]
    public GameObject m_ToFollow;

    public Vector3 m_Offset;
    // Start is called before the first frame update
    void Start()
    {
        if (m_ToFollow == null)
            Debug.LogWarning("The \"ToFollow\"-Object is not set", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = m_ToFollow.transform.position + m_Offset;
    }
}
