using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuSport : Jeu
{
    public bool IsActive = false;
    
    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
    public int NbInputAFaireParInput = 10;


    public float DureePauseQuandErreur = 0.5f;

    public Color ColorWhenDisable;

    [Header("Gestion Input")]
    [SerializeField]
    public List<KeyPair> PairsDeTouches;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;

    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;

    private int IdPairChoisie;

    private int Nb1erInputFait = 0;
    private int Nb2emeInputFait = 0;
    private GameObject PrefabSpawned1;
    private GameObject PrefabSpawned2;

    private AppuiInput InputScript1;
    private AppuiInput InputScript2;

    private List<HitBoxPair> ItemHitboxList = new List<HitBoxPair>();

    private int Status; //0 rien du tout, 1 le 1er Input a commencé, 2 le deuxieme Input

    [System.Serializable]
    public class KeyPair
    {
        public string Key1;
        public string Key2;
    }

    public class HitBoxPair
    {
        public int Id;
        public Rect Rectangle;
    }



    // Start is called before the first frame update
    void Start()
    {
       // StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive && Nb1erInputFait == 0 && Nb2emeInputFait == 0)
        {
            if (Input.GetKeyDown(InputScript1.KeyCode))
            {
                Nb1erInputFait++;
                Status = 1;
                InputScript1.Recolor(ColorWhenDisable);
                InputScript2.Recolor(new Color(255, 255, 255, 255));
                print("Status 1");
                print(InputScript1.KeyCode + " pressed    Nb1 = " + Nb1erInputFait + "\rNb2 = " + Nb2emeInputFait);
                return;
            }
            else if (Input.GetKeyDown(InputScript2.KeyCode))
            {
                Nb2emeInputFait++;
                Status = 2;
                InputScript2.Recolor(ColorWhenDisable);
                InputScript1.Recolor(new Color(255, 255, 255, 255));
                print("Status 2");
                print(InputScript1.KeyCode + " pressed    Nb1 = " + Nb1erInputFait + "\rNb2 = " + Nb2emeInputFait);
                return;
            }

        }
        
        if(IsActive)
        {
            if (Input.GetKeyDown(InputScript1.KeyCode))
            {
                print("yolo");
                if (Status == 1)
                {
                    if (Nb1erInputFait == Nb2emeInputFait)
                    {
                        Nb1erInputFait++;
                        InputScript1.Recolor(ColorWhenDisable);
                        InputScript2.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == NbInputAFaireParInput)
                        {
                            ResetGame();
                            gameManager.EndGame(this);
                        }
                    }
                    else
                    {
                        StartCoroutine(StopGame(InputScript2));
                    }
                }
                if (Status == 2)
                {
                    if (Nb1erInputFait < Nb2emeInputFait)
                    {
                        Nb1erInputFait++;
                        InputScript1.Recolor(ColorWhenDisable);
                        InputScript2.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == NbInputAFaireParInput)
                        {
                            ResetGame();
                            gameManager.EndGame(this);
                        }
                    }
                    else
                    {
                        StartCoroutine(StopGame(InputScript2));
                    }
                }
                print(InputScript1.KeyCode + " pressed    Nb1 = " + Nb1erInputFait + "\rNb2 = " + Nb2emeInputFait);
            }
            if (Input.GetKeyDown(InputScript2.KeyCode))
            {
                print("yolo");
                if (Status == 1)
                {
                    if (Nb1erInputFait > Nb2emeInputFait)
                    {
                        Nb2emeInputFait++;

                        InputScript2.Recolor(ColorWhenDisable);
                        InputScript1.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == NbInputAFaireParInput)
                        {
                            ResetGame();
                            gameManager.EndGame(this);
                        }
                    }
                    else
                    {
                        StartCoroutine(StopGame(InputScript1));
                    }
                }
                if (Status == 2)
                {
                    if (Nb1erInputFait == Nb2emeInputFait)
                    {
                        Nb2emeInputFait++;
                        InputScript2.Recolor(ColorWhenDisable);
                        InputScript1.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == NbInputAFaireParInput)
                        {
                            ResetGame();
                            gameManager.EndGame(this);
                        }
                    }
                    else
                    {
                        StartCoroutine(StopGame(InputScript1));
                    }
                }
                print(InputScript2.KeyCode + " pressed    Nb1 = " + Nb1erInputFait + "\rNb2 = " + Nb2emeInputFait);
            }
        }
    }

     IEnumerator StopGame(AppuiInput inputThatMustBeWhite)
    {
        IsActive = false;

        InputScript1.Recolor(ColorWhenDisable);
        InputScript2.Recolor(ColorWhenDisable);

        yield return new WaitForSeconds(DureePauseQuandErreur);
        
        inputThatMustBeWhite.Recolor(new Color(255, 255, 255, 255));

        IsActive = true;
    }

    public override void StartGame()
    {
        IsActive = true;
        IdPairChoisie = Random.Range(0, PairsDeTouches.Count - 1);
        SpawnPrefab();


    }

    private void SpawnPrefab()
    {
        RectTransform rectT = PrefabInputToSpam.GetComponent<RectTransform>();

        Rect rec = PlaceRandom(new Vector2(rectT.rect.width, rectT.rect.height));
        if (rec.x == -666 && rec.y == -666 && rec.width == -666 && rec.height == -666) return;

        PrefabSpawned1 = Instantiate(PrefabInputToSpam, new Vector3(rec.x, rec.y, 0), Quaternion.identity);
        PrefabSpawned1.transform.SetParent(gameCanvas.transform, false);
        ItemHitboxList.Find(x => x.Rectangle.Equals(rec)).Id = PrefabSpawned1.GetInstanceID();
        InputScript1 = PrefabSpawned1.GetComponent<AppuiInput>();
        InputScript1.SetKeyCode(PairsDeTouches[IdPairChoisie].Key1);

         rectT = PrefabInputToSpam.GetComponent<RectTransform>();

         rec = PlaceRandom(new Vector2(rectT.rect.width, rectT.rect.height));
        if (rec.x == -666 && rec.y == -666 && rec.width == -666 && rec.height == -666) return;

        PrefabSpawned2 = Instantiate(PrefabInputToSpam, new Vector3(rec.x, rec.y, 0), Quaternion.identity);
        PrefabSpawned2.transform.SetParent(gameCanvas.transform, false);
        ItemHitboxList.Find(x => x.Rectangle.Equals(rec)).Id = PrefabSpawned1.GetInstanceID();
        InputScript2 = PrefabSpawned2.GetComponent<AppuiInput>();
        InputScript2.SetKeyCode(PairsDeTouches[IdPairChoisie].Key2);
    }

    private void ResetGame()
    {
        Nb1erInputFait = 0;
        Nb2emeInputFait = 0;
        Status = 0;
        ItemHitboxList.Clear();
        Destroy(PrefabSpawned1);
        Destroy(PrefabSpawned2);
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
