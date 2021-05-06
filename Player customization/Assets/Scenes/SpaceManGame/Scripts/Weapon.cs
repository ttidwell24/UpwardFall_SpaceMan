using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //
    public int Damage = 10;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform hitPreFab;
    public Transform MuzzleFlashPreFab;

    private float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    // handle camera shaking
    public float camShakeAmt = 0.05f;
    public float camShakeLengh = 0.1f;
    CameraShake camShake;

    public string weaponShootSound = "DefaultShot";

    private float timeToFire = 0;
    Transform firePoint;

    private PlayerStats stats;

    //caching
    AudioManager audioManager;

    // Start is called before the first frame update
    void Awake ()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint? WHAT?!");
        }
    }

    private void Start()
    {
       camShake = GameMaster.gm.GetComponent<CameraShake>();

        if (camShake == null)
        {
            Debug.LogError("No cameraShake script found on GM object.");
        }

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("FREAK OUT! No audiomanager found in scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        stats = PlayerStats.instance;

        if (stats.fireRate == 0)
        {
            if (Input.GetButtonDown ("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton ("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1/stats.fireRate;
                Shoot();
            }
        }
    }

    void Shoot ()
    {
        Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine (firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage.");
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Damage);
                if (stats.fireRate >= 7)
                {
                    audioManager.PlaySound("MachineGunHit");
                }
                else
                {
                    audioManager.PlaySound("Hit");
                } 
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
                

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect (Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.06f);

        if (hitNormal != new Vector3 (9999, 9999, 9999))
        {
            Instantiate(hitPreFab, hitPos, Quaternion.FromToRotation (Vector3.forward, hitNormal));
        }

        Transform clone = (Transform)Instantiate(MuzzleFlashPreFab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3 (size, size, size);
        Destroy (clone.gameObject, 0.02f);

        //shake the camera
        camShake.Shake(camShakeAmt, camShakeLengh);

        //Play shoot sound
        if (stats.fireRate >= 7)
        {
            audioManager.PlaySound("MachineGunShoot");
        }
        else
        {
            audioManager.PlaySound(weaponShootSound);
        }   
    }

}
