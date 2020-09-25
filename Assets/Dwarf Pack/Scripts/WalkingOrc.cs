using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WalkingOrc : MonoBehaviour
{
    private Animator animator;
    //public float walkspeed = 5;
    private float horizontal;
    private float vertical;
    //private float rotationDegreePerSecond = 1000;
    private bool isAttacking = false;
    public GameObject gamecam;
    public Vector2 camPosition;
    private bool dead;
    public Vector3 finalDwarfLookDir;
    public float dwarfLagSpeed = 10f;
    public GameObject character;
    public float closestDwarfEnemyDistance;
    public int dwarfTypeId;

    public Camera cam;
    public NavMeshAgent agent;
    public Vector3 activeDwarfPos;
    public DwarfEnum dwarfType;

    public bool isDwarfPlaying = true;
    void Start()
    {
        setCharacter();
        dead = false;
        this.transform.position = new Vector3(0, 0, -15.8f);
    }

    void FixedUpdate()
    {
        if (animator && !dead)
        {
            //walk
            horizontal = -activeDwarfPos.x;//Input.GetAxis("Horizontal");
            vertical = -activeDwarfPos.z;// Input.GetAxis("Vertical");

            Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
            float speedOut;

            if (stickDirection.sqrMagnitude > 1) stickDirection.Normalize();

            if (!isAttacking && Vector3.Distance(this.transform.position, activeDwarfPos) > 0.5f)

                speedOut = stickDirection.sqrMagnitude;
            else
                speedOut = 0;

            //if (stickDirection != Vector3.zero && !isAttacking)
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(stickDirection, Vector3.up), rotationDegreePerSecond * Time.deltaTime);
            //GetComponent<Rigidbody>().velocity = transform.forward * speedOut * walkspeed + new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            animator.SetFloat("Speed", speedOut);
        }
    }

    void Update()
    {
        if (!dead)
        {
            if (GameManager.Instance.EnemyList[0] != null)
            {
                ClosestEnemy();
            }

            // move camera
            if (gamecam)
                gamecam.transform.position = transform.position + new Vector3(0, camPosition.x, -camPosition.y);

            // attack  Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") &&  

            if (Vector3.Distance(this.transform.position, activeDwarfPos) < 0.5f)
            {


                DwarfRotation();
                if (!isAttacking && closestDwarfEnemyDistance < GameManager.Instance.DwarfDataList[dwarfTypeId].range)
                {

                    isAttacking = true;
                    //animator.Play("Attack");
                    if (isDwarfPlaying)
                    {
                        animator.SetTrigger("Attack");
                        isDwarfPlaying = false;
                    }
                    StartCoroutine(stopAttack(1));
                    activateTrails(true);
                }
            }
            else
            {
                agent.SetDestination(activeDwarfPos);
            }

            animator.SetBool("isAttacking", isAttacking);

            #region switch character
            //switch character

            //if (Input.GetKeyDown("left"))
            //{
            //	setCharacter(-1);
            //	isAttacking = true;
            //	StartCoroutine(stopAttack(1f));
            //}

            //if (Input.GetKeyDown("right"))
            //{
            //	setCharacter(1);
            //	isAttacking = true;
            //	StartCoroutine(stopAttack(1f));
            //}

            // death
            //if (Input.GetKeyDown("m"))
            //    StartCoroutine(selfdestruct());
            #endregion
        }

    }

    public IEnumerator stopAttack(float lenght)
    {
        yield return new WaitForSeconds(lenght); // attack lenght
        isAttacking = false;
        activateTrails(false);
        if (dwarfType == 0)
        {
            GameManager.Instance.GetDwarfDamage(dwarfType, GameManager.Instance.bestTarget);
        }
    }

    public IEnumerator selfdestruct()
    {
        animator.SetTrigger("isDead");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        dead = true;

        yield return new WaitForSeconds(1.3f);
        GameObject.FindWithTag("GameController").GetComponent<gameContoller>().resetLevel();
    }

    public void setCharacter()
    {
        character.SetActive(true);
        if (character.GetComponent<triggerProjectile>())
            character.GetComponent<triggerProjectile>().clearProjectiles();
        animator = GetComponentInChildren<Animator>();
    }

    public void activateTrails(bool state)
    {
        var tails = GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer tt in tails)
        {
            tt.enabled = state;
        }
    }
    public void SetPosition(Vector3 dwarfPos)
    {
        activeDwarfPos = dwarfPos;
    }

    public void ClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Vector3 currentPos = this.transform.position;
        foreach (var target in GameManager.Instance.EnemyList)
        {
            Vector3 directionToTarget = target.transform.position - currentPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;

                GameManager.Instance.bestTarget = target;
                closestDwarfEnemyDistance = Vector3.Distance(this.gameObject.transform.position, GameManager.Instance.bestTarget.transform.position);
            }
        }
    }

    public void DwarfRotation()
    {
        if (GameManager.Instance.bestTarget != null)
        {
            
            Vector3 dwarfLookDir = character.transform.position - GameManager.Instance.bestTarget.transform.position;
            dwarfLookDir.y = 0f;
            finalDwarfLookDir = Vector3.Lerp(finalDwarfLookDir, dwarfLookDir, Time.deltaTime * dwarfLagSpeed);
            character.transform.rotation = Quaternion.LookRotation(-finalDwarfLookDir);
        }
    }



}


















//if (Input.GetMouseButtonDown(0))
//{

//    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//    RaycastHit rayHit;

//    if (Physics.Raycast(ray, out rayHit))
//    {

//        agent.SetDestination(rayHit.point);
//    }
//    //
//}