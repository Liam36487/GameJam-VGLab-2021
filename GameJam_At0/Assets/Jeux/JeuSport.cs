using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeuSport : Jeu
{
    public bool IsActive = false;

    public AudioSource AudioSource;
    public AudioClip hitSound;
    public AudioClip hitSound2;
    public AudioClip badHitSound;
    public AudioClip TransSound;

    public float shakerRateGoodInput = 0.1f;
    public float shakerRateBadInput = 0.3f;

    public GameObject PrefabInputToSpam;
    //où afficher les inputs à faire
    public Canvas gameCanvas;
   
    public Color ColorWhenDisable;

    public List<Difficulte> Difficultes;

    [Header("Gestion Input")]
    [SerializeField]
    public List<KeyPair> PairsDeTouches;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;

    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;

    private int NumDifficulteActuelle = 0;
    private StressReceiver Shaker;

    private int Nb1erInputFait = 0;
    private int Nb2emeInputFait = 0;
    private int NbTour = 0;
    private GameObject PrefabSpawned1;
    private GameObject PrefabSpawned2;

    private AppuiInput InputScript1;
    private AppuiInput InputScript2;

    public int idJeu = 3;

    private List<HitBoxPair> ItemHitboxList = new List<HitBoxPair>();


    [System.Serializable]
    public class Difficulte
    {
        public int NbInputAFaireParInput = 10;
        public int NbTourAFaire = 2;
        public float DureePauseQuandErreur = 0.5f;
        public float TempsEntreLesSpawn = 1f;
        public float DureeDeVieInput = 5f;
    }

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
        if (Camera.main != null)
        {
            Shaker = Camera.main.GetComponent<StressReceiver>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive && PrefabSpawned1 != null && PrefabSpawned2 != null && Nb1erInputFait == 0 && Nb2emeInputFait == 0)
        {
            if (Input.GetKeyDown(InputScript1.KeyCode))
            {
                Nb1erInputFait++;
                AudioSource.PlayOneShot(hitSound);
                if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
                Status = 1;
                InputScript1.Recolor(ColorWhenDisable);
                InputScript2.Recolor(new Color(255, 255, 255, 255));
                return;
            }
            else if (Input.GetKeyDown(InputScript2.KeyCode))
            {
                Nb2emeInputFait++;
                AudioSource.PlayOneShot(hitSound2);
                if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
                Status = 2;
                InputScript2.Recolor(ColorWhenDisable);
                InputScript1.Recolor(new Color(255, 255, 255, 255));
                return;
            }

        }
        
        if(IsActive)
        {
            if (PrefabSpawned1 != null && PrefabSpawned2 != null && Input.GetKeyDown(InputScript1.KeyCode))
            {
                print("yolo");
                if (Status == 1)
                {
                    if (PrefabSpawned1 != null && PrefabSpawned2 != null && Nb1erInputFait == Nb2emeInputFait)
                    {
                        Nb1erInputFait++;
                        AudioSource.PlayOneShot(hitSound);
                        if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
                        if (InputScript1 != null)
                            InputScript1.Recolor(ColorWhenDisable);
                        if (InputScript2 != null)
                            InputScript2.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == Difficultes[NumDifficulteActuelle].NbInputAFaireParInput)
                        {
                            EndTour();
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
                        AudioSource.PlayOneShot(hitSound);
                        if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
                        if (InputScript1 != null)
                            InputScript1.Recolor(ColorWhenDisable);

                        if (InputScript2 != null)
                            InputScript2.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == Difficultes[NumDifficulteActuelle].NbInputAFaireParInput)
                        {
                            EndTour();
                        }
                    }
                    else
                    {
                        StartCoroutine(StopGame(InputScript2));
                    }
                }
            }
            if (PrefabSpawned1 != null && PrefabSpawned2 != null && Input.GetKeyDown(InputScript2.KeyCode))
            {
                print("yolo");
                if (Status == 1)
                {
                    if (Nb1erInputFait > Nb2emeInputFait)
                    {
                        Nb2emeInputFait++;
                        AudioSource.PlayOneShot(hitSound2);
                        if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);

                        if (InputScript2 != null)
                            InputScript2.Recolor(ColorWhenDisable);
                        if (InputScript1 != null)
                            InputScript1.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == Difficultes[NumDifficulteActuelle].NbInputAFaireParInput)
                        {
                            EndTour();
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
                        AudioSource.PlayOneShot(hitSound2);
                        if (Shaker != null) Shaker.InduceStress(shakerRateGoodInput);
                        if (InputScript2 != null)
                            InputScript2.Recolor(ColorWhenDisable);
                        if (InputScript1 != null)
                            InputScript1.Recolor(new Color(255, 255, 255, 255));
                        if (Nb1erInputFait == Nb2emeInputFait && Nb1erInputFait == Difficultes[NumDifficulteActuelle].NbInputAFaireParInput)
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
            }
        }
    }

    IEnumerator AutoKillInput(GameObject prefab1, GameObject prefab2)
    {

        yield return new WaitForSeconds(Difficultes[NumDifficulteActuelle].DureeDeVieInput);

        if (prefab1 == null && prefab2 == null)
        {
            print("deja mort");
        }
        else
        {
            Nb1erInputFait = 0;
            Destroy(prefab1);
            Nb2emeInputFait = 0;
            Destroy(prefab2);
            ItemHitboxList.Clear();
            StartCoroutine(WaitBeforeSpawn(Difficultes[NumDifficulteActuelle].TempsEntreLesSpawn));
        }
    }

    public void EndTour()
    {
        NbTour++;
        Nb1erInputFait = 0;
        Nb2emeInputFait = 0;
        Status = 0;
        if(NbTour >= Difficultes[NumDifficulteActuelle].NbTourAFaire)
        {
            ResetGame();
            gameManager.EndGame(this);
        }
        else
        {
            Destroy(PrefabSpawned1);
            Destroy(PrefabSpawned2);
            ItemHitboxList.Clear();
            StartCoroutine(WaitBeforeSpawn(Difficultes[NumDifficulteActuelle].TempsEntreLesSpawn));
        }
    }

    IEnumerator WaitBeforeSpawn(float tempsSpawnApresFinDeTour)
    {
        yield return new WaitForSeconds(tempsSpawnApresFinDeTour);

        SpawnPrefab();
    }

     IEnumerator StopGame(AppuiInput inputThatMustBeWhite)
    {
        IsActive = false;
        AudioSource.PlayOneShot(badHitSound);
        if (Shaker != null) Shaker.InduceStress(shakerRateBadInput); 

        if (InputScript1 != null)
            InputScript1.Recolor(ColorWhenDisable);
        if(InputScript2 != null)
            InputScript2.Recolor(ColorWhenDisable);

        yield return new WaitForSeconds(Difficultes[NumDifficulteActuelle].DureePauseQuandErreur);

        if (inputThatMustBeWhite != null)
            inputThatMustBeWhite.Recolor(new Color(255, 255, 255, 255));

        IsActive = true;
    }

    public override void StartGame(int numDifficulte)
    {
        IsActive = true;
        if (numDifficulte >= Difficultes.Count)
            NumDifficulteActuelle = Difficultes.Count - 1;
        else
            NumDifficulteActuelle = numDifficulte;
        StartCoroutine(WaitBeforeSpawn(Difficultes[NumDifficulteActuelle].TempsEntreLesSpawn));


    }

    private void SpawnPrefab()
    {
        AudioSource.PlayOneShot(TransSound);
        RectTransform rectT = PrefabInputToSpam.GetComponent<RectTransform>();

        int IdPairChoisie = Random.Range(0, PairsDeTouches.Count - 1);

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

        StartCoroutine(AutoKillInput(PrefabSpawned1, PrefabSpawned2));
    }

    private void ResetGame()
    {
        Nb1erInputFait = 0;
        Nb2emeInputFait = 0;
        Status = 0;
        NbTour = 0;
        Destroy(PrefabSpawned1);
        Destroy(PrefabSpawned2);
        ItemHitboxList.Clear();
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
