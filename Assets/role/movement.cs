using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public float m_speed;  //move speed
    public float m_TurnSpeed;
    public float m_JumpForce;
    public float m_InitialHeight;
    public string id;
    public string nickname;
    [HideInInspector] public bool m_TurnControl;
    [HideInInspector] public bool m_MoveControl;

    private GameObject step_effect;
    private GameObject jump_effect;
    private Rigidbody m_Rigidbody;
    private string m_forward;
    private string m_backforward;
    private string m_left;
    private string m_right;
    private string m_MouseX;
    private string m_MouseY;
    private string m_Jump;
    private float m_MouseInputValue;
    private bool m_IsTouching = true;

    public void EnableTurnControl()
    {
        m_TurnControl = true;
    }

    public void DisableTurnControl()
    {
        m_TurnControl = false;
    }

    public void DisableMoveControl()
    {
        m_MoveControl = false;
    }

    public void EnableMoveControl()
    {
        m_MoveControl = true;
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void setup(string id,string nickname)
    {
        name = id;
        this.id = id;
        this.nickname = nickname;
        GetComponentsInChildren<Text>()[0].text = nickname;
    }

    // Use this for initialization
    private void Start()
    {
        DisableTurnControl();
        DisableMoveControl();
        m_InitialHeight = transform.position.y+0.01f;
        m_forward = "w";
        m_backforward = "s";
        m_left = "a";
        m_right = "d";
        m_MouseX = "Mouse X";
        m_MouseY = "Mouse Y";
        m_Jump = "Jump";
        step_effect = GameObject.Find("step_sound");
        jump_effect = GameObject.Find("jump_sound");
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void Move()
    {
        /*if (Input.GetButton(m_forward))
        {
            Vector3 movement = transform.forward * m_speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
        if (Input.GetButton(m_backforward))
        {
            Vector3 movement = -transform.forward * m_speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
        if (Input.GetButton(m_left))
        {
            Vector3 movement = new Vector3(-transform.forward.z, 0f, transform.forward.x) * m_speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
        if (Input.GetButton(m_right))
        {
            Vector3 movement = new Vector3(transform.forward.z, 0f, -transform.forward.x) * m_speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }*/
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = transform.forward * vertical + transform.right * horizontal;
        m_Rigidbody.velocity = new Vector3(m_speed * direction.x, m_Rigidbody.velocity.y, m_speed * direction.z);

        float InputValuex = Input.GetAxis("Vertical"); 
         /*Vector3 movement = transform.forward * InputValuex * m_speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);*/
        float InputValuey = Input.GetAxis("Horizontal");
        /*movement = new Vector3(-transform.forward.z, 0f, transform.forward.x) * (-InputValuey) * m_speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);*/
        if (((InputValuex != 0 || InputValuey != 0) && m_Rigidbody.velocity.y <= 0.01f && m_Rigidbody.velocity.y >= -0.01f) && !step_effect.GetComponent<AudioSource>().isPlaying)//若水平方向有速度且不在空中则播放走路音效
        {
            step_effect.GetComponent<AudioSource>().Play();
        }
        else if(((InputValuex == 0 && InputValuey == 0) || (m_Rigidbody.velocity.y > 0.01f || m_Rigidbody.velocity.y < -0.01f)) && step_effect.GetComponent<AudioSource>().isPlaying)//若水平方向无速度或者在空中则停止播放走路音效
        {
            step_effect.GetComponent<AudioSource>().Stop();
        }

    }

    private void Turn()
    {
        m_MouseInputValue = Input.GetAxis(m_MouseX);
        float turn = m_MouseInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    private void OnCollisionEnter()
    {
        m_IsTouching = true;
    }

    private void Jump()
    {
        if (Input.GetButtonDown(m_Jump) && m_Rigidbody.velocity.y <= 0.01f && m_Rigidbody.velocity.y >= -0.01f) //按下跳跃键同时竖直分速度为零则启动跳跃
        {
            m_IsTouching = false;
            m_Rigidbody.velocity = new Vector3(0f, 1f, 0f) * m_JumpForce;
            jump_effect.GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + new Vector3(transform.forward.x, 0f, transform.forward.z));

        //GameObject.Find("nickname").GetComponent<RectTransform>().rotation = GameObject.Find("Main Camera").transform.rotation;让昵称对准相机貌似行不通
        if (m_TurnControl) Turn();
        if (m_MoveControl) {
            Move();
            Jump();
        }
    }
}
