using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas CanvasUI;
    public List<Player> Players;

    private List<Jeu> JeuxALancer;

    private int IndexJeu = 0;


    // Start is called before the first frame update
    void Start()
    {
        JeuxALancer = new List<Jeu>();
        
    }

    public void MovePlayer()
    {
        Players[0].EngageMoving(new Vector3(33, 7, 12), 0.002f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EndGame(Jeu jeu)
    {
        jeu.gameObject.SetActive(false);
        IndexJeu++;
        if (IndexJeu >= JeuxALancer.Count)
        {
            IndexJeu = 0;
            JeuxALancer.Clear();
            ShowChoixJeux();
        }
        else
            StartJeu(JeuxALancer[IndexJeu]);
    }

    public void StartJeu(Jeu jeu)
    {
        CanvasUI.gameObject.SetActive(false);
        jeu.gameObject.SetActive(true);
        jeu.StartGame(IndexJeu);
    }

    public void ChooseGame(Jeu jeu)
    {
        if (JeuxALancer.Count >= 3) StartJeu(JeuxALancer[0]);
        JeuxALancer.Add(jeu);
        if (JeuxALancer.Count >= 3) StartJeu(JeuxALancer[0]);
    }

    public void DisableButton(Button button)
    {
        button.interactable = false;
    }

    public void ShowChoixJeux()
    {
        CanvasUI.gameObject.SetActive(true);
        foreach (Button buttons in CanvasUI.GetComponentsInChildren<Button>())
        {
            buttons.interactable = true;
        }
    }

    public void SetDifficulte(int i)
    {
        IndexJeu = i;
    }
}
