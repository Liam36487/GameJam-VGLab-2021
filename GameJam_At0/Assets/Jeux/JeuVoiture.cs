using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuVoiture : Jeu
{
    public AudioSource sourceBase;

    public AudioClip crashsound;


    public bool IsActive = false;
    public float shakerRateGoodInput = 0.1f;
    public float shakerRateBadInput = 0.3f;
    
    public List<Difficulte> Difficultes;
    [Header("Gestion Input")]
    public List<KeyImagePair> ListeToucheAAppuye;
    public GameObject PrefabInputToSpam;
    //o� afficher les inputs � faire
    public Canvas gameCanvas;

    public int idJeu = 4;
    
    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;
    
    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;
    
    private int NbInputFait = 0;
    private AppuiInput InputScript;
    private GameObject prefabSpawned;
    private float RandomTimer;
    private int NumDifficulteActuelle = 0;
    private StressReceiver Shaker;

    [System.Serializable]
    public class Difficulte
    {
        public int NbInputAFaire = 3;
        public float DureeDeVieInput = 1f;
        public float DureePauseQuandErreur = 0.5f;

        [Header("Random Spawn Range APRES DESTRUCTION")]
        public float MinRangeSpawn = 2f;
        public float MaxRangeSpawn = 5f;
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
        if (Input.anyKeyDown)
        {
            print("active : " + IsActive + "\tPrefabSpawned : " + (prefabSpawned != null));
            if (InputScript != null) print("Key : " + Input.GetKeyDown(InputScript.KeyCode));
        }
        if (IsActive && prefabSpawned != null && Input.GetKeyDown(InputScript.KeyCode))
        {
            print("keypressed " + Input.GetKeyDown(InputScript.KeyCode));
            NbInputFait++;
            sourceBase.PlayOneShot(InputScript.Clip);
            if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
            Destroy(prefabSpawned);
            if (NbInputFait >= Difficultes[NumDifficulteActuelle].NbInputAFaire)
            {
                ResetGame();
                gameManager.EndGame(this);
                print("fin du jeu");
            }
        }
        else if (IsActive && prefabSpawned != null && Input.anyKeyDown && !Input.GetKeyDown(InputScript.KeyCode))
        {
            //Jouer son accident
            NbInputFait--;
            sourceBase.PlayOneShot(crashsound);
            if (Shaker != null) Shaker.InduceStress(shakerRateBadInput);
            if (NbInputFait < 0) NbInputFait = 0;
            Destroy(prefabSpawned);
            //StartCoroutine(StopTime());
        }
        else if (IsActive && prefabSpawned == null && Input.anyKeyDown)
        {
            //Jouer son accident
            NbInputFait--;
            sourceBase.PlayOneShot(crashsound);
            if (Shaker != null) Shaker.InduceStress(shakerRateBadInput);
            if (NbInputFait < 0) NbInputFait = 0;
        }

        if (IsActive && prefabSpawned == null)
        {
            RandomTimer -= Time.deltaTime;
            if (RandomTimer <= 0)
            {
                SpawnPrefab();
                RandomTimer = Random.Range(Difficultes[NumDifficulteActuelle].MinRangeSpawn, Difficultes[NumDifficulteActuelle].MaxRangeSpawn);
            }
        }
    }

    public override void StartGame(int numDifficulte)
    {
        IsActive = true;
        if (numDifficulte >= Difficultes.Count)
            NumDifficulteActuelle = Difficultes.Count - 1;
        else
            NumDifficulteActuelle = numDifficulte;

        RandomTimer = Random.Range(Difficultes[NumDifficulteActuelle].MinRangeSpawn, Difficultes[NumDifficulteActuelle].MaxRangeSpawn);
    }

    IEnumerator StopTime()
    {
        IsActive = false;

        yield return new WaitForSeconds(Difficultes[NumDifficulteActuelle].DureePauseQuandErreur);
        
        IsActive = true;
    }

    private void SpawnPrefab()
    {
        Vector2 pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        
        prefabSpawned = Instantiate(PrefabInputToSpam, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        prefabSpawned.transform.SetParent(gameCanvas.transform, false);
        InputScript = prefabSpawned.GetComponent<AppuiInput>();
        int rand = Random.Range(0, ListeToucheAAppuye.Count - 1);
        InputScript.SetImageKeyCode(ListeToucheAAppuye[rand]);
        InputScript.Clip = ListeToucheAAppuye[rand].Sound;

        StartCoroutine(AutoKillInput(prefabSpawned));
    }
    private void ResetGame()
    {
        NbInputFait = 0;
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
            Destroy(prefabSpawned);
        }
    }
    
}
