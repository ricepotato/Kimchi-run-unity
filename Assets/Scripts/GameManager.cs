using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Intro,
    Playing,
    Dead
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State = GameState.Intro;

    public float PlayStartTime;

    public int lives = 3;

    [Header("References")]
    public GameObject IntroUI;
    public GameObject DeadUI;
    public GameObject EnermySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldenSpawner;

    public Player playerScript;

    public TMP_Text ScoreText;

    public float CalculateGameSpeed(){
        if(State != GameState.Playing){
            return 3f;
        }
        float speed = 3f + (0.5f * Mathf.Floor(CalculateScore() / 10f));
        return Mathf.Min(speed, 20);
    }

    void Awake(){
        // befoer start
        if(Instance == null){
            Instance = this;
        }
    }
    void Start()
    {
        IntroUI.SetActive(true);
        
    }

    float CalculateScore(){
        return Time.time - PlayStartTime;
    }

    void SaveHighScore(){
        int score = Mathf.FloorToInt(CalculateScore());
        int currentHighScore = PlayerPrefs.GetInt("highscore");
        if(score > currentHighScore){
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
    }

    int GetHighScore() {
        return PlayerPrefs.GetInt("highscore");
    }

    // Update is called once per frame
    void Update()
    {
        if(State == GameState.Playing) {
            ScoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore());
        }else if(State == GameState.Dead){
            ScoreText.text = "High Score: " + GetHighScore();
        }
        // 게임 시작 상태 변경 intro ui 비활성화 spawner 활성화
        if(State == GameState.Intro && Input.GetKeyDown(KeyCode.Space)){
            State = GameState.Playing;
            IntroUI.SetActive(false);
            EnermySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldenSpawner.SetActive(true);
            PlayStartTime = Time.time;
        }

        // 케릭터 사망
        if(State == GameState.Playing && lives == 0){
            playerScript.KillPlayer();
            EnermySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpawner.SetActive(false);
            State = GameState.Dead;
            DeadUI.SetActive(true);
            SaveHighScore();
        }

        if(State == GameState.Dead && Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene("main");
        }
    }
}
