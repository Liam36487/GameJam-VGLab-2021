using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuVoiture : Jeu
{
    public bool IsActive = false;
    
    [Header("Gestion Input")]
    public List<KeyImagePair> ListeToucheAAppuye;
    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
    public int NbInputAFaireParInput = 3;
    public float DureeDeVieInput = 1f;
    public float DureePauseQuandErreur = 0.5f;

    [Header("Random Spawn Range")]
    public float xRangeSpawn = 2f;
    public float YRangeSpawn = 5f;
    
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
    
    
    // Start is called before the first frame update
    void Start()
    {
       // StartGame();
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
            Destroy(prefabSpawned);
            if (NbInputFait >= NbInputAFaireParInput)
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
            StartCoroutine(StopTime());
        }
        else if (IsActive && prefabSpawned == null && Input.anyKeyDown)
        {
            //Jouer son accident
            NbInputFait--;
        }

        if (IsActive && prefabSpawned == null)
        {
            RandomTimer -= Time.deltaTime;
            if (RandomTimer <= 0)
            {
                SpawnPrefab();
                RandomTimer = Random.Range(xRangeSpawn, YRangeSpawn);
            }
        }
    }

    public override void StartGame()
    {
        IsActive = true;
    }

    IEnumerator StopTime()
    {
        IsActive = false;

        yield return new WaitForSeconds(DureePauseQuandErreur);
        
        IsActive = true;
    }

    private void SpawnPrefab()
    {
        Vector2 pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        
        prefabSpawned = Instantiate(PrefabInputToSpam, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        prefabSpawned.transform.SetParent(gameCanvas.transform, false);
        InputScript = prefabSpawned.GetComponent<AppuiInput>();
        InputScript.SetImageKeyCode(ListeToucheAAppuye[Random.Range(0, ListeToucheAAppuye.Count-1)]);


        StartCoroutine(AutoKillInput(prefabSpawned));
    }
    private void ResetGame()
    {
        NbInputFait = 0;
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
            Destroy(prefabSpawned);
        }
    }
    
}
