using System.Collections.Generic;
using UnityEngine;

public class Searchlight : MonoBehaviour
{
    private List<Monster> visibleMonsters = new List<Monster>();
    private float rotateSpeed = 75f;

    private void Update()
    {
        // TODO do we want mouse controls?
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

        float rotationDirection = -Input.GetAxisRaw("Horizontal");
        transform.rotation *= Quaternion.Euler(0, 0, rotationDirection * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster otherMonster = other.GetComponent<Monster>();
        if (otherMonster)
        {
            visibleMonsters.Add(otherMonster);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Monster otherMonster = other.GetComponent<Monster>();
        if (otherMonster)
        {
            visibleMonsters.Remove(otherMonster);
        }
    }

    // TODO maybe should get closest first
    public Monster GetRandomTarget()
    {
        if (visibleMonsters == null || visibleMonsters.Count == 0) return null;
        
        return visibleMonsters[Random.Range(0, visibleMonsters.Count)];
    }
    
    public bool IsTargetVisible(Monster target)
    {
        return visibleMonsters.Contains(target);
    }
}
