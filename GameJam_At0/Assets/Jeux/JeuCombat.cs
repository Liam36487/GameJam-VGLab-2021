using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuCombat : Jeu
{
    public AK.Wwise.Event PunchSound;
    public AK.Wwise.Event badpunchsound;
    public AK.Wwise.Event transitionSound;
     
    public bool IsActive = false;

    public float shakerRateGoodInput = 0.2f;
    public float shakerRateBadInput = 0.5f;


    public List<Difficulte> Difficultes;

    [Header("Gestion Input")]
    public string[] ListeToucheAAppuye;
    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
    
    
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

    private int NumDifficulteActuelle = 0;
    private StressReceiver Shaker;

    [System.Serializable]
    public class Difficulte
    {
        public int NbInputAFaireParInput = 3;
        public int nbTourAFaire = 3;
        public float DureeDeVieInput = 3f;
        public float TempsEntreLesSpawn = 1f;
    }


    // Start is called before the first frame update
    void Start()
    {
        // StartGame();
        if (Camera.main != null)
        {
            Shaker = Camera.main.GetComponent<StressReceiver>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive && prefabSpawned != null && Input.GetKeyDown(InputScript.KeyCode))
        {
            NbInputFait++;
            // AkSoundEngine.PostEvent("punch", gameObject);
            PunchSound.Post(gameObject);
            if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
            if (NbInputFait >= Difficultes[NumDifficulteActuelle].NbInputAFaireParInput)
            {
                EndTour();
            }
        }

        if (IsActive && prefabSpawned != null && Input.anyKeyDown && !Input.GetKeyDown(InputScript.KeyCode))
        {
            EndTour();
            NbToursFait--;
            badpunchsound.Post(gameObject);
            if (Shaker != null) Shaker.InduceStress(shakerRateBadInput);
        }
    }

    public override void StartGame(int numDifficulte)
    {
        IsActive = true;
        if (numDifficulte >= Difficultes.Count)
            NumDifficulteActuelle = Difficultes.Count - 1;
        else
            NumDifficulteActuelle = numDifficulte;
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {

        Vector2 pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        
        prefabSpawned = Instantiate(PrefabInputToSpam, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        prefabSpawned.transform.SetParent(gameCanvas.transform, false);
        InputScript = prefabSpawned.GetComponent<AppuiInput>();
        InputScript.SetKeyCode(ListeToucheAAppuye[Random.Range(0, ListeToucheAAppuye.Length-1)]);
        transitionSound.Post(gameObject);
        StartCoroutine(AutoKillInput(prefabSpawned));
    }
    private void ResetGame()
    {
        NbInputFait = 0;
        NbToursFait = 0;
    }

    IEnumerator AutoKillInput(GameObject prefab)
    {
        
        yield return new WaitForSeconds(Difficultes[NumDifficulteActuelle].DureeDeVieInput);

        if (prefab == null)
        {
            print("deja mort");
        }
        else
        {
            NbInputFait = 0;
            Destroy(prefabSpawned);
            StartCoroutine(WaitBeforeSpawn(Difficultes[NumDifficulteActuelle].TempsEntreLesSpawn));
        }
    }

    private void EndTour()
    {
        NbToursFait++;
        Destroy(prefabSpawned);
        if (NbToursFait >= Difficultes[NumDifficulteActuelle].nbTourAFaire)
        {
            ResetGame();
            gameManager.EndGame(this);
            print("fin du jeu");
        }
        else
        {
            NbInputFait = 0;
            StartCoroutine(WaitBeforeSpawn(Difficultes[NumDifficulteActuelle].TempsEntreLesSpawn));
        }
    }

    IEnumerator WaitBeforeSpawn(float tempsSpawnApresFinDeTour)
    {
        yield return new WaitForSeconds(tempsSpawnApresFinDeTour);

        SpawnPrefab();
    }
}
