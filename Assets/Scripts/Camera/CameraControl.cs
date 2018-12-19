using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float m_MinDis = 8f;
    public float m_MaxDis = 24f;
    public float m_CurDis = 16f;
    public float m_ScrollSpeed;
    public float m_Angle = Mathf.PI / 6;
    public float m_AngleSpeed;
    public float m_MaxAngle = Mathf.PI / 3;
    public float m_MinAngle = 0f;
    public bool is_Third;
    [HideInInspector] public Transform m_Targets;
    [HideInInspector] public bool m_AngleControl;

    private string m_MouseX;
    private string m_MouseY;
    private string m_MouseScrollWheel;
    private float m_MouseInputValue;
    private RaycastHit hit;

    public void EnableAngleControl()
    {
        m_AngleControl = true;
    }

    public void DisableAngleControl()
    {
        m_AngleControl = false;
    }

    private void Awake()
    {
        is_Third = true;
        m_Targets = GameObject.Find("role").transform;
    }

    // Use this for initialization
    void Start()
    {
        DisableAngleControl();
        m_MouseX = "Mouse X";
        m_MouseY = "Mouse Y";
        m_MouseScrollWheel = "Mouse ScrollWheel";
    }

    // Update is called once per frame
    void Update()
    {
        if (m_AngleControl)AngleControl();
        if (is_Third)
        {
            Vector3 tar = m_Targets.position - m_Targets.forward * Mathf.Cos(m_Angle) * m_CurDis + new Vector3(0.0f, Mathf.Sin(m_Angle) * m_CurDis, 0.0f);
            Vector3 direction = tar - m_Targets.position;
            int layermask = 1 << 9;//只检测与场景物体的碰撞
            if (Physics.Raycast(m_Targets.position, direction, out hit, m_CurDis, layermask))
            {
                tar = hit.point;
            }
            transform.position = tar;
            //Vector3 dis = (tar - transform.position);
            //if (dis.magnitude > (dis * Time.deltaTime * 30 / dis.magnitude).magnitude)
            //{
            //transform.position += dis / 10;
            //}
            //transform.LookAt(m_Targets.position/* + new Vector3(transform.forward.z, 0f, -transform.forward.x)*/, new Vector3(0f, 1f, 0f));
            Vector3 dif = (m_Targets.position - transform.position);
            float factor = Mathf.Sqrt(dif.x * dif.x + dif.z * dif.z);
            Vector3 inc = new Vector3(dif.z * m_CurDis / factor / m_CurDis, 0f, -dif.x * m_CurDis / factor / m_CurDis);
            //Vector3 upo = new Vector3((dif + inc).x * Mathf.Abs((dif + inc).y) / Mathf.Sqrt(Mathf.Pow((dif + inc).x, 2) + Mathf.Pow((dif + inc).z, 2)), Mathf.Sqrt(Mathf.Pow((dif + inc).x, 2) + Mathf.Pow((dif + inc).z, 2)), (dif + inc).z * Mathf.Abs((dif + inc).y) / Mathf.Sqrt(Mathf.Pow((dif + inc).x, 2) + Mathf.Pow((dif + inc).z, 2))) / m_CurDis;
            Vector3 upo = new Vector3(-(dif.y * dif.x) / factor / m_CurDis, factor / m_CurDis, -(dif.y * dif.z) / factor / m_CurDis);
            //(dif + inc).x*Mathf.Abs((dif + inc).y) / Mathf.Sqrt(Mathf.Pow((dif + inc).x, 2) + Mathf.Pow((dif + inc).z, 2))
            transform.LookAt(m_Targets.position + inc + upo, new Vector3(0f, 1f, 0f));
        }
        else
        {
            transform.position = m_Targets.position;
            transform.LookAt(m_Targets.position + m_Targets.forward * Mathf.Cos(m_Angle) - new Vector3(0.0f, Mathf.Sin(m_Angle), 0.0f), new Vector3(0f, 1f, 0f));
        }
    }

    private void AngleControl()
    {
        m_MouseInputValue = -Input.GetAxis(m_MouseY);
        float turn = m_MouseInputValue * m_AngleSpeed * Time.deltaTime;
        m_Angle += turn;
        if (m_Angle > m_MaxAngle) m_Angle = m_MaxAngle;
        if (m_Angle < m_MinAngle) m_Angle = m_MinAngle;

        m_MouseInputValue = -Input.GetAxis(m_MouseScrollWheel);
        float scroll = m_MouseInputValue * m_ScrollSpeed * Time.deltaTime;
        m_CurDis += scroll;
        if (m_CurDis > m_MaxDis) m_CurDis = m_MaxDis;
        if (m_CurDis < m_MinDis) m_CurDis = m_MinDis;
    }
}
