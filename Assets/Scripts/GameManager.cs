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
    public bool socketConnected;
    public string myUsername;
    public string questObj;
    public string bossType;
    public int bossHealth;

    // References to other scripts
    private LoginManager loginManager;
    private SoloManager soloManager;
    private Enemy enemy;

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
        socket = GetComponent<SocketIOComponent>();

        //socket.On("open", TestOpen);
        socket.On("open", (SocketIOEvent e) =>
        {
            socketConnected = true;
        });
        socket.On("error", TestError);
        socket.On("close", TestClose);

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
        socket.On("party on obj", (SocketIOEvent e) => {
            CancelInvoke();
            SceneManager.LoadScene("Fight");

            bossType = e.data.list[0].str;
            bossHealth = (int) e.data.list[1].n;
            socket.Emit("transition fight");
        });
        socket.On("boss hit", (SocketIOEvent e) => {
            // If the scene has changed, do nothing
            if (SceneManager.GetActiveScene().name != "Fight") return;

            // If we haven't found SoloManager, find it now.
            if (enemy == null)
                enemy = GameObject.FindGameObjectWithTag("Monster").GetComponent<Enemy>();

            int remainingHealth = (int)e.data.list[0].n;
            enemy.UpdateHealth(remainingHealth);
        });
        socket.On("boss defeated", (SocketIOEvent) => {
            socket.Emit("back transition solo");
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

    public void invitePlayer()
    {
        JSONObject invitee = new JSONObject(JSONObject.Type.OBJECT);
        invitee.AddField("username", "user0");
        socket.Emit("invite", invitee);
    }

    void SendCoordinates()
    {
        int jitter_x = Random.Range(0, 2); // 0,1
        int jitter_y = Random.Range(0, 2); // 0,1
        
        JSONObject coord = new JSONObject(JSONObject.Type.OBJECT);
        coord.AddField("x", 50 + jitter_x);
        coord.AddField("y", 50 + jitter_y);
        socket.Emit("cur coord", coord);
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
