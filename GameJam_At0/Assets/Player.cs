using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool HasToMove = false;

    public Vector3 position;
    public Vector3 targetPosition;
    public float speed;
    public int IndexSerieDeJeu = 0;

    public List<Preferences> JeuxPref;


    [System.Serializable]
    public class Preferences
    {
        public GameObject PrefabPanelPref;
        public Jeu.IdOfGame[] IdJeuxPref;
       
    }
    

    void Update()
    {
        if (HasToMove)
        {
            Vector3 movement = (targetPosition - position);
            movement.Normalize();
            movement *= speed;

            if (movement.sqrMagnitude >= (targetPosition - position).sqrMagnitude)
            {
                position = targetPosition;
                HasToMove = false;
            }
            else
            {
                print(position);
                position += movement;
                gameObject.GetComponent<Transform>().position = movement;

            }

        }
    }

    public void EngageMoving(Vector3 targetPos, float speed)
    {
        //position = gameObject.GetComponent<Transform>().position;
        //targetPosition = targetPos;
        //this.speed = speed;
        //HasToMove = true;
    }
}
