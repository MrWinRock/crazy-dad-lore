using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Transform target;  
    public float speed;

    void Awake()
    {
        // ถ้า target ไม่ถูก assign → หา TargetRed ใน parent
        if (target == null && transform.parent != null)
        {
            Transform t = transform.parent.Find("TargetRed");
            if (t != null) target = t;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.fixedDeltaTime
        );
    }
}