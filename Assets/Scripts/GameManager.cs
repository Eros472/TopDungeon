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
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        PlayerPrefs.DeleteAll();

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

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
    public Weapon weapon;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;
    public GameObject hud;
    public GameObject menu;

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

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        // Is weapon maxed out
        if(weaponPrices.Count <= weapon.weaponLevel)
            return false;
        
        if(pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    // Experience System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[r];
            r++;

            if(r == xpTable.Count) // Maxed out
                return r;
        }

        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while(r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        Debug.Log("Level up!");
        player.OnLevelUp();
        OnHitpointChange();
    }

    // On Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death Menu and Respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

    // Save state
        public void SaveState()
    {
        string s = "";
        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

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
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        if (data.Length < 4)
        {
            Debug.LogWarning("Invalid save data, clearing SaveState.");
            PlayerPrefs.DeleteKey("SaveState");
            return;
        }

        pesos = int.Parse(data[1]);

        // Experience
        experience = int.Parse(data[2]);

        if(GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        // Change weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        // Re-link the player in new scene
        player = GameObject.FindWithTag("Player")?.GetComponent<Player>();

        // Re-link floatingTextManager in the new scene
        if (floatingTextManager == null)
            floatingTextManager = FindObjectOfType<FloatingTextManager>();

        Debug.Log($"Loaded SaveState: {pesos} pesos, {experience} xp");
    }
}