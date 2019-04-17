using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallProjectile : MonoBehaviour, IProjectile
{
    public GameObject explosion;
    public GameObject hitMarker;
    public int baseDamage = 2;
    public float lifetime = 3f;
    public float gravity = -0.098f;
    private float timePassed;
    private Rigidbody rigidbody;
    private GameObject placedHitMarker;

    public IHurtboxController Hurtbox { get; set; }
    public Transform TargetTransform { get; set; }

    public AudioClip[] RockSoundsList;
    public AudioSource audioSource;

    void Awake()
    {
        Hurtbox = GetComponentInChildren<IHurtboxController>();
        rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        PlaceMarker();

        transform.localScale = new Vector3(Random.Range(0.3f, 1) * transform.localScale.x, Random.Range(0.3f, 1) * transform.localScale.y, Random.Range(0.3f, 1) * transform.localScale.z);
        rigidbody.AddTorque(new Vector3(Random.Range(-5,5), Random.Range(-5, 5), Random.Range(-5, 5)), ForceMode.Impulse);

        SetupAudioSource();
        audioSource.clip = RockSoundsList[Random.Range(0, RockSoundsList.Length - 1)];
        audioSource.volume = PlayerPrefs.GetFloat("volumeEffects")*0.25f;
        audioSource.Play();
    }

    public void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        AudioManager.OnSFXVolChange += OnVolumeChange;
    }

    void Update()
    {
        rigidbody.velocity += new Vector3(0f, gravity, 0f);

        timePassed += Time.deltaTime;
        if (timePassed > lifetime)
            OnHit(null);
    }

    public void OnHit(GameObject other)
    {
        if (explosion)
        {
            ObjectPoolController.current.CheckoutTemporary(explosion, transform.position, 2);
        }

        gameObject.SetActive(false);
        Destroy(placedHitMarker);
        Destroy(gameObject, 1f);
    }

    private void PlaceMarker()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.GetMask("StealthCollider", "Character", "Hurbox")))
        {
            placedHitMarker = Instantiate(hitMarker, new Vector3(transform.position.x, hit.point.y, transform.position.z), Quaternion.Euler(0,0,0));
        }
    }

    public void OnVolumeChange(float volume)
    {
        audioSource.volume = volume;
    }

    void OnDestroy()
    {
        AudioManager.OnSFXVolChange -= OnVolumeChange;
    }
}
