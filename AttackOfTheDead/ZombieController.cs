//Created by Justin Simmons

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(AudioSource))]
public class ZombieController : MonoBehaviour
{
    [Tooltip("The type of character this object will be during play. Should be a scriptable object 'CharacterType'.")]
    public CharacterType zombieType;
    [Tooltip("The type of blood splatter that occurs upon death. Should be a game object with a particle effect attached.")]
    public GameObject bloodSplatterType;
    [Tooltip("The type of sound the zombie makes upon death. Should be an mp3 or wav file.")]
    public AudioClip zombieDeathSound;
    [Tooltip("The type of sound the zombie makes upon killing a survivor. Should be an mp3 or wav file.")]
    public AudioClip zombieEatingSound;

    [HideInInspector] public bool chooseTargetRandomly;
    [HideInInspector] public Vector3 zombieStartingPos;
    [HideInInspector] public Vector3 prioritySurvivorPos;
    [HideInInspector] public AudioSource zombieAudio;

    private NavMeshAgent zombieNav;
    private Rigidbody zombieRb;
    private HealthController healthController;
    private GameObject[] survivors;
    private GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        zombieNav = GetComponent<NavMeshAgent>();
        zombieRb = GetComponent<Rigidbody>();
        zombieAudio = GetComponent<AudioSource>();
        healthController = GetComponentInChildren<HealthController>();
        survivors = GameObject.FindGameObjectsWithTag("Survivor");
    }

    private void Start()
    {
        //Sets the max health, starting position, sound, and speed of the zombie
        healthController.SetMaxHealth(zombieType.characterHealth);
        healthController.RestoreHealth();
        zombieStartingPos = this.transform.position;
        zombieAudio.clip = zombieDeathSound;
        SetZombieSpeed(zombieType.characterSpeed);

        //Checks for the closest survivor to target every second
        InvokeRepeating("FindSurvivorTarget", 1f, 1f);
    }

    //Updates the movement of the zombie based on if the target changes
    private void Update()
    {
        if (prioritySurvivorPos != null)
            zombieNav.SetDestination(prioritySurvivorPos);
    }

    public void FindSurvivorTarget()
    {
        zombieRb.velocity = Vector3.zero;

        if (chooseTargetRandomly)
        {
            ChooseRandomSurvivor();
            chooseTargetRandomly = false;
        }
        else
        {
            float target = 1000;

            foreach (GameObject survivor in survivors)
            {
                if (survivor != null)
                {
                    var trans = survivor.transform;
                    var dist = Vector3.Distance(trans.position, this.transform.position);

                    if (dist < target)
                    {
                        prioritySurvivorPos = trans.position;
                        target = dist;
                    }
                }
            }
        }
    }

    //Chooses a survivor randomly
    private void ChooseRandomSurvivor()
    {
        prioritySurvivorPos = survivors[Random.Range(0, survivors.Length)].transform.position;
    }

    //Sets a new zombie speed
    public void SetZombieSpeed(float newSpeed)
    {
        zombieNav.speed = newSpeed;
    }

    //Creates a blood splatter and zombie sound when dying
    public void ZombieDeath()
    {
        zombieAudio.clip = zombieDeathSound;
        zombieAudio.Play();

        var newBlood = Instantiate(bloodSplatterType, this.transform.position, this.transform.rotation);
        Destroy(newBlood, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //SURVIVOR INTERACTIONS//
        if (collision.gameObject.CompareTag("Survivor"))
        {
            var trans = collision.gameObject.transform;

            zombieAudio.clip = zombieEatingSound;
            zombieAudio.Play();

            Instantiate(zombieType.zombieInfectionPrefab, trans.position, trans.rotation);

            Destroy(collision.gameObject);

            gm.survivorsRemainingCount--;
        }
    }
}