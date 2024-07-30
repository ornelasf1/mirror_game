
using System.Threading;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public int speed = 100;

    public Material material;
    public HealthBar healthBar;
    [SerializeField] private LayerMask layerThatIgnoresLaser;
    private float damageMultiplier = 10f;

    void Awake() {
        Vector3 laserStartPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.7f, 0));
        transform.position = new Vector3(laserStartPosition.x, laserStartPosition.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStateManager.Instance.IsGameActive()) {
            return;
        }
        // if (Input.GetKey(KeyCode.A))
        //     transform.Rotate(Vector3.forward * speed * Time.deltaTime);

        // if (Input.GetKey(KeyCode.D))
        //     transform.Rotate(Vector3.back * speed * Time.deltaTime);

        Destroy(GameObject.Find("Laser Beam"));
        // if (Input.GetKey(KeyCode.Space))
        // {
        // }
        new LaserBeam(transform.position, transform.up, material, layerThatIgnoresLaser);
    }

    public void DealSelfDamage(int damage) {
        healthBar.DealDamage(damage * Time.deltaTime * damageMultiplier);

    }
}
