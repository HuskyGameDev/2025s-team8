using Godot;
using System;

public partial class SaveLoad : Node
{
    // LoadBasic(string s)
    //   load the save file "<s>.hdsave", and return only basic data
    // (anything needed for the SaveSelect process).
    public static SaveState LoadBasic(string s)
    {
        // Redundant, but this might actually be useful when SaveState is more developed
        SaveState sv = new SaveState();
        using var f = FileAccess.Open("user://saves/" + s + ".hdsave", FileAccess.ModeFlags.Write);
        sv.saveName = f.GetLine();
        f.Close();
        return sv;
    }

    // Load(string s)
    //   load the save file "<s>.hdsave", and return all data
    public static SaveState Load(string s)
    {
        SaveState sv = new SaveState();
        using var f = FileAccess.Open("user://saves/" + s + ".hdsave", FileAccess.ModeFlags.Write);
        sv.saveName = f.GetLine();
        sv.weaponId = f.GetLine();
        sv.armorId = f.GetLine();
        sv.consumableId = f.GetLine();
        sv.inv = f.GetCsvLine();
        sv.stash = f.GetCsvLine();
        f.Close();
        return sv;
    }


    // Save(SaveState sv)
    //   save the player's data (in SaveState sv) to a ".hdsave" (depending on the save name)
    // This will overwrite their save data.
    public static void Save(SaveState sv)
    {
        using var f = FileAccess.Open("user://saves/" + sv.saveName + ".hdsave", FileAccess.ModeFlags.Write);
        f.StoreLine(sv.saveName);
        f.StoreLine(sv.weaponId);
        f.StoreLine(sv.armorId);
        f.StoreLine(sv.consumableId);
        f.StoreCsvLine(sv.inv);
        f.StoreCsvLine(sv.stash);
        f.Close();
    }

    public class SaveState
    {
        public string saveName;

        public string weaponId;
        public string armorId;
        public string consumableId;

        public string[] inv;

        public string[] stash;

        public SaveState()
        {
            saveName = "";
            weaponId = "";
            armorId = "";
            consumableId = "";
            inv = new string[0];
            stash = new string[0];
        }
        
    }
}
