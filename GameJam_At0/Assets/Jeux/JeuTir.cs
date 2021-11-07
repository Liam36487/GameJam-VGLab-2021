using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuTir : Jeu
{
    public bool IsActive = false;

    public List<Sprite> ListSprites;
    public List<Difficulte> Difficultes;
    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
    
    public Texture2D cursorTexture;

    private int NbCiblesCassees = 0;
    private ClicInput InputScript;
    private GameObject prefabSpawned;

    public int idJeu = 2;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;
    
    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;
    
    private float RandomTimer;
    private List<HitBoxPair> ItemHitboxList = new List<HitBoxPair>();
    private int NumDifficulteActuelle = 0;
    

    public class HitBoxPair
    {
        public int Id;
        public Rect Rectangle;
    }

    [System.Serializable]
    public class Difficulte
    {
        public int NbCibleACasser = 5;
        public float DureeDeVieInput = 1.5f;

        [Header("Random Spawn Range")]
        public float MinRangeSpawn = 0.5f;
        public float MaxRangeSpawn = 1.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomTimer = Random.Range(Difficultes[NumDifficulteActuelle].MinRangeSpawn, Difficultes[NumDifficulteActuelle].MaxRangeSpawn);
       // StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive && Input.GetMouseButtonDown(0))
        {
            if (NbCiblesCassees >= Difficultes[NumDifficulteActuelle].NbCibleACasser - 1)
            {
                ResetGame();
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                gameManager.EndGame(this);
                print("fin du jeu");
            }
        }
        

        if (IsActive)
        {
            RandomTimer -= Time.deltaTime;
            if(RandomTimer <= 0)
            {
                SpawnPrefab();
                RandomTimer = Random.Range(Difficultes[NumDifficulteActuelle].MinRangeSpawn, Difficultes[NumDifficulteActuelle].MaxRangeSpawn);
            }
        }
    }

    public override void StartGame(int numDifficulte)
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2, cursorTexture.height/2), CursorMode.Auto);
        if (numDifficulte >= Difficultes.Count)
            NumDifficulteActuelle = Difficultes.Count - 1;
        else
            NumDifficulteActuelle = numDifficulte;
        IsActive = true;
    }
    
    private void SpawnPrefab()
    {
        RectTransform rectT = PrefabInputToSpam.GetComponent<RectTransform>();

        Rect rec = PlaceRandom(new Vector2(rectT.rect.width, rectT.rect.height));
        if (rec.x == -666 && rec.y == -666 && rec.width == -666 && rec.height == -666) return;
               
        prefabSpawned = Instantiate(PrefabInputToSpam, new Vector3(rec.x, rec.y, 0), Quaternion.identity);
        prefabSpawned.transform.SetParent(gameCanvas.transform, false);
        ItemHitboxList.Find(x => x.Rectangle.Equals(rec)).Id = prefabSpawned.GetInstanceID();
        InputScript = prefabSpawned.GetComponent<ClicInput>();
        InputScript.JeuTir = this;
        InputScript.SetImage(ListSprites[Random.Range(0, ListSprites.Count - 1)]);
        InputScript.Expiration = Difficultes[NumDifficulteActuelle].DureeDeVieInput;
        
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
        ItemHitboxList.Remove(ItemHitboxList.Find(x => x.Id == cible.GetInstanceID()));
        Destroy(cible);
    }

    public void RemoveHitBox(GameObject cible)
    {
        ItemHitboxList.Remove(ItemHitboxList.Find(x => x.Id == cible.GetInstanceID()));
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

        ItemHitboxList.Add(new HitBoxPair { Rectangle = hitBox });

        return hitBox;
    }

    private bool HasCollision(Rect itemHitBox)
    {
        foreach (HitBoxPair item in ItemHitboxList)
        {
            if (item.Rectangle.Overlaps(itemHitBox))
                return true;
        }
        return false;
    }
}
