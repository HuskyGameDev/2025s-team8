using Godot;
using System;

public partial class PlayerData : Node
{
    public SaveState save = null;
    private const string saveLoc = "user://saves/";

    public override void _Ready()
    {
        if (!DirAccess.DirExistsAbsolute(saveLoc))
        {
            DirAccess.MakeDirAbsolute(saveLoc);
        }
    }

    // Load(string s)
    //   load the save file "<s>.hdsave", and return all data
    public static SaveState Load(string s)
    {
        SaveState sv = new SaveState();
        using var f = FileAccess.Open(saveLoc + s + ".hdsave", FileAccess.ModeFlags.Read);
        try
        {
            sv.version = f.GetLine().ToInt();
        } catch (FormatException)
        {
            return null; // Old/invalid file
        }
        sv.saveName = f.GetLine();
        sv.playerName = f.GetLine();
        sv.weaponId = f.GetLine();
        sv.weapon2Id = f.GetLine();
        sv.armorId = f.GetLine();
        sv.consumableId = f.GetLine();
        sv.consumable2Id = f.GetLine();
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
        using var f = FileAccess.Open(saveLoc + sv.saveName + ".hdsave", FileAccess.ModeFlags.Write);
        f.StoreLine("" + sv.version);
        f.StoreLine(sv.saveName);
        f.StoreLine(sv.playerName);
        f.StoreLine(sv.weaponId);
        f.StoreLine(sv.weapon2Id);
        f.StoreLine(sv.armorId);
        f.StoreLine(sv.consumableId);
        f.StoreLine(sv.consumable2Id);
        f.StoreCsvLine(sv.inv);
        f.StoreCsvLine(sv.stash);
        f.Close();
    }

    public static bool DoesSaveExist(string s)
    {
        return FileAccess.FileExists(saveLoc + s + ".hdsave");
    }

    public static void Delete(string s)
    {
        DirAccess.RemoveAbsolute(saveLoc + s + ".hdsave");
    }

    public class SaveState
    {
        public int version;
        public string saveName;
        public string playerName;

        public string weaponId;
        public string weapon2Id;
        public string armorId;
        public string consumableId;
        public string consumable2Id;

        public string[] inv;

        public string[] stash;

        public SaveState()
        {
            version = 1;
            saveName = "";
            playerName = "";
            weaponId = "";
            weapon2Id = "";
            armorId = "";
            consumableId = "";
            consumable2Id = "";
            inv = new string[0];
            stash = new string[0];
        }

    }
}
