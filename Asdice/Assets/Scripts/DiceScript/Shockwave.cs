using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField]
    private int points;

    [SerializeField]
    private float maxRadius;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float startWidth;

    [SerializeField]
    private float force;

    [SerializeField]
    private LineRenderer linerender;

    private void Awake()
    {
        linerender.positionCount = points + 1;
    }

    public async UniTask Blast() //responsible for blast effect
    {
        linerender.positionCount = points + 1;
        float currentRadius = 0f; // finds the current radius of shockwave
        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed; //increases the radius till it reaches the target radius
            Draw(currentRadius);
            Damage(currentRadius);
            await UniTask.Yield();
        }
    }
    
    private void Damage(float currentRadius) //responsible for the force exerted on objects
    {
        Collider[] hittingObjects = Physics.OverlapSphere(transform.position, currentRadius);

        for(int i = 0; i < hittingObjects.Length; i++)
        {
            Rigidbody rb = hittingObjects[i].GetComponent<Rigidbody>();

            if(!rb)
            {
                continue;
            }

            Vector3 direction = (hittingObjects[i].transform.position - transform.position).normalized;
            rb.AddForce(direction * force, ForceMode.Impulse); //adds an impulse to every object that comes into contact
        }
    }


    private void Draw(float currentRadius) //responsible for drawing the shockwave
    {
        float anglebetween = 360f / points;

        for ( int i = 0; i <= points; i++) //in this part the shockwave travels with respect to the direction and position of the points
        {
            float angle = i * anglebetween * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius; 

            linerender.SetPosition(i, position); 
        }

        linerender.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius/maxRadius);
    }
}
