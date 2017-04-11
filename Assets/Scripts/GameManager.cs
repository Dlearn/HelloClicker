using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class GameManager : MonoBehaviour {

    // Constants
    public int PING_FREQUENCY = 5;

    // Static singletons
    public static SocketIOComponent socket;
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    // Public Variables
    public bool socketConnected, usernameInput;
    public string myUsername;
    public string questObj;
    public bool selectedMaceNotSword;
    public string bossType;
    public int bossHealth;

    // References to other scripts
    private LoginManager loginManager;
    private SoloManager soloManager;
    private QuestManager questManager;
    private FightManager fightManager;
    private Enemy enemy;
    private Player player;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists, if not, set instance to this
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this: Destroy it, there can only ever be one instance of GameManager
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        // username has not been input
        usernameInput = false;

        socket = GetComponent<SocketIOComponent>();

        socket.On("open", (SocketIOEvent e) =>
        {
            if (usernameInput)
            {
                JSONObject data = new JSONObject(JSONObject.Type.STRING);
                data.str = myUsername;
                socket.Emit("add user", data);
            }
            socketConnected = true;
        });
        //socket.On("error", TestError);
        socket.On("close", (SocketIOEvent e) => {
            if (!usernameInput) return;
            if (SceneManager.GetActiveScene().name != "Quest") return;

            if (questManager == null)
                questManager = GameObject.Find("Main Camera").GetComponent<QuestManager>();

            questManager.UpdateArriveUI(e.data.list[0].str, e.data.list[1].b, e.data.list[2].b, e.data.list[3].str, e.data.list[4].b, e.data.list[5].b);
        });

        socket.On("login", (SocketIOEvent e) => {
            SceneManager.LoadScene("Solo");
        });
        socket.On("solo players", (SocketIOEvent e) => {
            // If the scene has changed, do nothing
            if (SceneManager.GetActiveScene().name != "Solo") return;

            // If we haven't found SoloManager, find it now.
            if (soloManager == null)
                soloManager = GameObject.Find("Main Camera").GetComponent<SoloManager>();

            var obj = e.data;
            soloManager.UpdateList(obj);
        });
        socket.On("form party", (SocketIOEvent e) => {
            socket.Emit("transition quest");
            if (SceneManager.GetActiveScene().name != "Solo") return;
            soloManager.HideInviteListPanel();
            // Cancel cur coord invokation
            CancelInvoke();

            questObj = e.data.list[0].str;
            SceneManager.LoadScene("Quest");

            // START WALKING
            //InvokeRepeating("SendCoordinates", 0, PING_FREQUENCY);
        });
        socket.On("arrive data", (SocketIOEvent e) => {
            // If the scene has changed, do nothing
            if (SceneManager.GetActiveScene().name != "Quest") return;

            if (questManager == null)
                questManager = GameObject.Find("Main Camera").GetComponent<QuestManager>();

            questManager.UpdateArriveUI(e.data.list[0].str, e.data.list[1].b, e.data.list[2].b, e.data.list[3].str, e.data.list[4].b, e.data.list[5].b);
        });
        socket.On("party on obj", (SocketIOEvent e) => {
            CancelInvoke();
            SceneManager.LoadScene("Prep");
            socket.Emit("transition prep");
        });
        socket.On("party is ready", (SocketIOEvent e) => {
            SceneManager.LoadScene("Fight");
            socket.Emit("transition fight");
        });
        socket.On("spawn boss", (SocketIOEvent e) => {
            bossType = e.data.list[0].str;
            bossHealth = (int)e.data.list[1].n;

            // If the scene has changed, do nothing
            if (SceneManager.GetActiveScene().name != "Fight") Debug.LogError("Not yet loaded Fight");

            // If we haven't found FightManager, find it now.
            if (fightManager == null)
                fightManager = GameObject.Find("Main Camera").GetComponent<FightManager>();

            fightManager.SpawnEnemyInXSeconds(1);
        });
        socket.On("boss hit", (SocketIOEvent e) => {
            // If the scene has changed, do nothing
            if (SceneManager.GetActiveScene().name != "Fight") return;

            // If we haven't found Enemy, find it now.
            if (enemy == null)
                enemy = GameObject.FindGameObjectWithTag("Monster").GetComponent<Enemy>();

            int remainingHealth = (int)e.data.list[0].n;
            enemy.UpdateHealth(remainingHealth);
        });
        socket.On("quest completed", (SocketIOEvent) => {
            socket.Emit("back transition solo");

            // If we haven't found FightManager, find it now.
            if (player == null)
                player = GameObject.Find("Main Camera").GetComponent<Player>();

            player.Victory();
        });
    }

    void Update()
    {
        if (socketConnected)
        {
            socketConnected = false;
            if (SceneManager.GetActiveScene().name != "LoginSplash")
            {
                Debug.LogWarning("Open, but not in LoginSplash");
                return;
            }

            if (loginManager == null)
                loginManager = GameObject.Find("Main Camera").GetComponent<LoginManager>();

            loginManager.MakeLoginVisible();
        }
    }

    public void Attack()
    {
        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("damage", UnityEngine.Random.Range(10, 31));
        socket.Emit("attack", data);
    }

    void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
}
