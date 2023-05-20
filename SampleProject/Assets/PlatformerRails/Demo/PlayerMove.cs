using UnityEngine;
using PlatformerRails;

[RequireComponent(typeof(MoverOnRails))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float Accelaration = 20f;
    [SerializeField]
    float Drag = 5f;
    [SerializeField]
    float JumpSpeed = 5f;
    [SerializeField]
    float Gravity = 15f;

    [SerializeField, Space]
    float GroundDistance = 0.5f;
    [SerializeField]
    float GroundCheckLength = 0.05f;

    MoverOnRails Controller;
    void Awake()
    {
        Controller = GetComponent<MoverOnRails>();
    }

	private void OnEnable()
	{
        Controller.OnLocalPositionUpdated += CorrectGroundDistance;
	}

	private void OnDisable()
	{
        Controller.OnLocalPositionUpdated -= CorrectGroundDistance;
    }

	void FixedUpdate()
    {
        var velocity = Controller.Velocity;
        //To make X value 0 means locate the character just above the rail
        velocity.x = -Controller.Position.x * 5f;
        //Changing Z value in local position means moving toward rail direction
        velocity.z += Input.GetAxisRaw("Horizontal") * Accelaration * Time.fixedDeltaTime;
        velocity.z -= Controller.Velocity.z * Drag * Time.fixedDeltaTime;
        //Y+ axis = Upwoard (depends on rail rotation)
        var distance = CheckGroundDistance();
        if (distance != null)
        {
            //Controller.Velocity.y = (GroundDistance - distance.Value) / Time.fixedDeltaTime; //ths results for smooth move on slopes
            velocity.y = 0f;
            if (Input.GetButtonDown("Jump"))
                velocity.y = JumpSpeed;
        }
        else
            velocity.y -= Gravity * Time.fixedDeltaTime;

        Controller.Velocity = velocity;
    }

    void CorrectGroundDistance()
	{
        var distance = CheckGroundDistance();
        if (distance != null)
            Controller.Position += Vector3.up * (GroundDistance - distance.Value);
    }

    float? CheckGroundDistance()
    {
        RaycastHit info;
        var hit = Physics.Raycast(transform.position, -transform.up, out info, GroundDistance + GroundCheckLength);
        if (hit)
            return info.distance;
        else
            return null;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.down * (GroundDistance + GroundCheckLength));
        Gizmos.DrawWireCube(Vector3.down * GroundDistance, Vector3.right / 2 + Vector3.forward / 2);
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}
