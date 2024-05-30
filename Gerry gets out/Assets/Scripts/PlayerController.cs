using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region private
    CharacterController p_CharacterController;
    #endregion
    #region public
    public int m_Speed;
    public int m_RotationSpeed;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        p_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Player movement
        Vector3 movement = new Vector3(horizontalInput,0, verticalInput);
        movement = movement.normalized;
        p_CharacterController.Move(movement * Time.deltaTime * m_Speed);

        // Player rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle+90, 0));
    }
}
