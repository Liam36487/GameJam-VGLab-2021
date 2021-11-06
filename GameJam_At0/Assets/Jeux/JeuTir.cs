using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuTir : Jeu
{
    public bool IsActive = false;

    [Header("Gestion Input")]
    public string[] ListeToucheAAppuye;
    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
    public int NbCibleACasser = 5;
    public float DureeDeVieInput = 1.5f;
    
    private int NbCiblesCassees = 0;
    private ClicInput InputScript;
    private GameObject prefabSpawned;

    [Header("Random Spawn Range")]
    public float xRangeSpawn;
    public float YRangeSpawn;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;
    
    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;
    

    private float RandomTimer;
    private HashSet<Rect> ItemHitboxList = new HashSet<Rect>();

    // Start is called before the first frame update
    void Start()
    {
        RandomTimer = Random.Range(xRangeSpawn, YRangeSpawn);
       // StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive && Input.GetMouseButtonDown(0))
        {
            if (NbCiblesCassees >= NbCibleACasser)
            {
                ResetGame();
                gameManager.EndGame(this);
                print("fin du jeu");
            }
        }
        if(IsActive)
        {
            RandomTimer -= Time.deltaTime;
            if(RandomTimer <= 0)
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

    private void SpawnPrefab()
    {
        RectTransform rectT = PrefabInputToSpam.GetComponent<RectTransform>();

        Rect rec = PlaceRandom(new Vector2(rectT.rect.width, rectT.rect.height));
        if (rec.x == -666 && rec.y == -666 && rec.width == -666 && rec.height == -666) return;
               
        prefabSpawned = Instantiate(PrefabInputToSpam, new Vector3(rec.x, rec.y, 0), Quaternion.identity);
        prefabSpawned.transform.SetParent(gameCanvas.transform, false);
        InputScript = prefabSpawned.GetComponent<ClicInput>();
        InputScript.JeuTir = this;
        InputScript.Expiration = DureeDeVieInput;
        
    }
    private void ResetGame()
    {
        NbCiblesCassees = 0;
        ClicInput[] remaningInputs = GameObject.FindObjectsOfType<ClicInput>();
        foreach (ClicInput input in remaningInputs)
            Destroy(input.gameObject);
    }
    
    public void CibleCasse(GameObject cible)
    {
        NbCiblesCassees++;
        Destroy(cible);
    }

    public Rect PlaceRandom(Vector2 itemHitBox)
    {

        bool placed = false;

        Rect hitBox = new Rect
        {
            width = itemHitBox.x,
            height = itemHitBox.y
        };

        int error = 1000;

        while (!placed)
        {
            hitBox.x = Random.Range(xMin, xMax);
            hitBox.y = Random.Range(yMin, yMax);

            placed = !HasCollision(hitBox);

            if (error == 0)
                return new Rect(-666, -666, -666, -666);
            else
                error--;
        }

        ItemHitboxList.Add(hitBox);

        return hitBox;
    }

    private bool HasCollision(Rect itemHitBox)
    {
        foreach (Rect item in ItemHitboxList)
        {
            if (item.Overlaps(itemHitBox))
                return true;
        }
        return false;
    }
}
