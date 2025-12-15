using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] string groundTag = "Ground"; // ¶¥°¨Áö ÅÂ±×
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
