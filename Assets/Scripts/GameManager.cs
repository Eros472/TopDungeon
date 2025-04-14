using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    /*private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }*/

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);

        // Fallback assignment
        if (floatingTextManager == null)
            floatingTextManager = FindObjectOfType<FloatingTextManager>();
    }




    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    public Player player;

    //public weapon weapon..
    public FloatingTextManager floatingTextManager;

    // Logic
    public int pesos;
    public int experience;

    // Save state
    /*
     * INT preferredSkin
     * INT pesos
     * INT experience
     * INT weaponLevel
     */

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);

    }

    /*public void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
        {
            Debug.Log("No save found.");
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        if (data.Length < 4)
        {
            Debug.LogWarning("Save data is invalid or incomplete. Clearing SaveState.");
            PlayerPrefs.DeleteKey("SaveState");
            return;
        }

        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);

        //player = GameObject.FindWithTag("Player")?.GetComponent<Player>();

        Debug.Log("Loaded SaveState: " + pesos + " pesos, " + experience + " xp");
    }*/

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
        {
            Debug.Log("No save found.");
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        if (data.Length < 4)
        {
            Debug.LogWarning("Invalid save data, clearing SaveState.");
            PlayerPrefs.DeleteKey("SaveState");
            return;
        }

        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);

        // Re-link the player in new scene
        player = GameObject.FindWithTag("Player")?.GetComponent<Player>();

        // Re-link floatingTextManager in the new scene
        if (floatingTextManager == null)
            floatingTextManager = FindObjectOfType<FloatingTextManager>();

        Debug.Log($"Loaded SaveState: {pesos} pesos, {experience} xp");
    }




}




