using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Range(1f, 1000f)]
    public float TurnSpeed = 100f;

    [Range(1f, 50f)]
    public float MoveSpeed = 10f;

    public Transform Player;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        if (mouseX != 0f && Input.GetButton("Fire2"))
        {
            Vector3 mouseTurn = new Vector3(0f, mouseX * TurnSpeed * Time.deltaTime, 0f);
            this.transform.Rotate(mouseTurn);
        }

        if (this.transform.position != Player.position)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, Player.position, MoveSpeed * Time.deltaTime);
        }
    }
}