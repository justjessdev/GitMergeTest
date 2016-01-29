  using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ItemDatabaseScript : MonoBehaviour {
    public Item[] database = {
                                new Item(0, "Clothing Fiber"),
                                new Item(1, "Strand of Hair"),
                                new Item(2, "Shoeprint"),
                                new Item(3, "Blood"),
								new Item(4, "Weapon Type"),
                                new Item(5, "Angle of Attack")
                              };

    void Start() {

    }

    public Item FetchItemByID(int a_id) {
        for (int i = 0; i < database.Length; ++i) { 
            if(database[i].id == a_id) {
                return database[i];
            }        
        }
        return null;
    }
}

public class Item {
    public int id;
    public string type;

    public Item(int a_id, string a_type) {
        this.id = a_id;
        this.type = a_type;
    }

    public Item() {
        this.id = -1;
    }
}

public class ClueAttribute {
    public enum Trait { None, ClothingColor, HairColor, Height, Gender, BloodType, Class };

    public Trait trait = Trait.None;

    public int traitData = 0;
}

public class Clue {
    public Item item;
    public List<ClueAttribute> attributes;
}