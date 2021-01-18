using System.Collections;
using UnityEngine;

public class Fighter : MonoBehaviour
{

    private GameController gameController;
    private int health = 100,
        damage = 10,
        speed = 30,
        range = 10;

    // [SerializeField] private GameObject trace;

    [SerializeField] private int teamNum;

    public int Health { get => health; set => health = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Range { get => range; set => range = value; }


    // Start is called before the first frame update
    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

    }


    public void ReceiveDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            Destroy(gameObject);

    }

    private IEnumerator Follow(GameObject enemy)
    {
        Transform start = transform;
        float count = 0;

        while (enemy && Vector3.Distance(transform.position, enemy.transform.position) > 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            if (enemy)
            {
                transform.position = Vector3.Lerp(start.position, enemy.transform.position, 0.01f * count);
                count += speed / 10000f;
            }
        }
    }



    private IEnumerator Attack(GameObject enemy)
    {

        // GameObject attackTrace = null;

        while (enemy)
        {
            yield return new WaitForSeconds(1f);
            if (enemy)
                if (Vector3.Distance(transform.position, enemy.transform.position) <= Range / 10)
                {

                    enemy.GetComponent<Fighter>().ReceiveDamage(Damage);

                    /*
                    Destroy(attackTrace);
                    attackTrace = Instantiate(trace, transform.parent);
                    LineRenderer traceLR = trace.GetComponent<LineRenderer>();
                    traceLR.SetPositions(new Vector3[] { transform.position, enemy.transform.position });
                    traceLR.startColor = teamNum == 0 ? Color.red : Color.blue;
                    traceLR.endColor = traceLR.startColor;
                    */

                }
        }

        ChangeTarget();
    }

    private void ChangeTarget()
    {
        GameObject newtarget = gameController.FindClosestEnemy(transform, teamNum);
        if (newtarget)
        {
            StartCoroutine(Follow(newtarget));
            StartCoroutine(Attack(newtarget));
        }
    }

    public void StartGame()
    {
        ChangeTarget();
    }
}
