gg1126
Guy Goudeau
April 6, 2016
ADGP-125 Assessment Documentation

I.1. Description of the Problem
a.) 	Name: ADGP-125
Problem Statement: Students implement turn based combat, demonstrating the ability to design, plan and build a scalable application.
Problem Specification: Design your program from scratch. What is required?
Classes, enums, structs, Lists, Dictionaries, etc... How are you using them to accomplish your design? Create an algorithm for leveling characters. Documentation of code, diagrams of design. This must be written and planned carefully. After creating the design, implement it exactly as you designed. Any changes must be approved. Ensure that the systems created are generic enough to support multiple implementations. Windows Forms. Unity Ugui. Application must have the ability to save the current state and load a state through use of XML.Serialization using SOAP format. Application must be versioned using git. Test your systems thoroughly. Singletons and Finite State Machines are implemented to solve a problem. What is that problem? Test and ensure that they are being used properly.

1.3.2. Output Items
a.)	Description: updatePlayer() is called when the player attacks, where the game then switches to the enemy’s turn and "Enemy hit Player for " + enemy.strength + " damage.\n" is outputted. If an enemy is killed, "Player killed an enemy! Here comes another one!\n" is outputted.
If the player dies, "Player has died! GAME OVER!\n" is outputted.
Type: private void function
Range of Acceptable Values: n/a
b.)	Description: updateEnemy() is called when the player attacks, where the game then switches to the enemy’s turn and "Player hit Enemy for " + player.strength + " damage.\n" is outputted. If the player dies, "Player has died! GAME OVER!\n" is outputted.
Type: private void function
Range of Acceptable Values: n/a
c.)	Description: load_button_Click() is called when the the load button is clicked, where it then outputs "Game has been loaded.\n".
Type: private void function
Range of Acceptable Values: n/a


1.4.1 Description
My program comes in two formats, a Windows Forms Application and a Unity project. They are both nearly the same project, with minor differences in the code to make it work when I transferred my code to Unity. There are several buttons, the first that you have to click being the New Game button, then the player and enemy take turns attacking. When an enemy is defeated, the player gains experience and levels up based off of an algorithm. When you level up, you gain more health and strength. The game can be saved and loaded using XML.


II.2 Information About the Objects
a.) 	Name: SaveGameData
Description: a class that holds slots for health, strength, level and experience to be used as the player’s value for these stats when the game is saved and loaded.
Class Attributes: int hp, int str, int lvl, int exp, public int SaveHealth, public int SaveStrength, public int SaveLevel, public int SaveExperience
Visibility: public
b.)	Name: Enemy
	Description: Enemy unit class for the player to fight.
	Class Attributes: private int hp, private int str, public Enemy() constructor, getters and setters for health and strength
Visibility: public
c.) 	Name: Player
	Description: Player unit class for the user to control and fight Enemies with
	Class Attributes: private int hp, str, lvl & exp along with getters and setters for each, public Player() constructor
	Visibility: public
d.) 	Name: FSM
	Description: finite state machine class to handle game states
	Class Attributes: public Enum cstate
	Visibility: public
e.) 	Name: Utilities
	Description: class that handles XML serialization and deserialization
	Class Attributes: functions
	Visibility: public static

Class Operations
a.)	Prototype: void SerializeXML<T>(string s, T t, string path)
Description: XML Serializes data to an existing path, or creates a path if none exist yet
Visibility: public static
b.)	Prototype: T DeserializeXML<T>(string s)
Description: Opens a .xml file and deserializes the date
Visibility: public static
c.)	Prototype: bool AddState(T state)
	Description: checks dictionary to see if state is already added, otherwise it adds it.
	Visibility: public
d.)	Prototype: bool AddTransition(T from, T to)
	Description: checks dictionary to see if states are already linked for transition, otherwise adds it
	Visibility: public
e.)	Prototype: T Switch(T to)
	Description: switches to a different valid state
	Visibility: public
f.)	Prototype: int takeDamage(int d)
	Description: decrements health by d, which is a unit’s strength value
	Visibility: public
g.)	Prototype: int Attack(Enemy other)
	Description: for Player, calls takeDamage function to damage Enemy
	Visibility: public
h.)	Prototype: Player Kill(Enemy other)
	Description: adds experience on Enemy kill and handles player level up
	Visibility: public
i.)	Prototype: int Attack(Player other)
	Description: for Enemy, calls takeDamage function to damage Player
	Visibility: public
j.)	Prototype: void updateLabels()
	Description: refreshes every label to reflect the current values for the units in combat
	Visibility: private 
k.)	Prototype: void updatePlayer()
	Description: If player has more than 0 health, this function lets him attack an enemy and then updates labels, as well as outputting what happened and how much damage was done. If an enemy if killed, makes a new enemy. If player reaches 0 health, player dies and the game ends.
	Visibility: private
