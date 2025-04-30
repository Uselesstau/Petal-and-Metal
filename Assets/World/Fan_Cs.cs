using UnityEngine;

public class Fan_Cs : MonoBehaviour
{
    [SerializeField] private float fanForce;
    [SerializeField] private Animator animator;
    
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        float width = transform.localScale.x * 2;
        for (int i = 0; i < 9; i++)
        {
            Vector3 pos = transform.rotation * new Vector3(width/5 * (i + 1) - width,0,0);
            LayerMask mask = LayerMask.GetMask("Petal") + LayerMask.GetMask("Metal") + LayerMask.GetMask("Box");
            RaycastHit2D hit = Physics2D.Raycast(transform.position + pos, transform.up, 5, mask);
            Debug.DrawRay(transform.position + pos, transform.up * 5);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * fanForce, ForceMode2D.Force);
            }
        }
        
        animator.speed = fanForce/40f;
    }
}
