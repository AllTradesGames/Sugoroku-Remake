using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum ItemEffect
{
    None,
    Attack,
    Defense,
    Movement,
    Evade,
    Escape,
    CritAttack,
    CritDefend,
    CritEmpty,
    CritStun,
    CritLegDmg,
    Voodoo,
    MoveHeal,
    RestUp,
    Crutch,
    NoPanic,
    NoStun,
    NoLegDmg,
    NoEmpty,
    RollCap5,
    CausePanic,
    CauseEmpty,
    CauseStun,
    CauseLegDmg,
    NPC0NoCounter,
    NPC1NoCounter,
    NPC2NoCounter,
    NPC3NoCounter
}

public class DataControl : MonoBehaviour
{
    public const int NUM_ITEM_SECTIONS = 3;
    public const int ITEMS_PER_SECTION = 20;

    public string savedCharacterFileName = "SavedCharacters.dat";
    public string itemFilePath = "Assets/Resources/ItemData/";
    public string itemFileName = "ItemFile.dat";
    // TODO - Item list is currently being generated in ItemListClass constructor
    public ItemListClass masterItemListClass = new ItemListClass();
    public CharacterListClass savedCharacterListClass = new CharacterListClass();
    public List<Character> playerList = new List<Character>();

    private List<Item> tempItemList = new List<Item>();
    private int tempInt = 0;
    private BinaryFormatter bf = new BinaryFormatter();
    private FileStream file;


    void Awake()
    {
        File.Delete(Application.persistentDataPath + "/" + savedCharacterFileName);   // TODO REMOVE AFTER TESTING

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);

            // TODO Create Item Files so these lines can be un-commented

            /*tempInt = NUM_ITEM_SECTIONS * ITEMS_PER_SECTION;
            for (int ii = 0; ii < tempInt; ii++)
            {
                masterItemListClass.list.Add(null);
            }
            Debug.Log("Initialized masterItemListClass.list with " + masterItemListClass.list.Count + " null entries.");

            for(int ii=0; ii<NUM_ITEM_SECTIONS; ii++)
            {
                LoadItemList(ii);
            }*/

            LoadSavedCharacters();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            File.Delete(Application.persistentDataPath + "/" + savedCharacterFileName);   // TODO REMOVE AFTER TESTING
            Debug.Log("Deleted saved characters...");
            LoadSavedCharacters();
        }
    }


    public void LoadSavedCharacters()
    {
        
        if (File.Exists(Application.persistentDataPath + "/" + savedCharacterFileName))
        {
            Debug.Log("Loading saved characters...");
            file = File.Open(Application.persistentDataPath + "/" + savedCharacterFileName, FileMode.Open);
            savedCharacterListClass = (CharacterListClass)bf.Deserialize(file);
            file.Close();

            playerList.Clear();
            foreach (Character character in savedCharacterListClass.list)
            {
                if(character.inParty)
                {
                    playerList.Add(character);
                }
            }
        }
        else
        {
            // TODO Mock character data here - REMOVE AFTER TESTING
            savedCharacterListClass.list.Add(new Character(0));
            savedCharacterListClass.list.Add(new Character(1));
            savedCharacterListClass.list.Add(new Character(2));
            SaveCharacters();
            LoadSavedCharacters();

            /*Debug.Log("Creating empty saved character file..");
            SaveCharacters();*/
        }
    } 


    public void SaveCharacters()
    {
        Debug.Log("Saving Character file...");
        file = File.Create(Application.persistentDataPath + "/" + savedCharacterFileName);
        bf.Serialize(file, savedCharacterListClass);
        file.Close();
    }


    public void LoadItemList(int section)
    {
        if (masterItemListClass.loadedSections[section])
        {
            Debug.Log("Section " + section + " already loaded.");
        }
        else
        {
            Debug.Log("Loading item section " + section + "...");
            if (File.Exists(itemFilePath + section + itemFileName))
            {
                file = File.Open(itemFilePath + section + itemFileName, FileMode.Open);
                tempItemList = ((ItemListClass)bf.Deserialize(file)).list;
                file.Close();

                tempInt = ITEMS_PER_SECTION * section;
                masterItemListClass.list.InsertRange(tempInt, tempItemList);
                masterItemListClass.list.RemoveRange(tempInt + NUM_ITEM_SECTIONS, NUM_ITEM_SECTIONS);
            }
            else
            {
                Debug.LogError("'" + itemFilePath + section + itemFileName + "' file not found.");
            }


        }
    }


}