l.)	Prototype: void updateEnemy()
	Description: If player has more than 0 health, this function lets him be attacked by an enemy and then updates labels, as well as outputting what happened and how much damage was done. If player reaches 0 health, player dies and the game ends. 
	Visibility: private
m.)	Prototype: void playerHandler()
	Description: Switches GameState to enemy state and calls updatePlayer().
	Visibility: private
n.)	Prototype: void enemyHandler()
	Description: Switches GameState to player state and calls updateEnemy().
	Visibility: private
o.)	Prototype: void attack_Click(object sender, EventArgs e)
	Description: calls playerHandler(), makes a new SaveGameData object with temporary values and sets it equal to the overall save values
	Visibility: private
p.)	Prototype: void enemy_attack_Click(object sender, EventArgs e)
	Description: calls enemyHandler(), makes a new SaveGameData object with temporary values and sets it equal to the overall save values
	Visibility: private
q.)	Prototype: void newGame_button_Click(object sender, EventArgs e)
	Description: Clears the combatLog game object, sets player and enemy values back to default, switches gamestate to player, calls updateLabels(), and makes a new SaveGameData object with temporary values and sets it equal to the overall save values. In essence, starts the game over.
	Visibility: private
r.)	Prototype: void save_button_Click(object sender, EventArgs e)
	Description: gives a path for data to be saved to and serializes data to a .xml file
	Visibility: private
s.)	Prototype: void load_button_Click(object sender, EventArgs e)
	Description: Clears the combatLog game object, deserializes XML data from a .xml file at a specified path, and sets player’s stat values to the values saved in the file, then calls updateLabels().
	Visibility: private

III.1 Source Code
Form1.cs
namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private SaveGameData SuperSave;
        private Player player = Player.instance;
        private Enemy enemy = new Enemy();
        public Form1()
        {
            
            InitializeComponent();
            phase_label.Text = GameState.init.ToString();
            attack.Enabled = false;
            enemy_attack.Enabled = false;
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
            ph_label.Text = player.health.ToString();
            ps_label.Text = player.strength.ToString();
            pl_label.Text = player.level.ToString();
            pe_label.Text = player.experience.ToString();
            phase_label.Text = GAMEFSM.cState.ToString();
            eh_label.Text = enemy.health.ToString();
            es_label.Text = enemy.strength.ToString();
        }

        private void updatePlayer()
        {
            if (player.health > 0)
            {
                combatLog.AppendText("Player hit Enemy for " + player.strength + " damage.\n");
                player.Attack(enemy);
                updateLabels();
                if (enemy.health <= 0)
                {
                    combatLog.AppendText("Player killed an enemy! Here comes another one!\n");
                    player.Kill(enemy);
                    enemy.health = 50;
                    updateLabels();
                }
            }
            attack.Enabled = false;
            enemy_attack.Enabled = true;
            if (player.health <= 0)
            {
                player.health = 0;
                attack.Enabled = false;
                enemy_attack.Enabled = false;
                combatLog.AppendText("Player has died! GAME OVER!\n");
            }
        }

        private void updateEnemy()
        {
            if (player.health > 0)
            {
                combatLog.AppendText("Enemy hit Player for " + enemy.strength + " damage.\n");
                enemy.Attack(player);
                updateLabels();
            }
            enemy_attack.Enabled = false;
            attack.Enabled = true;
            if (player.health <= 0)
            {
                player.health = 0;
                attack.Enabled = false;
                enemy_attack.Enabled = false;
                combatLog.AppendText("Player has died! GAME OVER!\n");
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

        private void attack_Click(object sender, EventArgs e)
        {
            playerHandler();
            SaveGameData save = new SaveGameData(player.health, player.strength, player.level, player.experience);
            SuperSave = save;
        }

        private void enemy_attack_Click(object sender, EventArgs e)
        {
            enemyHandler();
            SaveGameData save = new SaveGameData(player.health, player.strength, player.level, player.experience);
            SuperSave = save;
        }

        private void newGame_button_Click(object sender, EventArgs e)
        {
            combatLog.Clear();
            player.health = 100;
            player.strength = 10;
            player.level = 1;
            player.experience = 0;
            enemy.health = 50;
            enemy.strength = 3;
            GAMEFSM.Switch(GameState.player);
            updateLabels();
            attack.Enabled = true;
            enemy_attack.Enabled = false;
            SaveGameData save = new SaveGameData(player.health, player.strength, player.level, player.experience);
            SuperSave = save;
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Guy.Goudeau\Desktop\Homework\Second Semester\WindowsFormsApplication2\WindowsFormsApplication2\Saves\";
            Utilities.SerializeXML<SaveGameData>("Stats", SuperSave, path);
            combatLog.AppendText("Game has been saved.\n");
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            combatLog.Clear();
            combatLog.AppendText("Game has been loaded.\n");
            string path = @"C:\Users\Guy.Goudeau\Desktop\Homework\Second Semester\WindowsFormsApplication2\WindowsFormsApplication2\Saves\Stats";
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
}

Combat.cs
namespace Combat
{

    interface IDamageable
    {
        int takeDamage(int d);
    }
    interface IStats
    {
        int health { get; set; }
        int strength { get; set; }
    }
    interface IPlayerAttack
    {
        int Attack(Enemy other);
    }
    interface IEnemyAttack
    {
        int Attack(Player other);
    }
    interface ILevel
    {
        int level { get; set; }
        int experience { get; set; }
    }
    interface IKill
    {
        Player Kill(Enemy other);
    }

    class Player : IDamageable, IStats, IPlayerAttack, ILevel, IKill
    {
        static private Player _instance;
        static public Player instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Player();
                }
                return _instance;
            }
        }

        private int hp;
        private int str;
        private int lvl;
        private int exp;

        public Player()
        {
            hp = 100;
            str = 10;
            lvl = 1;
            exp = 0;
        }

        public int health
        {
            get { return hp; }
            set { hp = value; }
        }
        public int strength
        {
            get { return str; }
            set { str = value; }
        }
        public int level
        {
            get { return lvl; }
            set { lvl = value; }
        }
        public int experience
        {
            get { return exp; }
            set { exp = value; }
        }

        public int takeDamage(int d)
        {
            return health -= d;
        }

        public int Attack(Enemy other)
        {
            return other.takeDamage(strength);
        }

        public Player Kill(Enemy other)
        {
            experience += 25;
            Player temp = this;
            if (other.health <= 0)
            {
                if (experience == 25 + (level * 50))
                {
                    temp.level += 1;
                    temp.health = 100;
                    temp.health += 5 * level;
                    temp.strength += 5;
                }
            }
            return temp;
        }
    }

    class Enemy : IDamageable, IStats, IEnemyAttack
    {
        private int hp;
        private int str;

        public Enemy()
        {
            hp = 50;
            str = 3;
        }

        public int health
        {
            get { return hp; }
            set { hp = value; }
        }

        public int strength
        {
            get { return str; }
            set { str = value; }
        }

        public int takeDamage(int d)
        {
            return health -= d;
        }

        public int Attack(Player other)
        {
            return other.takeDamage(strength);
        }
    }
}

