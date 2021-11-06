using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuCombat : Jeu
{
    public bool IsActive = false;

    [Header("Gestion Input")]
    public string[] ListeToucheAAppuye;
    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
    public int NbInputAFaireParInput = 3;
    public int nbTourAFaire = 3;
    public float DureeDeVieInput = 3f;
    
    private int NbInputFait = 0;
    private int NbToursFait = 0;
    private AppuiInput InputScript;
    private GameObject prefabSpawned;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;
    
    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;
    

    // Start is called before the first frame update
    void Start()
    {
       // StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive && Input.GetKeyDown(InputScript.KeyCode))
        {
            NbInputFait++;
            if (NbInputFait >= NbInputAFaireParInput)
            {
                EndTour();
            }
        }
    }

    public override void StartGame()
    {
        IsActive = true;
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        Vector2 pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        
        prefabSpawned = Instantiate(PrefabInputToSpam, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        prefabSpawned.transform.SetParent(gameCanvas.transform, false);
        InputScript = prefabSpawned.GetComponent<AppuiInput>();
        InputScript.SetKeyCode(ListeToucheAAppuye[Random.Range(0, ListeToucheAAppuye.Length-1)]);

        StartCoroutine(AutoKillInput(prefabSpawned));
    }
    private void ResetGame()
    {
        NbInputFait = 0;
        NbToursFait = 0;
    }

    IEnumerator AutoKillInput(GameObject prefab)
    {
        
        yield return new WaitForSeconds(DureeDeVieInput);

        if (prefab == null)
        {
            print("deja mort");
        }
        else
        {
            NbInputFait = 0;
            Destroy(prefabSpawned);
            //perdre 1PV
            SpawnPrefab();
        }
    }

    private void EndTour()
    {
        NbToursFait++;
        Destroy(prefabSpawned);
        if (NbToursFait >= nbTourAFaire)
        {
            ResetGame();
            gameManager.EndGame(this);
            print("fin du jeu");
        }
        else
        {
            NbInputFait = 0;
            SpawnPrefab();
        }
    }
}
