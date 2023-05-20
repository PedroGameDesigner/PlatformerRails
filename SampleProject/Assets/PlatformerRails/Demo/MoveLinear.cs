using UnityEngine;
using PlatformerRails;

[RequireComponent(typeof(MoverOnRails))]
public class MoveLinear : MonoBehaviour {
    [SerializeField]
    Vector3 Velocity;

    MoverOnRails Controller;
    void Start()
    {
        Controller = GetComponent<MoverOnRails>();
        Controller.Velocity = this.Velocity;
    }

    void FixedUpdate()
    {
        var velocity = Controller.Velocity;
        velocity.x = -Controller.Position.x * 5f;
        Controller.Velocity = velocity;
    }
}