FSM.cs
namespace FinateStateMachine
{
    ///
    struct Link<T>
    {
        public T from;
        public T to;
    }

    /// <summary>
    /// this is fsm
    /// </summary>
    /// <typeparam name="T">this needs to be a list of enums or some shit</typeparam>
    class FSM<T>
    {
        private Dictionary<T, List<Link<T>>> dict = new Dictionary<T, List<Link<T>>>();

        T currentState;
        public Enum cState
        {
            get { return currentState as Enum; }
            
        }
        public FSM(T initial)
        {
            
         //   var v = Enum.GetValues(typeof(T));
         // look up how to find out all enums from a generic type
            AddState(initial);
            currentState = initial;
        }

        public bool AddState(T state)
        {
            if (dict.ContainsKey(state))
            {
                return false;
            }
            dict.Add(state, new List<Link<T>>());
            return true;
        }


        public bool AddTransition(T from, T to)
        {
            Link<T> lin = new Link<T>();
            lin.from = from;
            lin.to = to;
            if (dict[from].Contains(lin))
            {
                return false;
            }
            dict[from].Add(lin);
            return true;
        }

        public T Switch(T to)
        {
            Link<T> temp = new Link<T>();
            temp.from = currentState;
            temp.to = to;
            foreach (Link<T> l in dict[currentState])
            {
                if (l.Equals(temp))
                {
                    currentState = to;
                    return currentState;
                }
            }
            return currentState;
        }
    }
}

Utilities.cs
namespace Serializer
{
    public static class Utilities
    {
        public static void SerializeXML<T>(string s, T t, string path)
        {
            if (Directory.Exists(path))
            {
                using (FileStream fs = File.Create(path + s + ".xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(fs, t);
                    fs.Close();
                }
            }
            else
            {
                string appName = System.Windows.Forms.Application.ProductName;
                Directory.CreateDirectory(path + appName);
            }

        }

        /// <summary>
        /// deserialize from a path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string s)
        {

            T t; //We will use the as the return value      

            using (FileStream fs = File.OpenRead(s + ".xml"))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                t = (T)deserializer.Deserialize(fs);
                fs.Close();
            }

            return t;
        }

    }
}

In Unity: ButtonController.cs
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


IV.3 Operating Directions
Navigate to the project folder and launch the executable
Click the New Game button
Attack until you are finished with the game