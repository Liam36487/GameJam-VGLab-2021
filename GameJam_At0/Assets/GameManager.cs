using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas CanvasUI;
    public List<Player> Players;

    private List<Jeu> JeuxALancer;

    public AudioClip sonBouton;

    private int IndexJeu = 0;
    private int IndexPlayer = 0;

    
    private int IndexJeuAVerif;

    private int CptGoodGame = 0;

    GameObject PanelTextAffiche;

    public GameObject CanvasFinJeu;
    public GameObject CanvasFinJeuFinal;

    // Start is called before the first frame update
    void Start()
    {
        JeuxALancer = new List<Jeu>();
        PanelTextAffiche = Instantiate(Players[IndexPlayer].JeuxPref[Players[IndexPlayer].IndexSerieDeJeu].PrefabPanelPref, CanvasUI.transform);
    }

    public void PlayerLeave(Player player)
    {
        if(Players.IndexOf(player) == 0)
            Players[0].GetComponent<Animator>().Play("Base Layer.Garcon_sortie", 0, 0);
        if (Players.IndexOf(player) == 1)
            Players[1].GetComponent<Animator>().Play("Base Layer.Fille_sortie", 0, 0);
        if (Players.IndexOf(player) == 2)
            Players[2].GetComponent<Animator>().Play("Base Layer.Lilly_sortie", 0, 0);
    }
    public void PlayerEnter(Player player)
    {
        if (Players.IndexOf(player) == 0)
            Players[0].GetComponent<Animator>().Play("Base Layer.Garcon_Entree", 0, 0);
        if (Players.IndexOf(player) == 1)
            Players[1].GetComponent<Animator>().Play("Base Layer.Fille_Entree", 0, 0);
        if (Players.IndexOf(player) == 2)
            Players[2].GetComponent<Animator>().Play("Base Layer.Lilly_Entree", 0, 0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EndGame(Jeu jeu)
    {
        jeu.gameObject.SetActive(false);
        StartCoroutine(WaitEndGame(jeu));
    }

    private IEnumerator WaitEndGame(Jeu jeu)
    {
        CanvasFinJeu.SetActive(true);
        CanvasFinJeu.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(1.5f);

        CanvasFinJeu.SetActive(false);
        IndexJeu++;
        if (IndexJeu >= JeuxALancer.Count)
        {
            IndexJeu = 0;
            IndexJeuAVerif = 0;
            CptGoodGame = 0;
            JeuxALancer.Clear();
            ChangePlayer();
        }
        else
            StartJeu(JeuxALancer[IndexJeu]);

    }

    public void StartJeu(Jeu jeu)
    {
        Destroy(PanelTextAffiche);
        CanvasUI.gameObject.SetActive(false);
        jeu.gameObject.SetActive(true);
        jeu.StartGame(IndexJeu);
    }

    public void ChooseGame(Jeu jeu)
    {
        if (JeuxALancer.Count >= 3) StartJeu(JeuxALancer[0]);
        JeuxALancer.Add(jeu);
        print("IndexPlayer : " + IndexPlayer + "\tIndexSerieDeJeu : " + Players[IndexPlayer].IndexSerieDeJeu + "\tIndexJeuVerif : " + IndexJeuAVerif);
        if (Players[IndexPlayer].JeuxPref[Players[IndexPlayer].IndexSerieDeJeu].IdJeuxPref[IndexJeuAVerif] == jeu.IdJeu)
        {
            CptGoodGame++;
        }
        IndexJeuAVerif++;
        if (JeuxALancer.Count >= 3)
        {
            if (CptGoodGame != 3)
            {
                IndexJeu = 0;
                IndexJeuAVerif = 0;
                CptGoodGame = 0;
                JeuxALancer.Clear();
                ShowChoixJeux();
            }
            else
            {
                Players[IndexPlayer].IndexSerieDeJeu++;
                //Players[IndexPlayer].JeuxPref.RemoveAt(IndexSerieDeJeu);
                StartJeu(JeuxALancer[0]);
            }
        }
    }

    public void DisableButton(Button button)
    {
        button.interactable = false;
    }

    private void ChangePlayer()
    {
        Destroy(PanelTextAffiche);
        PlayerLeave(Players[IndexPlayer]);
        IndexPlayer++;
        if (IndexPlayer >= Players.Count)
        {
            if (Players[IndexPlayer - 1].IndexSerieDeJeu >= 3)
            {
                CanvasFinJeuFinal.SetActive(true);
                print("FIN DU GAMEGAME");
            }
            else
            {
                IndexPlayer = 0;
                ShowChoixJeux();
                PanelTextAffiche = Instantiate(Players[IndexPlayer].JeuxPref[Players[IndexPlayer].IndexSerieDeJeu].PrefabPanelPref, CanvasUI.transform);
                PlayerEnter(Players[IndexPlayer]);
            }

        }
        else
        {
            ShowChoixJeux();
            PanelTextAffiche = Instantiate(Players[IndexPlayer].JeuxPref[Players[IndexPlayer].IndexSerieDeJeu].PrefabPanelPref, CanvasUI.transform);
            PlayerEnter(Players[IndexPlayer]);
        }
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

    public void PlaySoundButton()
    {
        GetComponent<AudioSource>().PlayOneShot(sonBouton);
    }
}
