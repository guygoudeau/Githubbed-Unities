using UnityEngine;
using Combat;
using FinateStateMachine;
using Serializer;
using System;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    private SaveGameData SuperSave;
    private Player player = Player.instance;
    private Enemy enemy = new Enemy();

    Text playerhealth;
    Text playerstrength;
    Text playerlevel;
    Text playerexperience;
    Text enemyhealth;
    Text enemystrength;
    Text phaselabel;
    Text combatlog;
    Button playerattack;
    Button enemyattack;
    Button savebutton;
    Button loadbutton;

    void Start ()
    {
        playerhealth = GameObject.Find("HpValue").GetComponent<Text>();
        playerstrength = GameObject.Find("StrValue").GetComponent<Text>();
        playerlevel = GameObject.Find("LvlValue").GetComponent<Text>();
        playerexperience = GameObject.Find("ExpValue").GetComponent<Text>();
        enemyhealth = GameObject.Find("EnHpValue").GetComponent<Text>();
        enemystrength = GameObject.Find("EnStrValue").GetComponent<Text>();
        phaselabel = GameObject.Find("PhaseValue").GetComponent<Text>();
        combatlog = GameObject.Find("combatLog").GetComponent<Text>();
        playerattack = GameObject.Find("PlayerAttackButton").GetComponent<Button>();
        enemyattack = GameObject.Find("EnemyAttackButton").GetComponent<Button>();
        savebutton = GameObject.Find("SaveButton").GetComponent<Button>();
        loadbutton = GameObject.Find("LoadButton").GetComponent<Button>();
        phaselabel.text = GameState.init.ToString();
        playerattack.gameObject.SetActive(false);
        enemyattack.gameObject.SetActive(false);
        savebutton.gameObject.SetActive(false);
        loadbutton.gameObject.SetActive(false);
        GAMEFSM = new FSM<GameState>(GameState.init);
        GAMEFSM.AddState(GameState.init);
        GAMEFSM.AddState(GameState.player);
        GAMEFSM.AddState(GameState.enemy);
        GAMEFSM.AddState(GameState.end);
        GAMEFSM.AddTransition(GameState.init, GameState.player);
        GAMEFSM.AddTransition(GameState.player, GameState.enemy);
        GAMEFSM.AddTransition(GameState.enemy, GameState.player);
        GAMEFSM.AddTransition(GameState.player, GameState.end);
        GAMEFSM.AddTransition(GameState.enemy, GameState.end);
        SuperSave = new SaveGameData();
    }
    private FSM<GameState> GAMEFSM;
    enum GameState
    {
        init, player, enemy, end
    }

    [Serializable]
    public class SaveGameData
    {
        public SaveGameData() { }
        public SaveGameData(int hp, int str, int lvl, int exp)
        {
            SaveHealth = hp;
            SaveStrength = str;
            SaveLevel = lvl;
            SaveExperience = exp;
        }
        public int SaveHealth;
        public int SaveStrength;
        public int SaveLevel;
        public int SaveExperience;
    }


    private void updateLabels()
    {
        playerhealth.text = player.health.ToString();
        playerstrength.text = player.strength.ToString();
        playerlevel.text = player.level.ToString();
        playerexperience.text = player.experience.ToString();
        phaselabel.text = GAMEFSM.cState.ToString();
        enemyhealth.text = enemy.health.ToString();
        enemystrength.text = enemy.strength.ToString();
    }

    private void updatePlayer()
    {
        if (player.health > 0)
        {
            combatlog.text += ("Player hit Enemy for " + player.strength + " damage.\n");
            player.Attack(enemy);
            updateLabels();
            if (enemy.health <= 0)
            {
                combatlog.text += ("Player killed an enemy! Here comes another one!\n");
                player.Kill(enemy);
                enemy.health = 50;
                updateLabels();
            }
        }
        playerattack.gameObject.SetActive(false);
        enemyattack.gameObject.SetActive(true);
        if (player.health <= 0)
        {
            player.health = 0;
            playerattack.gameObject.SetActive(false);
            enemyattack.gameObject.SetActive(false);
            combatlog.text += ("Player has died! GAME OVER!\n");
        }
    }

    private void updateEnemy()
    {
        if (player.health > 0)
        {
            combatlog.text += ("Enemy hit Player for " + enemy.strength + " damage.\n");
            enemy.Attack(player);
            updateLabels();
        }
        enemyattack.gameObject.SetActive(false);
        playerattack.gameObject.SetActive(true);
        if (player.health <= 0)
        {
            player.health = 0;
            playerattack.gameObject.SetActive(false);
            enemyattack.gameObject.SetActive(false);
            combatlog.text += ("Player has died! GAME OVER!\n");
        }
    }

    private void playerHandler()
    {
        GAMEFSM.Switch(GameState.enemy);
        updatePlayer();
    }

    private void enemyHandler()
    {
        GAMEFSM.Switch(GameState.player);
        updateEnemy();
    }

    public void attack_Click()
    {
        playerHandler();
        SaveGameData save = new SaveGameData(player.health, player.strength, player.level, player.experience);
        SuperSave = save;
    }

    public void enemy_attack_Click()
    {
        enemyHandler();
        SaveGameData save = new SaveGameData(player.health, player.strength, player.level, player.experience);
        SuperSave = save;
    }

    public void newGame_button_Click()
    {
        combatlog.text = "";
        player.health = 100;
        player.strength = 10;
        player.level = 1;
        player.experience = 0;
        enemy.health = 50;
        enemy.strength = 3;
        GAMEFSM.Switch(GameState.player);
        updateLabels();
        playerattack.gameObject.SetActive(true);
        enemyattack.gameObject.SetActive(false);
        savebutton.gameObject.SetActive(true);
        loadbutton.gameObject.SetActive(true);
        SaveGameData save = new SaveGameData(player.health, player.strength, player.level, player.experience);
        SuperSave = save;
    }

    public void save_button_Click()
    {
        string path = @"C:\Users\Guy.Goudeau\Desktop\New Unity Projects\ADGP-125\Assets\Saves\";
        Utilities.SerializeXML<SaveGameData>("Stats", SuperSave, path);
        combatlog.text += ("Game has been saved.\n");
    }

    public void load_button_Click()
    {
        combatlog.text = "";
        combatlog.text += ("Game has been loaded.\n");
        string path = @"C:\Users\Guy.Goudeau\Desktop\New Unity Projects\ADGP-125\Assets\Saves\Stats";
        Utilities.DeserializeXML<SaveGameData>(path);
        SaveGameData save = Utilities.DeserializeXML<SaveGameData>(path);
        player.health = save.SaveHealth;
        player.strength = save.SaveStrength;
        player.level = save.SaveLevel;
        player.experience = save.SaveExperience;
        enemy.health = 50;
        updateLabels();
    }
}
	

